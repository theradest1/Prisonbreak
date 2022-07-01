using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	string[] eventData;
	public GameObject playerPrefab;
	public ServerComm serverComm;

    public void rawEvents(string eventString){
		eventString = eventString.Substring(1, eventString.Length-2); //get rid of brackets
		string[] events = eventString.Split(',');
		foreach (string indiEvent in events)
    	{
			Debug.Log(indiEvent);
			string pureEvent = indiEvent.Substring(1, indiEvent.Length-2); //get rid of quotations (from the server sending a list that was converted to a string full of strings)
            Debug.Log(pureEvent);
			eventData = pureEvent.Split(' ');
			Invoke(eventData[0], 0);
        }
	}


	//These events are not specifically for this client, Ex: leave() is not if this client leaves
	void damage(){
		Debug.Log("Damaged player with ID " + eventData[1] + " for " + eventData[2] + " health"); //damaged player's ID, damage
	}

	void leave(){
		Debug.Log("Player with ID " + eventData[1] + " has left the game"); //Player's ID
		Destroy(GameObject.Find(eventData[1]));
	}

	void join(){
		Debug.Log("Player with ID " + eventData[1] + " has joined the game with usraname " + eventData[2] + " and are a " + eventData[3]); //ID, usraname, team
		GameObject targetPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

		ClientMovement movement = targetPlayer.GetComponent<ClientMovement>();
		movement.player = GameObject.Find(serverComm.ID.ToString()); //letting the other player's object know who the client here is
		targetPlayer.name = eventData[1];
		movement.SetUsrname(eventData[2]);
	}
}
