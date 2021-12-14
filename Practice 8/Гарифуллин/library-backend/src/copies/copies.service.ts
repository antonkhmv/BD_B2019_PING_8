import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Borrowing } from 'src/borrowings/entities/borrowing.entity';
import { Repository } from 'typeorm';
import { CreateCopyDto } from './dto/create-copy.dto';
import { UpdateCopyDto } from './dto/update-copy.dto';
import { Copy } from './entities/copy.entity';

@Injectable()
export class CopiesService {
  constructor(
    @InjectRepository(Copy)
    private $copy: Repository<Copy>,
  ) { }
  create(createCopyDto: CreateCopyDto) {
    return this.$copy.save({
      book: { isbn: createCopyDto.bookIsbn },
      number: createCopyDto.number,
      shelfPosition: createCopyDto.shelfPosition
    });
  }

  findAll() {
    return this.$copy.find();
  }

  findOne(id: number) {
      return this.$copy.findOneOrFail({ where: { id }, relations: ["book"] });
  }

  update(id: number, updateCopyDto: UpdateCopyDto) {
    return this.$copy.update({ id }, {
      book: { isbn: updateCopyDto.bookIsbn },
      number: updateCopyDto.number,
      shelfPosition: updateCopyDto.shelfPosition
    });
  }

  remove(id: number) {
    return this.$copy.delete({ id });
  }
}
