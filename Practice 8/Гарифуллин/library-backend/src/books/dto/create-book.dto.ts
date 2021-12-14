export class CreateBookDto {
    isbn: string
    title: string
    author: string
    pagesCount: number
    publicationYear: number
    publisherName: string
    categoryNames: string[]
}
