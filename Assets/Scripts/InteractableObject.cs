using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
	public string action;
	public bool callFunction;
	public UnityEvent function;

	public void callUnityEvent(){
		if(callFunction){
			function.Invoke();
		}
	}

}
