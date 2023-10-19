using ETicaretApi.Application.Abstractions.Storage;
using ETicaretApi.Application.Abstractions.Token;
using ETicaretApi.Infranstructure.Enums;
using ETicaretApi.Infranstructure.Services;
using ETicaretApi.Infranstructure.Services.Storage;
using ETicaretApi.Infranstructure.Services.Storage.Azure;
using ETicaretApi.Infranstructure.Services.Storage.Local;
using ETicaretApi.Infranstructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace ETicaretApi.Infranstructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }

        public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
