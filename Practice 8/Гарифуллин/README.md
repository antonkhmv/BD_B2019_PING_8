# Гарифуллин Руслан Ильфатович, задание 8

### Задача 1

#### Reader
```sql
CREATE TABLE readers (
  number SERIAL PRIMARY KEY,
  lastName VARCHAR(40),
  firstName VARCHAR(40),
  address VARCHAR(100),
  birthDate DATE,
)
```

#### Publisher
```sql
CREATE TABLE publishers (
  number SERIAL PRIMARY KEY,
  lastName VARCHAR(40),
  firstName VARCHAR(40),
  address VARCHAR(100),
  birthDate DATE,
)
```

#### Book
```sql
CREATE TABLE Book
(
    isbn SERIAL PRIMARY KEY,
    title VARCHAR(100),
    author VARCHAR(100),
    pagesNum INTEGER,
    pubYear INTEGER,
    pubName VARCHAR(100),
    FOREIGN KEY (pubName) REFERENCES Publisher(pubName)
)
```

#### Category
```sql
CREATE TABLE Category
(
    categoryName VARCHAR(40) PRIMARY KEY,
    parentCat VARCHAR(40),
    FOREIGN KEY (parentCat) REFERENCES Category(categoryName)
)
```

#### Copy
```sql
CREATE TABLE Copy
(
    isbn INTEGER,
    copyNumber INTEGER PRIMARY KEY,
    shelfPosition INTEGER,
    FOREIGN KEY (isbn) REFERENCES Book(isbn)
)
```

#### Borrowing
```sql
CREATE TABLE Borrowing
(
    readerNr INTEGER,
    isbn INTEGER,
    copyNumber INTEGER,
    returnDate DATE,
    FOREIGN KEY (readerNr) REFERENCES Reader(id),
    FOREIGN KEY (isbn) REFERENCES Book(isbn),
    FOREIGN KEY (copyNumber) REFERENCES Copy(copyNumber)
)
```

### BookCat
```sql
CREATE TABLE BookCat
(
    isbn INTEGER,
    categoryName VARCHAR(40),
    FOREIGN KEY (isbn) REFERENCES Book(isbn),
    FOREIGN KEY (categoryName) REFERENCES Category(categoryName)
)
```

### Задача 2
Работа по созданию CRUD, генерации таблиц, миграции и генерации данных для таблиц выполнена на Node.JS с использованием фреймфорка Nest.JS и системы ORM TypeORM. Исходный код можно увидеть в `library-backend/`.

Для запуска необходимо окружение Node.JS, после установки выполнить следующие команды:
```bash
npm install
npm start
```

После каждого изменения кода в TypeScript, необходимо собирать проект:
```bash
npm run build
```

Миграции выполняются отправкой команд в TypeORM при помощи следующих команд (не забудьте скомпилировать код командой выше):
```bash
npx typeorm migration:generate -n Book -d src/migrations # Генерация миграции на базе измененной сущности
npx typeorm migration:create -n Book -d src/migrations # Создание пустой миграции
npx typeorm migration:show # Отображение списка выполненных и невыполненных миграций
npx typeorm migration:run # Запуск миграции
npx typeorm migration:rollback # Откат миграции
```