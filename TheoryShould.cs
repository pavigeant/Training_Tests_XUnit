namespace Training_Tests_XUnit;

public class TheoryShould
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void AcceptsInlineData(int value)
    {
        // Inline data accept values as object, so boxing and unboxing occurs
        // doesn't work with decimal values
        Assert.InRange(value, 1, 3);
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void AcceptsMemberData(int value)
    {
        Assert.InRange(value, 1, 3);
    }

    [Theory]
    [ClassData(typeof(DataClass))] // any class that implements IEnumerable<object[]>
    public void AcceptsClassData(int value)
    {
        Assert.InRange(value, 1, 3);
    }

    // TheoryData<T> only works starting with C# 11 (.net 7) which introduce generic attributes
    //[Theory]
    //[TheoryData<int>(1)]
    //[TheoryData<int>(2)]
    //[TheoryData<int>(3)]
    //public void AcceptsStronglyTypedInlineData(int value)
    //{
    //    // Inline data accept values as object, so boxing and unboxing occurs
    //    // doesn't work with decimal values
    //    Assert.InRange(value, 1, 3);
    //}

    public static IEnumerable<object[]> Data => new List<object[]>
    {
        new object[] { 1 },
        new object[] { 2 },
        new object[] { 3 }
    };

    public class DataClass : List<object[]>
    {
        public DataClass()
        {
            Add(new object[] { 1 });
            Add(new object[] { 2 });
            Add(new object[] { 3 });
        }
    }
}