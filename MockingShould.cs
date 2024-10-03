using Moq;
using Moq.Protected;

namespace Training_Tests_XUnit;

public class MockingShould
{
    // Mocking only works on Interface, abstract classes or
    // can be used with virtual methods of concrete classes.
    // Anything else or a sealed class will not work.

    [Fact]
    public void CreateAFakeObjectWhichDoesNothing()
    {
        // Use Mock<IStudentService>() to create a mock object
        // that can be configured to return specific values.

        // Use Mock.Of<IStudentService>() to return a mocked or
        // fake object that does nothing by default.
        var fake = Mock.Of<IStudentService>();

        // By default, the fake object will return the default value of the return type
        var student = fake.GetStudent(1);
        Assert.Null(student);
    }

    [Fact]
    public void ObtainTheMockFromAFake()
    {
        var fake = Mock.Of<IStudentService>();

        // Obtain the mock object from the fake object
        var mock = Mock.Get(fake);

        // The mock object is the same as the fake object
        Assert.Equal(fake, mock.Object);

        // or create a new mock object directly
        var newMock = new Mock<IStudentService>();

        // The new mock object is not the same as the fake object
        Assert.NotEqual(fake, newMock.Object);
    }

    [Fact]
    public void ConfigureAFakeToReturnAValue()
    {
        var mock = new Mock<IStudentService>();

        // In the setup, we specify the method to be called and the return value
        // The method arguments should also be specific, if you know the values
        mock.Setup(x => x.GetStudent(1))
            .Returns(new Student(1, "John", 25));

        // You can also specify that the call will throw an exception
        // Note that the same method can be setup multiple times, as long
        // as the arguments are different.
        // Also, if there are any optional arguments, you must specify them
        // in the setup
        mock.Setup(x => x.GetStudent(2))
            .Throws<IndexOutOfRangeException>();

        var student = mock.Object.GetStudent(1);
        Assert.NotNull(student);
        Assert.Equal("John", student.Name);

        Assert.Throws<IndexOutOfRangeException>(() => mock.Object.GetStudent(2));
    }

    [Fact]
    public void ConfigureAFakeToReturnASequenceOfValues()
    {
        var mock = new Mock<IStudentService>();
        // We can simulate multiple calls to the same method
        mock.SetupSequence(x => x.GetStudent(1))
            .Returns(new Student(1, "John", 25))
            .Returns(new Student(2, "Jane", 24));

        var first = mock.Object.GetStudent(1);
        var second = mock.Object.GetStudent(1);

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Equal("John", first.Name);
        Assert.Equal("Jane", second.Name);

        Assert.Null(mock.Object.GetStudent(1));
    }

    [Fact]
    public void AllowValidationOnTheArgument()
    {
        var mock = new Mock<IStudentService>();

        // If you don't care about the parameter value, you can use It.IsAny<T>()
        mock.Setup(x => x.GetStudent(It.IsAny<int>()))
            .Returns(new Student(1, "John", 25));

        mock.Setup(x => x.GetStudent(It.IsIn(1, 2, 3)))
            .Returns(new Student(2, "Jane", 25));

        // Other options are It.Is<T>(predicate), It.IsInRange<T>(from, to, rangeKind)
        // It.IsIn(values), It.IsNotIn(values), It.IsNotNull<T>() and It.IsRegex(pattern)

        // This will return John
        var student = mock.Object.GetStudent(2);
        Assert.NotNull(student);
        Assert.Equal("Jane", student.Name);

        // This will also return John
        student = mock.Object.GetStudent(12345);
        Assert.NotNull(student);
        Assert.Equal("John", student.Name);
    }

    [Fact]
    public void SupportCallback()
    {
        var mock = new Mock<IStudentService>();

        var callBefore = 0;
        var callAfter = 0;
        // You can use a callback to return a value based on the input
        mock.Setup(x => x.Process())
            // Callback is invoked before the method is called
            .Callback(() => callBefore++)
            .Returns(1)
            // And this one is called after, if it is successful
            .Callback(() => callAfter++);

        _ = mock.Object.Process();
        _ = mock.Object.Process();
        _ = mock.Object.Process();

        Assert.Equal(3, callBefore);
        Assert.Equal(3, callAfter);
    }

    [Fact]
    public void VerifyThatAMethodWasInvoked()
    {
        var mock = new Mock<IStudentService>();

        _ = mock.Object.GetStudent(1);

        // Verify that the method was called
        // A previous setup is not required to verify a method
        // Times allows you to specify the number of times the method should be called
        // It supports Once, AtLeastOnce, AtMost, AtMostOnce, Exactly, Between and Never
        mock.Verify(x => x.GetStudent(1), Times.Once);

        // You can also verify that the method was not called
        mock.Verify(x => x.GetStudent(2), Times.Never);

        // Or you can verify that the mock was not used for anything else
        mock.VerifyNoOtherCalls();
    }

    [Fact]
    public void VerifyThatOnlySetupMethodsWereInvoked()
    {
        var mock = new Mock<IStudentService>();

        mock.Setup(x => x.GetStudent(99))
            .Returns(new Student(1, "John", 25))
            .Verifiable();

        _ = mock.Object.GetStudent(99);

        mock.Verify();
    }

    [Fact]
    public void VerifyAProtectedMethodWasInvoked()
    {
        var mock = new Mock<StudentService>();

        // Protected methods can be verified using the Protected() method
        mock.Protected()
            .Setup<int>("ProcessCore")
            .Returns(1);

        _ = mock.Object.Process();

        mock.Protected().Verify("ProcessCore", Times.Once());

        // The reason to test a protected method is it is designed to be consumed by
        // an inheriting class. If you have a protected method that is not to be called by
        // an inheriting class, then it is a good candidate for removal or conversion to private.
    }
}

public interface IStudentService
{
    Student? GetStudent(int id);

    int Process();
}

public class StudentService : IStudentService
{
    public Student? GetStudent(int id) => null;

    public int Process() => ProcessCore() + 1;

    protected virtual int ProcessCore() => 0;
}
