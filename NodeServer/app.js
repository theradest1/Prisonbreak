const express = require('express')

const app = express();
const port = 3000;
const host = "0.0.0.0";
const timoutMax = 3; //seconds
const timoutCheck = 1000; //ms
var IDcounter = 0;
var players = 0;
var playerData = {};
var changingData = {};

setInterval(checkDisconnectTimers, timoutCheck);

function checkDisconnectTimers(){
	var playersToRemove = [];
	for(var subNode in playerData){
		if(playerData[subNode].timout <= timoutMax){
			playerData[subNode].timout += timoutCheck/1000;
		}
		else{
			playersToRemove.push(playerData[subNode].ID);
		}
	}
	for(var player in playersToRemove){
		var ID = playersToRemove[player];
		console.log("Player with ID " + ID + " timed out.");
		delete playerData[ID];
		delete changingData[ID];
		for(var subNode in changingData){
			changingData[subNode].events.push("leave " + ID);
		}
	}
	
}

app.get("/", (req, res) =>{
	res.send("Server is up");
});

app.get("/players", (req, res) =>{
	res.send(players.toString());
});

app.get("/join/:usrname/:team", (req, res) =>{
	var data = {
		"name": req.params["usrname"],
		"ID": IDcounter,
		"team": req.params["team"],
		"level": 3.7, //Get this and inventory from a server-stored JSON file
		"health": 100,
		"timout": 0,
		"inventory": [
			{ item: "pistol" },
			{ item: "keycard" }
		],
	};
	var ID = IDcounter;
	playerData[ID] = data;
	changingData[ID] = {"pos": "(0, 25, 0)", "rot": "(0, 0, 0)", "events": [], "ID": ID};
	res.json({"ID": ID, "level": 3.7});
	for(var subNode in changingData){
		if(ID != subNode){
			changingData[subNode].events.push("join " + ID + " " + req.params["usrname"] + " " + req.params["team"] + " " + playerData[ID].health);
		}
		else{
			for(var ID_ in changingData){
				if(ID != ID_){
					changingData[ID].events.push("join " + ID_ + " " + playerData[ID_].name + " " + playerData[ID_].team + " " + playerData[ID_].health);
				}
			}
		}
	}
	console.log("Player with ID " + ID + " joined the game.");
	//console.log(playerData);
	//console.log(changingData);
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
	playerData[ID].timout = 0;
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
	//test if damage event
	var eventData = event.split(' ');
	if(eventData[0] == "damage"){
		playerData[eventData[1]].health -= parseFloat(eventData[2]);
	}
	//console.log(changingData);
})

app.listen(port, host, () => {
	console.log("Server started on port " + port);
});
