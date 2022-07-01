using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	string[] eventData;
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

	void damage(){
		Debug.Log("Damaged player with ID " + eventData[2] + " for " + eventData[1] + " health");
	}
}
