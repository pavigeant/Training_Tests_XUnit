// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Usage",
    "xUnit1042:The member referenced by the MemberData attribute returns untyped data rows",
    Justification = "TheoryData<T> only works starting with C# 11 (.net 7) which introduce generic attributes",
    Scope = "namespaceanddescendants",
    Target = "~N:Training_Tests_XUnit")]
