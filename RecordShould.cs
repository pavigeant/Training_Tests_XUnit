namespace Training_Tests_XUnit;

public class RecordShould
{
    [Fact]
    public async Task CaptureExceptionWithoutStoppingTheTest()
    {
        static void DoSomethingBad() { throw new ArgumentException("Bad stuff"); }
        static Task DoSomethingBadAsync() { throw new ArgumentException("Bad stuff"); }

        var ex1 = Record.Exception(() => DoSomethingBad());
        var ex2 = await Record.ExceptionAsync(() => DoSomethingBadAsync());

        Assert.NotNull(ex1);
        Assert.NotNull(ex2);
        Assert.Equal("Bad stuff", ex1.Message);
        Assert.Equal("Bad stuff", ex2.Message);
    }
}
