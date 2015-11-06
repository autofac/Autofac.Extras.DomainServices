using Autofac.Integration.Web;
using Xunit;

namespace Autofac.Extras.DomainServices.Test
{
    public class DomainServiceFactoryFixture
    {
        [Fact]
        public void FactoryCanCreateDomainService()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FakeDomainService>();

            var containerProvider = new TestContainerProvider(builder.Build());
            var factory = new AutofacDomainServiceFactory(containerProvider);
            var service = factory.CreateDomainService(typeof(FakeDomainService), null);

            Assert.IsType<FakeDomainService>(service);
        }

        [Fact]
        public void FactoryCanDisposeDomainService()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FakeDomainService>();
            FakeDomainService service;
            var containerProvider = new TestContainerProvider(builder.Build());
            var factory = new AutofacDomainServiceFactory(containerProvider);
            service = (FakeDomainService)factory.CreateDomainService(typeof(FakeDomainService), null);
            factory.ReleaseDomainService(service);
            Assert.False(service.IsDisposed);

            containerProvider.EndRequestLifetime();
            Assert.True(service.IsDisposed);
        }

        private class TestContainerProvider : IContainerProvider
        {
            private ILifetimeScope _request;

            public TestContainerProvider(IContainer container)
            {
                this.ApplicationContainer = container;
            }

            public void EndRequestLifetime()
            {
                if (this._request != null)
                {
                    this._request.Dispose();
                    this._request = null;
                }
            }

            public ILifetimeScope ApplicationContainer { get; private set; }

            public ILifetimeScope RequestLifetime
            {
                get
                {
                    if (this._request == null)
                    {
                        this._request = this.ApplicationContainer.BeginLifetimeScope();
                    }
                    return this._request;
                }
            }
        }
    }
}
