const { Client } = require("pg");
const faker = require("faker");
const process = require("process");

const client = new Client();
client.connect().catch(error => {
    console.error(error);
    process.exit(1);
});

const tableSizes = {
    Countries: +(process.argv.find(s => s.startsWith("--countries-count="))?.split("=")[1] ?? 20),
    Olympics: +(process.argv.find(s => s.startsWith("--olympics-count="))?.split("=")[1] ?? 2),
    Players: +(process.argv.find(s => s.startsWith("--players-count="))?.split("=")[1] ?? 20),
    Events: +(process.argv.find(s => s.startsWith("--events-count="))?.split("=")[1] ?? 20),
    Results: +(process.argv.find(s => s.startsWith("--results-count="))?.split("=")[1] ?? 20),
};

console.log(tableSizes);

const tables = {
    Countries: `
        name char(40),
        country_id char(3) unique,
        area_sqkm integer,
        population integer
    `,
    Olympics: `
        olympic_id char(7) unique,
        country_id char(3),
        city char(50),
        year integer,
        startdate date,
        enddate date,
        foreign key (country_id) references Countries(country_id)
	`,
    Players: `
        name char(40),
        player_id char(10) unique,
        country_id char(3),
        birthdate date,
        foreign key (country_id) references Countries(country_id)
    `,
    Events: `
        event_id char(7) unique,
        name char(40),
        eventtype char(20),
        olympic_id char(7),
        is_team_event integer check (is_team_event in (0, 1)),
        num_players_in_team integer,
        result_noted_in char(100),
        foreign key (olympic_id) references Olympics(olympic_id)
    `,
    Results: `
        event_id char(7),
        player_id char(10),
        medal char(7),
        result float,
        foreign key (event_id) references Events(event_id),
        foreign key (player_id) references players(player_id)
    `
};

(async () => {
    await client.query(`DROP TABLE ${Object.keys(tables).join(",")}`).catch(() => null);

    await Promise.all(
        Object.entries(tables)
            .map(
                ([tableName, tableSchema]) => client.query(`CREATE TABLE ${tableName} (${tableSchema})`)
                    .catch(error => {
                        console.error(error);
                        process.exit(1);
                    })
            )
    );

    const countryCodes = [];
    const genCountryCode = () => faker.address.countryCode() + (Math.floor(Math.random() * 10));
    await Promise.all(
        new Array(tableSizes.Countries).fill(0).map((_v, i) => {
            let countryCode;

            do {
                countryCode = genCountryCode();
            } while (countryCodes.includes(countryCode));

            const query = `INSERT INTO Countries VALUES(
                '${faker.address.country().substring(0, 40).replace(/'/g, "")}',
                '${countryCode}', 
                ${Math.round(Math.random() * 10000000)}, 
                ${Math.round(Math.random() * 1000000000)}
            )`
            return client.query(query).then(() => countryCodes.push(countryCode));
        })
    );

    const olympicIds = [];
    await Promise.all(
        new Array(tableSizes.Olympics).fill(0).map((_v, i) => {
            const oid = faker.git.commitSha().substring(0, 7).toUpperCase();
            const date = faker.date.past();
            const futureDate = new Date(date);
            futureDate.setDate(date.getDate() + 120);
            const countryId = countryCodes[Math.floor(Math.random() * countryCodes.length)];
            return client.query(`INSERT INTO Olympics VALUES(
                '${oid}',
                '${countryId}',
                '${faker.address.cityName()}',
                ${date.getFullYear()},
                '${date.toISOString()}',
                '${futureDate.toISOString()}'
            )`).then(() => olympicIds.push(oid))
        })
    );

    const playerIds = [];
    await Promise.all(
        new Array(tableSizes.Players).fill(0).map((_v, i) => {
            const pid = faker.git.commitSha().substring(0, 10).toUpperCase();
            const date = faker.date.past();
            playerIds.push(pid);
            const countryId = countryCodes[Math.floor(Math.random() * countryCodes.length)];
            const query = `INSERT INTO Players VALUES(
                '${faker.name.findName().replace(/'/g, "")}',
                '${pid}',
                '${countryId}',
                '${date.toISOString()}'
            )`;
            return client.query(query);
        })
    );

    const eventIds = [];
    await Promise.all(
        new Array(tableSizes.Events).fill(0).map((_v, i) => {
            const eid = `E${i}`;
            const is_team = Math.round(Math.random());
            return client.query(`INSERT INTO Events VALUES(
                '${eid}',
                '${faker.company.companyName().replace(/'/g, "")}',
                '${faker.company.companySuffix().toUpperCase()}',
                '${olympicIds[Math.floor(Math.random() * olympicIds.length)]}',
                ${is_team},
                ${is_team ? Math.floor(Math.random() * 32 + 1) : -1},
                '${faker.commerce.color()}'
            )`).then(() => eventIds.push(eid))
        })
    );

    await Promise.all(
        new Array(tableSizes.Results).fill(0).map((_v, i) => {
            const eventId = eventIds[Math.floor(Math.random() * eventIds.length)];
            const playerId = playerIds[Math.floor(Math.random() * playerIds.length)];
            return client.query(`INSERT INTO Results VALUES(
                '${eventId}',
                '${playerId}',
                '${["GOLD", "SILVER", "BRONZE"][Math.floor(Math.random() * 3)]}',
                ${Math.random() * 1000}
            )`)
        })
    );
})();
