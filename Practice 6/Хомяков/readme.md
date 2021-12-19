## Задание 6

- Для Олимпийских игр 2004 года сгенерируйте список (год рождения, количество игроков, количество золотых медалей), содержащий годы, в которые родились игроки, количество игроков, родившихся в каждый из этих лет, которые выиграли по крайней мере одну золотую медаль, и количество золотых медалей, завоеванных игроками, родившимися в этом году.

```sql
SELECT
    extract(
        year
        FROM
            pl.birthdate
    ) AS birth_year,
    Count(DISTINCT pl.name) FILTER (
        WHERE
            r.medal = 'GOLD'
    ) AS num_players,
    Count(*) FILTER (
        WHERE
            r.medal = 'GOLD'
    ) AS num_medals
FROM
    players pl
    JOIN results r ON r.player_id = pl.player_id
    JOIN EVENTS e ON e.event_id = r.event_id
WHERE
    e.olympic_id = (
        SELECT
            olympic_id
        FROM
            olympics
        WHERE
            year = 2004
    )
GROUP BY
    extract(
        year
        FROM
            pl.birthdate
    );
```

- Перечислите все индивидуальные (не групповые) соревнования, в которых была ничья в счете, и два или более игрока выиграли золотую медаль.

```sql
SELECT
    e.event_id,
    e.name
FROM
    results r
    JOIN EVENTS e ON e.event_id = r.event_id
WHERE
    e.is_team_event = 0
    AND r.medal = 'GOLD'
GROUP BY
    e.name,
    e.event_id
HAVING
    Count(*) > 1;
```

- Найдите всех игроков, которые выиграли хотя бы одну медаль (GOLD, SILVER и BRONZE) на одной Олимпиаде. (player-name, olympic-id).

```sql
SELECT
    o.olympic_id,
    p.name
FROM
    results r
    JOIN players p ON p.player_id = r.player_id
    JOIN EVENTS e ON e.event_id = r.event_id
    JOIN olympics o ON o.olympic_id = e.olympic_id
GROUP BY
    p.name,
    p.player_id,
    o.olympic_id;
```

- В какой стране был наибольший процент игроков (из перечисленных в наборе данных), чьи имена начинались с гласной?

```sql
SELECT
    name
FROM
    countries
WHERE
    country_id = (
        SELECT
            p.country_id
        FROM
            players p
        GROUP BY
            p.country_id
        ORDER BY
            Count(*) FILTER (
                WHERE
                    p.name ~ '^[AEOIU].*$'
            ) :: DECIMAL / Count(*) DESC
        LIMIT
            1
    );
```

- Для Олимпийских игр 2000 года найдите 5 стран с минимальным соотношением количества групповых медалей к численности населения.

```sql
SELECT
    c.name
FROM
    results r
    JOIN players p ON r.player_id = p.player_id
    JOIN EVENTS e ON e.event_id = r.event_id
    JOIN countries c ON c.country_id = p.country_id
WHERE
    e.is_team_event = 1
    AND e.olympic_id = (
        SELECT
            olympic_id
        FROM
            olympics
        WHERE
            year = 2000
    )
GROUP BY
    c.name,
    c.country_id,
    c.population
ORDER BY
    COUNT(*) :: DECIMAL / c.population
LIMIT
    5;
```
