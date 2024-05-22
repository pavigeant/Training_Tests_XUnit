using Xunit.Categories;

namespace Training_Tests_XUnit;

public class CategoryShould
{
    // Xunit.Categories namespace (should be placed in global using file)
    // 3 million downloads

    [Fact]
    [Trait("Category", "Unit")] // Way of setting a category when only using XUnit
    [DatabaseTest]
    [IntegrationTest]
    [LocalTest]
    [SnapshotTest]
    [SystemTest]
    [UnitTest]
    [Author("Pierre-Alain")] // Author should not be used as the whole team is responsible for the test and source control already tracks the author
    [Bug(id: 1234)] // This is a bug number in Azure DevOps Services
    [Category("SpecialCategory")] // This is a custom category
    [Description("This test is a sample of how to use the XUnit.Categories library")]
    [Documentation(workItemId: 1234)] // This is a user story that documents this test
    [Expensive] // Marks a test as expensive to run
    [Exploratory(workItemId: 1234)] // Marks a test as exploratory. See https://en.wikipedia.org/wiki/Exploratory_testing
    [Feature("FeatureName")] // This test is related to a feature
    [KnownBug(id: 1234)] // This is a known bug number in Azure DevOps Services
    [Specification(id: "ADR-04")] // This is a specification id or name
    [TestCase("TC-1234")] // This is a test case name or id. Can refer to an id in Azure DevOps Services
    [WorkItem(workItemId: 1234)] // This is a work item id in Azure DevOps Services

    public void NeverHaveEnoughAttributes()
    {
    }
}
