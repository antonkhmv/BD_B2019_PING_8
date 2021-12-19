# Дадугин Егор Артемович БПИ198
# Домашнее задание 12

## Формат для книг
```json
{ 
    "isbn": ,
    "year": ,
    "name": ,
    "author": , 
    "numPages": , 
    "copyNumber": , 
    "categories":[
        {
            "id": , 
            "name": , 
            "parentCategory": 
            
        },
        {
            "id": , 
            "name": , 
            "parentCategory": 
        }
    ],
    "publisher":{
                    "publisherName": , 
                    "publisherAddress": 
    },
    "position": 
} 
```

## Формат для читателей
```json
{ 
    "id": ,
    "name": , 
    "surname": , 
    "address": ,
    "birthDate": , 
} 
```

## Формат для бронирования
```json
{ 
    "id": ,

    "reader":{ 
        "id": ,
        "name": ,
        "surname": ,
        "address": ,
        "birthDate": ,
    },

    "books":[ 
        { 
            "isbn": ,
            "year": ,
            "name": ,
            "author": ,
            "numPages": ,
            "copyNumber": ,
            "category":[
                {
                    "id": ,
                    "name": ,
                    "parentCategory": 
                }, 
                {
                    "id": ,
                    "name": ,
                    "parentCategory": 
                }
            ],
            "publisher":{
                "publisherName": ,
                "publisherAddress":
            },
            "position": 
        }, 

        { 
            "isbn": ,
            "year": ,
            "name": ,
            "author": ,
            "numPages": ,
            "copyNumber": ,
            "category":[
                {
                    "id": ,
                    "name": ,
                    "parentCategory": 
                }, 
                {
                    "id": ,
                    "name": ,
                    "parentCategory": 
                }
            ],
            "publisher":{
                "publisherName": ,
                "publisherAddress":
            },
            "position": 
        }, 
    ],

    "returnDate": 
} 
```

## Формат для категорий
```json
{ 
    "id": ,
    "name": ,
    "parentCategory": ,
}
```
