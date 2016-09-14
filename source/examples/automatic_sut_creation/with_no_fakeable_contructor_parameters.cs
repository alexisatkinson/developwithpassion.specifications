using System;
using Machine.Fakes.Adapters.Moq;
using Machine.Specifications;

namespace developwithpassion.specifications.examples.automatic_sut_creation
{
  public class with_no_fakeable_contructor_parameters
  {
    [Subject(typeof(Calculator))]
    public class when_adding_2_numbers : use_engine<MoqFakeEngine>.observe<Calculator>
    {
      Establish c = () =>
      {
        depends.on(1);
        depends.on(DateTime.Now);
        depends.on("the value");
      };

      Because b = () =>
        result = sut.add(2, 3);

      It should_return_the_sum = () =>
        result.ShouldEqual(5);

      static int result;
    }

    public class Calculator
    {
      DateTime date;
      int number;
      string message;

      public Calculator(DateTime date, string message, int number)
      {
        this.date = date;
        this.number = number;
        this.message = message;
      }

      public int add(int first, int second)
      {
        return first + second;
      }
    }
  }
}
