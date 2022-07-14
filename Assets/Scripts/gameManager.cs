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
	public float camADSFOV;
	public float camBaseFOV;
	public GameObject player;

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
    }

	public void updateGUI(){
		//healthGUI.text = health.ToString();
		healthSlider.value = health;
		bulletsGUI.text = gunManager.bullets.ToString();
	}
}
