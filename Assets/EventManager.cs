using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public void rawEvent(string eventString){
		Debug.Log("Event: " + eventString);
	}
}
