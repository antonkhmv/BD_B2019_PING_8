import { Entity, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";

@Entity()
export class Category {
  @PrimaryColumn({unique: true})
  name: string

  @ManyToOne(type => Category, category => category.children)
  parent: Category;

  @OneToMany(type => Category, category => category.parent)
  children: Category[];
}