namespace database_testing_testcontainers;

public class PostgresUsers(Func<ValueTask<NpgsqlConnection>> connectionFactory)
{
    public async Task<string?> NameBy(int userId)
    {
        await using var connection = await connectionFactory();
        return await connection.QueryFirstOrDefaultAsync<string>("SELECT name FROM users WHERE id = @Id", new {Id = userId});
    }
}