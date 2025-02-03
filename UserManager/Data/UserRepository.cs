using UserManager.Models;
using UserManager.Services;

namespace UserManager.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User? GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public User? GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            user.Password = _passwordHasher.HashPassword(user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool ValidateUser(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return false;

            return _passwordHasher.VerifyPassword(password, user.Password);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
