using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractByCollision : MonoBehaviour
{
	public LayerMask layersAbleToTrigger;
	private void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.GetComponent<PlayerMovement>() != null){
			Debug.Log("hola");
		}
		//if(other.gameObject.GetComponent<Player>() != null){
		//	Debug.Log("player in radius");
		//}
	}
}
