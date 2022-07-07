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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//input
        if (Input.GetKeyDown("escape"))
        {
            print("Space key was pressed; attempting to leave server");
			StartCoroutine(serverComm.LeaveServer());
        }

		//gun input stuffs
		if(Input.GetKeyDown(KeyCode.Mouse0)){
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
}
