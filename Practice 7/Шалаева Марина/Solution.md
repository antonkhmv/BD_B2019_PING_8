# Задание 7

## Решение

Для генерации фейковых данных был выбран язык Python и библиотека Faker, которая предоставляет широкий выбор функций для генерации тестовых данных.

Исходный код на Python представлен ниже, а так же в файле HW7.ipynb. Полученные сгенерированные данные находятся в файле InsertInfo.sql.

``` python

from faker import Faker
from datetime import timedelta
import pycountry

def countries_generation(count, fake, result):
    
    country_id_list = []
    
    for _ in range(count):
        
        country_id = fake.unique.country_code('alpha-3')
        
        country_name = None
        while country_name is None or len(country_name) > 40 or country_name.find('\'') != -1:
            country_id = fake.unique.country_code('alpha-3')
            try:
                country_name = pycountry.countries.lookup(country_id).name
            except AttributeError:
                country_name = None
            
        country_id_list.append(country_id)
        
        area_sqkm = fake.random_int(1, 9999999)
        
        population = fake.random_int(1, 999999)
        
        string = "insert into countries values('{0}', '{1}', {2}, {3});"
        values = [country_name, country_id, area_sqkm, population]
        result.append(string.format(*values))

    result.append('')
    return country_id_list

def olympics_generation(count, country_id_list, fake, result):
    
    olympics_id_list = []
    
    for _ in range(count):
        
        olympics_city = fake.city()
        
        olympics_year = fake.unique.random_int(2002, 2018, 4)
        
        olympics_id = olympics_city[:3].upper() + str(olympics_year)
        olympics_id_list.append(olympics_id)
        
        olympics_country = fake.random_choices(country_id_list, length=1)[0]
        
        startdate = fake.date_object()
        startdate = startdate.replace(year=olympics_year)
        enddate = startdate + timedelta(days=30)
        
        string = "insert into olympics values('{0}', '{1}', '{2}', {3}, to_date('{4}', 'yyyy-mm-dd'), to_date('{5}', 'yyyy-mm-dd'));"
        values = [olympics_id, olympics_country, olympics_city, olympics_year, startdate, enddate]
        result.append(string.format(*values))

    result.append('')
    return olympics_id_list
    
def players_generation(count, country_id_list, fake, result):
    
    player_id_list = []
    
    for _ in range(count):
        
        player_name = fake.name()
        while len(player_name) > 40 or player_name.find('\'') != -1:
            player_name = fake.name()
        first_and_last_names = player_name.split(' ')
        player_id = fake.unique.numerify(first_and_last_names[-1][:5] + first_and_last_names[0][:3] + '##').upper()
        player_id_list.append(player_id)
        
        country = fake.random_choices(country_id_list, length=1)[0]

        birthdate = fake.date_of_birth(minimum_age=27, maximum_age=50)
        
        string = "insert into players values('{0}', '{1}', '{2}', to_date('{3}', 'yyyy-mm-dd'));"
        values = [player_name, player_id, country, birthdate]
        result.append(string.format(*values))
        
    result.append('')
    return player_id_list
    
def events_generation(count, olympics_id_list, fake, result):
   
    events_id_list = []
    
    for i in range(count):
        
        event_id = 'E' + str(i)
        events_id_list.append(event_id)
        
        name = fake.random_choices(['10000m', '1000m', '100m', '1500m', '5000m'], length=1)[0] + ' ' + \
            fake.random_choices(['Backstroke', 'Freestyle', 'Hurdles', 'Medley Relay', ''], length=1)[0] + ' ' + \
            fake.random_choices(['Men', 'Women'], length=1)[0]
        
        eventtype = fake.random_choices(['ATH', 'SWI'], length=1)[0]
        
        olympic_id = fake.random_choices(olympics_id_list, length=1)[0]
        
        is_team_event = fake.random_int(0, 1)
        
        if is_team_event == 1:
            num_players_in_team = fake.random_int(2, 6)
        else:
            num_players_in_team = -1
        
        result_noted_in = fake.random_choices(['milliseconds', 'seconds', 'meters', 'points'], length=1)[0]
        
        string = "insert into events values('{0}', '{1}', '{2}', '{3}', {4}, {5}, '{6}');"
        values = [event_id, name, eventtype, olympic_id, is_team_event, num_players_in_team, result_noted_in]
        result.append(string.format(*values))

    result.append('')
    return events_id_list
    
def results_generation(event_id_list, player_id_list, fake, result):
    
    for event_id in event_id_list:
        
        player = fake.random_choices(player_id_list, length=1)[0]

        player_counter = fake.random_int(1, 4)
        
        for i in range(player_counter):
            
            score = fake.pyfloat(positive=True, max_value=100)
            
            medal = fake.random_choices(['GOLD', 'SILVER', 'BRONZE'], length=1)[0]
            
            string = "insert into results values('{0}', '{1}', '{2}', {3});"
            values = [event_id, player, medal, round(score, 2)]
            result.append(string.format(*values))

result = []

country_id_list = countries_generation(80, fake, result)

olympics_id_list = olympics_generation(5, country_id_list, fake, result)

player_id_list = players_generation(800, country_id_list, fake, result)

events_id_list = events_generation(40, olympics_id_list, fake, result)

results_generation(events_id_list, player_id_list, fake, result)

print('\n'.join(result))
```
