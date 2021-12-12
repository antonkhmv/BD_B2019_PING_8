# Дадугин Егор Артемович БПИ198
# Домашнее задание 10

## Задача 1
### Постройте закрытие атрибута (Attribute Closure )(BD)+

BD+ = BDEGCA

## Задача 2

### А)
#### Определить нетривиальные функциональные зависимости в отношении
ProductNo -> Tax, UnitPrice, ProductName
CustomerNo -> CustomerName
UnitPrice, Quantity -> SubTotal
SubTotal, Tax -> Total
CustomerNo, ProductnNo, OrderDate -> Quantity

### Б)
#### Каковы ключи-кандидаты?

1) Найдем суперключи

ProductNo+ = ProductNo, Tax, UnitPrice, ProductName - не суперключ

CustomerNo+ -> CustomerNo, CustomerName - не суперключ

UnitPrice, Quantity+ -> UnitPrice, Quantity, SubTotal - не суперключ

SubTotal, Tax+ -> SubTotal, Tax, Total - не суперключ

CustomerNo, ProductnNo, OrderDate -> CustomerNo, ProductnNo, OrderDate, Quantity, Tax, UnitPrice, ProductName, SubTotal, Total, CustomerName - суперключ

2) CustomerNo, ProductnNo, OrderDate является ключом-кандидатом
## Задача 3

### А)
#### *Каковы все ключи-кандидаты?

1) Выясним для каждого аттрибута, является ли он суперключом

A+ = AD -> A не суперключ

AB+ = ABDC -> AB - суперключ

AC+ = ACDB -> AC - суперключ

2) AB и AC являются ключами-кандитатами, поэтому ответ: AB и AC

### Б)
#### Преобразуйте R в 3NF, используя алгоритм синтеза.

R1(A, D)
A -> D

R2(A, B, C)
AB -> C

R3(A, C, B)
AC -> B

R2 и R3 по сути являются эквивалентными, по этому нам необходимо оставить одну из них

Пусть в 3NF  будут находиться R1 и R2
