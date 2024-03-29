using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public GameObject playerPrefab;
	ServerComm serverComm;
	GameManager gameManager;
	PlayerMovement playerMovement;
	public List<AudioClip> sounds;
	public AudioSource audioSourcePrefab;

	void Start(){
		serverComm = GameObject.Find("Server").GetComponent<ServerComm>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
	}
    public void rawEvents(string eventString){
		eventString = eventString.Substring(1, eventString.Length-2); //get rid of brackets
		string[] events = eventString.Split(',');
		foreach (string indiEvent in events)
    	{
			//Debug.Log(indiEvent);
			string pureEvent = indiEvent.Substring(1, indiEvent.Length-2); //get rid of quotations (from the server sending a list that was converted to a string full of strings)
            //Debug.Log(pureEvent);
			string[] eventData = pureEvent.Split(' ');
			switch(eventData[0]){
				case "damage":
					//Debug.Log("damage event");
					damage(eventData[1], eventData[2]);
					break;
				case "leave":
					//Debug.Log("leave event");
					leave(eventData[1]);
					break;
				case "join":
					//Debug.Log("join event");
					join(eventData[1], eventData[2], eventData[3], eventData[4]);
					break;
				case "sound":
					//Debug.Log("sound event");
					//Debug.Log(eventData[2] + ", " + eventData[3] + ", " + eventData[4]);
					sound(eventData[1], new Vector3(float.Parse(eventData[2]), float.Parse(eventData[3]), float.Parse(eventData[4])));
					break;
				case "changeheld":
					//Debug.Log("Change held item event");
					changeHeldItem(eventData[1], eventData[2]);
					break;
				case "notify":
					notify(eventData[1]);
					break;
				default:
					Debug.LogError("Unknown event: " + eventData[0]);
					break;
			}
        }
	}

	void notify(string heistID){
		GameObject.Find("heist " + heistID).GetComponent<HeistScript>().startAlarm();
		if(serverComm.team == "police"){
			Debug.Log("Heist started at location " + heistID);
		}
	}

	//These events are not specifically for this client, Ex: leave() is not if this client leaves
	void damage(string ID, string damage){
		//Debug.Log("Damaged player with ID " + ID + " for " + damage + " health"); //damaged player's ID, damage
		float fDamage = int.Parse(damage);
		if(ID != serverComm.ID){
			ClientMovement hitPlayerScript = GameObject.Find(ID).GetComponent<ClientMovement>();
			hitPlayerScript.health -= fDamage;
			hitPlayerScript.updateHealth();
		}
		else{
			gameManager.health -= fDamage;
			if(gameManager.health <= 0f){
				playerMovement.teleport(new Vector3(0, 3, 0));
				StartCoroutine(serverComm.Event("damage " + ID + " " + -(100 - gameManager.health)));
			}
			gameManager.updateGUI();
		}
		
	}

	void leave(string ID){
		//Debug.Log("Player with ID " + ID + " has left the game"); //Player's ID
		Destroy(GameObject.Find(ID));
	}

	void join(string ID, string usrname, string team, string health){
		//Debug.Log("Player with ID " + ID + " has joined the game with usraname " + usrname + " and are a " + team + " with health of " + health); //ID, usraname, team, health
		GameObject targetPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

		ClientMovement movement = targetPlayer.GetComponent<ClientMovement>();
		movement.player = GameObject.Find(serverComm.ID.ToString()); //letting the other player's object know who the client here is
		targetPlayer.name = ID;
		movement.health = float.Parse(health);
		movement.updateHealth();
		movement.SetUsrname(usrname);
	}

	void sound(string soundID, Vector3 position){
		AudioSource sound = Instantiate(audioSourcePrefab, position, Quaternion.identity);
		sound.clip = sounds[int.Parse(soundID)];
		sound.Play();
		Destroy(sound.gameObject, sound.clip.length);
	}

	void changeHeldItem(string ID, string itemID){
		if(ID != serverComm.ID){
			GameObject.Find(ID).GetComponent<ClientMovement>().changeHeldItem(int.Parse(itemID));
		}
	}
}
