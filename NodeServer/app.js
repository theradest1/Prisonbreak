const express = require('express')

const app = express();
const port = 8000;
var IDcounter = 0;
var players = 0;

app.get("/", (req, res) =>{
	res.send("Hello World")
});

app.get("/join/:usrname/:team/", (req, res) =>{

	var data = {
		"name": req.params["usrname"],
		"team": req.params["team"],
		"ID": IDcounter,
		"location": "0 0 0", //location of spawn
		"rotation": "0 0 0", //might be changed later
		"level": 3.7, //Get this and inventory from a server-stored JSON file later
		"inventory": [
			{ item: "pistol" },
			{ item: "keycard" }
		]
	};
	players += 1;
	IDcounter += 1;
	res.json(data);
})

app.get("/leave/:id/", (req, res) =>{
	players -= 1;
	//save data here later
})
app.get("/user/:id/0", (req, res) =>{
	var data = {
		"usrname": "testUsrName",
		"usrID": req.params["id"],
		"usrLvl": 3.7,
		"inventory": [
			{ item: "pistol" },
			{ item: "keycard" }
		]
	};

	res.json(data)
})

app.get("/update/:pos/:rot/:vel/:ID", (req, res) => {
	console.log("Position: " + req.params["pos"] + " ," + "Rotation: " + req.params["rot"] + " ," + "Velocity: " + req.params["vel"]);
	res.json("Position: " + req.params["pos"] + " ," + "Rotation: " + req.params["rot"] + " ," + "Velocity: " + req.params["vel"]);
})

app.listen(port, () => {
	console.log("Server started on port " + port)
});