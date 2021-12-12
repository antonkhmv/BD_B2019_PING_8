
## Для Олимпийских игр 2004 года сгенерируйте список (год рождения, количество игроков, количество золотых медалей), содержащий годы, в которые родились игроки, количество игроков, родившихся в каждый из этих лет, которые выиграли по крайней мере одну золотую медаль, и количество золотых медалей, завоеванных игроками, родившимися в этом году.

``` sql
select 
  date_part('YEAR', p.Birthdate) as Year,
  count distinct p.Player_Id 
    filter where r.Medal = 'GOLD' as cnt_players,
  count r.Medal 
    filter where r.medal = 'GOLD' as cnt_medals
from 
  Events e join Olympics o
    on o.Olympic_Id = e.Olympic_Id and o.Year = 2004
  join Results r
    on r.Event_Id = e.Event_Id
  join Players p
    on p.Player_Id = r.Player_Id
group by Year;
```

## Перечислите все индивидуальные (не групповые) соревнования, в которых была ничья в счете, и два или более игрока выиграли золотую медаль.

``` sql
select 
  e.Event_Id, 
  e.Name
from 
  Results r join Events e
    on r.Event_Id = e.Event_Id
where 
  r.Medal = 'GOLD' 
  and e.Is_Team_Event = 0
group by 
  e.Event_Id, e.Name
having count(r.Medal) >= 2;
```

## Найдите всех игроков, которые выиграли хотя бы одну медаль (GOLD, SILVER и BRONZE) на одной Олимпиаде. (player-name, olympic-id).

``` sql
select
  p.Name, o.Olympic_Id
from
  Players p join Results r
    on r.Player_Id = p.Player_Id
  join Events e
    on e.Event_Id = r.Event_Id
  join Olympics o
    on o.Olympic_Id = e.Olympic_Id
where
  r.medal in ('GOLD', 'SILVER', 'BRONZE')
group by
  p.Name, o.Olympic_Id;
```

## В какой стране был наибольший процент игроков (из перечисленных в наборе данных), чьи имена начинались с гласной?

``` sql
with c_list as (
select
  p.Country_Id,
  count(*) filter (WHERE p.name ~* '^[AEIOU].*$')::float
  / count(*)
  as first_vowel_letter_percentage
from 
  Players p
group by
  p.Country_Id)
select 
  c.Name
from 
  Countries c join c_list
    on c_list.Country_Id = c.Country_Id
order by
  countries_list.first_vowel_letter_percentage desc,
  c.Name 
limit 1;
```

## Для Олимпийских игр 2000 года найдите 5 стран с минимальным соотношением количества групповых медалей к численности населения.

``` sql
select 
  c.Name,
  c.Population
from
  Players p join Countries c
    on c.Country_Id = p.Country_Id
  join Results r
    on r.Player_Id = p.Player_Id
  join Events e
    on e.Event_Id = r.Event_Id
  join Olympics o
    on o.Olympic_Id = e.Olympic_Id 
where
  e.Is_Team_Event = 1
  and o.Year = 2000
group by
  c.Name, c.Population
order by
  cast(count(r.Medal) AS decimal) / c.Population
```
