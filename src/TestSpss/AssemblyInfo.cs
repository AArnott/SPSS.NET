using Xunit;

// We can't handle tests executing concurrently because they try to write to the same file.
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
