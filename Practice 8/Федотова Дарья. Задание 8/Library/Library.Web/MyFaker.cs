using System;
using System.Collections.Generic;
using System.Text;
using Faker;

public static class MyFaker
{
    private static string GenerateLastName()
    {
        string lastName = Name.LastName();
        while (lastName.IndexOf('\'') != -1)
            lastName = Name.LastName();
        return lastName;
    }

    private static string GenerateFirstName()
    {
        string firstName = Name.FirstName();
        while (firstName.IndexOf('\'') != -1)
            firstName = Name.FirstName();
        return firstName;
    }

    private static string GenerateAddress()
    {
        string address = new StringBuilder(Address.USCity() + ", " +
                                           Address.StreetName() + ", " +
                                           Number.RandomNumber(1, 100) + ", " +
                                           Number.RandomNumber(1, 1000)).ToString();
        while (address.IndexOf('\'') != -1)
            address = new StringBuilder(Address.USCity() + ", " +
                                        Address.StreetName() + ", " +
                                        Number.RandomNumber(1, 100) + ", " +
                                        Number.RandomNumber(1, 1000)).ToString();

        return address;
    }

    private static DateTime GenerateBirthDate()
    {
        return Date.Birthday(7, 80);
    }

    private static DateTime GenerateDate()
    {
        return Date.Between(DateTime.Now, DateTime.Now.AddMonths(4));
    }

    private static string GenerateWord()
    {
        string word = Lorem.Word();
        word = word[0].ToString().ToUpper() + word.Substring(1);
        return word;
    }

    private static string GenerateCompanyName()
    {
        string company = Company.CatchPhrase();
        while (company.IndexOf('\'') != -1)
            company = Company.CatchPhrase();
        return company;
    }

    private static string GeneratePerson()
    {
        string person = Name.FullName();
        while (person.IndexOf('\'') != -1)
            person = Name.FullName();
        return person;
    }

    public static void GenerateReaders(int readerNumber, out List<int> idList, ref List<string> result)
    {
        idList = new List<int>();

        for (int id = 1; id <= readerNumber; id++)
        {
            idList.Add(id);

            string lastName = GenerateLastName();
            string firstName = GenerateFirstName();
            string address = GenerateAddress();
            DateTime birthDate = GenerateBirthDate();

            result.Add($"INSERT INTO public.\"Reader\" VALUES({id}, '{lastName}', '{firstName}', " +
                       $"'{address}', TO_DATE('{birthDate.Date.Day}.{birthDate.Date.Month}.{birthDate.Date.Year}', " +
                       $"'dd-mm-yyyy'));");
        }
    }

    public static void GeneratePublishers(int publishersNumber, out List<string> nameList, ref List<string> result)
    {
        nameList = new List<string>();

        for (int i = 0; i < publishersNumber; i++)
        {
            string publisherName = GenerateCompanyName();
            while (nameList.Contains(publisherName))
                publisherName = GenerateCompanyName();

            nameList.Add(publisherName);

            string address = GenerateAddress();

            result.Add($"INSERT INTO public.\"Publisher\" VALUES('{publisherName}', '{address}');");
        }
    }

    public static void GenerateBooks(int booksNumber, List<string> publishersNamesList,
        out List<int> isbnList, ref List<string> result)
    {
        isbnList = new List<int>();

        for (int i = 0; i < booksNumber; i++)
        {
            int isbn = Number.RandomNumber(1, 1000000);
            while (isbnList.Contains(isbn))
                isbn = Number.RandomNumber(1, 1000000);

            isbnList.Add(isbn);

            string title = GenerateWord();
            string author = GeneratePerson();
            int pages = Number.RandomNumber(1, 50000);
            int publishYear = Number.RandomNumber(1900, 2021);
            string publishName = publishersNamesList[Number.RandomNumber(0, publishersNamesList.Count)];

            result.Add($"INSERT INTO public.\"Book\" VALUES({isbn}, '{title}', '{author}', " +
                       $"{pages}, {publishYear}, '{publishName}');");
        }
    }

    public static void GenerateCopies(List<int> isbnList, out List<Tuple<int, int>> copiesKeyList,
        ref List<string> result)
    {
        copiesKeyList = new List<Tuple<int, int>>();

        foreach (int isbn in isbnList)
        {
            int copiesNumber = Number.RandomNumber(1, 5);
            for (int i = 1; i <= copiesNumber; i++)
            {
                copiesKeyList.Add(new Tuple<int, int>(isbn, i));

                result.Add($"INSERT INTO public.\"Copy\" VALUES({isbn}, {i}, {Number.RandomNumber(1, 700)});");
            }
        }
    }

    public static void GenerateBorrowings(int borrowingsNumber, List<int> readersIdList,
        List<Tuple<int, int>> copiesKeyList, ref List<string> result)
    {
        for (int i = 0; i < borrowingsNumber; i++)
        {
            int readerId = readersIdList[Number.RandomNumber(0, readersIdList.Count)];
            int randomCopyIndex = Number.RandomNumber(0, copiesKeyList.Count);
            DateTime returnDate = GenerateDate();

            result.Add($"INSERT INTO public.\"Borrowing\" VALUES({readerId}, {copiesKeyList[randomCopyIndex].Item1}," +
                       $" {copiesKeyList[randomCopyIndex].Item2}, " +
                       $"TO_DATE('{returnDate.Date.Day}.{returnDate.Date.Month}.{returnDate.Date.Year}', " +
                       $"'dd-mm-yyyy'));");
        }
    }

    public static void GenerateCategories(int categoriesNumber,
        out List<string> categoriesNamesList, ref List<string> result)
    {
        categoriesNamesList = new List<string>();

        int parentCategoriesSize = categoriesNumber / 2;

        for (int i = 1; i <= categoriesNumber; i++)
        {
            string categoryName = GenerateWord();
            while (categoriesNamesList.Contains(categoryName))
                categoryName = GenerateWord();

            categoriesNamesList.Add(categoryName);

            string res;
            if (i < parentCategoriesSize)
                res = $"INSERT INTO public.\"Category\" VALUES('{categoryName}', null);";
            else
                res = $"INSERT INTO public.\"Category\" VALUES('{categoryName}', " +
                      $"'{categoriesNamesList[Number.RandomNumber(0, categoriesNamesList.Count)]}');";
            result.Add(res);
        }
    }

    public static void GenerateBookCats(List<int> booksIsbnList, List<string> categoriesNamesList,
        ref List<string> result)
    {
        foreach (var isbn in booksIsbnList)
        {
            result.Add($"INSERT INTO public.\"BookCat\" VALUES({isbn}, " +
                       $"'{categoriesNamesList[Number.RandomNumber(0, categoriesNamesList.Count)]}');");
        }
    }

    public static void GenerateDataForHoleDatabase(int readersNumber, int publishersNumber, int booksNumber,
        int categoriesNumber, int borrowingsNumber)
    {
        List<string> result = new List<string>();

        GenerateReaders(readersNumber, out var readersIdList, ref result);
        GeneratePublishers(publishersNumber, out var publishersNamesList, ref result);
        GenerateBooks(booksNumber, publishersNamesList, out var booksIsbnList, ref result);
        GenerateCopies(booksIsbnList, out var copiesKeyList, ref result);
        GenerateCategories(categoriesNumber, out var categoriesNamesList, ref result);
        GenerateBorrowings(borrowingsNumber, readersIdList, copiesKeyList, ref result);
        GenerateBookCats(booksIsbnList, categoriesNamesList, ref result);
        foreach (var str in result)
        {
            Console.WriteLine(str);
        }
    }
}