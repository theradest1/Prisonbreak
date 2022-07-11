using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GUIManager : MonoBehaviour
{
	public static string usrname;
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

	public void JoinGame(TextMeshProUGUI wantedUsername){
		if(wantedUsername.text.Length >= 3){
			usrname = wantedUsername.text;
			SceneManager.LoadScene(1);
		}
	}
}
