# Дадугин Егор Артемович БПИ198
# Домашнее задание 6

### Для Олимпийских игр 2004 года сгенерируйте список (год рождения, количество игроков, количество золотых медалей), содержащий годы, в которые родились игроки, количество игроков, родившихся в каждый из этих лет, которые выиграли по крайней мере одну золотую медаль, и количество золотых медалей, завоеванных игроками, родившимися в этом году.

``` sql
select date_part('year', p.birthdate) as year,
       count(distinct p.name) filter (where r.medal = 'GOLD') as players_count,
       count(r) filter (where r.medal = 'GOLD') as gold_medals_count
from olympics o
join events e on o.olympic_id = e.olympic_id and o.year = 2004
join results r on e.event_id = r.event_id
join players p on r.player_id = p.player_id
group by date_part('year', p.birthdate)
```

### Перечислите все индивидуальные (не групповые) соревнования, в которых была ничья в счете, и два или более игрока выиграли золотую медаль.

``` sql
select e.event_id, e.name
from events e
join results r on e.event_id = r.event_id
where e.is_team_event = 0
group by e.event_id, e.name
having count(r.medal) filter ( where r.medal = 'GOLD' ) >=2
```

### Найдите всех игроков, которые выиграли хотя бы одну медаль (GOLD, SILVER и BRONZE) на одной Олимпиаде. (player-name, olympic-id).

``` sql
select distinct on (1) p.name, e.olympic_id
from results r
join players p on r.player_id = p.player_id
join events e on r.event_id = e.event_id
join olympics o on e.olympic_id = o.olympic_id
group by p.name, e.olympic_id, p.player_id
```

### В какой стране был наибольший процент игроков (из перечисленных в наборе данных), чьи имена начинались с гласной?

``` sql
select c.name, statistics.percents
from countries c
join (
    select p.country_id, count(*)
        filter (where p.name ~* '^[aeiouy].*$' ) :: float / count(*) as percents
    from players p
    group by p.country_id
    ) as statistics on c.country_id = statistics.country_id
order by statistics.percents desc, c.name
limit 1
```

### Для Олимпийских игр 2000 года найдите 5 стран с минимальным соотношением количества групповых медалей к численности населения.

``` sql
select c.name
from countries c
join players p on c.country_id = p.country_id
join results r on p.player_id = r.player_id
join events e on r.event_id = e.event_id
where e.is_team_event = 1 and e.olympic_id in (select olympics.olympic_id from olympics where year = 2000)
group by c.name, c.population
order by count(*) :: float / c.population
limit 5
```
