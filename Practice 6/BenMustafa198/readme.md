### 1.
 ```sql
 SELECT EXTRACT(YEAR FROM pl.birthdate), COUNT(DISTINCT pl.player_id), COUNT(res.medal) FROM players pl
 JOIN results res ON pl.player_id = res.player_id
 JOIN events event ON res.event_id = event.event_id
 JOIN olympics olymp ON event.olympic_id = olymp.olympic_id
 WHERE res.medal = 'GOLD' AND olymp.year = 2004
 GROUP BY EXTRACT(YEAR FROM pl.birthdate)
 ```

 ### 2.
 ```sql
 SELECT event.event_id FROM Events event
 JOIN results res ON event.event_id = res.event_id
 WHERE is_team_event = 0
 AND res.medal = 'GOLD'
 GROUP BY event.event_id
 HAVING COUNT(res.medal) > 1
 ```

 ### 3.
 Найдите всех игроков, которые выиграли хотя бы одну медаль (GOLD, SILVER и BRONZE) на одной Олимпиаде. (player-name, olympic-id).
 (Тут на самом деле не очень понятно - которые выиграли хоты бы одну медаль в каждой категории gold, silver и bronze или хотя бы одну медаль вообще)

 ```sql
SELECT player.name, olymp.olympic_id FROM players p
JOIN results r ON player.player_id = r.player_id
JOIN events e ON r.event_id = e.event_id
JOIN olympics olymps ON e.olympic_id = olymp.olympic_id
 ```

 ### 4.
 ```sql
SELECT c.country_id
FROM countries c, players p
WHERE p.name ~* '^[aeiou]' AND c.country_id = p.country_id
GROUB BY c.country_id
ORDER BY count(*) desc
LIMIT 1
 ```

 ### 5.
 ```sql
 SELECT c.country_id FROM olympics o
 JOIN events e ON e.olympic_id = o.olympic_id
 JOIN results r ON e.event_id = r.event_id
 JOIN players p ON p.player_id = r.player_id
 JOIN countries c ON c.country_id = p.country_id
 WHERE year = 2000 AND is_team_event = 1
 GROUP BY c.country_id, c.population
 ORDER BY CAST(COUNT(r.medal) AS decimal) / c.population
 LIMIT 5
 ```
