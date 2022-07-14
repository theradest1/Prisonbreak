using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
	public ServerComm serverComm;
	public float health;
	//public TextMeshProUGUI healthGUI;
	public Slider healthSlider;
	public TextMeshProUGUI bulletsGUI;
	public GunManager gunManager;
	public CarManager carManager;
	public InteractByCollision interactByCollision;
	public float camADSFOV;
	public float camBaseFOV;
	public GameObject player;
	Vector3 pastLoc;

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
		if(Input.GetKeyDown(KeyCode.Mouse1)){
			gunManager.gunTargetPos = gunManager.gunInfos[gunManager.gunID].adsPos;
			gunManager.ads = true;
			gunManager.camTargetFOV = camADSFOV;
		}
		if(Input.GetKeyUp(KeyCode.Mouse1)){
			gunManager.gunTargetPos = gunManager.gunInfos[gunManager.gunID].basePos;
			gunManager.ads = false;
			gunManager.camTargetFOV = camBaseFOV;
		}

		if(Input.GetKeyDown("r")){
			gunManager.reload();
		}
		if(Input.GetKeyDown("1")){ //I dont really care how disgusting this is
			gunManager.changeGun(0);
		}
		if(Input.GetKeyDown("2")){
			gunManager.changeGun(1);
		}
		if(Input.GetKeyDown("3")){
			gunManager.changeGun(2);
		}		
		if(Input.GetKeyDown("q")){
			carManager.SpawnCar(player.transform.position + Vector3.up * 20f);
		}
		if(Input.GetKeyDown("e")){
			interactByCollision.triggerEvent();
		}
    }

	public void updateGUI(){
		//healthGUI.text = health.ToString();
		healthSlider.value = health;
		bulletsGUI.text = gunManager.bullets.ToString();
	}

	public void getInCar(GameObject car){
		car.GetComponent<CarController>().playerControlling = true;
		player.GetComponent<PlayerMovement>().ableToMove = false;
		player.transform.SetParent(car.transform);
		pastLoc = player.transform.localPosition;
		player.transform.position = car.transform.position + Vector3.up * 3f;
	}

	public void leaveCar(GameObject car){
		car.GetComponent<CarController>().playerControlling = false;
		player.GetComponent<PlayerMovement>().ableToMove = true;
		player.transform.localPosition = pastLoc;
		player.transform.SetParent(null);
	}
}
