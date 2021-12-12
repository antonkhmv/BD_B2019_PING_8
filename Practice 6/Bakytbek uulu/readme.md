## Задание 6

Установить PostrgeSQL локально. Создать базу данных. 

Пример датасета для Oracle (для примера): http://pastebin.com/dEqPSAk3 

### Описание дата сета
У спортсмена есть олимпийское удостоверение, имя, пол, страна и дата рождения.
У каждой олимпиады есть год, сезон (летний или зимний), страна, где она проходила, и город.
Каждый спортсмен в базе данных участвует по крайней мере в одной олимпиаде. Спортсмен может участвовать в нескольких соревнованиях на одной Олимпиаде и фактически может участвовать более чем в одной Олимпиаде. Например, Майкл Армстронг участвовал в соревнованиях по плаванию, дайвингу и водному поло. Ян Торп участвовал в летних Олимпийских играх 2000 года в Сиднее и завоевал 3 золотые и 2 серебряные медали. В Афинах 2004 года он завоевал две золотые, одну серебряную и одну бронзовую медали.
У события есть название вида спорта, название события, место проведения, а также запланированное время и дата, какие спортсмены должны были участвовать в соревнованиях, в каких соревнованиях и как они разместились, и кто был победителем (победителями) события (например, какой медалью они были награждены). Вы можете предположить, что нулевые значения используются для победителей и мест размещения до тех пор, пока событие не будет проведено.
Соревнования на Олимпийских играх могут быть как индивидуальными, так и командными. Если это командное мероприятие, мы хотим знать, кто был членом каждой команды.<br/>

### Задание 

Напишие SQL запросы

* Для Олимпийских игр 2004 года сгенерируйте список (год рождения, количество игроков, количество золотых медалей), содержащий годы, в которые родились игроки, количество игроков, родившихся в каждый из этих лет, которые выиграли по крайней мере одну золотую медаль, и количество золотых медалей, завоеванных игроками, родившимися в этом году.
```sql
SELECT date_part('year', p.birthdate) AS yearOfBirth,
       count(DISTINCT r.player_id)    AS countOfPlayers,
       count(*)                       AS countOfGoldMedals
FROM players p
         JOIN results r ON p.player_id = r.player_id
         JOIN events e ON r.event_id = e.event_id
         JOIN olympics o ON e.olympic_id = o.olympic_id
WHERE o.year = 2004
  AND r.medal = 'GOLD'
GROUP BY date_part('year', p.birthdate);
```
* Перечислите все индивидуальные (не групповые) соревнования, в которых была ничья в счете, и два или более игрока выиграли золотую медаль.
```sql
SELECT DISTINCT e
FROM results leftRes
         JOIN results rightRes
              ON leftRes.event_id = rightRes.event_id
                  AND leftRes.medal = rightRes.medal
                  AND leftRes.medal = 'GOLD'
                  AND leftRes.player_id <> rightRes.player_id
         JOIN events e on e.event_id = leftRes.event_id
    AND e.is_team_event = 0
```
* Найдите всех игроков, которые выиграли хотя бы одну медаль (GOLD, SILVER и
BRONZE) на одной Олимпиаде. (player-name, olympic-id).
```sql
SELECT p.name, e.olympic_id
FROM results r
         JOIN
     players p ON r.player_id = p.player_id
         JOIN events e on r.event_id = e.event_id
```
* В какой стране был наибольший процент игроков (из перечисленных в наборе данных), чьи имена начинались с гласной?
```sql
SELECT *
FROM (SELECT country_id,
             count(*) as cnt
      FROM players p
      WHERE lower(substr(name, 1, 1)) in ('a', 'e', 'i', 'o', 'u')
      GROUP BY country_id) cnum
ORDER BY cnt DESC
LIMIT 1
```
* Для Олимпийских игр 2000 года найдите 5 стран с минимальным соотношением количества групповых медалей к численности населения.
```sql
SELECT *
FROM (SELECT p.country_id, c.population, count(*) as medals
      FROM (SELECT *
            FROM olympics
            WHERE year = 2000) ol2000
               JOIN events e
                    ON e.olympic_id = ol2000.olympic_id
                        AND e.is_team_event = 1
               JOIN results r on e.event_id = r.event_id
               JOIN players p on p.player_id = r.player_id
               JOIN countries c on c.country_id = p.country_id
      GROUP BY c.population, p.country_id) as cpm
ORDER BY medals / cpm.population
LIMIT 5

```