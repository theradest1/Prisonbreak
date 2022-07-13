using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carController : MonoBehaviour
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


    // Update is called once per frame
    void Update()
    {
		if(Input.GetKey(KeyCode.Space)){
			currentBrakingForce = brakingForce;
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
}
