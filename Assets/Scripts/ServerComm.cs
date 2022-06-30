using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class ServerComm : MonoBehaviour
{
	public string usrname = "TheMostAmazingUsrname";
	public string ID;
	public string team = "prisoner";
	public float level;
	public Transform playerTransform;
	
	public float updateDelay = .1f;
	public string serverAddress = "http://192.168.0.24:3000/";
	
	public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(Join());
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
			Debug.LogError("Could not join, try again and check server status");
		}
		else{
			Debug.Log("Joined server succesfully");
			string data = www.downloadHandler.text;

			JSONNode processedData = ProcessJSON(data);
			ID = processedData["ID"];
			playerTransform.position = new Vector3(0f, 0f, 0f); //change to get from node
			level = processedData["level"];

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
				//Debug.Log("Recieved Data: " + processedData);
				foreach (JSONNode subNode in processedData){
					//Debug.Log("Node: " + subNode.ToString());
					if(subNode["ID"] != usrname){
						GameObject targetPlayer = GameObject.Find(subNode["ID"]);
						if(targetPlayer != null){
							ClientMovement movement = targetPlayer.GetComponent<ClientMovement>();
							if(movement != null){
								movement.targetPos = StringToVector3(subNode["pos"]);
							}
							targetPlayer.transform.rotation = Quaternion.Euler(StringToVector3(subNode["rot"]));
						}
						else{
							targetPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
							ClientMovement movement = targetPlayer.GetComponent<ClientMovement>();
							movement.player = GameObject.Find(ID.ToString());
							targetPlayer.name = subNode["ID"];
							movement.SetUsrname(subNode["name"]);
							Debug.Log("name: " + subNode["name"]);
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

		Application.Quit();
		UnityEditor.EditorApplication.isPlaying = false; //This needs to be commented out on build or it just wont build
	}

	public Vector3 StringToVector3(string str){
		str = str.Substring(1, str.Length-2); //get rid of parenthisis
		string[] parts = str.Split(',');
		return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
	}

	public IEnumerator Event(string info){
		string address = serverAddress + "event/" + info;
		UnityWebRequest www = UnityWebRequest.Get(address);
		yield return www.SendWebRequest();
		Debug.Log("Made server request: " + address);
	}
}
