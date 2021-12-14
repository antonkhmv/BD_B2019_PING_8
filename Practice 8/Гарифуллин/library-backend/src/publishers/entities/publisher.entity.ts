import { Book } from "src/books/entities/book.entity";
import { Column, Entity, OneToMany, PrimaryColumn } from "typeorm";

@Entity()
export class Publisher {
  @PrimaryColumn({unique: true})
  name: string

  @Column()
  address: string

  @OneToMany(type => Book, book => book.publisher)
  books: Book[]
}