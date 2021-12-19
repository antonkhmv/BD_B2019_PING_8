# Федотова Дарья БПИ198

## Задание 8

### Создание проекта

Для выполнения данного задания был выбран язык C#. За основу был взят проект ASP.NET Core Web Application. Основная папка с логикой веб-приложения, а также с логикой консольного взаимодействия с пользователем, называется Library.Web, папка с сущностями и с логикой работы с ними называется Library.Entities. Для корректной работы были также добавлены NuGet пакеты Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Tools, Npgsql.EntityFrameworkCore.PostgreSQL, Faker.Data.

### Создание сущностей

В папке Library.Entities были созданы все необходимые классы, названия которых являются названиями соответствующих таблиц. В каждом классе перечислены свойства, которые будут являться колонками в таблице. Например:

``` cs
public class Book
{
    public int Isbn { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public int PagesNum { get; set; }

    public int PubYear { get; set; }

    public string PubName { get; set; }
}
```

Для того, чтобы учесть наличие составных ключей в некоторых таблицах, в классе LibraryContext, унаследованном от DbContext, в методе OnModelCreating(ModelBuilder modelBuilder) были прописаны следующие строчки кода:

``` cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Book>().HasKey(o => new {o.Isbn});
    modelBuilder.Entity<Category>().HasKey(o => new {o.CategoryName});
    modelBuilder.Entity<Copy>().HasKey(o => new {o.Isbn, o.CopyNumber});
    modelBuilder.Entity<Borrowing>().HasKey(o => new {o.ReaderNr, o.Isbn, o.CopyNumber});
    modelBuilder.Entity<BookCat>().HasKey(o => new {o.Isbn, o.CategoryName});
    modelBuilder.Entity<Publisher>().HasKey(o => new {o.PubName});
}
```

### Создание базы данных

Для создания базы данных нужно было открыть Package Console Manager в Visual Studio и прописать команду 'add-migration "AddLibraryDatabase"'. При изменения каких-либо данных в классах нужно лишь прописать в командной строке 'update-database' и изменения "подтянутся" автоматически. Также предусмотрена возможность откатить состояние базы данных к любой миграции.

### Добавление фейковых данных

Для генерации фейковых данных был использован NuGet пакет Faker.Data, вся логика данной генерации находится в классе MyFaker (проект Library.Web).

### CRUD 

Вся логика для работы с сущностями (создание, чтение, удаление и обновление данных) прописана в классах BookService, BorrowingService, CopyService и LibraryService.

### Результат

Для того, чтобы оценить результаты, к которым я пришла, можно скачать проект, запустить класс Program проекта (или .exe файл) и с помощью простого консольного взаимодействия посмотреть на работу прописанных команд.