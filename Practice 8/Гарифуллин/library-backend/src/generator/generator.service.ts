import { Injectable, OnModuleInit } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Book } from 'src/books/entities/book.entity';
import { Borrowing } from 'src/borrowings/entities/borrowing.entity';
import { Category } from 'src/categories/entities/category.entity';
import { Copy } from 'src/copies/entities/copy.entity';
import { Publisher } from 'src/publishers/entities/publisher.entity';
import { Reader } from 'src/readers/entities/reader.entity';
import { Repository } from 'typeorm';
import * as faker from 'faker';

@Injectable()
export class GeneratorService implements OnModuleInit {
  constructor(
    @InjectRepository(Book)
    private $book: Repository<Book>,
    @InjectRepository(Borrowing)
    private $borrowing: Repository<Borrowing>,
    @InjectRepository(Category)
    private $category: Repository<Category>,
    @InjectRepository(Copy)
    private $copy: Repository<Copy>,
    @InjectRepository(Publisher)
    private $publisher: Repository<Publisher>,
    @InjectRepository(Reader)
    private $reader: Repository<Reader>,
  ) { }
  async onModuleInit() {
    const readersCount = await this.$reader.count();
    if (readersCount) return;

    const readers = await Promise.all(
      new Array(10).fill(0).map(() =>
        this.$reader.save({
          firstName: faker.name.firstName(),
          lastName: faker.name.lastName(),
          address: faker.address.streetAddress(),
          birthDate: faker.date.past()
        }).catch(() => null)).filter(x => !!x)
    );
    const getReader = () => readers[Math.floor(Math.random() * readers.length)];

    const publishers = await Promise.all(
      new Array(10).fill(0).map(() =>
        this.$publisher.save({
          name: faker.company.companyName(),
          address: faker.address.streetAddress()
        }).catch(() => null)).filter(x => !!x)
    );
    const getPublisher = () => publishers[Math.floor(Math.random() * publishers.length)];

    const categories = await Promise.all(
      new Array(4).fill(0).map(() =>
        this.$category.save({
          name: faker.music.genre(),
        }).catch(() => null)).filter(x => !!x)
    );
    const getCategory = () => categories[Math.floor(Math.random() * categories.length)];

    const books = await Promise.all(
      new Array(10).fill(0).map(() =>
        this.$book.save({
          isbn: Math.floor(Math.random() * 9000000 + 1000000) + "" + Math.floor(Math.random() * 900000 + 100000),
          title: faker.company.companyName(),
          author: faker.name.findName(),
          pagesCount: Math.floor(Math.random() * 900 + 100),
          publicationYear: Math.floor((Math.random() * 1000) + 1100),
          publisher: getPublisher(),
          categories: [getCategory(), getCategory(), getCategory()].filter((v, i, s) => s.findIndex(o => o && v && o.name === v.name) === i)
        }).catch(() => null)).filter(x => !!x)
    );
    const getBook = () => books[Math.floor(Math.random() * books.length)];


    const copies = await Promise.all(
      new Array(20).fill(0).map(() =>
        this.$copy.save({
          book: getBook(),
          number: Math.floor(Math.random() * 100),
          shelfPosition: Math.floor(Math.random() * 100)
        }).catch(() => null)).filter(x => !!x)
    );
    const getCopy = () => copies[Math.floor(Math.random() * copies.length)];

    const borrowings = await Promise.all(
      new Array(20).fill(0).map(() => {
        const copy = getCopy();
        return this.$borrowing.save({
          reader: getReader(),
          book: copy.book,
          bookIsbn: copy.bookIsbn,
          copy,
          returnDate: faker.date.future()
        }).catch(() => null)
      }).filter(x => !!x));
  }
}
