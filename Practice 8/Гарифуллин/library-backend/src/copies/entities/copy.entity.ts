import { Book } from "src/books/entities/book.entity";
import { Borrowing } from "src/borrowings/entities/borrowing.entity";
import { Column, Entity, ManyToOne, OneToMany, PrimaryColumn, PrimaryGeneratedColumn, Unique } from "typeorm";

@Entity()
@Unique(["book", "number"])
export class Copy {
  @PrimaryGeneratedColumn()
  id: number

  @Column()
  number: number

  @Column()
  shelfPosition: number

  @ManyToOne(type => Book, book => book.copies)
  book: Book

  @OneToMany(type => Borrowing, borrowing => borrowing.copy)
  borrowings: Borrowing[]
}