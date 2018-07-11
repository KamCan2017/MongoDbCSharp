using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Core.ServiceClient
{
    public static class ServiceClient<T>
    {
        private static readonly ChannelFactory<T> ChannelFactory;
        static ServiceClient()
        {
            var access = ServiceEndpoint.GetEndPointOfService<T>();
            ChannelFactory = new ChannelFactory<T>(access.Binding, access.EndPoint);
        }

        public static T GetService()
        {
            IClientChannel clientChannel = (IClientChannel)ChannelFactory.CreateChannel();
            return (T)clientChannel;
        }

        public static TResult Execute<TResult>(Func<T, TResult> action)
        {
            IClientChannel clientChannel = (IClientChannel)ChannelFactory.CreateChannel();
            TResult result;

            bool success = false;
            try
            {
                result = action((T)clientChannel);
                clientChannel.Close();
                success = true;
            }
            finally
            {
                if (!success)
                {
                    clientChannel.Abort();
                }
            }
            return result;
        }

        public static async Task<TResult> ExecuteAsync<TResult>(Func<T, Task<TResult>> action)
        {
            IClientChannel clientChannel = (IClientChannel)ChannelFactory.CreateChannel();

            bool success = false;
            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            try
            {
                taskCompletionSource.TrySetResult(await action((T)clientChannel));
                clientChannel.Close();
                success = true;
            }
            catch (Exception ex)
            {
                taskCompletionSource.TrySetException(ex);
            }
            finally
            {
                if (!success)
                {
                    clientChannel.Abort();
                }
            }
            return await taskCompletionSource.Task;
        }

    }


    public static class ServiceEndpoint
    {
        private static readonly Dictionary<Type, ChannelAccess> _mappingEndpoint = new Dictionary<Type, ChannelAccess>();

        static ServiceEndpoint()
        {           

            _mappingEndpoint.Add(typeof(IDeveloperService),
                new ChannelAccess()
                {
                    EndPoint = "http://localhost:59548/DeveloperService.svc",
                    Binding = new BasicHttpBinding() { MaxReceivedMessageSize = 268435456 }
                }
                    );
        }

        public static ChannelAccess GetEndPointOfService<T>()
        {
            if (_mappingEndpoint.TryGetValue(typeof(T), out ChannelAccess channelAccess))
                return channelAccess;

            return new ChannelAccess() { Binding = new BasicHttpBinding() };
        }

    }


    public class ChannelAccess
    {
        public string EndPoint { get; set; }

        public Binding Binding { get; set; }
    }
}
