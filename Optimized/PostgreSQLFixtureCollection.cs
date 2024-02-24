namespace database_testing_testcontainers.Optimized;

[CollectionDefinition(nameof(PostgreSQLFixtureCollection), DisableParallelization = true)]
public class PostgreSQLFixtureCollection : ICollectionFixture<PostgreSQLFixture>
{
}