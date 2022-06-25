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
		StartCoroutine(updatePlayers());

    }

	IEnumerator Join()
	{	
		UnityWebRequest www = UnityWebRequest.Get(serverAddress + "join/" + usrname + "/" + team + "/");
		yield return www.SendWebRequest();

		if(www.result != UnityWebRequest.Result.Success){
			Debug.Log("Somethig Went wrong: " + www.error);
		}
		else{
			string data = www.downloadHandler.text;
			JSONNode processedData = ProcessJSON(data);
			ID = processedData["ID"];
			playerTransform.position = new Vector3(0f, 0f, 0f); //change to get from node
			level = processedData["level"];

		}
	}

	IEnumerator updatePlayers()
	{
		while(true){
			string address = serverAddress + "update/" + playerTransform.position + "/" + playerTransform.rotation.eulerAngles + "/" + velocity + "/" + ID + "/";
			Debug.Log(address);
			UnityWebRequest www = UnityWebRequest.Get(address);
			yield return www.SendWebRequest();

			if(www.result != UnityWebRequest.Result.Success){
				Debug.Log("Somethig Went wrong: " + www.error);
			}
			else{
				string data = www.downloadHandler.text;
				JSONNode processedData = ProcessJSON(data);
				print(processedData);
			}
			yield return new WaitForSeconds(updateDelay);
		}
	}

    string GetWebData( string address )
	{
		UnityWebRequest www = UnityWebRequest.Get(address);
		//www.SendWebRequest();
		Debug.Log(address);

		if(www.result != UnityWebRequest.Result.Success){
			return "Somethig Went wrong: " + www.error;
		}
		else{
			return www.downloadHandler.text;
		}
	}
	
	JSONNode ProcessJSON(string raw)
	{
		JSONNode node = JSON.Parse(raw);
		Debug.Log(node);
		return node;
	}
}
