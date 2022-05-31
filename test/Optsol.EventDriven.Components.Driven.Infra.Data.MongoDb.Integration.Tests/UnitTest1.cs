using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Contexts;

namespace Optsol.EventDriven.Components.Driven.Infra.Data.MongoDb.Integration.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile($@"Settings/appsettings.mongo.json")
                    .Build();

            var services = new ServiceCollection();

            services.AddMongoContext<MongoContext>(configuration);
            services.AddAutoMapper(typeof(TestViewModelToEntity));
            services.AddMongoRepository<ITestMongoReadRepository, TestMongoReadRepository>("Optsol.Components.Test.Utils");

            var provider = services.BuildServiceProvider();

            return provider;
        }
    }
}