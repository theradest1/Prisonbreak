using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This is only for the player

public class InteractByCollision : MonoBehaviour
{
	public GameManager gameManager;
	InteractableObject activeInteractableObject;
	//public UnityEvent unityEvent;
	//public string action;
	private void OnTriggerEnter(Collider collider)
	{
		InteractableObject interactableObject = collider.gameObject.GetComponent<InteractableObject>();

		if(interactableObject != null){
			Debug.Log("Able to interact: " + interactableObject.action);
			activeInteractableObject = interactableObject;
			//unityEvent.Invoke();
		}
	}

	public void triggerEvent(){
		if(activeInteractableObject != null){
			Debug.Log("Interacted with: " + activeInteractableObject.action);

			//all events that can be triggered by pressing 'e'
			switch(activeInteractableObject.action){
				case "Get In Car":
					gameManager.getInCar(activeInteractableObject.gameObject);
					break;
			}
		}
	}
}
