using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider wheelFR;
    public WheelCollider wheelFL;
    public WheelCollider wheelBR;
    public WheelCollider wheelBL;

	public float acceleration;
	float currentAcceleration;
	public float brakingForce;
	float currentBrakingForce;
	public float maxTurnAngle;
	public float currentTurnAngle;
	public bool playerControlling = false;


    // Update is called once per frame
    void Update()
    {
		if(playerControlling){
			if(Input.GetKey(KeyCode.Space)){
				currentBrakingForce = brakingForce;
			}
			else if(Input.GetKey(KeyCode.DownArrow)){
				currentBrakingForce = brakingForce/3f;
			}
			else{
				currentBrakingForce = 0;
			}
			wheelBL.brakeTorque  = currentBrakingForce;
			wheelBR.brakeTorque  = currentBrakingForce;

			currentAcceleration = acceleration * Input.GetAxis("Vertical");
			Debug.Log(currentAcceleration);
			wheelBR.motorTorque = currentAcceleration;
			wheelBL.motorTorque = currentAcceleration;

			currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
			wheelFL.steerAngle = currentTurnAngle;
			wheelFR.steerAngle = currentTurnAngle;
		}
		else{
			currentBrakingForce = brakingForce;
			wheelBR.motorTorque = 0f;
			wheelBL.motorTorque = 0f;
		}

	}
}
