import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { CreateBookDto } from './dto/create-book.dto';
import { UpdateBookDto } from './dto/update-book.dto';
import { Book } from './entities/book.entity';

@Injectable()
export class BooksService {
  constructor(
    @InjectRepository(Book)
    private $book: Repository<Book>,
  ) { }
  create(createBookDto: CreateBookDto) {
    return this.$book.save({
      isbn: createBookDto.isbn,
      title: createBookDto.title,
      author: createBookDto.author,
      pagesCount: createBookDto.pagesCount,
      publicationYear: createBookDto.publicationYear,
      publisher: createBookDto.publisherName ? { name: createBookDto.publisherName } : void 0,
      categories: createBookDto.categoryNames?.map(name => ({ name }))
    });
  }

  findAll() {
    return this.$book.find();
  }

  findOne(isbn: string) {
    return this.$book.findOneOrFail({ where: { isbn }, relations: ["publisher", "categories"] });
  }

  update(isbn: string, updateBookDto: UpdateBookDto) {
    const book = this.findOne(isbn);

    return this.$book.save({
      isbn,
      ...book,
      title: updateBookDto.title,
      author: updateBookDto.author,
      pagesCount: updateBookDto.pagesCount,
      publicationYear: updateBookDto.publicationYear,
      publisher: updateBookDto.publisherName ? { name: updateBookDto.publisherName } : void 0,
      categories: updateBookDto.categoryNames?.map(name => ({ name }))
    });
  }

  remove(isbn: string) {
    return this.$book.delete({ isbn });
  }
}
