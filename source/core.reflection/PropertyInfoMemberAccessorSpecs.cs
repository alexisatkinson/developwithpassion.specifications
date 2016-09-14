using System;
using System.Reflection;
using Machine.Fakes.Adapters.Moq;
using Machine.Specifications;

namespace developwithpassion.specifications.core.reflection
{
  [Subject(typeof(PropertyInfoMemberAccessor))]
  public class PropertyInfoMemberAccessorSpecs
  {
    public class concern : use_engine<MoqFakeEngine>.observe<MemberAccessor, PropertyInfoMemberAccessor>
    {
      Establish c = delegate
      {
        original_value = "original";
        PropertyInfoTargetItem.static_value = original_value;
        the_target_type = typeof(PropertyInfoTargetItem);
        writable_member = the_target_type.GetProperty("static_value");
        non_writable_member = the_target_type.GetProperty("read_only_static_value");
        non_writable_member_with_private_mutator =
          the_target_type.GetProperty("read_only_static_value_with_private_mutator");
        writable_member.ShouldNotBeNull();
        depends.on(writable_member);
      };

      protected static PropertyInfo writable_member;
      protected static string original_value;
      protected static Type the_target_type;
      protected static PropertyInfo non_writable_member;
      protected static PropertyInfo non_writable_member_with_private_mutator;
    }

    public class PropertyInfoTargetItem
    {
      public static string static_value { get; set; }
      public static string read_only_static_value_with_private_mutator { get; private set; }

      public static string read_only_static_value
      {
        get { return "blah"; }
      }
    }

    public class when_gettings_its_accessor_type : concern
    {
      It should_return_the_type_of_its_property = () =>
        sut.accessor_type.ShouldEqual(writable_member.PropertyType);
    }

    public class when_getting_its_value : concern
    {
      Because b = () =>
        result = sut.get_value(the_target_type);

      It should_get_the_value_of_the_field = () =>
        result.ShouldEqual(PropertyInfoTargetItem.static_value);

      protected static object result;
    }

    [Subject(typeof(PropertyInfoMemberAccessor))]
    public class when_setting_its_value : concern
    {
      public class and_the_property_is_writable : concern
      {
        Establish c = () =>
          value_to_change_to = "blasfsfd";

        Because b = () =>
          sut.change_value_to(the_target_type, value_to_change_to);

        It should_change_the_value_of_the_field = () =>
          PropertyInfoTargetItem.static_value.ShouldEqual(value_to_change_to);

        protected static string value_to_change_to;
      }

      public class and_the_property_is_non_writable : concern
      {
        Establish c = () =>
        {
          original = PropertyInfoTargetItem.read_only_static_value;
          value_to_change_to = "blasfsfd";
          depends.on(non_writable_member);
        };

        Because b = () =>
          sut.change_value_to(the_target_type, value_to_change_to);

        It should_not_attempt_to_update_the_value = () =>
          PropertyInfoTargetItem.read_only_static_value.ShouldEqual(original);

        protected static string value_to_change_to;
        static string original;
      }

      public class and_the_property_has_a_private_mutator : concern
      {
        Establish c = () =>
        {
          value_to_change_to = "blasfsfd";
          depends.on(non_writable_member_with_private_mutator);
        };

        Because b = () =>
          sut.change_value_to(the_target_type, value_to_change_to);

        It should_update_the_value_to_the_target_value = () =>
          PropertyInfoTargetItem.read_only_static_value_with_private_mutator.ShouldEqual(value_to_change_to);

        protected static string value_to_change_to;
      }
    }
  }
}
