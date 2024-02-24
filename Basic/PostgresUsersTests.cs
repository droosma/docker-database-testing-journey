namespace database_testing_testcontainers.Basic;

public class PostgresUsersTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder().Build();

    [Fact]
    public async Task NameBy_WhenUserExists_ReturnsName()
    {
        var connectionString = _postgres.GetConnectionString();

        var dataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
        var connectionFactory = () => dataSource.OpenConnectionAsync();

        await using var connection = await connectionFactory();
        await connection.ExecuteAsync("CREATE TABLE users (id BIGSERIAL PRIMARY KEY, name TEXT)");
        await connection.ExecuteAsync("INSERT INTO users (id, name) VALUES (1, 'John Doe')");

        var sut = new PostgresUsers(connectionFactory);
        var name = await sut.NameBy(1);

        name.Should().Be("John Doe");
    }

    public async Task InitializeAsync() => await _postgres.StartAsync();
    public Task DisposeAsync() => _postgres.DisposeAsync().AsTask();
}