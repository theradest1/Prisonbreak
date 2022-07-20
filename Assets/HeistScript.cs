using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeistScript : MonoBehaviour
{
    public AudioSource alarm;
	[HideInInspector]
	public bool beingRobbed = false;
	[HideInInspector]
	public bool silentAlarm = false;
	public float alarmLength;
	
    // Update is called once per frame
    void Update()
    {
    }

	public void startAlarm(){
		if(!silentAlarm){
			alarm.Play();
			Invoke("stopAlarm", alarmLength);
		}
	}

	public void stopAlarm(){
		alarm.Stop();
	}
}
