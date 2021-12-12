import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Book } from 'src/books/entities/book.entity';
import { Borrowing } from 'src/borrowings/entities/borrowing.entity';
import { Category } from 'src/categories/entities/category.entity';
import { Copy } from 'src/copies/entities/copy.entity';
import { Publisher } from 'src/publishers/entities/publisher.entity';
import { Reader } from 'src/readers/entities/reader.entity';
import { GeneratorService } from './generator.service';

@Module({
  imports: [
    TypeOrmModule.forFeature([
      Book,
      Borrowing,
      Copy,
      Category,
      Publisher,
      Reader
    ])
  ],
  providers: [GeneratorService]
})
export class GeneratorModule { }
