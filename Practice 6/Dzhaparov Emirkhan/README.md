# Джапаров Эмирхан, БПИ198  

## Задание 6

### Задача 1

``` sql
/*Для Олимпийских игр 2004 года сгенерируйте список
  (год рождения, количество игроков, количество золотых медалей),
  содержащий годы, в которые родились игроки, количество игроков,
  родившихся в каждый из этих лет, которые выиграли по крайней мере
  одну золотую медаль, и количество золотых медалей,
  завоеванных игроками, родившимися в этом году.*/

SELECT date_part('year', birthdate), COUNT(DISTINCT players.player_id), COUNT(medal)
FROM players
    JOIN results
    ON players.player_id = results.player_id
    JOIN events
    ON results.event_id = events.event_id
    JOIN olympics
    ON events.olympic_id = olympics.olympic_id
WHERE city = 'Athens' AND medal = 'GOLD'
GROUP BY date_part('year', birthdate)
```

### Задача 2

``` sql
/*Перечислите все индивидуальные (не групповые)
  соревнования, в которых была ничья в счете,
  и два или более игрока выиграли золотую медаль.*/
select events.event_id, events.name, events.olympic_id, COUNT(*) as num_of_gold_medals
from events
join results on events.event_id = results.event_id
where events.is_team_event = 0 AND medal = 'GOLD'
group by events.event_id, events.name, events.olympic_id
having COUNT(*) > 1;
```
### Задача 3

```sql
/*Найдите всех игроков, которые выиграли хотя бы
одну медаль (GOLD, SILVER и BRONZE) на одной
Олимпиаде. (player-name, olympic-id).*/

/*
 Не понял задание. Сделал так:
 нашел всех спортсменов, которые выигрывали
 хотя бы одну медаль на какой-то из олимпиад.
 */

SELECT players.name, olympics.olympic_id
from players
join results
    on players.player_id = results.player_id
join events
    on events.event_id = results.event_id
join olympics
    on events.olympic_id = olympics.olympic_id
where medal in ('GOLD' , 'SILVER' , 'BRONZE')
group by players.player_id, players.name, olympics.olympic_id
having COUNT(medal) > 0;
```
### Задача 4

```sql
/*В какой стране был наибольший процент игроков
(из перечисленных в наборе данных), чьи имена
начинались с гласной?*/

/* Если нужна только одна страна, даже если их несколько*/
select countries.name,
                sum(
                        case
                            when (players.name LIKE 'A%'
                                or players.name LIKE 'E%'
                                or players.name LIKE 'I%'
                                or players.name LIKE 'O%'
                                or players.name LIKE 'U%')
                                then 1
                            else 0
                            end),
                COUNT(*),
                round(
                        CAST(sum(
                                     case
                                         when (players.name LIKE 'A%'
                                             or players.name LIKE 'E%'
                                             or players.name LIKE 'I%'
                                             or players.name LIKE 'O%'
                                             or players.name LIKE 'U%')
                                             then 1
                                         else 0
                                         end
                                 )
                                 / CAST(COUNT(*) AS float) * 100 as numeric), 2
                    ) as percentage
         from countries
                  join players
                       on countries.country_id = players.country_id
         group by countries.name
         order by percentage DESC
         LIMIT 1;


/*Если нужны все страны с максимальным процентом*/
select *
from (
         select countries.name,
                sum(
                        case
                            when (players.name LIKE 'A%'
                                or players.name LIKE 'E%'
                                or players.name LIKE 'I%'
                                or players.name LIKE 'O%'
                                or players.name LIKE 'U%')
                                then 1
                            else 0
                            end),
                COUNT(*),
                round(
                        CAST(sum(
                                     case
                                         when (players.name LIKE 'A%'
                                             or players.name LIKE 'E%'
                                             or players.name LIKE 'I%'
                                             or players.name LIKE 'O%'
                                             or players.name LIKE 'U%')
                                             then 1
                                         else 0
                                         end
                                 )
                                 / CAST(COUNT(*) AS float) * 100 as numeric), 2
                    ) as percentage
         from countries
                  join players
                       on countries.country_id = players.country_id
         group by countries.name
         /*order by percentage DESC*/
     ) all_percentages
where percentage = (select max(percentage) from
    (select countries.name,
                sum(
                        case
                            when (players.name LIKE 'A%'
                                or players.name LIKE 'E%'
                                or players.name LIKE 'I%'
                                or players.name LIKE 'O%'
                                or players.name LIKE 'U%')
                                then 1
                            else 0
                            end),
                COUNT(*),
                round(
                        CAST(sum(
                                     case
                                         when (players.name LIKE 'A%'
                                             or players.name LIKE 'E%'
                                             or players.name LIKE 'I%'
                                             or players.name LIKE 'O%'
                                             or players.name LIKE 'U%')
                                             then 1
                                         else 0
                                         end
                                 )
                                 / CAST(COUNT(*) AS float) * 100 as numeric), 2
                    ) as percentage
         from countries
                  join players
                       on countries.country_id = players.country_id
         group by countries.name
         /*order by percentage DESC*/
     ) all_percentages);
```

### Задача 5

```sql
/*Для Олимпийских игр 2000 года найдите 5 стран
с минимальным соотношением количества групповых
медалей к численности населения.*/
select c.name, COUNT(medal), c.population, CAST(COUNT(medal) as float) / c.population as proportion
from countries c
join players p on c.country_id = p.country_id
join results r on p.player_id = r.player_id
join events e on r.event_id = e.event_id
where e.is_team_event = 1 and e.olympic_id = 'SYD2000'  
group by c.name, c.population
order by proportion
LIMIT 5 
```



