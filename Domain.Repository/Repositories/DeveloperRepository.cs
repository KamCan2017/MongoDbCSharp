﻿using Client.Core.Model;
using Common;
using Core;
using Core.QueueClient;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private QueueClient _userChangedQueueClient;
        private QueueClient _usersSavedQueueClient;

        public DeveloperRepository()
        {
            //Add queue client
            _userChangedQueueClient = new QueueClient(QueueConfig.ExchangeUser, QueueConfig.SeverityUser);
            _usersSavedQueueClient = new QueueClient(QueueConfig.ExchangeUser, QueueConfig.SeverityMultipleUsers);
        }

        public BsonDocument CreateDocument(DeveloperModel developer)
        {
            if (developer == null)
                return null;

            var doc = developer.ToBsonDocument();
            return doc;
        }

        public async Task<DeveloperModel> SaveAsync(DeveloperModel model)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);
            if (model.KnowledgeBase != null && model.KnowledgeBase.Any())
            {
                model.KnowledgeIds = model.KnowledgeBase.Select(p => p.ID).ToList();
            }
            model.KnowledgeBase = null;

            await collection.InsertOneAsync(model);
            Console.WriteLine("document added: " + model.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.CountDocuments(filter).ToString());

            await FillKnowledge(model);

            //Publish the  saved entity
            _userChangedQueueClient.Publish(model);
            return model;
        }

        public async Task<IEnumerable<DeveloperModel>> SaveAsync(IEnumerable<DeveloperModel> entities)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            foreach (var model in entities)
            {
                if (model.KnowledgeBase != null && model.KnowledgeBase.Any())
                {
                    model.KnowledgeIds = model.KnowledgeBase.Select(p => p.ID).ToList();
                }
                model.KnowledgeBase = null;
            } 

            await collection.InsertManyAsync(entities);
            _usersSavedQueueClient.Publish(entities);
            return entities;
        }


        public async Task<DeveloperModel> UpdateAsync(DeveloperModel entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            //copy the array list to update separatly the array list
            var array = new List<string>();
            if (entity.KnowledgeBase != null && entity.KnowledgeBase.Any())
            {
                array = entity.KnowledgeBase.Select(p => p.ID).ToList();
            }
            entity.KnowledgeIds = null;
            entity.KnowledgeBase = null;

            var temp = await collection.ReplaceOneAsync(d => d.ID == entity.ID, entity);
            
            //update separatly the array list
            var update = Builders<DeveloperModel>.Update.Set(p => p.KnowledgeIds, array);
            await collection.UpdateOneAsync(d => d.ID == entity.ID, update);

            Console.WriteLine("document updated: " + entity.ToJson());

            var filter = new BsonDocument();
            Console.WriteLine("count:" + collection.CountDocuments(filter).ToString());

            entity.KnowledgeIds = array;
            await FillKnowledge(entity);

            //Publish the  saved entity
            _userChangedQueueClient.Publish(entity);

            return entity;
        }

        public async Task<bool> UpdateDocumentAsync(BsonDocument doc)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Developer);

            var filter1 = Builders<BsonDocument>.Filter.Eq("_id", doc["_id"]);
            await collection.ReplaceOneAsync(filter1, doc);
            Console.WriteLine("document updated: " + doc.ToJson());

            var filter = new BsonDocument();
            Console.WriteLine("count:" + collection.CountDocuments(filter).ToString());


            return true;
        }

        public async Task<BsonDocument> InsertDocumentAsync(BsonDocument doc)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Developer);

            await collection.InsertOneAsync(doc);
            Console.WriteLine("document added: " + doc.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.CountDocuments(filter).ToString());

            return doc;
        }

        public async Task<IEnumerable<DeveloperModel>> FindAllAsync()
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);
            var filter = new BsonDocument();

            var entities = await collection.Find(filter).ToListAsync();
            Console.WriteLine("developers count: " + entities.Count);
            if(entities.Any())
            {
                foreach(var entity in entities)
                {
                  await FillKnowledge(entity);
                }
            }
            return entities;
        }

        public async Task<DeveloperModel> FindByIdAsync(string id)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            var result = await collection.FindAsync(d => d.ID == id);
            var entity = result.FirstOrDefault();
            if (entity != null)
                await FillKnowledge(entity);

            return entity;
        }


        public async Task<bool> DeleteByIdAsync(ObjectId id)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            var result = await collection.DeleteOneAsync(d => d.ID == id.ToString());

            //check the document count
            var docs = await collection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("document count: " + docs.Count);

            return result.DeletedCount == 1;
        }

        public async Task<bool> DeleteAsync(IDeveloper entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            var result = await collection.DeleteOneAsync(d => d.ID == entity.ID);

            //check the document count
            //var docs = await collection.Find(new BsonDocument()).ToListAsync();
            //Console.WriteLine("document count: " + docs.Count);

            return result.DeletedCount == 1;
        }
        

        public async Task<bool> DeleteAllAsync()
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);
            var filter = new BsonDocument();
            var result = await collection.DeleteManyAsync(filter);
            return result.DeletedCount > 0;
        }


        public async Task<IEnumerable<DeveloperModel>> FindByTextSearchAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<DeveloperModel>();

            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            var filter = Builders<DeveloperModel>.Filter.Text(text, new TextSearchOptions()
            {
                DiacriticSensitive = true, CaseSensitive = false
            });
            var entities = await collection.Find(filter).ToListAsync();
            foreach (var entity in entities)
            {
                await FillKnowledge(entity);
            }
            return entities;
        }

        public async Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperView()
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.DeveloperView);
            var filter = new BsonDocument();

            var docs = await collection.Find(filter).ToListAsync();
            return docs;
        }

        public async Task<DeveloperModel> CloneAsync(DeveloperModel entity)
        {
            var clonedObj = new DeveloperModel();

            if(entity.KnowledgeBase != null && entity.KnowledgeBase.Any())
               clonedObj.KnowledgeBase = new ObservableCollection<KnowledgeModel>(entity.KnowledgeBase.ToList());

            clonedObj.CompanyName = entity.CompanyName;
            clonedObj.KnowledgeIds = null;
            clonedObj.Gender = entity.Gender;
            clonedObj.Name = entity.Name + "_cloned";
            
            clonedObj = await SaveAsync(clonedObj) as DeveloperModel;
            await FillKnowledge(clonedObj);

            return clonedObj;
        }


        private async Task FillKnowledge(DeveloperModel entity)
        {
            if(entity.KnowledgeIds != null && entity.KnowledgeIds.Any())
            {
                var ids = new List<ObjectId>();
                entity.KnowledgeIds.ForEach(o => ids.Add(ObjectId.Parse(o)));
                var collection = MongoClientManager.DataBase.GetCollection<KnowledgeModel>(CollectionNames.Knowledge);
                var filter = Builders<KnowledgeModel>.Filter.AnyIn("_id", ids);
                var doc = filter.ToBsonDocument();
                var entities = await collection.Find(filter).ToListAsync();
                entity.KnowledgeBase = new ObservableCollection<KnowledgeModel>(entities);
            }
        }
    }
}
