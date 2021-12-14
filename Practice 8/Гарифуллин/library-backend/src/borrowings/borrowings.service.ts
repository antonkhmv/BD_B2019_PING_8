import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { CreateBorrowingDto } from './dto/create-borrowing.dto';
import { UpdateBorrowingDto } from './dto/update-borrowing.dto';
import { Borrowing } from './entities/borrowing.entity';

@Injectable()
export class BorrowingsService {
  constructor(
    @InjectRepository(Borrowing)
    private $borrowing: Repository<Borrowing>,
  ) { }
  create(createBorrowingDto: CreateBorrowingDto) {
    return this.$borrowing.save({
      returnDate: createBorrowingDto.returnDate,
      book: { isbn: createBorrowingDto.bookIsbn },
      copy: { number: createBorrowingDto.copyNumber, book: { isbn: createBorrowingDto.bookIsbn } },
      reader: { number: createBorrowingDto.readerNumber }
    });
  }

  findAll() {
    return this.$borrowing.find();
  }

  findOne(id: number) {
      return this.$borrowing.findOneOrFail({ where: { id }, 
        relations: ["reader", "book", "copy"], });
  }

  update(id: number, updateBorrowingDto: UpdateBorrowingDto) {
    return this.$borrowing.update({ id }, {
      returnDate: updateBorrowingDto.returnDate,
      book: { isbn: updateBorrowingDto.bookIsbn },
      copy: { number: updateBorrowingDto.copyNumber, book: { isbn: updateBorrowingDto.bookIsbn } },
      reader: { number: updateBorrowingDto.readerNumber }
    });
  }

  remove(id: number) {
    return this.$borrowing.delete({ id });
  }
}
