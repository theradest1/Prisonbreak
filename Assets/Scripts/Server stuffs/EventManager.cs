using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public GameObject playerPrefab;
	public ServerComm serverComm;
	public GameManager gameManager;
	public List<AudioClip> sounds;
	public AudioSource audioSourcePrefab;

    public void rawEvents(string eventString){
		eventString = eventString.Substring(1, eventString.Length-2); //get rid of brackets
		string[] events = eventString.Split(',');
		foreach (string indiEvent in events)
    	{
			//Debug.Log(indiEvent);
			string pureEvent = indiEvent.Substring(1, indiEvent.Length-2); //get rid of quotations (from the server sending a list that was converted to a string full of strings)
            Debug.Log(pureEvent);
			string[] eventData = pureEvent.Split(' ');
			switch(eventData[0]){
				case "damage":
					Debug.Log("damage event");
					damage(eventData[1], eventData[2]);
					break;
				case "leave":
					Debug.Log("leave event");
					leave(eventData[1]);
					break;
				case "join":
					Debug.Log("join event");
					join(eventData[1], eventData[2], eventData[3]);
					break;
				case "sound":
					Debug.Log("sound event");
					Debug.Log(eventData[2] + ", " + eventData[3] + ", " + eventData[4]);
					sound(eventData[1], new Vector3(float.Parse(eventData[2]), float.Parse(eventData[3]), float.Parse(eventData[4])));
					break;
			}
        }
	}


	//These events are not specifically for this client, Ex: leave() is not if this client leaves
	void damage(string ID, string damage){
		Debug.Log("Damaged player with ID " + ID + " for " + damage + " health"); //damaged player's ID, damage
		float fDamage = int.Parse(damage);
		if(ID != serverComm.ID){
			ClientMovement hitPlayerScript = GameObject.Find(ID).GetComponent<ClientMovement>();
			hitPlayerScript.health -= fDamage;
			hitPlayerScript.updateHealth();
		}
		else{
			gameManager.health -= fDamage;
			gameManager.updateGUI();
		}
		
	}

	void leave(string ID){
		Debug.Log("Player with ID " + ID + " has left the game"); //Player's ID
		Destroy(GameObject.Find(ID));
	}

	void join(string ID, string usrname, string team){
		Debug.Log("Player with ID " + ID + " has joined the game with usraname " + usrname + " and are a " + team); //ID, usraname, team
		GameObject targetPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

		ClientMovement movement = targetPlayer.GetComponent<ClientMovement>();
		movement.player = GameObject.Find(serverComm.ID.ToString()); //letting the other player's object know who the client here is
		targetPlayer.name = ID;
		movement.SetUsrname(usrname);
	}

	void sound(string soundID, Vector3 position){
		AudioSource sound = Instantiate(audioSourcePrefab, position, Quaternion.identity);
		sound.clip = sounds[int.Parse(soundID)];
		sound.Play();
		Destroy(sound.gameObject, sound.clip.length);
	}
}
