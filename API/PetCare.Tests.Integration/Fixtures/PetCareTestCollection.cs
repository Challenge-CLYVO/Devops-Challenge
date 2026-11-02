using Xunit;

namespace PetCare.Tests.Integration.Fixtures;

[CollectionDefinition("PetCare Collection")]
public class PetCareTestCollection
    : ICollectionFixture<PetCareApiFactory>
{
}