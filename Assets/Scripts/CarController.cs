using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
	[Header("Parts")]
    public WheelCollider wheelFR;
    public WheelCollider wheelFL;
    public WheelCollider wheelBR;
    public WheelCollider wheelBL;
	public GameObject headlights;

	[Header("Settings")]
	public float acceleration;
	float currentAcceleration;
	public float brakingForce;
	float currentBrakingForce;
	public float maxTurnAngle;
	public float currentTurnAngle;

	[HideInInspector]
	public bool playerControlling = false;
	[HideInInspector]
	public bool headlightsOn = false;


    // Update is called once per frame
    void Update()
    {
		if(playerControlling){
			if(Input.GetKey(KeyCode.Space)){
				currentBrakingForce = brakingForce;
			}
			else{
				currentBrakingForce = 0;
			}
			currentAcceleration = acceleration * Input.GetAxis("Vertical");
			currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
		}
		else{
			currentBrakingForce = brakingForce;
			currentAcceleration = 0f;
			currentTurnAngle = 0;
		}
		wheelBR.motorTorque = currentAcceleration;
		wheelBL.motorTorque = currentAcceleration;

		wheelFL.steerAngle = currentTurnAngle;
		wheelFR.steerAngle = currentTurnAngle;

		wheelBL.brakeTorque  = currentBrakingForce;
		wheelBR.brakeTorque  = currentBrakingForce;

	}

	public void SwitchHeadlights(){
		headlightsOn = !headlightsOn;
		headlights.SetActive(headlightsOn);
	}
}
