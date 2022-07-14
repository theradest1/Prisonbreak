using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractByCollision : MonoBehaviour
{
	public UnityEvent unityEvent;
	public string action;
	private void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.GetComponent<PlayerMovement>() != null){
			Debug.Log("triggered interaction " + action);
			//unityEvent.Invoke();
		}
	}
}
