﻿using Core;
using Core.Interfaces;
using Core.ServiceClient;
using Developer;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Developer.ViewModels
{
    public class KnowledgeEditorViewModel : BindableBase
    {
        private DelegateCommand<Window> _saveCommand;
        private DelegateCommand _loadCommand;
        private KnowledgeModel _knowledge;
        private readonly IEventAggregator _eventAggregator;
        private IEnumerable<KnowledgeModel> _knowledges;
        private KnowledgeModel _selectedItem;

        public KnowledgeEditorViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _saveCommand = new DelegateCommand<Window>(window => Accept(window), CanExecuteSaveCommand);
            _loadCommand = new DelegateCommand(LoadData);

            Knowledge = new KnowledgeModel();
        }        

        public DelegateCommand <Window> SaveCommand { get { return _saveCommand; } }

        public DelegateCommand LoadCommand { get { return _loadCommand; } }


        public KnowledgeModel Knowledge
        {
            get { return _knowledge; }
            set
            {
                if(_knowledge != null)
                    _knowledge.PropertyChanged -= Knowledge_PropertyChanged;

                SetProperty(ref _knowledge, value);

                if (_knowledge != null)
                    _knowledge.PropertyChanged += Knowledge_PropertyChanged;
            }
        }

        public KnowledgeModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                if (_selectedItem != null)
                {
                    Knowledge.Technology = _selectedItem.Technology;
                    Knowledge.Language = _selectedItem.Language;
                }
                else
                    Knowledge = new KnowledgeModel();
            }
        }

        public IEnumerable<KnowledgeModel> Knowledges
        {
            get { return _knowledges; }
            set
            {
                SetProperty(ref _knowledges, value);
            }
        }

        public List<string> Technologies
        {
            get
            {
                return new List<string>()
                {
                    Technology.APSNet,
                    Technology.NoSQL,
                    Technology.QT,
                    Technology.WPF,
                    Technology.Apps,
                    Technology.UWP,
                    Technology.WinForms,
                    Technology.Xamarin,
                    Technology.ImageProcessing,
                };
            }
        }


        public List<string> Languages
        {
            get
            {
                return new List<string>()
                {
                    ProgrammingLanguage.CPlus,
                    ProgrammingLanguage.CScharp,
                    ProgrammingLanguage.Html,
                    ProgrammingLanguage.Java,
                    ProgrammingLanguage.Python,
                    ProgrammingLanguage.TypScript,
                    ProgrammingLanguage.Catia,
                    ProgrammingLanguage.Xml
                };
            }
        }

        public void LoadData()
        {
            Task.Run(async() => 
            {
                Knowledges = await ServiceClient<IKnowledgeService>.ExecuteAsync(o => o.FindAllAsync());
            });
        }

        private void Knowledge_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }


        private bool CanExecuteSaveCommand(Window arg)
        {
            return Knowledge != null && Knowledge.IsValid;
        }

        private bool Accept(Window window)
        {
            window.DialogResult = true;
            window.Close();

            var copy = Knowledges.FirstOrDefault(u => u.Technology == Knowledge.Technology && u.Language == Knowledge.Language);
            if (copy != null)
                Knowledge = copy;

            _eventAggregator.GetEvent<AddKnowledgePubEvent>().Publish(Knowledge);

            return true;
        }
    }
}
