using Xunit.Sdk;

namespace Training_Tests_XUnit;

public class AssertShould
{
    private readonly List<Student> _students;

    public AssertShould()
    {
        _students = new List<Student>
        {
            new("John", 20),
            new("Jane", 21),
            new("Doe", 22)
        };
    }

    [Fact(Skip = "You should have a real good reason to skip a test. A skipped test shouldn't stay skipped very long or should just be deleted if it has become irrelevant")]
    public void SkipTest()
    {
        // This test will be skipped

        // A code that is no longer working and is making a test fail is NOT a good reason to skip a test
    }

    [Fact]
    public void AssertAllItemsInCollection()
    {
        // Assert all takes an action which is another Assert call
        Assert.All(_students, s => Assert.True(s.Age > 18));

        // Supports index as well
        Assert.All(_students, (s, index) => Assert.True(s.Age > 18));

        // Also available is AllAsync for case when the collection is IAsyncEnumerable or the test is asyn5c
    }

    [Fact]
    public void CheckIfListContains()
    {
        // Search for a specific item in the collection using the default comparer
        Assert.Contains(new("John", 20), _students);
        Assert.DoesNotContain(new("Alice", 44), _students);

        // Works for these types
        // - HashSet<T>
        // - IEnumerable<T> (with optional comparer)
        // - IAsyncEnumerable<T> (with optional comparer)
        // - IReadOnlySet<T>
        // - ISet<T>
        // - SortedSet<T>
        // - ImmutableHashSet<T>
        // - ImmutableSortedSet<T>
        // - Dictionary<TKey, TValue>
        // - IDictionary<TKey, TValue>
        // - IReadOnlyDictionary<TKey, TValue>
        // - ConcurrentDictionary<TKey, TValue>
        // - ImmutableDictionary<TKey, TValue>
        // - ReadOnlyDictionary<TKey, TValue>
    }

    [Fact]
    public void CheckIfStringContains()
    {
        // Can also search for a substring
        Assert.Contains("world", "Hello World", StringComparison.CurrentCultureIgnoreCase);
        Assert.DoesNotContain("world", "Hello World", StringComparison.CurrentCulture);

        // Works for these types
        // - String
        // - Memory<char>
        // - ReadOnlyMemory<char>
        // - Span<char>
        // - ReadOnlySpan<char>
    }

    [Fact]
    public void VerifyCollectionEmpty()
    {
        var inactiveStudents = Array.Empty<Student>();

        Assert.Empty(inactiveStudents);
        Assert.NotEmpty(_students);

        // Works for these types
        // - IEnumerable
        // - IAsyncEnumerable<T>
    }

    [Fact]
    public void CheckForDistinctCollection()
    {
        var duplicates = new List<Student>() {
            new("John", 20),
            new("John", 20),
            new("Jane", 21),
            new("Doe", 22)
        };

        Assert.Distinct(_students);

        Assert.Throws<DistinctException>(() => Assert.Distinct(duplicates));
    }

    [Fact]
    public void CheckForValidRegularExpression()
    {
        var emailRegEx = "^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$";
        Assert.Matches(emailRegEx, "bob@test.com");
        Assert.DoesNotMatch(emailRegEx, "icantwritenemailproperly@bob@test.com");

        // Support a RegEx object
    }

    [Fact]
    public void CheckForStartOrEndOfString()
    {
        var name = "Roger Cadoret";
        Assert.StartsWith("Roger", name);
        Assert.EndsWith("Cadoret", name);

        // Works for these types
        // - String
        // - Memory<char>
        // - ReadOnlyMemory<char>
        // - Span<char>
        // - ReadOnlySpan<char>
    }

    [Fact]
    public void CheckForEquality()
    {
        // Can compare two objects for equality using the default object comparer
        Assert.Equal(new("John", 20), _students.First());

        // Use the default comparer for the type
        Assert.Equal(_students, new List<Student>(_students));

        // Use a custom comparer. Either a IEqualityComparer or a Func<T, T, bool>
        // Can compare list of objects
        Assert.Equal(_students, new List<Student>(_students), (s1, s2) => s1.Name.Equals(s2.Name));

        // Support comparer string while ignoring whitespaces
        Assert.Equal("Hello World\n", "      Hello World\r\n", ignoreLineEndingDifferences: true, ignoreAllWhiteSpace: true, ignoreWhiteSpaceDifferences: true);

        // When comparing numbers, you can specify a precision or a tolerance
        Assert.Equal(10.005, 10.0051, precision: 3);

        // For DateTime, you can specify a precision
        Assert.Equal(new DateTime(2024, 05, 20, 13, 05, 19), new DateTime(2024, 05, 20, 13, 05, 25), precision: TimeSpan.FromSeconds(10));

        // There are 50 overloads for Equal. For all types of value type, string, span, memory and also enumerable

        // Warning: do not use Equals (coming from Object class), it is not the same as Assert.Equal
    }

    [Fact]
    public void CheckIfTwoObjectsOfDifferentTypesAreEquivalent()
    {
        var student = new Student("John", 20);
        var teacher = new Teacher("John", 20, "A123");

        Assert.Equivalent(student, teacher);

        // A flag called Strict will disallow extra properties
        Assert.Throws<EquivalentException>(() => Assert.Equivalent(student, teacher, strict: true));
    }

    [Fact]
    public void ImmediatelyFail()
    {
        Assert.Throws<FailException>(() => Assert.Fail("Fast Fail"));
    }

    [Fact]
    public void CheckForBoolean()
    {
        Assert.False(false);
        Assert.True(true);

        // DO NOT use False/True when comparing two values, use Equal instead. See the message from the analyzer.
        Assert.False(0 == 1);
        Assert.True(0 == 0);
    }

    [Fact]
    public void CheckIfValueIsInRange()
    {
        // Support any type that implements IComparable
        Assert.InRange(5, 1, 10);

        // Can also supply custom IComparer<T>
        var amount1 = new Money(100, "USD");
        var amount2 = new Money(200, "USD");
        var amount3 = new Money(300, "USD");
        var comparer = new MoneyComparer();
        Assert.InRange(amount2, amount1, amount3, comparer);
        Assert.NotInRange(amount1, amount2, amount3, comparer);
    }

    [Fact]
    public void CheckIfObjectIsOfType()
    {
        object someObject = new Student("John", 20);

        // IsType
        var student = Assert.IsType<Student>(someObject);
        Assert.IsNotType<Teacher>(student);

        var castedIntoEnumerable = Assert.IsAssignableFrom<IEnumerable<Student>>(_students);
        // Still a List<Student> even if casted into IEnumerable<Student>
        var listOfStudents = Assert.IsType<List<Student>>(castedIntoEnumerable);
        Assert.IsNotAssignableFrom<IEnumerable<Teacher>>(listOfStudents);
    }

    [Fact]
    public void GroupMultipleAssertion()
    {
        Assert.Throws<MultipleException>(() =>
            Assert.Multiple(
                // We have multiple failures here, but they will stop at the first Assert exception
                () => Assert.Equal("NotJohn", _students[0].Name),
                () => Assert.Equal(99, _students[0].Age)));
    }

    [Fact]
    public void CheckForNull()
    {
        string? name = null;
        int? age = 20;

        Assert.Null(name);

        // NotNull with a nullable struct will return the struct value
        int nonNullableAge = Assert.NotNull(age);
        Assert.Equal(20, nonNullableAge);
    }

    [Fact]
    public void CheckForSet()
    {
        var set1 = new HashSet<int> { 2, 3 };
        var set2 = new HashSet<int> { 1, 2, 3, 4 };
        var set3 = new HashSet<int> { 1, 2, 3, 4 };

        Assert.Subset(set2, set1);
        Assert.ProperSubset(set2, set1);
        Assert.Superset(set1, set2);
        Assert.ProperSuperset(set1, set2);

        // https://mathworld.wolfram.com/ProperSubset.html
        // [1, 2, 3, 4] is not a proper subset or superset of [1, 2, 3, 4]
        Assert.Throws<ProperSubsetException>(() => Assert.ProperSubset(set2, set3));
        Assert.Throws<ProperSupersetException>(() => Assert.ProperSuperset(set3, set2));
    }

    [Fact]
    public void CheckForSameInstance()
    {
        // Check if two objects are the same instance (reference equality)
        Assert.Same(_students, _students);
    }

    [Fact]
    public void CheckAndReturnSingleItemOfCollection()
    {
        var students = new List<Student>
        {
            new("John", 20)
        };

        // Checks if the collection has only one item and returns that item
        var student = Assert.Single(students);
        Assert.Equal("John", student.Name);

        // Will warn to use Single
        // Assert.Equal(1, students.Count);
    }

    [Fact]
    public async Task InterceptsException()
    {
        static void DoSomethingBad() { throw new ArgumentException("Bad stuff"); }
        static Task DoSomethingBadAsync() { throw new ArgumentException("Bad stuff"); }

        // You must specify the exception type you expect to receive. Must be the exact exception.
        // Throws returns the exception that can then further be asserted
        var ex1 = Assert.Throws<ArgumentException>(() => DoSomethingBad());
        Assert.Equal("Bad stuff", ex1.Message);
        var ex2 = await Assert.ThrowsAsync<ArgumentException>(() => DoSomethingBadAsync());
        Assert.Equal("Bad stuff", ex2.Message);

        // Does not work because exception is not of type ArgumentException
        //Assert.Throws<Exception>(() => DoSomethingBad());

        // ThrowAny will catch any exception and derived
        var ex3 = Assert.ThrowsAny<Exception>(() => DoSomethingBad());
        Assert.Equal("Bad stuff", ex3.Message);
        var ex4 = await Assert.ThrowsAnyAsync<Exception>(() => DoSomethingBadAsync());
        Assert.Equal("Bad stuff", ex4.Message);
    }
}

public record Student(string Name, int Age);

public record Teacher(string Name, int Age, string EmployeeId);

public record Money(decimal Amount, string Currency);

public class MoneyComparer : IComparer<Money>
{
    public int Compare(Money? x, Money? y)
    {
        // implements null check
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        if (x.Currency != y.Currency)
            throw new InvalidOperationException("Cannot compare different currencies");

        return x.Amount.CompareTo(y.Amount);
    }
}