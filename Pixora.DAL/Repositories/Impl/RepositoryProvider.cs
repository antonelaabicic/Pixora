using Microsoft.Extensions.DependencyInjection;
using Pixora.DAL.Repositories.Interfaces;

namespace Pixora.DAL.Repositories.Impl
{
    public class RepositoryProvider : IRepositoryProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public RepositoryProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetRepository<T>() where T : class
        {
            var repository = _serviceProvider.GetService<T>() ??
                throw new InvalidOperationException($"Repository of type {typeof(T).FullName} is not registered.");

            return repository;
        }
    }
}