using Xunit.Sdk;

namespace Training_Tests_XUnit;

public class ClassFixtureShould : IClassFixture<ClassFixture>
{
    private readonly ClassFixture _fixture;

    public ClassFixtureShould(ClassFixture fixture)
    {
        // A class fixture is created once for the test class and it's content will be shared among all tests

        // The fixture is created and injected in the constructor
        _fixture = fixture;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void AcceptsFixture(int sequence)
    {
        // Avoid using conditional Assert in your test. It's better to create a new test for each condition
        if (sequence == 3)
            Assert.Throws<InRangeException>(() => Assert.InRange(sequence, 1, 2));
        else
            Assert.InRange(sequence, 1, 2);

        _fixture.Count++;
    }
}

public class ClassFixture
{
    public int Count { get; set; }
}
