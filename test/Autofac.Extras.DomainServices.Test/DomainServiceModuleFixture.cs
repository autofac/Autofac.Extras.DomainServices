using System;
using System.ServiceModel.DomainServices.Server;
using Moq;
using Xunit;

namespace Autofac.Extras.DomainServices.Test
{
    public class DomainServiceModuleFixture
    {
        [Fact]
        public void ModuleInitalizeDomainServices()
        {
            var builder = new ContainerBuilder();
            var domainServiceMock = new Mock<DomainService>();
            builder.RegisterInstance(domainServiceMock.Object).As<DomainService>();
            builder.RegisterModule<AutofacDomainServiceModule>();
            using (var container = builder.Build())
            {
                var service = container.Resolve<DomainService>(TypedParameter.From(domainServiceContext));
                Assert.NotNull(service);
                domainServiceMock.Verify(ds => ds.Initialize(domainServiceContext));
            }
        }

        readonly DomainServiceContext domainServiceContext = new DomainServiceContext(new FakeServiceProvider(), DomainOperationType.Invoke);

        class FakeServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }
        }
    }
}
