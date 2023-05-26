using UserManagementRepository.Entity;

namespace UserManagementRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDBContext _context;
        public UserRepository(UserDBContext context)
        {
            _context = context;
        }
        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);        }

        public User Login(string username, string password)
        {
           return _context.Users.Where(a=>a.Username==username & a.Password== password).FirstOrDefault();
        }
    }

    public interface IUserRepository
    {
        User GetUserById(int userId);
        User Login(string username, string password);
    }
}