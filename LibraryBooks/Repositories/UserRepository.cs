using LibraryBooks.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooks.Repositories
{
    public interface IUserRepository : ILibraryContext
    {
        IEnumerable<User> GetAllUsers();
        User? GetUserById(int userId);

        User? GetUserByLogin(string login);

        void AddUser(User user);
        void UpdateUser(int userId, User user);
        void UpdateImage(int userId, ImageFile imageFile);
        bool DeleteUser(int userId);



    }
    public class UserRepository : IUserRepository
    {
        private readonly LibraryContext _context;

        public UserRepository(LibraryContext context) => _context = context;

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool DeleteUser(int userId)
        {
            var user = GetUserById(userId);
            if(user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.Include(u => u.ImageFile);
        }

        public User? GetUserById(int userId)
        {
            return _context.Users.Include(u => u.ImageFile).FirstOrDefault(u => u.Id == userId);
        }

        public void UpdateUser(int userId, User user)
        {
            var existingUser = GetUserById(userId);
            if(existingUser != null)
            {
                existingUser.Update(user);
                _context.SaveChanges();
            }
        }
        public LibraryContext GetLibraryContext()
        {
            return _context;
        }

        public User? GetUserByLogin(string login)
        {
            return _context.Users.Include(u => u.ImageFile).FirstOrDefault(u => u.Login == login);
        }

        public void UpdateImage(int userId, ImageFile imageFile)
        {
            var existingUser = GetUserById(userId);
            if (existingUser != null)
            {
                existingUser.ImageFileId = imageFile.Id;
                _context.SaveChanges();
            }
        }
    }
}
