# Задание 5

## 1

```sql
SELECT EXTRACT(YEAR FROM Players.birthdate), COUNT(DISTINCT Players.player_id), COUNT(Results.medal)
FROM players
    JOIN Results res ON Players.player_id = res.player_id
    JOIN Events ev ON Results.event_id = ev.event_id
    JOIN Olympics ol ON Events.olympic_id = ol.olympic_id
WHERE Results.medal = 'GOLD' AND Olympics.year = 2004
GROUP BY EXTRACT(YEAR FROM Players.birthdate)
```

## 2

```sql
SELECT Events.event_id, Events.name 
FROM Results r
         JOIN Events e ON e.event_id = r.event_id
WHERE e.is_team_event = 0
  AND r.medal = 'GOLD'
GROUP BY e.name, e.event_id
HAVING count(*) > 1;
```

## 3


```sql
SELECT Players.name, Players.olympic_id
FROM Players
  JOIN Results ON Players.player_id = Results.player_id
  JOIN Events ON Results.event_id = Events.event_id
  JOIN Olympics ON Events.olympic_id = Olympics.olympic_id;
```

## 4

```sql
SELECT leading_vowels.country_id, cast(vowels as decimal)/total FROM (
  SELECT Players.country_id, COUNT(*) as vowels FROM Players
  WHERE LEFT(Players.name, 1) IN ('A', 'E', 'I', 'O', 'U')
  GROUP BY Players.country_id
) AS leading_vowels
JOIN (
  SELECT Players.country_id, count(*) AS total FROM Players
  GROUP BY Players.country_id
) AS countries ON leading_vowels.country_id = countries.country_id
ORDER BY 2 DESC LIMIT 1;
```

## 5

```sql
SELECT country.country_id
FROM Olympics
  JOIN Events 
    ON Events.olympic_id = Olympics.olympic_id
  JOIN Countries 
    ON Сountries.country_id = Players.country_id
  JOIN Players 
    ON Players.player_id = Results.player_id
  JOIN Results 
    ON Events.event_id = Results.event_id
WHERE year = 2000
  AND is_team_event = 1
GROUP BY country.country_id, country.population
ORDER BY CAST(COUNT(Results.medal) AS decimal) / country.population
LIMIT 5;
```

## 6

```sql
SELECT c.country_id
FROM Olympics o
         JOIN Events e ON e.olympic_id = o.olympic_id
         JOIN Results r ON e.event_id = r.event_id
         JOIN Players p ON p.player_id = r.player_id
         JOIN Countries c ON c.country_id = p.country_id
WHERE year = 2000 AND is_team_event = 1
GROUP BY c.country_id, c.population
ORDER BY CAST(COUNT(r.medal) AS decimal) / c.population
LIMIT 5
```
