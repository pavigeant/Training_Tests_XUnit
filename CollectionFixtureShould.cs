using Xunit.Abstractions;

namespace Training_Tests_XUnit;

[Collection(nameof(CollectionFixtureCollection))]
public class CollectionFixtureShould
{
    private readonly ITestOutputHelper _output;
    private readonly CollectionFixture _fixture;

    public CollectionFixtureShould(CollectionFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _fixture = fixture;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void AcceptsFixture(int sequence)
    {
        _output.WriteLine($"Sequence: {sequence}. Fixture Count: {_fixture.Count}");

        _fixture.Count++;
    }
}

[Collection(nameof(CollectionFixtureCollection))]
public class CollectionFixtureShouldAlso
{
    private readonly ITestOutputHelper _output;
    private readonly CollectionFixture _fixture;


    public CollectionFixtureShouldAlso(CollectionFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _fixture = fixture;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void AcceptsFixture(int sequence)
    {
        _output.WriteLine($"Sequence: {sequence}. Fixture Count: {_fixture.Count}");

        _fixture.Count++;
    }
}

public class CollectionFixture
{
    public int Count { get; set; } = 1;
}

// This class is never created by the test runner. It's only used to define the collection name
[CollectionDefinition(nameof(CollectionFixtureCollection))]
public class CollectionFixtureCollection : ICollectionFixture<CollectionFixture> { }