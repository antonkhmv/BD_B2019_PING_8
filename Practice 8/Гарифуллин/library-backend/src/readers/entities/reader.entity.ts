import { Borrowing } from "src/borrowings/entities/borrowing.entity";
import { Column, Entity, OneToMany, PrimaryGeneratedColumn } from "typeorm";

@Entity()
export class Reader {
  @PrimaryGeneratedColumn()
  number: number

  @Column()
  lastName: string

  @Column()
  firstName: string

  @Column()
  address: string

  @Column()
  birthDate: Date

  @OneToMany(type => Borrowing, borrowing => borrowing.reader)
  borrowings: Borrowing[]
}