using System;
using System.Collections.Generic;
using LibraryDB.Application;
using LibraryDB.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LibraryDB.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            do
            {
                ConsoleApplication();
                Console.WriteLine("Для выхода нажмите Escape.\n" +
                                  "Для продолжения нажмите любую кнопку.\n");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            // CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        private static void ConsoleApplication()
        {
            Console.Clear();
            Console.WriteLine("Список команд:\n" +
                              "ESCAPE - Выход из приложения\n" +
                              "Q (Й) - Вывод списка всех книг\n" +
                              "W (Ц) - Вывод списка всех экземпляров книг\n" +
                              "E (У) - Вывод списка всех бронирований\n" +
                              "R (К) - Вывод списка всех издательств\n" +
                              "T (Е) - Вывод списка всех читателей\n" +
                              "Y (Н) - Добавление новой книги\n" +
                              "U (Г) - Добавление нового экземпляра (копии) книги\n" +
                              "I (Ш) - Добавление нового бронирования\n" +
                              "O (Щ) - Удаление книги\n" +
                              "P (З) - Удаление экземпляра (копии) книги\n" +
                              "A (Ф) - Удаление бронирования\n" +
                              "S (Ы) - Изменение информации о книге\n" +
                              "D (В) - Изменение информации об экземпляре (копии) книги\n" +
                              "F (А) - Нахождение конкретной книги\n" +
                              "G (П) - Нахождение конкретного экземпляра книги\n");

            var context = new LibraryDbContext();
            var bookService = new BookService(context);
            var borrowingService = new BorrowingService(context);
            var copyService = new CopyService(context);
            var publisherService = new PublisherService(context);
            var readerService = new ReaderService(context);

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    PrintBooks(bookService.GetAllBooks());
                    break;
                case ConsoleKey.W:
                    PrintCopies(copyService.GetAllCopies());
                    break;
                case ConsoleKey.E:
                    PrintBorrowings(borrowingService.GetAllBorrowings());
                    break;
                case ConsoleKey.R:
                    PrintPublishers(publisherService.GetAllPublishers());
                    break;
                case ConsoleKey.T:
                    PrintReaders(readerService.GetAllReaders());
                    break;
                case ConsoleKey.Y:
                    AddNewBook(bookService, publisherService);
                    break;
                case ConsoleKey.U:
                    AddNewCopy(bookService, copyService);
                    break;
                case ConsoleKey.I:
                    AddNewBorrowing(borrowingService, copyService, readerService);
                    break;
                case ConsoleKey.O:
                    DeleteBook(bookService);
                    break;
                case ConsoleKey.P:
                    DeleteCopy(copyService);
                    break;
                case ConsoleKey.A:
                    DeleteBorrowing(borrowingService, readerService, copyService, bookService);
                    break;
                case ConsoleKey.S:
                    UpdateBook(bookService);
                    break;
                case ConsoleKey.D:
                    UpdateCopy(copyService);
                    break;
                case ConsoleKey.F:
                    FindBook(bookService);
                    break;
                case ConsoleKey.G:
                    FindCopy(copyService);
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }

        private static void PrintBooks(IEnumerable<Book> list)
        {
            foreach (var book in list)
                Console.WriteLine(book.ToString());
        }

        private static void PrintCopies(IEnumerable<Copy> list)
        {
            foreach (var copy in list)
                Console.WriteLine(copy.ToString());
        }

        private static void PrintBorrowings(IEnumerable<Borrowing> list)
        {
            foreach (var borrowing in list)
                Console.WriteLine(borrowing.ToString());
        }

        private static void PrintPublishers(IEnumerable<Publisher> list)
        {
            foreach (var publisher in list)
                Console.WriteLine(publisher.ToString());
        }

        private static void PrintReaders(IEnumerable<Reader> list)
        {
            foreach (var reader in list)
                Console.WriteLine(reader.ToString());
        }

        private static void AddNewBook(BookService bookService, PublisherService publisherService)
        {
            Console.WriteLine("Формат ввода: ISBN, Title, Author, PagesNum, PubYear, PubName\n" +
                              "Пример:\n" +
                              "467495, her, Richard Hutchinson, 14, 1985, happy\n" +
                              "После ввода всех полей через пробел и запятую нажмите Enter.\n" +
                              "Имя издателя (последнее поле) должно быть взято из базы данных,\n" +
                              "так что в случае, если вы хотите посмотреть список издательств,\n" +
                              "нажмите R (в русской раскладке К). Если хотите продолжить без\n" +
                              "просмотра списка, введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.R)
                PrintPublishers(publisherService.GetAllPublishers());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 6)
            {
                Console.WriteLine("Проверьте количество вводимых полей! Их должно быть ровно 6");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                bookService.AddNewBook(int.Parse(str[0]), str[1], str[2],
                    int.Parse(str[3]), int.Parse(str[4]), str[5]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("При добавлении объекта в базу данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void AddNewCopy(BookService bookService, CopyService copyService)
        {
            Console.WriteLine("Формат ввода: ISBN, shelfPosition\n" +
                              "Пример:\n" +
                              "467495, 121\n" +
                              "После ввода полей через пробел и запятую нажмите Enter.\n" +
                              "Номер копии будет посчитан автоматически.\n" +
                              "ISBN книги должно быть взято из базы данных,\n" +
                              "так что в случае, если вы хотите посмотреть список книг,\n" +
                              "нажмите Q (в русской раскладке Й). Если хотите продолжить без\n" +
                              "просмотра списка, введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.Q)
                PrintBooks(bookService.GetAllBooks());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 2)
            {
                Console.WriteLine("Проверьте количество вводимых полей! Их должно быть ровно 2");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                copyService.AddNewCopy(int.Parse(str[0]), int.Parse(str[1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("При добавлении объекта в базу данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void AddNewBorrowing(BorrowingService borrowingService, CopyService copyService,
            ReaderService readerService)
        {
            Console.WriteLine("Формат ввода: ReaderNumber, ISBN, CopyNumber, ReturnDate\n" +
                              "Пример:\n" +
                              "10, 467495, 121, 17.07.2022\n" +
                              "После ввода полей через пробел и запятую нажмите Enter.\n" +
                              "Номер читателя ReaderNumber и ISBN книги должны быть взяты из базы данных,\n" +
                              "так что в случае, если вы хотите посмотреть список книг,\n" +
                              "нажмите W (в русской раскладке Ц), если хотите посмотреть\n" +
                              "список читателей, нажмите T (в рус. раскладе Е).\n" +
                              "Можно просмотреть оба списка по очереди.\n" +
                              "Если хотите продолжить без просмотра списков, то введите любую букву.\n");

            for (int i = 0; i < 2; i++)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.W)
                    PrintCopies(copyService.GetAllCopies());
                else if (Console.ReadKey(true).Key == ConsoleKey.E)
                    PrintReaders(readerService.GetAllReaders());
                else
                    break;
            }

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 4)
            {
                Console.WriteLine("Проверьте количество вводимых полей! Их должно быть ровно 4");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                borrowingService.AddNewBorrowing(int.Parse(str[0]), int.Parse(str[1]),
                    int.Parse(str[2]), DateTime.ParseExact(str[3], "dd.MM.yyyy",
                        System.Globalization.CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Console.WriteLine("При добавлении объекта в базу данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void DeleteBook(BookService bookService)
        {
            Console.WriteLine("Для удаления книги введите ее ISBN.\n" +
                              "После ввода нажмите Enter.\n" +
                              "ISBN книги должен существовать в базе данных,\n" +
                              "так что в случае, если вы хотите посмотреть список книг,\n" +
                              "нажмите Q (в русской раскладке Й).\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n" +
                              "БУДЬТЕ ВНИМАТЕЛЬНЫ! После удаления книги все ее копии и\n" +
                              "бронирования, содержащие информацию о книге, будут также удалены!\n");

            if (Console.ReadKey(true).Key == ConsoleKey.Q)
                PrintBooks(bookService.GetAllBooks());

            int isbn;
            while (!int.TryParse(Console.ReadLine(), out isbn))
                Console.WriteLine("Необходимо ввести одно число!");

            try
            {
                bookService.DeleteBookByIsbn(isbn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void DeleteCopy(CopyService copyService)
        {
            Console.WriteLine("Для удаления экземпляра книги введите ее ISBN и номер.\n" +
                              "Формат ввода: ISBN, CopyNumber\n" +
                              "Пример:\n" +
                              "467495, 1\n" +
                              "После ввода через запятую и пробел нажмите Enter.\n" +
                              "ISBN и номер копии книги должны существовать в базе данных,\n" +
                              "так что в случае, если вы хотите посмотреть список экземпляров книг,\n" +
                              "нажмите W (в русской раскладке Ц).\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n" +
                              "БУДЬТЕ ВНИМАТЕЛЬНЫ! После удаления копии книги все\n" +
                              "бронирования, содержащие информацию о данном экземпляре,\n" +
                              "будут удалены!\n");

            if (Console.ReadKey(true).Key == ConsoleKey.Q)
                PrintCopies(copyService.GetAllCopies());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 2)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 2");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                copyService.DeleteCopyByIsbnAndCopyNumber(int.Parse(str[0]), int.Parse(str[1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void DeleteBorrowing(BorrowingService borrowingService, ReaderService readerService,
            CopyService copyService, BookService bookService)
        {
            Console.WriteLine("Нажмите:\n" +
                              "Q (Й) для удаления бронирований конкретного читателя\n" +
                              "W (Ц) для удаления бронирований с конкретной книгой\n" +
                              "E (У) для удаления бронирований с конкретным экземпляром конкретной книги\n" +
                              "R (К) для удаления бронирования по трем конкретным полям (ISBN книги,\n" +
                              "номер копии и номер читателя)\n");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    DeleteBorrowingByReader(borrowingService, readerService);
                    break;
                case ConsoleKey.W:
                    DeleteBorrowingByBook(borrowingService, bookService);
                    break;
                case ConsoleKey.E:
                    DeleteBorrowingByCopy(borrowingService, copyService);
                    break;
                case ConsoleKey.R:
                    DeleteBorrowingByHoleKey(borrowingService, copyService, readerService);
                    break;
            }
        }

        private static void DeleteBorrowingByReader(BorrowingService borrowingService,
            ReaderService readerService)
        {
            Console.WriteLine("Формат ввода: ReaderId\n" +
                              "Пример:\n" +
                              "12\n" +
                              "После ввода нажмите Enter.\n" +
                              "Номер читателя должен существовать в базе данных,\n" +
                              "так что в случае, если вы хотите посмотреть список читателей,\n" +
                              "нажмите T (в русской раскладке Е).\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.T)
                PrintReaders(readerService.GetAllReaders());

            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
                Console.WriteLine("Необходимо ввести одно число!");

            try
            {
                borrowingService.DeleteBorrowingByReader(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void DeleteBorrowingByBook(BorrowingService borrowingService, BookService bookService)
        {
            Console.WriteLine("Формат ввода: ISBN\n" +
                              "Пример:\n" +
                              "43234\n" +
                              "После ввода нажмите Enter.\n" +
                              "ISBN книги должно существовать в базе данных,\n" +
                              "так что в случае, если вы хотите посмотреть список книг,\n" +
                              "нажмите Q (в русской раскладке Й).\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.Q)
                PrintBooks(bookService.GetAllBooks());

            int isbn;
            while (!int.TryParse(Console.ReadLine(), out isbn))
                Console.WriteLine("Необходимо ввести одно число!");

            try
            {
                borrowingService.DeleteBorrowingByBookIsbn(isbn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void DeleteBorrowingByCopy(BorrowingService borrowingService, CopyService copyService)
        {
            Console.WriteLine("Формат ввода: ISBN, CopyNumber\n" +
                              "Пример:\n" +
                              "43234, 2\n" +
                              "После ввода нажмите Enter.\n" +
                              "ISBN книги должно существовать в базе данных,\n" +
                              "так что в случае, если вы хотите посмотреть список книг,\n" +
                              "нажмите Q (в русской раскладке Й).\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.W)
                PrintCopies(copyService.GetAllCopies());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 2)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 2");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                borrowingService.DeleteBorrowingByCopy(int.Parse(str[0]), int.Parse(str[1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void DeleteBorrowingByHoleKey(BorrowingService borrowingService, CopyService copyService,
            ReaderService readerService)
        {
            Console.WriteLine("Формат ввода: ReaderNumber, ISBN, CopyNumber\n" +
                              "Пример:\n" +
                              "1, 43234, 2\n" +
                              "После ввода нажмите Enter.\n" +
                              "Номер читателя, ISBN книги и номер копии должны существовать\n" +
                              "в базе данных, так что в случае, если вы хотите посмотреть список\n" +
                              "копий книг, нажмите W (в русской раскладке Ц);\n" +
                              "если хотите посмотреть список читателей, нажмите T (в русской раскладке Е).\n" +
                              "Можно просмотреть оба списка по очереди.\n" +
                              "Если хотите продолжить без просмотра списков, то введите любую букву.\n");

            for (int i = 0; i < 2; i++)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.W)
                    PrintCopies(copyService.GetAllCopies());
                else if (Console.ReadKey(true).Key == ConsoleKey.E)
                    PrintReaders(readerService.GetAllReaders());
                else
                    break;
            }

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 3)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 3");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                borrowingService.DeleteBorrowing(int.Parse(str[0]), int.Parse(str[1]),
                    int.Parse(str[2]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void UpdateBook(BookService bookService)
        {
            Console.WriteLine("Нажмите:\n" +
                              "Q (Й) для изменения одного ISBN на другой\n" +
                              "W (Ц) для изменения названия книги\n" +
                              "E (У) для изменения автора книги\n" +
                              "R (К) для изменения количества страниц в книге\n" +
                              "T (Е) для изменения года публикации книги\n");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    UpdateBooksIsbn(bookService);
                    break;
                case ConsoleKey.W:
                    UpdateBooksTitle(bookService);
                    break;
                case ConsoleKey.E:
                    UpdateBooksAuthor(bookService);
                    break;
                case ConsoleKey.R:
                    UpdateBooksPagesNumber(bookService);
                    break;
                case ConsoleKey.T:
                    UpdateBooksPubYear(bookService);
                    break;
            }
        }

        private static void UpdateBooksIsbn(BookService bookService)
        {
            Console.WriteLine("Формат ввода: oldISBN, newISBN\n" +
                              "Пример:\n" +
                              "3214, 221\n" +
                              "После ввода полей через запятую и пробел нажмите Enter.\n" +
                              "ISBN книги должен существовать в базе данных, так что в случае,\n" +
                              "если вы хотите посмотреть список книг, нажмите Q (в русской раскладке Й)\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.Q)
                PrintBooks(bookService.GetAllBooks());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 2)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 2");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                bookService.UpdateBooksIsbn(int.Parse(str[0]), int.Parse(str[1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void UpdateBooksTitle(BookService bookService)
        {
            Console.WriteLine("Формат ввода: ISBN, Title\n" +
                              "Пример:\n" +
                              "3214, Wind\n" +
                              "После ввода полей через запятую и пробел нажмите Enter.\n" +
                              "ISBN книги должен существовать в базе данных, так что в случае,\n" +
                              "если вы хотите посмотреть список книг, нажмите Q (в русской раскладке Й)\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.Q)
                PrintBooks(bookService.GetAllBooks());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 2)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 2");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                bookService.UpdateBooksTitle(int.Parse(str[0]), str[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void UpdateBooksAuthor(BookService bookService)
        {
            Console.WriteLine("Формат ввода: ISBN, Title\n" +
                              "Пример:\n" +
                              "3214, Emma Swan\n" +
                              "После ввода полей через запятую и пробел нажмите Enter.\n" +
                              "ISBN книги должен существовать в базе данных, так что в случае,\n" +
                              "если вы хотите посмотреть список книг, нажмите Q (в русской раскладке Й)\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.Q)
                PrintBooks(bookService.GetAllBooks());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 2)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 2");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                bookService.UpdateBooksAuthor(int.Parse(str[0]), str[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void UpdateBooksPagesNumber(BookService bookService)
        {
            Console.WriteLine("Формат ввода: ISBN, PagesNumber\n" +
                              "Пример:\n" +
                              "3214, 100\n" +
                              "После ввода полей через запятую и пробел нажмите Enter.\n" +
                              "ISBN книги должен существовать в базе данных, так что в случае,\n" +
                              "если вы хотите посмотреть список книг, нажмите Q (в русской раскладке Й)\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.Q)
                PrintBooks(bookService.GetAllBooks());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 2)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 2");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                bookService.UpdateBooksPagesNumber(int.Parse(str[0]), int.Parse(str[1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void UpdateBooksPubYear(BookService bookService)
        {
            Console.WriteLine("Формат ввода: ISBN, PublishYear\n" +
                              "Пример:\n" +
                              "3214, 2021\n" +
                              "После ввода полей через запятую и пробел нажмите Enter.\n" +
                              "ISBN книги должен существовать в базе данных, так что в случае,\n" +
                              "если вы хотите посмотреть список книг, нажмите Q (в русской раскладке Й)\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.Q)
                PrintBooks(bookService.GetAllBooks());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 2)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 2");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                bookService.UpdateBooksPubYear(int.Parse(str[0]), int.Parse(str[1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void UpdateCopy(CopyService copyService)
        {
            Console.WriteLine("Нажмите:\n" +
                              "Q (Й) для изменения номера копии книги\n" +
                              "W (Ц) для изменения позиции копии книги на полке\n");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    UpdateCopyNumber(copyService);
                    break;
                case ConsoleKey.W:
                    UpdateCopyShelfPosition(copyService);
                    break;
            }
        }

        private static void UpdateCopyNumber(CopyService copyService)
        {
            Console.WriteLine("Формат ввода: ISBN, OldCopyNumber, NewCopyNumber\n" +
                              "Пример:\n" +
                              "3214, 3, 10\n" +
                              "После ввода полей через запятую и пробел нажмите Enter.\n" +
                              "ISBN книги и старый номер копии должны существовать в базе данных,\n" +
                              "так что в случае, если вы хотите посмотреть список копий книг,\n" +
                              "нажмите W (в русской раскладке Ц)\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.W)
                PrintCopies(copyService.GetAllCopies());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 3)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 3");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                copyService.UpdateCopyNumber(int.Parse(str[0]), int.Parse(str[1]),
                    int.Parse(str[2]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void UpdateCopyShelfPosition(CopyService copyService)
        {
            Console.WriteLine("Формат ввода: ISBN, OldShelfPosition, NewShelfPosition\n" +
                              "Пример:\n" +
                              "3214, 3, 10\n" +
                              "После ввода полей через запятую и пробел нажмите Enter.\n" +
                              "ISBN книги и старый номер позиции на полук должны существовать в базе данных,\n" +
                              "так что в случае, если вы хотите посмотреть список копий книг,\n" +
                              "нажмите W (в русской раскладке Ц)\n" +
                              "Если хотите продолжить без просмотра списка, то введите любую букву.\n");

            if (Console.ReadKey(true).Key == ConsoleKey.W)
                PrintCopies(copyService.GetAllCopies());

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 3)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 3");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                copyService.UpdateCopyShelfPosition(int.Parse(str[0]), int.Parse(str[1]),
                    int.Parse(str[2]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void FindBook(BookService bookService)
        {
            Console.WriteLine("Нажмите:\n" +
                              "Q (Й) для поиска книги по ISBN\n" +
                              "W (Ц) для поиска книги по имени категории\n");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    FindBookByIsbn(bookService);
                    break;
                case ConsoleKey.W:
                    FindBookByCategoryName(bookService);
                    break;
            }
        }

        private static void FindBookByIsbn(BookService bookService)
        {
            Console.WriteLine("Формат ввода: ISBN\n" +
                              "Пример:\n" +
                              "3214\n");

            int isbn;
            while (!int.TryParse(Console.ReadLine(), out isbn))
                Console.WriteLine("Необходимо ввести одно число!");

            try
            {
                Console.WriteLine(bookService.FindBookByIsbn(isbn).ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void FindBookByCategoryName(BookService bookService)
        {
            Console.WriteLine("Формат ввода: CategoryName\n" +
                              "Пример:\n" +
                              "head\n");

            var str = Console.ReadLine()!.Trim();

            try
            {
                var res = bookService.FindBookByCategoryName(str);
                foreach (var item in res)
                    Console.WriteLine(item.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void FindCopy(CopyService copyService)
        {
            Console.WriteLine("Нажмите:\n" +
                              "Q (Й) для поиска копии по ISBN и номеру\n" +
                              "W (Ц) для поиска копий по ISBN\n");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    FindCopyByIsbnAndCopyNumber(copyService);
                    break;
                case ConsoleKey.W:
                    FindCopiesByIsbn(copyService);
                    break;
            }
        }

        private static void FindCopyByIsbnAndCopyNumber(CopyService copyService)
        {
            Console.WriteLine("Формат ввода: ISBN, CopyNumber\n" +
                              "Пример:\n" +
                              "3214, 3\n");

            string[] str = Console.ReadLine()!.Trim().Split(", ");
            while (str.Length != 2)
            {
                Console.WriteLine("Проверьте количество вводимых чисел! Их должно быть ровно 2");
                str = Console.ReadLine()!.Trim().Split(", ");
            }

            try
            {
                Console.WriteLine(copyService.FindCopyByIsbnAndCopyNumber(int.Parse(str[0]),
                    int.Parse(str[1])).ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }

        private static void FindCopiesByIsbn(CopyService copyService)
        {
            Console.WriteLine("Формат ввода: ISBN\n" +
                              "Пример:\n" +
                              "2123\n");

            int isbn;
            while (!int.TryParse(Console.ReadLine(), out isbn))
                Console.WriteLine("Необходимо ввести одно число!");

            try
            {
                var res = copyService.FindCopiesByIsbn(isbn);
                foreach (var item in res)
                    Console.WriteLine(item.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("При удалении объекта из базы данных " +
                                  $"произошла ошибка \"{ex.Message}\"\n" +
                                  "Попробуйте снова!");
            }
        }
    }
}