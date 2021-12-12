### Для Олимпийских игр 2004 года сгенерируйте список (год рождения, количество игроков, количество золотых медалей), содержащий годы, в которые родились игроки, количество игроков, родившихся в каждый из этих лет, которые выиграли по крайней мере одну золотую медаль, и количество золотых медалей, завоеванных игроками, родившимися в этом году.

``` sql
SELECT DATE_PART('YEAR', p.Birthdate) AS birthdate_year,
  COUNT(DISTINCT p.Player_Id) FILTER (WHERE r.Medal = 'GOLD'),
  COUNT(r.Medal) FILTER (WHERE r.medal = 'GOLD')

FROM Events e
  
  JOIN Olympics o
  ON o.Olympic_Id = e.Olympic_Id AND o.Year = 2004
  
  JOIN Results r
  ON r.Event_Id = e.Event_Id
  
  JOIN Players p
  ON p.Player_Id = r.Player_Id
  
GROUP BY birthdate_year;
```

### Перечислите все индивидуальные (не групповые) соревнования, в которых была ничья в счете и два или более игрока выиграли золотую медаль.

``` sql
SELECT e.Event_Id, e.Name

FROM Results r

  JOIN Events e 
  ON r.Event_Id = e.Event_Id AND
    e.Is_Team_Event = 0

WHERE r.Medal = 'GOLD'

GROUP BY e.Event_Id, e.Name
HAVING COUNT(r.Medal) >= 2;
```

### Найдите всех игроков, которые выиграли хотя бы одну медаль (GOLD, SILVER и BRONZE) на одной Олимпиаде. (player-name, olympic-id).

``` sql
SELECT p.Name, o.Olympic_Id

FROM Players p

  JOIN Results r
  ON r.Player_Id = p.Player_Id
  AND r.medal IN ('GOLD', 'SILVER', 'BRONZE')
  
  JOIN Events e
  ON e.Event_Id = r.Event_Id
  
  JOIN Olympics o
  ON o.Olympic_Id = e.Olympic_Id
  
GROUP BY p.Name, o.Olympic_Id;
```

### В какой стране был наибольший процент игроков (из перечисленных в наборе данных), чьи имена начинались с гласной?

``` sql
SELECT c.Name
FROM Countries c

	JOIN (SELECT p.Country_Id,
		  COUNT(*) FILTER (WHERE p.name ~* '^[AEIOU].*$')::float
		  	/ COUNT(*) AS first_vowel_letter_percentage
    
	FROM Players p
	GROUP BY p.Country_Id)
	
	AS countries_list
	ON countries_list.Country_Id = c.Country_Id
	
ORDER BY countries_list.first_vowel_letter_percentage DESC,
c.Name LIMIT 1;
```

### Для Олимпийских игр 2000 года найдите 5 стран с минимальным соотношением количества групповых медалей к численности населения.

``` sql
SELECT c.Name, c.Population
FROM Players p
  
  JOIN Countries c
  ON c.Country_Id = p.Country_Id

  JOIN Results r
  ON r.Player_Id = p.Player_Id
  
  JOIN Events e
  ON e.Event_Id = r.Event_Id AND e.Is_Team_Event = 1
  
  JOIN Olympics o
  ON o.Olympic_Id = e.Olympic_Id AND o.Year = 2000

GROUP BY c.Name, c.Population
ORDER BY CAST(COUNT(r.Medal) AS decimal) / c.Population
LIMIT 5;
```
