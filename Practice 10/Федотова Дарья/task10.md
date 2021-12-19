**Федотова Дарья БПИ198**
**Задание 10**

**Задача 1**
Отношение (A, B, C, D, E, G) имеет следующие функциональные зависимости:
- AB → C
- C → A
- BC → D
- ACD → B
- D → EG
- BE → C
- CG → BD
- CE → AG

Постройте закрытие атрибута (Attribute Closure )(BD)+
**Решение:** {BD} D → EG {BDEG} BE → C {BCDEG} CE → AG {ABCDEG} 


**Задача 2**
Посмотрите на отношения: Order (ProductNo, ProductName, CustomerNo, CustomerName,OrderDate,UnitPrice, Quantity, SubTotal, Tax, Total)

Ставка налога зависит от Товара (например, 20 % для книг или 30 % для предметов роскоши). В день допускается только один заказ на продукт и клиента (несколько заказов объединяются).

- А) Определить нетривиальные функциональные зависимости в отношении

{ProductNo} → {ProductName, UnitPrice, Tax} 
{CustomerNo} → {CustomerName}
{ProductNo, CustomerNo, OrderDate} → {Quantity} 
{UnitPrice, Quantity} → {SubTotal} 
{SubTotal, Tax} → {Total} 
{Total, Tax} → {SubTotal} 
{Total, SubTotal} → {Tax} 

- Б) Каковы ключи-кандидаты?

{ProductNo, CustomerNo, OrderDate} 


**Задача 3**
Рассмотрим соотношение R(A, B, C, D) со следующими функциональными зависимостями: F = {A→D, AB→ C, AC→ B}

- А) \*Каковы все ключи-кандидаты?

AttrClosure(F,AB) = {AB}->{ABD}->{ABCD} => AB is superkey AttrClosure(F,A) = {A}->{AD}
AttrClosure(F,B) = {B}
AB - ключ-кандидат

AttrClosure(F,AC) = {AC}->{ACD}->{ABCD} => AC is superkey AttrClosure(F,A) = {A}->{AD}
AttrClosure(F,C) = {C}
AC - ключ-кандидат

- Б) Преобразуйте R в 3NF, используя алгоритм синтеза.

A→D => R1 = {A, D}
AB→C => R2 = {A, B, C}
AC→B => R3 = {A, C, B}
R3 ⊆ R2 => R1 = {A, D}; R2 = {A, B, C} 

