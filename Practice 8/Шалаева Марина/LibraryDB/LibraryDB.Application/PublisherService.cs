using System.Collections.Generic;
using System.Linq;
using LibraryDB.Domain;

namespace LibraryDB.Application
{
    public class PublisherService
    {
        private readonly LibraryDbContext _db;

        public PublisherService(LibraryDbContext db)
        {
            _db = db;
        }

        public List<Publisher> GetAllPublishers()
            => _db.Publishers.ToList();
    }
}