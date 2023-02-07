using Blog.Models;
using Blog.Repositories;
using Microsoft.Data.SqlClient;

namespace Blog
{
    class Program
    {
        private const string CONNECTION_STRING = @"server=localhost; database=Blog; integrated Security=True; TrustServerCertificate=True";

        static void Main(string[] args)
        {
            using var connection = new SqlConnection(CONNECTION_STRING);
            var userRepository = new Repository<User>(connection);
            var roleRepository = new Repository<Role>(connection);
            var categoryRepository = new Repository<Category>(connection);
            var postRepository = new Repository<Post>(connection);
            var tagRepository = new Repository<Tag>(connection);
            var userRoleRepository = new Repository<UserRole>(connection);
            var postTagRepository = new Repository<PostTag>(connection);

            string opcao = string.Empty;
            bool exibirMenu = true;

            while (exibirMenu)
            {
                Console.Clear();
                Console.WriteLine("Digite sua opçõao");
                Console.WriteLine(" 1 - Cadastrar Usuário");
                Console.WriteLine(" 2 - Cadastrar Perfil");
                Console.WriteLine(" 3 - Cadastrar Categoria");
                Console.WriteLine(" 4 - Cadastrar Tag");
                Console.WriteLine(" 5 - Cadastrar Post");
                Console.WriteLine(" 6 - Vincular Usuário a Perfil");
                Console.WriteLine(" 7 - Vincular Post a Tag");
                Console.WriteLine(" 8 - Encontrar Usuários e seus perfis");
                Console.WriteLine(" 9 - Exibir usuáios");
                Console.WriteLine("10 - Exibe tags e seus posts");
                Console.WriteLine("11 - Exibe quantos posts cada categoria tem");
                Console.WriteLine("00 - Encerrar Interface");

                switch (Console.ReadLine())
                {
                    case "1":
                        CreateUser(userRepository);
                        break;
                    case "2":
                        CreateRole(roleRepository);
                        break;
                    case "3":
                        CreateCategory(categoryRepository);
                        break;
                    case "4":
                        CreateTag(tagRepository);
                        break;
                    case "5":
                        CreatePost(postRepository);
                        break;
                    case "6":
                        CreateUserRole(userRoleRepository);
                        break;
                    case "7":
                        CreatePostTag(postTagRepository);
                        break;
                    case "8":
                        ReadUserWithRole(connection);
                        break;
                    case "9":
                        ReadUsers(userRepository);
                        break;
                    case "10":
                        ReadTagWithPost(connection);
                        break;
                    case "11":
                        ReadCategoryWithPost(connection);
                        break;
                    case "00":
                        exibirMenu = false;
                        break;

                    default: Console.WriteLine("Opção inválida"); break;
                }
                Console.WriteLine("Pressione uma tecla para continuar");
                Console.ReadLine();
            }
            Console.WriteLine("O progama se encerrou");

            ReadCategoryWithPost(connection);
        }

        private static void CreateUser(Repository<User> repository)
        {


            Console.WriteLine("Insira a Bio");
            string bio = Console.ReadLine();

            Console.WriteLine("Insira o e-mail");
            string email = Console.ReadLine();

            Console.WriteLine("Insira a imagem");
            string image = Console.ReadLine();

            Console.WriteLine("Insira o nome");
            string name = Console.ReadLine();

            Console.WriteLine("Insira o slug");
            string slug = Console.ReadLine();

            Console.WriteLine("Insira a senha");
            string senha = Console.ReadLine();

            var user = new User
            {
                Bio = bio,
                Email = email,
                Image = image,
                Name = name,
                Slug = slug,
                PasswordHash = senha
            };

            repository.Create(user);
        }

        private static void CreateRole(Repository<Role> repository)
        {
            Console.WriteLine("Insira o nome");
            string name = Console.ReadLine();

            Console.WriteLine("Insira o slog");
            string slug = Console.ReadLine();

            var role = new Role
            {
                Name = name,
                Slug = slug,
            };

            repository.Create(role);
        }

        private static void CreateCategory(Repository<Category> repository)
        {
            Console.WriteLine("Insira o nome");
            string name = Console.ReadLine();

            Console.WriteLine("Insira o slog");
            string slug = Console.ReadLine();

            var category = new Category
            {
                Name = name,
                Slug = slug,
            };

            repository.Create(category);
        }

        private static void CreateUserRole(Repository<UserRole> repository)
        {
            Console.WriteLine("Insira o id do Usuário");
            string userId = Console.ReadLine();

            Console.WriteLine("Insira o id do perfil");
            string roleId = Console.ReadLine();

            var role = new UserRole
            {
                UserId = int.Parse(userId),
                RoleId = int.Parse(roleId),
            };

            repository.Create(role);
        }

        private static void CreatePostTag(Repository<PostTag> repository)
        {
            Console.WriteLine("Insira o id do Post");
            string postId = Console.ReadLine();

            Console.WriteLine("Insira o id da tag");
            string tagId = Console.ReadLine();

            var postTag = new PostTag
            {
                PostId = int.Parse(postId),
                TagId = int.Parse(tagId),
            };

            repository.Create(postTag);
        }

        private static void CreateTag(Repository<Tag> repository)
        {

            Console.WriteLine("Insira o id do Usuário");
            string name = Console.ReadLine();

            Console.WriteLine("Insira o id do perfil");
            string slug = Console.ReadLine();

            var tag = new Tag
            {
                Name = name,
                Slug = slug,
            };

            repository.Create(tag);
        }

        private static void CreatePost(Repository<Post> repository)
        {
            Console.WriteLine("Insira o id do autor");
            string author = Console.ReadLine();

            Console.WriteLine("Insira o id da categoria");
            string category = Console.ReadLine();

            Console.WriteLine("Insira o slug");
            string slug = Console.ReadLine();

            Console.WriteLine("Insira o Body");
            string body = Console.ReadLine();

            Console.WriteLine("Insira o Sumary");
            string summary = Console.ReadLine();

            Console.WriteLine("Insira o Titulo");
            string tittle = Console.ReadLine();

            var post = new Post
            {
                AuthorId = int.Parse(author),
                CategoryId = int.Parse(category),
                Slug = slug,
                Body = body,
                Summary = summary,
                Title = tittle,
                LastUpdateDate = DateTime.Now,
            };

            repository.Create(post);
        }

        private static void ReadUsers(Repository<User> repository)
        {
            var users = repository.Read();
            foreach (var item in users)
                Console.WriteLine(item.Email);
        }

        private static void ReadUser(Repository<User> repository)
        {
            var user = repository.Read(2);
            Console.WriteLine(user?.Email);
        }

        private static void UpdateUser(Repository<User> repository)
        {
            var user = repository.Read(2);
            user.Email = "hello@balta.io";
            repository.Update(user);

            Console.WriteLine(user?.Email);
        }

        private static void DeleteUser(Repository<User> repository)
        {
            var user = repository.Read(2);
            repository.Delete(user);
        }

        private static void ReadUserWithRole(SqlConnection connection)
        {
            var repository = new UserRepository(connection);
            var users = repository.ReadWithRole();

            foreach (var user in users)
            {
                Console.WriteLine($"{user.Name}, {user.Email}");
                foreach (var role in user.Roles) Console.WriteLine($" - {role.Slug}, ");
            }
        }

        private static void ReadCategoryWithPost(SqlConnection connection)
        {
            var repository = new CategoryRepository(connection);
            var category = repository.ReadWithPost();

            foreach (var item in category)
            {
                Console.WriteLine($"{item.Name} tem {item.PostsCount} posts");            
            }

        }

        private static void ReadTagWithPost(SqlConnection connection)
        {
            var repository = new TagRepository(connection);
            var tags = repository.ReadWithPost();

            foreach (var tag in tags)
            {
                Console.WriteLine($"{tag.Name}, tem {tag.Posts.Count} posts ");
            }
        }
    }
}
