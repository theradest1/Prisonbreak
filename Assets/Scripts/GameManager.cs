using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
	ServerComm serverComm;

	[HideInInspector]
	public float health = 100f;
	public float healthIncreasePerFrame = .001f;
	public float money = 0f;

	[HideInInspector]
	public float stolenMoney = 0f;
	//public TextMeshProUGUI healthGUI;
	public Slider healthSlider;
	public TextMeshProUGUI bulletsGUI;
	public TextMeshProUGUI moneyGUI;
	public TextMeshProUGUI stolenMoneyGUI;
	public TextMeshProUGUI actionGUI;
	public string action;
	GunManager gunManager;
	CarManager carManager;
	public InteractByCollision interactByCollision;
	public float camADSFOV;
	public float camBaseFOV;
	GameObject player;
	PlayerMovement playerMovement;
	Vector3 pastLoc;

	void Start(){
		serverComm = GameObject.Find("Server").GetComponent<ServerComm>();

		gunManager = this.gameObject.GetComponent<GunManager>();
		carManager = this.gameObject.GetComponent<CarManager>();

		player = GameObject.Find("Player");
		playerMovement = player.GetComponent<PlayerMovement>();

	}
    // Update is called once per frame
    void Update()
    {
		if(health < 100f){
			health += healthIncreasePerFrame * Time.deltaTime;
			healthSlider.value = health;
		}
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
		if(health <= 0f){
			health = 100f;
			playerMovement.teleport(new Vector3(-10, -20, -10));
		}
		moneyGUI.text = "$" + money;
		stolenMoneyGUI.text = "$" + stolenMoney;
		healthSlider.value = health;
		bulletsGUI.text = gunManager.bullets.ToString();
		actionGUI.text = action;
	}

	public void getInCar(GameObject car){
		car.GetComponent<CarController>().playerControlling = true;
		playerMovement.ableToMove = false;
		playerMovement.drivingCar = true;

		player.transform.SetParent(car.transform);
		pastLoc = player.transform.localPosition;
		playerMovement.targetPos = new Vector3(0, 3, -4);
	}

	public void leaveCar(GameObject car){
		car.GetComponent<CarController>().playerControlling = false;
		playerMovement.ableToMove = true;
		playerMovement.drivingCar = false;
		player.transform.localPosition = pastLoc;
		player.transform.SetParent(null);
	}

	public void addToStolenMoney(float newStolenMoney){
		stolenMoney += newStolenMoney;
		updateGUI();
	}

	public void getStolenMoney(){
		money += stolenMoney;
		stolenMoney = 0f;
		updateGUI();
	}
}
