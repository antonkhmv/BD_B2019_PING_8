Reader( ID, LastName, FirstName, Address, BirthDate)

Book ( ISBN, Title, Author, PagesNum, PubYear, PubName)

Publisher ( PubName, PubAdress)

Category ( CategoryName, ParentCat)

Copy ( ISBN, CopyNumber, ShelfPosition)

Borrowing ( ReaderNr, ISBN, CopyNumber, ReturnDate)

BookCat ( ISBN, CategoryName )

---

Reader:
``` js
{
	"ReaderID", 
	"LastName", 
	"FirstName", 
	"Address", 
	"BirthDate",
}
```

BookCopy:
``` js
{
	"ISBN",
	"Title",
	"Author",
	"PagesNum",
	"PubYear",
	"PubName",
	"PubAdress",
	"CopyNumber",
	"ShelfPosition",
	"Category":
	{
		"CategoryName",
		"ParentCat"
	}
}
```

Borrowing:
``` js
{
	"ReaderID",
	"BookCopy",
	"ReturnDate"
}
```
