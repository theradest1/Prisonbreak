using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class security : MonoBehaviour
{
	ServerComm serverComm;

	public float damageWhenTouched;
	public bool notifyPoliceWhenTouched;
	public int heistID;
	HeistScript heistScript;
	public bool triggerWhenHitByPolice;

	void Start(){
		serverComm = GameObject.Find("Server").GetComponent<ServerComm>();
		heistScript = GameObject.Find("heist " + heistID).GetComponent<HeistScript>();

		if(heistScript == null){
			Debug.LogError("Couldn't find 'heist " + heistID + "' to get script");
		}
	}

    private void OnTriggerEnter(Collider collider)
	{
		if(collider.GetComponent<PlayerMovement>() != null){
			if(triggerWhenHitByPolice || serverComm.team == "prisoner"){
				Debug.Log("Player was hit by laser");
				if(notifyPoliceWhenTouched){
					StartCoroutine(serverComm.Event("notify " + heistID));
				}
				StartCoroutine(serverComm.Event("damage " + collider.gameObject.name + " " + damageWhenTouched));
			}
		}
	}
}
