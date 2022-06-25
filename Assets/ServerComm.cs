using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerComm : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(GetWebData("http://localhost:8000/user/", 320));
    }

    IEnumerator GetWebData( string address, int myID)
	{
		UnityWebRequest www = UnityWebRequest.Get(address + myID);
		yield return www.SendWebRequest();

		if(www.result != UnityWebRequest.Result.Success){
			Debug.LogError("Somethig Went wrong: " + www.error);
		}
		else{
			Debug.Log(www.downloadHandler.text);
		}
	}
}
