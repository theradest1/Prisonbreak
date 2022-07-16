using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientMovement : MonoBehaviour
{
	[HideInInspector]
	public Vector3 targetPos;
	public float speed;
	public Canvas canvas;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI healthText;
	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public float health;
	[HideInInspector]
	public int heldGunID;
	public List<GameObject> guns;

	public void SetUsrname(string usrname){
		nameText.text = usrname;
	}
	public void updateHealth(){
		healthText.text = health.ToString();
	}
    
    void Update()
    {
		canvas.transform.LookAt(player.transform.position);
		transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }

	public void changeHeldItem(int newGunID){
		foreach(GameObject gun in guns){
			gun.SetActive(false);
		}
		if(newGunID != -1){
			guns[newGunID].SetActive(true);
		}
		heldGunID = newGunID;
	}
}
