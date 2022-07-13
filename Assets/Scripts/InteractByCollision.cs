using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractByCollision : MonoBehaviour
{
	public CarController carController;

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.GetComponent<PlayerMovement>() != null){
			Debug.Log("player in radius");
		}
	}
}
