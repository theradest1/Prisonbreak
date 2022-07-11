using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientMovement : MonoBehaviour
{
	public Vector3 targetPos;
	public float speed;
	public bool smooth;
	public Canvas canvas;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI healthText;
	public GameObject player;
	public float health;

	public void SetUsrname(string usrname){
		nameText.text = usrname;
	}
	public void updateHealth(){
		healthText.text = health.ToString();
	}
    // Update is called once per frame
    void Update()
    {
		canvas.transform.LookAt(player.transform.position);
		if(smooth){
			transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
		}
        else{
			transform.position = targetPos;
		}
    }
}
