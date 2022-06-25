const express = require('express')

const app = express();
const port = 8000;
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
		"location": "(0, 0, 0)", //Change loc and rot
		"rotation": "(0, 0, 0)",
		"velocity": "(0, 0, 0)",
		"level": 3.7, //Get this and inventory from a server-stored JSON file
		"inventory": [
			{ item: "pistol" },
			{ item: "keycard" }
		]
	};
	playerData[IDcounter] = data;
	res.json({"ID": IDcounter, "level": 3.7});
	console.log(playerData);
	players += 1;
	IDcounter += 1;
})

app.get("/update/:pos/:rot/:vel/:ID", (req, res) => {
	var ID = req.params["ID"];
	playerData[ID].position = req.params["pos"];
	playerData[ID].rotation = req.params["rot"];
	playerData[ID].velocty = req.params["vel"];
	res.json("recieved");
	console.log(playerData);
})

app.get("/leave/:id/", (req, res) =>{
	players -= 1;
	//save data here later
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

 
app.listen(port, () => {
	console.log("Server started on port " + port)
});