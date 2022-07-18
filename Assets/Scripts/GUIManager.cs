using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using System.Text.RegularExpressions; //for regex

public class GUIManager : MonoBehaviour
{
	public static string usrname;
	public static string workingAddress;// = "http://192.168.0.24:3000/";
	public static string team;
	public TextMeshProUGUI usrnameError;
	public TMP_InputField wantedUsername;

	void Start(){
		Cursor.lockState = CursorLockMode.None;
	}
	public void turnOffCanvas(GameObject canvas){
		canvas.SetActive(false);
	}

	public void turnOnCanvas(GameObject canvas){
		canvas.SetActive(true);
	}

	public void leaveGame(){
		UnityEditor.EditorApplication.isPlaying = false; //This needs to be commented out on build or it just wont build
		Application.Quit();
	}

	public void JoinGame(){
		if(wantedUsername.text.Length >= 3){
			usrname = wantedUsername.text;
			Debug.Log("Trying to connect to public server ip.");
			StartCoroutine(tryToConnect("http://75.100.205.73:3000/"));
			Debug.Log("Trying to connect to local server ip.");
			StartCoroutine(tryToConnect("http://192.168.0.17:3000/"));
		}
		else{
			usrnameError.text = "Username is not long enough";
		}
	}
	void Update(){
		string allowedChars = "1234567890abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUFWYXZ_";
		int strLen = wantedUsername.text.Length;
		if(strLen >= 1){
			if(!allowedChars.Contains(wantedUsername.text[strLen - 1])){
				wantedUsername.text = wantedUsername.text.Substring(0, strLen - 1);
			}
		}
		if(wantedUsername.text.Length > 15){
			wantedUsername.text = wantedUsername.text.Substring(0, 14);
		}
	}

	IEnumerator tryToConnect(string serverAddress){
		string address = serverAddress + "players";
		UnityWebRequest www = UnityWebRequest.Get(address);
		Debug.Log("Made server request: " + address);
		yield return www.SendWebRequest();
		if(www.result == UnityWebRequest.Result.Success){
			workingAddress = serverAddress;
			SceneManager.LoadScene(1);
			yield break;
		}

		Debug.Log("Couldn't connect through public IP");
	}

	public void changeTeams(string newTeam){
		team = newTeam;
	}
}
