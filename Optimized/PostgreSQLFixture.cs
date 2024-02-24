namespace database_testing_testcontainers.Optimized;

public sealed class PostgreSQLFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder().Build();

    public Func<ValueTask<NpgsqlConnection>> ConnectionFactory = default!;
    public string ConnectionString { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        ConnectionString = _postgres.GetConnectionString();
        var dataSource = new NpgsqlDataSourceBuilder(ConnectionString).Build();
        ConnectionFactory = () => dataSource.OpenConnectionAsync();
    }

    public Task DisposeAsync() => _postgres.DisposeAsync().AsTask();
}