==================================================================
==================================================================
=========               Roulette Game                    =========
==================================================================
==================================================================

The solution is designed following the SOLID principle and the 
structural design pattern.

It has been written in C# with .Net Core 5 and SQLite.

When the project is launched, it will use swagger.
It will also create the "roulette.db" with all the tables if it 
doesn't exist.

1. Database structure
=====================
The database is made in SQLite and is named "roulette.db".
It consists of only 2 tables.

1.1. Bets
=========
This table has the following columns:
Id           INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
BetType      TEXT    NOT NULL,
BetValue     TEXT    NOT NULL,
Amount       REAL    NOT NULL,
Payout       REAL,
IsWinningBet INTEGER NOT NULL,
SpinId       INTEGER NULL,
WasPayedout  INTEGER NOT NULL

1.2 Spins
=========
This table has the following columns:
Id           INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
Number       INTEGER NOT NULL,
Colour       TEXT    NOT NULL,
Time         TEXT    NOT NULL


2. Endpoints
============
There  are 4 main endpoint:

- "placebet"
- "spin"
- "payout"
- "showPreviousSpins"

2.1. PlaceBet
=============
An HttpPost endpoint that takes a PlaceBetRequest object and return a 200 Status code if successful.
Its signature is: public async Task<IActionResult> PlaceBet(PlaceBetRequest request)

2.1.1. PlaceBetRequest
======================
The object consists of:

string BetType // e.g., "number", "color"
string BetValue // e.g., "17", "red", "black", "green"
decimal Amount

2.2. Spin
=========
An HttpPost endpoint that takes no parameters but will return to the front end a 200 Status code with a SpinResult object if successful.
Its signature is: public async Task<IActionResult> Spin()

2.2.1. SpinResult
=================
The object consists of: 

int Number
string Colour

2.3. Payout
===========
An HttpPost endpoint that takes no parameters and return a 200 Status code if successful.

2.4. ShowPreviousSpins
======================
An HttpGet endpoint that takes no parameters but will return to the front end a 200 Status code with a PreviousSpinsResponse object if successful.

2.4.1. PreviousSpinsResponse
============================
The object consists of: 

int Number
string Colour
DateTime Time