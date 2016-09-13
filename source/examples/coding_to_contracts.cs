using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Specifications;

namespace developwithpassion.specifications.examples
{
  public class coding_to_contracts
  {
    [Subject(typeof(Calculator))]
    public class when_adding_2_numbers : use_engine<RhinoFakeEngine>.observe<ICalculate, Calculator>
    {
      Because b = () =>
        //when inheriting from use_engine<RhinoFakeEngine>.observe<Contract,Class> the sut field
        //will be of the contract type
        //to get access to the class instance, you can use the concrete_sut
        //field (mostly if you want to make direct assertions against the class of the
        //system under test
        result = sut.add(2, 3);

      It should_return_the_sum = () =>
        result.ShouldEqual(5);

      static int result;
    }

    public class Calculator : ICalculate
    {
      public int add(int first, int second)
      {
        return first + second;
      }
    }
  }

  public interface ICalculate
  {
    int add(int first, int second);
  }
}