import { Book } from "src/books/entities/book.entity";
import { Copy } from "src/copies/entities/copy.entity";
import { Reader } from "src/readers/entities/reader.entity";
import { Column, Entity, ManyToOne, PrimaryGeneratedColumn, Unique } from "typeorm";

@Entity()
@Unique(["reader", "book", "copy"])
export class Borrowing {
  @PrimaryGeneratedColumn()
  id: number

  @ManyToOne(type => Reader, reader => reader.borrowings)
  reader: Reader

  @ManyToOne(type => Book, book => book.borrowings)
  book: Book

  @ManyToOne(type => Copy, copy => copy.borrowings)
  copy: Copy
  
  @Column()
  returnDate: Date
}