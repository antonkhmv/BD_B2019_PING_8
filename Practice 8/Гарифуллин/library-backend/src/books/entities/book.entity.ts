import { Borrowing } from "src/borrowings/entities/borrowing.entity";
import { Category } from "src/categories/entities/category.entity";
import { Copy } from "src/copies/entities/copy.entity";
import { Publisher } from "src/publishers/entities/publisher.entity";
import { Column, Entity, JoinTable, ManyToMany, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";

@Entity()
export class Book {
  @PrimaryColumn({ unique: true })
  isbn: string

  @Column()
  title: string

  @Column()
  author: string

  @Column()
  pagesCount: number

  @Column()
  publicationYear: number

  @ManyToOne(type => Publisher, publisher => publisher.books)
  publisher: Publisher

  @OneToMany(type => Borrowing, borrowing => borrowing.book)
  borrowings: Borrowing[]

  @OneToMany(type => Copy, copy => copy.book)
  copies: Copy[]

  @ManyToMany(type => Category)
  @JoinTable()
  categories: Category[]
}
