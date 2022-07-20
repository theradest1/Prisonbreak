using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This is only for the player

public class InteractByCollision : MonoBehaviour
{
	GameManager gameManager;
	InteractableObject activeInteractableObject;
	string action;

	[HideInInspector]
	public bool lockEvent = false;
	
	void Start(){
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	private void OnTriggerEnter(Collider collider)
	{
		InteractableObject interactableObject = collider.gameObject.GetComponent<InteractableObject>();

		if(interactableObject != null && !lockEvent){
			Debug.Log("Able to interact: " + interactableObject.action);
			activeInteractableObject = interactableObject;
			action = interactableObject.action;
			gameManager.action = "Press E to " + action;
			gameManager.updateGUI();
			//unityEvent.Invoke();
		}
	}

	void OnTriggerExit(Collider collider){
		action = "";
		gameManager.action = action;
		gameManager.updateGUI();
		activeInteractableObject = null;
	}

	public void triggerEvent(){
		if(activeInteractableObject != null){
			lockEvent = true;
			activeInteractableObject.callUnityEvent();
			gameManager.ShowEvent(activeInteractableObject.action);
			Debug.Log("Interacted with: " + activeInteractableObject.action);

			//all events that can be triggered by pressing 'e'
			switch(action){
				case "Get In Car":
					gameManager.getInCar(activeInteractableObject.gameObject);
					lockEvent = true;
					action = "Leave Car";
					gameManager.action = "Press E to " +  action;
					gameManager.updateGUI();
					break;
				case "Leave Car":
					lockEvent = false;
					gameManager.leaveCar(activeInteractableObject.gameObject);
					break;
				case "Take Money":
					break;
				default:
					Debug.LogError("Unknown action: " + action);
					break;
			}
			lockEvent = false;
		}
	}
}
