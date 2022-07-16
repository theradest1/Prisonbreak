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
			//unityEvent.Invoke();
		}
	}

	public void triggerEvent(){
		if(activeInteractableObject != null){
			Debug.Log("Interacted with: " + activeInteractableObject.action);

			//all events that can be triggered by pressing 'e'
			switch(action){
				case "Get In Car":
					gameManager.getInCar(activeInteractableObject.gameObject);
					lockEvent = true;
					action = "Leave Car";
					break;
				case "Leave Car":
					lockEvent = false;
					gameManager.leaveCar(activeInteractableObject.gameObject);
					break;
			}
		}
	}
}
