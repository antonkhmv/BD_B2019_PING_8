const { Client } = require('pg')
const faker = require('faker/locale/ru')
const process = require('process')
require('dotenv').config()

const client = new Client()
client.connect()

const PROCESS_AGRS = process.argv.slice(2)

const parseProcessArg = (argName, defaultValue) => {
	return +(
		PROCESS_AGRS.find(arg => arg.startsWith(`--${argName}`))?.split(
			'='
		)[1] ?? defaultValue
	)
}

const TABLE_SIZES = {
	countries: parseProcessArg('countries', 10),
	olympics: parseProcessArg('olympics', 2),
	players: parseProcessArg('players', 50),
	events: parseProcessArg('events', 20),
	results: parseProcessArg('results', 20),
}

const DB_SCHEMA = {
	countries: `
		name char(40),
		country_id char(3) unique,
		area_sqkm integer,
		population integer
	`,
	olympics: `
		olympic_id char(7) unique,
		country_id char(3),
		city char(50),
		year integer,
		startdate date,
		enddate date,
		foreign key (country_id) references Countries(country_id)
	`,
	players: `
		name char(40),
		player_id char(10) unique,
		country_id char(3),
		birthdate date,
		foreign key (country_id) references Countries(country_id)
	`,
	events: `
		event_id char(7) unique,
		name char(40),
		eventtype char(20),
		olympic_id char(7),
		is_team_event integer check (is_team_event in (0, 1)),
		num_players_in_team integer,
		result_noted_in char(100),
		foreign key (olympic_id) references Olympics(olympic_id)
	`,
	results: `
		event_id char(7),
		player_id char(10),
		medal char(7),
		result float,
		foreign key (event_id) references Events(event_id),
		foreign key (player_id) references players(player_id)
	`,
}

;(async () => {
	// Remove tables if they already exist
	await client.query(
		`DROP TABLE IF EXISTS ${Object.keys(DB_SCHEMA).join(',')}`
	)

	// Create tables with corresponding schemas
	await Promise.all(
		Object.entries(DB_SCHEMA).map(([name, schema]) =>
			client.query(`CREATE TABLE ${name} (${schema})`)
		)
	).catch(err => {
		console.log(`Error while creating tables: ${err}`)
		// process.exit(1)
	})

	// Fill countries table
	let countryIds = new Set()

	await Promise.all(
		Array.apply(null, Array(TABLE_SIZES.countries)).map(() => {
			let countryId

			do {
				countryId = faker.address.countryCode()
			} while (countryIds.has(countryId))

			return client
				.query(
					`INSERT INTO Countries VALUES(
						'${faker.address.country().substr(0, 40)}',
						'${countryId}',
						${Math.round(Math.random() * 17098245 + 1)},
						${Math.round(Math.random() * 1500000000 + 100)}
					)`
				)
				.then(() => countryIds.add(countryId))
		})
	)

	// Fill olympics table
	let olympicIds = new Set()

	await Promise.all(
		Array.apply(null, Array(TABLE_SIZES.olympics)).map(() => {
			let olympicId

			do {
				olympicId = faker.datatype.uuid().substr(0, 7)
			} while (olympicIds.has(olympicId))

			const startDate = faker.date.past()
			const endDate = new Date()
			endDate.setDate(startDate.getDate() + 180)

			return client
				.query(
					`INSERT INTO Olympics VALUES(
						'${olympicId}',
						'${Array.from(countryIds)[faker.datatype.number(countryIds.size - 1)]}',
						'${faker.address.city()}',
						${startDate.getFullYear()},
						'${startDate.toDateString()}',
						'${endDate.toDateString()}'
					)`
				)
				.then(() => olympicIds.add(olympicId))
		})
	)

	// Fill players table
	let playersIds = new Set()

	await Promise.all(
		Array.apply(null, Array(TABLE_SIZES.players)).map(() => {
			let playerId

			do {
				playerId = faker.datatype.uuid().substr(0, 8)
			} while (playersIds.has(playerId))

			return client
				.query(
					`INSERT INTO Players VALUES(
						'${faker.name.firstName()} ${faker.name.lastName()}',
						'${playerId}',
						'${Array.from(countryIds)[faker.datatype.number(countryIds.size - 1)]}',
						'${faker.date.past(50).toDateString()}'
					)`
				)
				.then(() => playersIds.add(playerId))
		})
	)

	// Fill events table
	let eventIds = new Set()

	await Promise.all(
		Array.apply(null, Array(TABLE_SIZES.players)).map((_, i) => {
			const eventId = `E${i}`
			const isTeamEvent = Math.random() > 0.5 ? 1 : 0

			return client
				.query(
					`INSERT INTO Events VALUES(
						'${eventId}',
						'${faker.vehicle.manufacturer().substr(0, 40)}',
						'${faker.vehicle.model().substr(0, 20)}',
						'${Array.from(olympicIds)[faker.datatype.number(olympicIds.size - 1)]}',
						${isTeamEvent},
						${isTeamEvent ? faker.datatype.number(10) : -1},
						'${faker.vehicle.color()}'
					)`
				)
				.then(() => eventIds.add(eventId))
		})
	)

	// Fill results table

	await Promise.all(
		Array.apply(null, Array(TABLE_SIZES.results)).map(() => {
			return client.query(
				`INSERT INTO Results VALUES(
						'${Array.from(eventIds)[faker.datatype.number(eventIds.size - 1)]}',
						'${Array.from(playersIds)[faker.datatype.number(playersIds.size - 1)]}',
						'${['GOLD', 'SILVER', 'BRONZE'][faker.datatype.number(2)]}',
						${Math.random() * 100}
					)`
			)
		})
	)

	console.log('Database successfully filled with fake data')
	process.exit(0)
})()
