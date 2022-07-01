const express = require('express')

const app = express();
const port = 3000;
const host = "0.0.0.0";
var IDcounter = 0;
var players = 0;
var playerData = {};
var changingData = {};

app.get("/", (req, res) =>{
	res.send("Server is up")
});

app.get("/join/:usrname/:team", (req, res) =>{
	var data = {
		"name": req.params["usrname"],
		"team": req.params["team"],
		"level": 3.7, //Get this and inventory from a server-stored JSON file
		"inventory": [
			{ item: "pistol" },
			{ item: "keycard" }
		],
	};
	var ID = IDcounter;
	playerData[ID] = data;
	changingData[ID] = {"pos": "(0, 0, 0)", "rot": "(0, 0, 0)", "events": [], "ID": ID};
	res.json({"ID": ID, "level": 3.7});
	for(var subNode in changingData){
		if(ID.toString != subNode["ID"]){
			changingData[subNode].events.push("join " + ID + " " + req.params["usrname"] + " " + req.params["team"]);
		}
	}
	console.log("Player with ID " + ID + " joined the game.");
	console.log(playerData);
	players += 1;
	IDcounter += 1;
})

app.get("/update/:pos/:rot/:ID", (req, res) => {
	//console.log("Recieved Server Request: " + req)
	var ID = req.params["ID"];

	changingData[ID].pos = req.params["pos"];
	changingData[ID].rot = req.params["rot"];
	res.json(changingData); //send only the other players's data later
	//console.log(changingData);
	changingData[ID].events = [];
})

app.get("/leave/:ID", (req, res) =>{
	players -= 1;
	//save data here later
	res.send("recieved");
	var ID = req.params["ID"]
	console.log("Player with ID " + ID + " left the game.");
	delete playerData[ID];
	delete changingData[ID];
	for(var subNode in changingData){
		changingData[subNode].events.push("leave " + ID);
	}
})

app.get("/event/:info", (req, res) => {
	var event = req.params['info'];
	res.send("recieved");
	for(var subNode in changingData){
		changingData[subNode].events.push(event);
	}
	console.log(changingData);
})

app.listen(port, host, () => {
	console.log("Server started on port " + port);
});
