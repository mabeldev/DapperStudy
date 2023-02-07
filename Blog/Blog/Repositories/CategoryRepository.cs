using Blog.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Blog.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        private readonly SqlConnection _connection;

        public CategoryRepository(SqlConnection connection) : base(connection)
            => _connection = connection;

        public IEnumerable<Category> ReadWithPost()
        {
            var query = @"
        SELECT
            [Category].Name,
            COUNT([Post].Id) as PostsCount
        FROM
            [Post]
            LEFT JOIN [Category] ON [Post].[CategoryId] = [Category].[Id]
        GROUP BY [Category].Name";

            var categories = _connection.Query<Category>(query);
            return categories;
        }
    }
}