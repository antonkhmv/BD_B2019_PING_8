using System;
using System.Collections.Generic;
using System.Linq;
using LibraryDB.Domain;

namespace LibraryDB.Application
{
    public class ReaderService
    {
        private readonly LibraryDbContext _db;

        public ReaderService(LibraryDbContext db)
        {
            _db = db;
        }
        
        public List<Reader> GetAllReaders()
            => _db.Readers.ToList();
        
        /// <summary>
        /// Получение читателя по его
        /// идентификационному номеру.
        /// </summary>
        /// <param name="readerNum">
        /// идентификационный номер</param>
        /// <returns>возвращает читателя с
        /// Id равным readerNum</returns>
        public Reader FindReaderByReaderNumber(int readerNum)
            => _db.Readers.FirstOrDefault(o => o.Id == readerNum)!;

        /// <summary>
        /// Добавление объекта класса Reader
        /// в таблицу из базы данных.
        /// </summary>
        /// <param name="reader">объект
        /// класса Reader</param>
        /// <returns>возвращает true,
        /// если операция прошла успешно</returns>
        private bool AddNewReader(Reader reader)
        {
            try
            {
                _db.Readers.Add(reader);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Создание нового объекта класса Reader
        /// для добавления его в таблицу.
        /// </summary>
        /// <returns>возвращает true, если операция
        /// добавления прошла успешно</returns>
        public bool AddNewReader(int id, string lastName, string firstName,
            string address, DateTime birthDate)
        {
            var res = _db.Readers.FirstOrDefault(o => o.Id == id);
            if (res != null)
                return false;
            Reader reader = new()
            {
                Id = id,
                LastName = lastName,
                FirstName = firstName,
                Address = address,
                BirthDate = birthDate
            };
            return AddNewReader(reader);
        }

        /// <summary>
        /// Удаление читатетя по его идентификационному
        /// номеру.
        /// </summary>
        public void DeleteReaderById(int id)
        {
            var res = (from reader in _db.Readers
                where reader.Id == id
                select reader).ToList();

            if (res == null || res.Count <= 0)
                return;
            foreach (var reader in res)
            {
                _db.Readers.Remove(reader);
            }

            _db.SaveChanges();
        }
    }
}