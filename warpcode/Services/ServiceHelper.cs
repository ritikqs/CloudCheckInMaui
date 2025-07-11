using Microsoft.Extensions.DependencyInjection;
using System;

namespace CCIMIGRATION.Services
{
    public static class ServiceHelper
    {
        private static IServiceProvider _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceHelper has not been initialized. Call Initialize() first.");
            }

            return _serviceProvider.GetService<T>();
        }

        public static T GetRequiredService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceHelper has not been initialized. Call Initialize() first.");
            }

            return _serviceProvider.GetRequiredService<T>();
        }

        public static object GetService(Type serviceType)
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceHelper has not been initialized. Call Initialize() first.");
            }

            return _serviceProvider.GetService(serviceType);
        }

        public static object GetRequiredService(Type serviceType)
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceHelper has not been initialized. Call Initialize() first.");
            }

            return _serviceProvider.GetRequiredService(serviceType);
        }
    }
}
