const express = require('express')
const app = express();
const port = 8000

app.get("/", (req, res) =>{
	res.send("Hello World")
});

app.get("/user/:id", (req, res) =>{
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

app.listen(port, () => {
	console.log("Server started up")
});