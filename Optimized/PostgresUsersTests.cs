namespace database_testing_testcontainers.Optimized;

[Collection(nameof(PostgreSQLFixtureCollection))]
public class PostgresUsersTests(PostgreSQLFixture fixture) : IAsyncLifetime
{
    [Fact]
    public async Task NameBy_WhenUserDoesNotExist_ReturnsNull()
    {
        await using var connection = await fixture.ConnectionFactory();
        await connection.ExecuteAsync("INSERT INTO users (id, name) VALUES (1, 'John Doe')");

        var sut = new PostgresUsers(fixture.ConnectionFactory);
        var name = await sut.NameBy(2);

        name.Should().BeNull();
    }

    [Fact]
    public async Task NameBy_WhenUserExists_ReturnsName()
    {
        await using var connection = await fixture.ConnectionFactory();
        await connection.ExecuteAsync("INSERT INTO users (id, name) VALUES (1, 'John Doe')");

        var sut = new PostgresUsers(fixture.ConnectionFactory);
        var name = await sut.NameBy(1);

        name.Should().Be("John Doe");
    }

    public async Task InitializeAsync()
    {
        await using var connection = await fixture.ConnectionFactory();
        await connection.ExecuteAsync("CREATE TABLE users (id BIGSERIAL PRIMARY KEY, name TEXT)");
    }

    public async Task DisposeAsync()
    {
        await using var connection = await fixture.ConnectionFactory();
        await connection.ExecuteAsync("DROP TABLE users");
    }
}