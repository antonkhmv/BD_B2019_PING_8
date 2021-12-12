import { Module } from '@nestjs/common';
import { BooksModule } from './books/books.module';
import { CopiesModule } from './copies/copies.module';
import { BorrowingsModule } from './borrowings/borrowings.module';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Book } from './books/entities/book.entity';
import { Borrowing } from './borrowings/entities/borrowing.entity';
import { Copy } from './copies/entities/copy.entity';
import { Category } from './categories/entities/category.entity';
import { Publisher } from './publishers/entities/publisher.entity';
import { Reader } from './readers/entities/reader.entity';
import { GeneratorModule } from './generator/generator.module';

@Module({
  imports: [
    BooksModule,
    CopiesModule,
    BorrowingsModule,
    TypeOrmModule.forRoot({
      type: 'postgres',
      host: 'localhost',
      port: 5432,
      username: 'postgres',
      password: 'example',
      database: 'postgres',
      entities: [
        Book,
        Borrowing,
        Copy,
        Category,
        Publisher,
        Reader
      ],
    }),
    GeneratorModule
  ],
})
export class AppModule { }
