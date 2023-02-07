using Blog.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;

namespace Blog.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        private readonly SqlConnection _connection;

        public TagRepository(SqlConnection connection) : base(connection)
            => _connection = connection;

        public List<Tag> ReadWithPost()
        {
            var query = @"
                SELECT
                    [Tag].*,
                    [Post].*
                FROM
                    [Tag]
                    LEFT JOIN [PostTag] ON [PostTag].[TagId] = [Tag].[Id]
                    LEFT JOIN [Post] ON [PostTag].[PostId] = [Post].[Id]";

            var users = new List<Tag>();
            var items = _connection.Query<Tag, Post, Tag>(
                query,
                (user, role) =>
                {
                    var usr = users.FirstOrDefault(x => x.Id == user.Id);
                    if (usr == null)
                    {
                        usr = user;
                        if (role != null)
                            usr.Posts.Add(role);
                        users.Add(usr);
                    }
                    else
                        usr.Posts.Add(role);

                    return user;
                }, splitOn: "Id");
            return users;
        }
    }
}