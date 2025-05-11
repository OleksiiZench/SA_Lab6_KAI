using Autofac;
using AutoFixture;
using FoodDelivery.Tests.Configuration;

namespace FoodDelivery.Tests.Fixtures
{
    public class TestFixture : IDisposable
    {
        public IContainer Container { get; }
        public Fixture AutoFixture { get; }

        public TestFixture()
        {
            var builder = TestDependencyConfig.ConfigureTestContainer();
            Container = builder.Build();
            AutoFixture = new Fixture();
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
