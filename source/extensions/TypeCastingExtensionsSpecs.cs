using System;
using System.Collections.Generic;
using developwithpassion.specifications.assertions.core;
using developwithpassion.specifications.assertions.type_specificity;
using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Specifications;

namespace developwithpassion.specifications.extensions
{
  public class TypeCastingExtensionsSpecs
  {
    public class BaseType
    {
    }

    public class OtherType
    {
    }

    public class SubType : BaseType
    {
    }

    [Subject(typeof(TypeCastingExtensions))]
    public class when_a_legitimate_downcast_is_made
    {
      Because b = () =>
        new List<int>().downcast_to<List<int>>();

      It should_not_fail = () =>
      {
      };
    }

    [Subject(typeof(TypeCastingExtensions))]
    public class when_an_illegal_downcast_is_attempted : use_engine<RhinoFakeEngine>.observe
    {
      Because b = () =>
        spec.catch_exception(() => 2.downcast_to<DateTime>());

      It should_throw_an_invalid_cast_exception = () =>
        spec.exception_thrown.should().be_an<InvalidCastException>();
    }

    [Subject(typeof(TypeCastingExtensions))]
    public class when_determining_if_an_object_is_not_an_instance_of_a_specific_type
    {
      It should_make_determination_based_on_whether_the_object_is_assignable_from_the_specific_type = () =>
      {
        new OtherType().is_not_a<BaseType>().ShouldBeTrue();
        new SubType().is_not_a<BaseType>().ShouldBeFalse();
      };
    }
  }
}