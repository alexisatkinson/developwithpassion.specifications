﻿using developwithpassion.specifications.core;
using developwithpassion.specifications.core.factories;
using developwithpassion.specifications.extensions;
using developwithpassion.specifications.faking;
using Machine.Fakes.Adapters.Moq;
using Machine.Fakes.Sdk;

namespace developwithpassion.specifications.specs.utility
{
  public class ObjectFactory
  {
    public static IManageFakes create_fakes_adapter<Target>() where Target : class
    {
      return new FakesAdapter(new SpecificationController<Target>(
        new MoqFakeEngine()));
    }

    public static ICreateFakeDelegates create_fake_delegate_factory()
    {
      return new FakeDelegateFactory();
    }

    public static IResolveADependencyForTheSUT create_sut_dependency_resolver<Target>(IManageFakes fakes)
    {
      return new SUTDependencyResolver(fakes,
        create_fake_delegate_factory());
    }

    public static IResolveADependencyForTheSUT create_sut_dependency_resolver<Target>() where Target : class
    {
      return create_sut_dependency_resolver<Target>(create_fakes_adapter<Target>());
    }

    public static IManageTheDependenciesForASUT create_dependencies<Target>() where Target : class
    {
      var fakes_adapter = create_fakes_adapter<Target>();
      return MainControllerFactory.new_instance().downcast_to<MainControllerFactory>()
        .dependency_registry_factory.create(fakes_adapter, create_sut_dependency_resolver<Target>(fakes_adapter));
    }

    public static IUpdateNonCtorDependenciesOnAnItem create_visitor<Target>() where Target : class
    {
      return create_visitor<Target>(create_dependencies<Target>());
    }

    public static IUpdateNonCtorDependenciesOnAnItem create_visitor<Target>(
      IManageTheDependenciesForASUT dependency_registry)
    {
      return MainControllerFactory.new_instance().downcast_to<MainControllerFactory>()
        .non_ctor_dependency_visitor_factory.create(dependency_registry);
    }

    public static DefaultSUTFactory<Target> create<Target>() where Target : class
    {
      var dependencies = create_dependencies<Target>();

      return new DefaultSUTFactory<Target>(dependencies,
        new NonCtorDependencyVisitorFactory().create(dependencies));
    }
  }
}
