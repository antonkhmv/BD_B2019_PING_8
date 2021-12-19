using System;
using System.Collections.Generic;
using Library.Entities;

public class ConsoleDialog
{
    public void StartDialog()
    {
        do
        {
            Console.WriteLine("Список команд:\n" +
                              "Esc \t | Выйти из приложения\n" +
                              "Q \t | Вывести содержимое всех таблиц\n" +
                              "W \t | Работа с книгами\n" +
                              "E \t | Работа с копиями книг\n" +
                              "R \t | Работа с бронированиями\n" +
                              "T \t | Создать фейковые данные\n");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    LibraryService libraryService = new LibraryService(new LibraryContext());
                    libraryService.PrintAllTables();
                    break;
                case ConsoleKey.W:
                    WorkingWithBooks();
                    break;
                case ConsoleKey.E:
                    WorkingWithCopies();
                    break;
                case ConsoleKey.R:
                    WorkingWithBorrowings();
                    break;
                case ConsoleKey.T:
                    CreateFakeData();
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }

    private void WorkingWithBooks()
    {
        Console.WriteLine("Список команд:\n" +
                          "Esc \t | Выйти из приложения\n" +
                          "Q \t | Вывести список всех книг\n" +
                          "W \t | Добавить книгу. После нажатия на W введите ISBN книги,\n" +
                          "  \t | название книги, имя автора, количество страниц, год публикации\n" +
                          "  \t | и имя издательства через запятую-пробел, затем нажмите Enter\n" +
                          "E \t | Удалить книгу. После нажатия на E введите ISBN книги и нажмите Enter\n" +
                          "R \t | Изменить информацию о названии книги. После нажатия на R введите ISBN\n" +
                          "  \t | книги и новое название через запятую-пробел и нажмите Enter.\n" +
                          "T \t | Изменить информацию об авторе книги. После нажатия на T введите ISBN\n" +
                          "  \t | книги и нового автора через запятую-пробел и нажмите Enter.\n" +
                          "Y \t | Изменить информацию о количестве страниц в книге. После нажатия на Y\n" +
                          "  \t | введите ISBN книги и новое количество страниц через запятую-пробел и\n" +
                          "  \t | нажмите Enter.\n" +
                          "U \t | Изменить информацию о годе публикации книги. После нажатия на U введите ISBN\n" +
                          "  \t | книги и новый год публикации через запятую-пробел и нажмите Enter.\n");

        LibraryContext libraryContext = new LibraryContext();
        LibraryService libraryService = new LibraryService(libraryContext);
        BookService bookService = new BookService(libraryContext);
        string[] str;
        try
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    libraryService.PrintAllBooks();
                    break;
                case ConsoleKey.W:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 6)
                    {
                        Console.WriteLine("Вы должны ввести ровно 6 значений через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        bookService.AddBook(int.Parse(str[0]), str[1], str[2],
                            int.Parse(str[3]), int.Parse(str[4]), str[5]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось добавить книгу из-за ошибки" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова");
                    }

                    break;
                case ConsoleKey.E:
                    int isbn;
                    while (!int.TryParse(Console.ReadLine(), out isbn))
                        Console.WriteLine("Необходимо ввести одно число!");
                    try
                    {
                        bookService.RemoveBook(isbn);
                        Console.WriteLine("Успех!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    break;
                case ConsoleKey.R:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 2)
                    {
                        Console.WriteLine("Вы должны ввести ровно 2 значения через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        bookService.UpdateBookTitle(int.Parse(str[0]), str[1]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось обновить данные о книге из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова");
                    }

                    Console.WriteLine("Успех!");
                    break;
                case ConsoleKey.T:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 2)
                    {
                        Console.WriteLine("Вы должны ввести ровно 2 значения через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        bookService.UpdateBookAuthor(int.Parse(str[0]), str[1]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось обновить данные о книге из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова");
                    }

                    Console.WriteLine("Успех!");
                    break;
                case ConsoleKey.Y:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 2)
                    {
                        Console.WriteLine("Вы должны ввести ровно 2 значения через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        bookService.UpdateBookPagesNumber(int.Parse(str[0]), int.Parse(str[1]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось обновить данные о книге из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова");
                    }

                    Console.WriteLine("Успех!");
                    break;
                case ConsoleKey.U:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 2)
                    {
                        Console.WriteLine("Вы должны ввести ровно 2 значения через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        bookService.UpdateBookPubYear(int.Parse(str[0]), int.Parse(str[1]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось обновить данные о книге из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова");
                    }

                    Console.WriteLine("Успех!");
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void WorkingWithCopies()
    {
        Console.WriteLine("Список команд:\n" +
                          "Esc \t | Выйти из приложения\n" +
                          "Q \t | Вывести список всех копий\n" +
                          "W \t | Вывести список копий конкретной книги.\n" +
                          "  \t | После нажатия на W введите ISBN книги и нажмите Enter.\n" +
                          "E \t | Добавить копию. После нажатия на E введите ISBN книги и\n" +
                          "  \t | позицию на полке через пробел-запятую и нажмите Enter.\n" +
                          "R \t | Удалить все копии одной книги. После нажатия на R введите\n" +
                          "  \t | ISBN книги и нажмите Enter\n" +
                          "T \t | Удалить копию. После нажатия на T введите ISBN книги и\n" +
                          "  \t | номер копии через запятую-пробел и нажмите на Enter\n" +
                          "Y \t | Изменить информацию о положении на полке. После нажатия на Y\n" +
                          "  \t | введите ISBN книги, номер копии и новую позицию на полке\n" +
                          "  \t | через запятую-пробел и нажмите Enter.\n");

        LibraryContext libraryContext = new LibraryContext();
        LibraryService libraryService = new LibraryService(libraryContext);
        CopyService copyService = new CopyService(libraryContext);
        string[] str;
        try
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    libraryService.PrintAllCopies();
                    break;
                case ConsoleKey.W:
                    int isbn;
                    while (!int.TryParse(Console.ReadLine(), out isbn))
                        Console.WriteLine("Необходимо ввести одно число!");
                    var list = copyService.FindCopiesByIsbn(isbn);
                    foreach (var item in list)
                    {
                        Console.WriteLine(item.ToString());
                    }

                    break;
                case ConsoleKey.E:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 2)
                    {
                        Console.WriteLine("Вы должны ввести ровно 2 числа через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        copyService.AddCopy(int.Parse(str[0]), int.Parse(str[1]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось добавить копию из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова");
                    }

                    Console.WriteLine("Успех!");
                    break;
                case ConsoleKey.R:
                    while (!int.TryParse(Console.ReadLine(), out isbn))
                        Console.WriteLine("Необходимо ввести одно число!");
                    try
                    {
                        copyService.RemoveBooksCopies(isbn);
                        Console.WriteLine("Успех!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось удалить копию из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова");
                    }

                    break;
                case ConsoleKey.T:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 2)
                    {
                        Console.WriteLine("Вы должны ввести ровно 2 числа через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        copyService.RemoveCopyByIsbnAndCopyNumber(int.Parse(str[0]), int.Parse(str[1]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось удалить копию книги из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова");
                    }

                    Console.WriteLine("Успех!");
                    break;
                case ConsoleKey.Y:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 3)
                    {
                        Console.WriteLine("Вы должны ввести ровно 3 числа через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        copyService.UpdateShelfPosition(int.Parse(str[0]),
                            int.Parse(str[1]), int.Parse(str[2]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось изменить информацию о копии книги из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова");
                    }

                    Console.WriteLine("Успех!");
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void WorkingWithBorrowings()
    {
        Console.WriteLine("Список команд:\n" +
                          "Esc \t | Выйти из приложения\n" +
                          "Q \t | Вывести список всех бронирований\n" +
                          "W \t | Вывести список ISBN книг, забронированных конкретным читателем.\n" +
                          "  \t | После нажатия на W введите номер читателя и нажмите Enter.\n" +
                          "E \t | Вывести список номеров читателей, забронировавших определенную книгу.\n" +
                          "  \t | После нажатия на E введите номер ISBN книги и нажмите Enter.\n" +
                          "R \t | Вывести список номеров читателей, бронировавших копию книги.\n" +
                          "  \t | После нажатия на R введите номер ISBN книги, запятую, пробел,\n" +
                          "  \t | номер копии книги и нажмите Enter (например, '321, 4').\n" +
                          "T \t | Добавить бронирование. После нажатия на T введите номер читателя,\n" +
                          "  \t | ISBN книги, номер копии и дату возврата через запятую-пробел и\n" +
                          "  \t | и нажмите Enter (например, '1332, 39123, 6, 10.01.2022')\n" +
                          "Y \t | Удалить бронирование. После нажатия на Y введите номер читателя,\n" +
                          "  \t | ISBN книги и номер копии через запятую-пробел и нажмите Enter\n" +
                          "  \t | (например, '1238, 1232, 1').\n" +
                          "U \t | Изменить информацию о дате возврата. После нажатия на U введите\n" +
                          "  \t | номер читателя, ISBN книги, номер копии и новую дату возврата через\n" +
                          "  \t | запятую-пробел и нажмите Enter (например, '1332, 39123, 6, 10.01.2022')\n");

        LibraryContext libraryContext = new LibraryContext();
        LibraryService libraryService = new LibraryService(libraryContext);
        BorrowingService borrowingService = new BorrowingService(libraryContext);
        string[] str;
        try
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    libraryService.PrintAllBorrowings();
                    break;
                case ConsoleKey.W:
                    int readerNumber;
                    while (!int.TryParse(Console.ReadLine(), out readerNumber))
                        Console.WriteLine("Необходимо ввести одно число!");
                    var list = borrowingService.FindBorrowedBooks(readerNumber);
                    foreach (var item in list)
                    {
                        Console.WriteLine(item.ToString());
                    }

                    break;
                case ConsoleKey.E:
                    int isbn;
                    while (!int.TryParse(Console.ReadLine(), out isbn))
                        Console.WriteLine("Необходимо ввести одно число!");
                    list = borrowingService.FindReadersWhoBorrowedBook(isbn);
                    foreach (var item in list)
                    {
                        Console.WriteLine(item.ToString());
                    }

                    break;
                case ConsoleKey.R:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 2)
                    {
                        Console.WriteLine("Вы должны ввести ровно 2 числа через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        List<int> readerIds = borrowingService.FindReadersWhoBorrowedCopy
                            (int.Parse(str[0]), int.Parse(str[1]));
                        foreach (var item in readerIds)
                        {
                            Console.WriteLine(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось найти бронирование из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова");
                    }

                    break;
                case ConsoleKey.T:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 4)
                    {
                        Console.WriteLine("Вы должны ввести ровно 4 числа через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        borrowingService.AddBorrowing(int.Parse(str[0]), int.Parse(str[1]),
                            int.Parse(str[2]), DateTime.ParseExact(str[3], "dd.MM.yyyy",
                                System.Globalization.CultureInfo.InvariantCulture));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось добавить бронирование из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова!");
                    }

                    Console.WriteLine("Успех!");
                    break;
                case ConsoleKey.U:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 4)
                    {
                        Console.WriteLine("Вы должны ввести ровно 4 числа через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        borrowingService.UpdateBorrowingReturnDate(int.Parse(str[0]), int.Parse(str[1]),
                            int.Parse(str[2]), DateTime.ParseExact(str[3], "dd.MM.yyyy",
                                System.Globalization.CultureInfo.InvariantCulture));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось обновить бронирование из-за ошибки\n" +
                                          $"\"{ex.Message}\"\n" +
                                          "Попробуйте снова!");
                    }

                    Console.WriteLine("Успех!");
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void CreateFakeData()
    {
        Console.WriteLine("Примечание: все сгенерированные значения не будут добавлены в таблицы,\n" +
                          "они лишь продемонстрируют работу Faker'а.\n" +
                          "Список команд:\n" +
                          "Esc \t | Выйти из приложения\n" +
                          "Q \t | Сгенерировать данные для всех таблиц. После нажатия на Q\n" +
                          "  \t | введите количество читателей, количество издательств, количество\n" +
                          "  \t | книг, количество категорий и количество бронирований\n" +
                          "  \t | через запятую-пробел и нажмите Enter\n" +
                          "W \t | Сгенерировать читателей. После нажатия на W введите желаемое количество\n" +
                          "  \t | сгенерированных данных и нажмите Enter\n" +
                          "E \t | Сгенерировать издательства. После нажатия на W введите желаемое количество\n" +
                          "  \t | сгенерированных данных и нажмите Enter\n" +
                          "R \t | Сгенерировать издательства и книги. После нажатия на R введите желаемое количество\n" +
                          "  \t | издательства и книг через запятую-пробел и нажмите Enter\n");
        try
        {
            List<string> result;
            string[] str;
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Q:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 5)
                    {
                        Console.WriteLine("Вы должны ввести ровно 5 чисел через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    try
                    {
                        MyFaker.GenerateDataForHoleDatabase(int.Parse(str[0]), int.Parse(str[1]),
                            int.Parse(str[2]), int.Parse(str[3]), int.Parse(str[4]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\"{ex.Message}\"\n");
                    }

                    break;
                case ConsoleKey.W:
                    int readersNumber;
                    while (!int.TryParse(Console.ReadLine(), out readersNumber))
                        Console.WriteLine("Необходимо ввести одно число!");
                    result = new List<string>();
                    MyFaker.GenerateReaders(readersNumber, out var readersIdList, ref result);
                    foreach (var item in result)
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ConsoleKey.E:
                    int publishersNumber;
                    while (!int.TryParse(Console.ReadLine(), out publishersNumber))
                        Console.WriteLine("Необходимо ввести одно число!");
                    result = new List<string>();
                    MyFaker.GeneratePublishers(publishersNumber, out var publishersNamesList, ref result);
                    foreach (var item in result)
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ConsoleKey.R:
                    str = Console.ReadLine()!.Trim().Split(", ");
                    while (str.Length != 2)
                    {
                        Console.WriteLine("Вы должны ввести ровно 2 числа через запятую-пробел");
                        str = Console.ReadLine()!.Trim().Split(", ");
                    }

                    int booksNumber;
                    try
                    {
                        publishersNumber = int.Parse(str[0]);
                        booksNumber = int.Parse(str[1]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        break;
                    }

                    result = new List<string>();
                    MyFaker.GeneratePublishers(publishersNumber, out publishersNamesList, ref result);
                    result.Add("");
                    MyFaker.GenerateBooks(booksNumber, publishersNamesList, out var booksIsbnList, ref result);
                    foreach (var item in result)
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}