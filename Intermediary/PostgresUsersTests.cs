namespace database_testing_testcontainers.Intermediary;

public class PostgresUsersTests(PostgreSQLFixture fixture) : IClassFixture<PostgreSQLFixture>
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
}