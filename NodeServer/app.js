const express = require('express')

const app = express();
const port = 8000;
const host = "0.0.0.0";
var IDcounter = 0;
var players = 0;
var playerData = {
};

app.get("/", (req, res) =>{
	res.send("Hello World")
});

app.get("/join/:usrname/:team/", (req, res) =>{
	var data = {
		"name": req.params["usrname"],
		"team": req.params["team"],
		"pos": "(0, 0, 0)", //Change loc and rot
		"rot": "(0, 0, 0)",
		"vel": "(0, 0, 0)",
		"level": 3.7, //Get this and inventory from a server-stored JSON file
		"inventory": [
			{ item: "pistol" },
			{ item: "keycard" }
		],
		"events": {}
	};
	playerData[IDcounter] = data;
	res.json({"ID": IDcounter, "level": 3.7});
	console.log("Player with ID " + IDcounter + " joined the game.")
	console.log(playerData);
	players += 1;
	IDcounter += 1;
})

app.get("/update/:pos/:rot/:vel/:ID", (req, res) => {
	var ID = req.params["ID"];
	playerData[ID].pos = req.params["pos"];
	playerData[ID].rot = req.params["rot"];
	playerData[ID].vel = req.params["vel"];
	res.json(playerData);
	console.log(playerData);
})

app.get("/leave/:ID/", (req, res) =>{
	players -= 1;
	//save data here later
	res.json("recieved");
	console.log("Player with ID " + req.params["ID"] + " left the game.");
	delete playerData[req.params["ID"]];
})

app.get("/user/:ID/0", (req, res) =>{
	var data = {
		"usrname": "testUsrName",
		"usrID": req.params["ID"],
		"usrLvl": 3.7,
		"inventory": [
			{ item: "pistol" },
			{ item: "keycard" }
		]
	};

	res.json(data)
})

 
app.listen(port, host, () => {
	console.log("Server started on port " + port)
});