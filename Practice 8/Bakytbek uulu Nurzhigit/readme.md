# API

### Создание сущности Reader

Отправить POST запрос на http://localhost:8080/readers c телом

```json
{
    "name" : "Emir",
    "surname" : "Aparov",
    "address" : "Moscow",
    "birthday" : "2001-09-11"
}
```
![](reader_post.jpg)

![](after_reader_post.jpg)


### Редактирование сущности Reader

Отправляем PUT запрос на http://localhost:8080/readers с телом

```json
{
    "id" : 3,
    "name" : "Emirbek",
    "surname" : "Aparov",
    "address" : "Moscow",
    "birthday" : "2001-09-11"
}
```

![](reader_put.jpg)

![](after_reader_put.jpg)

P.S Нужно посмотреть какой id дал СУБД и подставить в значение ID, у меня в данном сдучае id = 3.

### Чтение сущности Reader

Отправляем GET запрос на http://localhost:8080/readers/3, где последнее число - id читателя.

![](reader_get.jpg)

### Удаление сущности Reader

Отправляем DELETE запрос на http://localhost:8080/readers/3, где последнее число - id читателя.

![](reader_delete.jpg)

![](after_reader_delete.jpg)

### BookCopy

![](copy_post.jpg)

![](copy_post_after.jpg)

![](copy_get.jpg)

### Rent

![](rent_post.jpg)

![](rent_post_after.jpg)

![](rent_get.jpg)
Ответ
```json
{
    "key": {
        "reader": {
            "id": 1,
            "name": "Nurzhigit",
            "surname": "Sydykov",
            "address": "Moscow",
            "birthday": "2000-09-06"
        },
        "bkkey": {
            "key": {
                "copyNumber": 1,
                "isbn": 2
            },
            "position": 2
        }
    },
    "returnDate": "2021-12-30"
}
```
![](rent_delete.jpg)

![](rent_delete_after.jpg)

Создал только 3 сущности, так как это занимает много времени.
Нужно указать пароль к бд в файле application.properies в паке resoures.









