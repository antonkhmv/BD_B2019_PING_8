import { Module } from '@nestjs/common';
import { BorrowingsService } from './borrowings.service';
import { BorrowingsController } from './borrowings.controller';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Borrowing } from './entities/borrowing.entity';

@Module({
  imports: [
    TypeOrmModule.forFeature([
      Borrowing,
    ])
  ],
  controllers: [BorrowingsController],
  providers: [BorrowingsService]
})
export class BorrowingsModule { }
