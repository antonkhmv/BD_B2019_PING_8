# Практическое задание 6

> Абдельсалам Шади Мазен, БПИ198

## Задание

-   Для Олимпийских игр 2004 года сгенерируйте список (год рождения, количество игроков, количество золотых медалей), содержащий годы, в которые родились игроки, количество игроков, родившихся в каждый из этих лет, которые выиграли по крайней мере одну золотую медаль, и количество золотых медалей, завоеванных игроками, родившимися в этом году.

```sql
SELECT date_part('year', p.birthdate),
    count(DISTINCT p.name) FILTER ( WHERE r.medal = 'GOLD') as player_count,
    count(r) FILTER ( WHERE r.medal = 'GOLD') as gold_count
FROM olympics o
    JOIN events e on o.olympic_id = e.olympic_id AND o.year = 2004
    JOIN results r on r.event_id = e.event_id
    JOIN players p on p.player_id = r.player_id
GROUP BY date_part('year', p.birthdate);
```

-   Перечислите все индивидуальные (не групповые) соревнования, в которых была ничья в счете, и два или более игрока выиграли золотую медаль.

```sql
SELECT r.event_id, e.name
FROM results r
         JOIN events e on r.event_id = e.event_id AND e.is_team_event = 0
GROUP BY r.event_id, e.name
HAVING count(r.medal) FILTER ( WHERE r.medal = 'GOLD' ) >= 2;
```

-   Найдите всех игроков, которые выиграли хотя бы одну медаль (GOLD, SILVER и
    BRONZE) на одной Олимпиаде. (player-name, olympic-id).

```sql
SELECT p.name, o.olympic_id
FROM players p
         JOIN results r ON p.player_id = r.player_id AND r.medal IN ('GOLD', 'SILVER', 'BRONZE')
         JOIN events e ON r.event_id = e.event_id
         JOIN olympics o ON e.olympic_id = o.olympic_id
GROUP BY p.player_id, p.name, o.olympic_id;
```

-   В какой стране был наибольший процент игроков (из перечисленных в наборе данных), чьи имена начинались с гласной?

```sql
SELECT c.name, country_stats.percentage
FROM countries c
         JOIN (
    SELECT p.country_id, count(*) FILTER ( WHERE p.name ~* '^[aeiou].*$')::float / count(*) AS percentage
    FROM players p
    GROUP BY p.country_id
) AS country_stats ON country_stats.country_id = c.country_id
ORDER BY country_stats.percentage DESC, c.name
LIMIT 1;
```

-   Для Олимпийских игр 2000 года найдите 5 стран с минимальным соотношением количества групповых медалей к численности населения.

```sql
SELECT c.name
FROM countries c
         JOIN players p ON p.country_id = c.country_id
         JOIN results r ON r.player_id = p.player_id
         JOIN events e ON e.event_id = r.event_id
WHERE e.olympic_id = (SELECT olympic_id FROM olympics WHERE year = 2000)
  AND e.is_team_event = 1
GROUP BY c.name, c.population
ORDER BY count(DISTINCT r.event_id)::float / c.population -- Если считать все медали в командном соревовании за одну, иначе без DISTINCT
LIMIT 5;
```
