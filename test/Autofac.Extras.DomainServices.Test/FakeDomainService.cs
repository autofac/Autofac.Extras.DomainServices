using System.ServiceModel.DomainServices.Server;

namespace Autofac.Extras.DomainServices.Test
{
    public class FakeDomainService : DomainService
    {
        public bool IsDisposed { get; protected set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
        }
    }
}
