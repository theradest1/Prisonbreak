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
	public Transform playerTransform;
	public Vector3 velocity = new Vector3(0, 0, 0);
	public float updateDelay = .1f;
	public float level;
	string serverAddress = "http://localhost:8000/";

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(Join());
		//StartCoroutine(GetWebData("http://localhost:8000/user/", 320));
    }

	IEnumerator Join()
	{	
		Debug.Log("Attempting to join");
		string address = serverAddress + "join/" + usrname + "/" + team + "/";
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
			Debug.Log("Response: " + data);

			JSONNode processedData = ProcessJSON(data);
			ID = processedData["ID"];
			playerTransform.position = new Vector3(0f, 0f, 0f); //change to get from node
			level = processedData["level"];
			
			Debug.Log("Started multiplayer communication");
			StartCoroutine(updatePlayers());
		}
	}

	IEnumerator updatePlayers()
	{
		while(true){
			string address = serverAddress + "update/" + playerTransform.position + "/" + playerTransform.rotation.eulerAngles + "/" + velocity + "/" + ID + "/";
			UnityWebRequest www = UnityWebRequest.Get(address);
			yield return www.SendWebRequest();
			Debug.Log("Made server request: " + address);

			if(www.result != UnityWebRequest.Result.Success){
				Debug.LogError("Somethig Went wrong: " + www.error);
			}
			else{
				string data = www.downloadHandler.text;
				JSONNode processedData = ProcessJSON(data);
				Debug.Log("Recieved Data: " + processedData);
			}
			yield return new WaitForSeconds(updateDelay);
		}
	}
	
	JSONNode ProcessJSON(string raw)
	{
		JSONNode node = JSON.Parse(raw);
		Debug.Log(node);
		return node;
	}
}
