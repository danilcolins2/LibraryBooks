using LibraryBooks.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooks.Repositories
{
    public interface IBorrowRepository : ILibraryContext
    {
        IEnumerable<Borrow> GetAllBorrows();
        Borrow? GetBorrowById(int borrowId);
        void AddBorrow(Borrow borrow);
        void UpdateBorrow(int borrowId, Borrow borrow);
        bool DeleteBorrow(int borrowId);

        bool SetReturnedDate(int borrowId, DateTime? date);
    }

    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibraryContext _context;

        public BorrowRepository(LibraryContext context) => _context = context;

        public void AddBorrow(Borrow borrow)
        {
            _context.Borrows.Add(borrow);
            _context.SaveChanges();
        }

        public bool DeleteBorrow(int borrowId)
        {
            var borrow = _context.Borrows.FirstOrDefault(x => x.Id == borrowId);
            if (borrow != null)
            {
                _context.Borrows.Remove(borrow);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Borrow> GetAllBorrows()
        {
            return _context.Borrows.Include(b => b.Book).Include(b => b.User);
        }

        public Borrow? GetBorrowById(int borrowId)
        {
            return _context.Borrows.FirstOrDefault(b => b.Id == borrowId);
        }

        public bool SetReturnedDate(int borrowId, DateTime? date)
        {
            var b = GetBorrowById(borrowId);
            if(b != null)
            {
                b.ReturnDate = date;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void UpdateBorrow(int borrowId, Borrow borrow)
        {
            var existingBorrow = GetBorrowById(borrowId);
            if (existingBorrow != null)
            {
                _context.Borrows.Remove(existingBorrow);
                _context.Borrows.Add(borrow);
            }
            _context.SaveChanges();
        }
        public LibraryContext GetLibraryContext()
        {
            return _context;
        }
    }
}
