using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMovement : MonoBehaviour
{
	public Vector3 targetPos;
	//public Vector3 pastPos = Transform.position;
	public float speed;
	public bool smooth;

    // Update is called once per frame
    void Update()
    {
		if(smooth){
			transform.position = Vector3.Lerp(transform.position, targetPos, speed);
		}
        else{
			transform.position = targetPos;
		}
    }
}
