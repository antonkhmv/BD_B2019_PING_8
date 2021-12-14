# Практическое задание №6

## Рычков Кирилл, БПИ198

## Задание 1

*Для Олимпийских игр 2004 года сгенерируйте список (год рождения, количество игроков, количество золотых медалей), содержащий годы, в которые родились игроки, количество игроков, родившихся в каждый из этих лет, которые выиграли по крайней мере одну золотую медаль, и количество золотых медалей, завоеванных игроками, родившимися в этом году.

```sql
SELECT EXTRACT(YEAR FROM Players.birthdate), COUNT(DISTINCT Players.player_id), COUNT(Results.medal)
FROM Players players
   JOIN Results results ON players.player_id = results.player_id
   JOIN Events events ON results.event_id = events.event_id
   JOIN Olympics olympics ON events.olympic_id = olympics.olympic_id
WHERE results.medal = 'GOLD' AND olympics.year = 2004
GROUP BY EXTRACT(YEAR FROM Players.birthdate)
```

*Перечислите все индивидуальные (не групповые) соревнования, в которых была ничья в счете, и два или более игрока выиграли золотую медаль.

## Задание 2
```sql
SELECT Events.event_id FROM Events events
JOIN Results results ON events.event_id = results.event_id
WHERE is_team_event = 0 AND results.medal = 'GOLD'
GROUP BY events.event_id
HAVING COUNT(results.medal) > 1
```

*Найдите всех игроков, которые выиграли хотя бы одну медаль (GOLD, SILVER и BRONZE) на одной Олимпиаде. (player-name, olympic-id).

## Задание 3
```sql
SELECT Players.name, Players.olympic_id
FROM Players players
  JOIN Results results ON players.player_id = results.player_id
  JOIN Events events ON results.event_id = events.event_id
  JOIN Olympics olympics ON events.olympic_id = olympics.olympic_id;
```

*В какой стране был наибольший процент игроков (из перечисленных в наборе данных), чьи имена начинались с гласной?

## Задание 4
```sql
SELECT countries.name, countryStats.percentage FROM Countries countries
JOIN(
    SELECT p.country_id, count(*) FILTER ( WHERE p.name ~* '^[aeiou].*$')::float / count(*) AS percentage
    FROM Players players
    GROUP BY players.country_id
) AS countryStats ON countryStats.country_id = countries.country_id
ORDER BY countryStats.percentage DESC, countries.name LIMIT 1;
```

*Для Олимпийских игр 2000 года найдите 5 стран с минимальным соотношением количества групповых медалей к численности населения.

## Задание 5
```sql
SELECT Countries.country_id FROM Olympics olympics
JOIN Events events ON events.olympic_id = olympics.olympic_id
JOIN Results results ON events.event_id = results.event_id
JOIN Players players ON players.player_id = results.player_id
JOIN Countries countries ON countries.country_id = players.country_id
WHERE YEAR = 2000 AND is_team_event = 1
GROUP BY countries.country_id, countries.population
ORDER BY CAST(COUNT(results.medal) AS decimal) / countries.population
LIMIT 5
```