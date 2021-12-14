# Гарифуллин Руслан Ильфатович Задание 6

### Пункт 3
Найдите всех игроков, которые выиграли хотя бы одну медаль (GOLD, SILVER и BRONZE) на одной Олимпиаде. (player-name, olympic-id).
```sql
SELECT p.name, o.olympic_id FROM players p
JOIN results r ON p.player_id = r.player_id
JOIN events e ON r.event_id = e.event_id
JOIN olympics o ON e.olympic_id = o.olympic_id
```
