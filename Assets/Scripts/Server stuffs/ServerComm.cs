using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class ServerComm : MonoBehaviour
{
	[HideInInspector]
	public string usrname;

	[HideInInspector]
	public string ID;
	public string team = "none";
	[HideInInspector]
	public float level;
	Transform playerTransform;
	PlayerMovement playerMovement;
	public float updateDelay = .1f;
	string serverAddress;
	EventManager eventManager;
	GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
		eventManager = GameObject.Find("GameManager").GetComponent<EventManager>();
		gameManager = eventManager.gameObject.GetComponent<GameManager>();
		playerTransform = GameObject.Find("Player").transform;
		playerMovement = playerTransform.gameObject.GetComponent<PlayerMovement>();
		serverAddress = GUIManager.workingAddress;
		usrname = GUIManager.usrname;
		team = GUIManager.team;
		if(serverAddress == null){
			SceneManager.LoadScene(0);
		}
		else{
			StartCoroutine(Join());
		}
    }

	IEnumerator Join()
	{	
		Debug.Log("Attempting to join");
		string address = serverAddress + "join/" + usrname + "/" + team;
		UnityWebRequest www = UnityWebRequest.Get(address);
		yield return www.SendWebRequest();

		Debug.Log("Made server request: " + address);

		if(www.result != UnityWebRequest.Result.Success){
			Debug.LogError("Somethig Went wrong: " + www.error);
			Debug.Log("Web address of server trying to join is " + address);
			Debug.LogError("Could not join, try again and check server status");
		}
		else{
			Debug.Log("Joined server succesfully");
			string data = www.downloadHandler.text;

			JSONNode processedData = ProcessJSON(data);
			ID = processedData["ID"];
			playerMovement.teleport(StringToVector3(processedData["pos"]));
			level = processedData["level"];
			gameManager.money = float.Parse(processedData["money"]);

			GameObject.Find("Player").name = ID;
			StartCoroutine(updatePlayers());
		}
	}

	IEnumerator updatePlayers()
	{
		while(true){
			string address = serverAddress + "update/" + playerTransform.position + "/" + playerTransform.rotation.eulerAngles + "/" + ID;
			UnityWebRequest www = UnityWebRequest.Get(address);
			yield return www.SendWebRequest();
			//Debug.Log("Made server request: " + address);

			if(www.result != UnityWebRequest.Result.Success){
				Debug.LogError("Somethig Went wrong: " + www.error);
			}
			else{
				string data = www.downloadHandler.text;
				JSONNode processedData = ProcessJSON(data);
				//Debug.Log("Recieved Data: " + processedData.ToString());
				foreach (JSONNode subNode in processedData){
					//Debug.Log("Node: " + subNode.ToString());
					if(subNode["ID"].ToString() != ID.ToString()){
						GameObject targetPlayer = GameObject.Find(subNode["ID"]);
						if(targetPlayer != null){
							ClientMovement movement = targetPlayer.GetComponent<ClientMovement>();
							if(movement != null){
								movement.targetPos = StringToVector3(subNode["pos"]);
							}
							targetPlayer.transform.rotation = Quaternion.Euler(StringToVector3(subNode["rot"]));
						}
					}
					else{
						//Debug.Log("Total events: " + subNode["events"].Count);
						if(subNode["events"].Count > 0){
							eventManager.rawEvents(subNode["events"].ToString());
							Debug.Log("Event recieved: " + subNode["events"].ToString());
						}
					}
				}
			}
			yield return new WaitForSeconds(updateDelay);
		}
	}
	
	JSONNode ProcessJSON(string raw)
	{
		JSONNode node = JSON.Parse(raw);
		return node;
	}

	public IEnumerator LeaveServer()
	{
		string address = serverAddress + "leave/" + ID;
		UnityWebRequest www = UnityWebRequest.Get(address);
		yield return www.SendWebRequest();
		Debug.Log("Made server request: " + address);
		if(www.result != UnityWebRequest.Result.Success){
			Debug.LogError("Somethig Went wrong: " + www.error);
			yield break;
		}
		else{
			Debug.Log("Left server succesfully");
		}

		UnityEditor.EditorApplication.isPlaying = false; //This needs to be commented out on build or it just wont build
		Application.Quit();
		
	}

	public Vector3 StringToVector3(string str){
		str = str.Substring(1, str.Length-2); //get rid of parenthisis
		string[] parts = str.Split(',');
		return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
	}

	public IEnumerator Event(string info){
		string address = serverAddress + "event/" + info;
		UnityWebRequest www = UnityWebRequest.Get(address);
		//Debug.Log("Made server request: " + address);
		yield return www.SendWebRequest();
	}
}
