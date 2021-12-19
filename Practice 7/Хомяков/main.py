from datetime import timedelta, date
import numpy as np
from random import choice
from faker import Faker
import pycountry


def val2str(val):
    if isinstance(val, str):
        return f"'{val}'"
    if isinstance(val, date):
        return f"to_date('{val}', 'yyyy-mm-dd')"
    return str(val)


def generate_insert_statements(name, values):
    ans = []
    for row in values:
        valueList = ", ".join([val2str(val) for val in row])
        ans.append(f'insert into {name} values({valueList});')
    return ans


def generate_values(tables):
    result = {}
    for name, table in tables.items():
        amount = table['amount']
        del table['amount']
        table_values = {}
        for column, func in table.items():
            vals = []
            if isinstance(func, list):
                if func[0] == 'foreign key':
                    tab, col = func[1].split('.')
                    foreign_id = result[tab][col]
                    vals = [func[2](foreign_id) for _ in range(amount)]
                elif func[0] == 'from value':
                    vals = func[1].split(', ')
                    aux = [table_values[val] for val in vals]
                    vals = [func[2](*(v[i] for v in aux)) for i in range(amount)]
                else:
                    vals = func[1]
            else:
                vals = [func() for _ in range(amount)]
            table_values[column] = vals
        result[name] = table_values
    return result


def generate():
    fake = Faker()

    ids = np.random.choice(len(pycountry.countries), size=100, replace=False)
    countries = np.array(list(pycountry.countries))[ids]

    tables = dict(
        countries=dict(
            name=['directly', [country.name for country in countries]],
            country_id=['from value', 'name', lambda x: pycountry.countries.get(name=x).alpha_3],
            area_sqkm=lambda: fake.random_int(int(2e4), int(2e7)),
            population=lambda: fake.random_int(int(5e5), int(1e9)),
            amount=100
        ),

        olympics=dict(
            city=lambda: fake.city(),
            year=lambda: fake.unique.random_int(1900, 2008, 4),
            olympic_id=['from value', 'city, year', lambda c, y: c[:3].upper() + str(y)],
            country_id=['foreign key', 'countries.country_id', choice],
            startdate=['from value', 'year', lambda x: fake.date_object().replace(year=x)],
            enddate=['from value', 'startdate', lambda d: d + timedelta(days=30)],
            amount=10
        ),

        players=dict(
            name=lambda: fake.name(),
            player_id=['from value', 'name',
                       lambda x: fake.unique.numerify(x.split()[1][:5] + x.split()[0][:3] + '##').upper()],
            country_id=['foreign key', 'countries.country_id', choice],
            birthdate=lambda: fake.date_of_birth(minimum_age=47, maximum_age=80),
            amount=1000
        ),

        events=dict(
            event_id=['directly', ['E' + str(i) for i in range(100)]],
            name=lambda: choice(['10000m', '1000m', '100m', '1500m', '5000m']) + ' ' + \
                         choice(['Backstroke', 'Freestyle', 'Hurdles', 'Medley Relay', '']) + ' ' + \
                         choice(['Men', 'Women']),
            eventtype=lambda: choice(['ATH', 'SWI']),
            olympic_id=['foreign key', 'olympics.olympic_id', choice],
            is_team_event=lambda: fake.random_int(0, 1),
            num_players_in_team=['from value', 'is_team_event', lambda x: fake.random_int(2, 6) if x else -1],
            result_noted_in=lambda: choice(['milliseconds', 'seconds', 'meters', 'points']),
            amount=100
        ),

        results=dict(
            event_id=['foreign key', 'events.event_id', choice],
            player_id=['foreign key', 'players.player_id', choice],
            medal=lambda: choice(['GOLD', 'SILVER', 'BRONZE']),
            result=lambda: fake.pyfloat(positive=True, max_value=10.0),
            amount=1000
        )
    )

    order = dict(
        countries='name country_id area_sqkm population',
        olympics='olympic_id country_id city year startdate enddate',
        players='name player_id country_id birthdate',
        events='event_id name eventtype olympic_id is_team_event num_players_in_team result_noted_in',
        results='event_id player_id medal result',
    )

    for name, values in generate_values(tables).items():
        vals = []
        amount = len(list(values.values())[0])
        for i in range(amount):
            vals.append([values[key][i] for key in order[name].split()])
        print('\n'.join(generate_insert_statements(name, vals)))


if __name__ == '__main__':
    generate()