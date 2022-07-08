using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

	public ServerComm serverComm;
	public float health;
	public TextMeshProUGUI healthGUI;
	public TextMeshProUGUI bulletsGUI;
	public GunManager gunManager;

    // Update is called once per frame
    void Update()
    {
		//input
        if (Input.GetKeyDown("escape"))
        {
            print("attempting to leave server");
			StartCoroutine(serverComm.LeaveServer());
        }

		//gun input stuffs
		if(Input.GetKey(KeyCode.Mouse0)){
			gunManager.shoot();
		}

		if(Input.GetKeyDown("r")){
			gunManager.reload();
		}
		if(Input.GetKeyDown("1")){
			gunManager.changeGun(0);
		}
		if(Input.GetKeyDown("2")){
			gunManager.changeGun(1);
		}
    }

	public void updateGUI(){
		healthGUI.text = health.ToString();
		bulletsGUI.text = gunManager.bullets.ToString();
	}

	public GameObject getLookedAtObject(GameObject cam, LayerMask mask){
		if(Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, Mathf.Infinity, mask)){
			GameObject obj = hit.collider.gameObject;
			return obj;
		}
		return null;
	}
}
