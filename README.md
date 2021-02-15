# Autofac.Extras.DomainServices

[Autofac](https://autofac.org) Domain Service factory for [WCF RIA Services](https://msdn.microsoft.com/en-us/library/ee707344(v=vs.91).aspx).

[![Build status](https://ci.appveyor.com/api/projects/status/7cy42ia922we7v3w?svg=true)](https://ci.appveyor.com/project/Autofac/autofac-extras-domainservices)

> :warning: **MAINTENANCE MODE**: This package is in maintenance-only mode. Bug fixes may be addressed and Autofac compatibility may be checked but no new features will be added.

Please file issues and pull requests for this package in this repository rather than in the Autofac core repo.

- [NuGet](https://www.nuget.org/packages/Autofac.Extras.DomainServices/)
- [Contributing](https://autofac.readthedocs.io/en/latest/contributors.html)

## Quick Start

To get Autofac integrated with RIA/domain services app you need to reference the Domain Services integration NuGet package, register services, and register the integration module.

```csharp
public class Global : HttpApplication, IContainerProviderAccessor
{
  // The IContainerProviderAccessor and IContainerProvider
  // interfaces are part of the web integration and are used
  // for registering/resolving dependencies on a per-request
  // basis.
  private static IContainerProvider _containerProvider;

  public IContainerProvider ContainerProvider
  {
    get { return _containerProvider; }
  }

  protected void Application_Start(object sender, EventArgs e)
  {
    var builder = new ContainerBuilder();

    // Register your domain services.
    builder
      .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
      .AssignableTo<DomainService>();

    // Add the RIA Services module so the "Initialize"
    // method gets called on your resolved services.
    builder.RegisterModule<AutofacDomainServiceModule>();

    // Build the container and set the container provider
    // as in standard web integration.
    var container = builder.Build();
    _containerProvider = new ContainerProvider(container);

    // Replace the DomainService.Factory with
    // AutofacDomainServiceFactory so things get resolved.
    var factory = new AutofacDomainServiceFactory(_containerProvider);
    DomainService.Factory = factory;
  }
}
```

When you write your domain services, use constructor injection and other standard patterns just like any other Autofac/IoC usage.

## Example

The Autofac examples repository (at tag `v3.5.2`) has a [Domain Services project](https://github.com/autofac/Examples/tree/v3.5.2/src/DomainServicesExample) that is consumed by a Silverlight application.
