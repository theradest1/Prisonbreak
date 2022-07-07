using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
	public int bullets;
	public int totalBullets;
	public List<GameObject> gunObjects;
	public GameObject gunHolder;
	public GameObject cam;
	public int gunID = 0;
	public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gunHolder.transform.rotation = cam.transform.rotation;
    }

	public void shoot(){
		if(bullets > 0){
			Debug.Log("Bang!!!!");
			bullets -= 1;
			gameManager.updateGUI();
			return;
		}
		reload();
	}

	public void reload(){
		Debug.Log("Reload");
		bullets = totalBullets;
		gameManager.updateGUI();
	}

	public void changeGun(int newGunID){
		foreach(GameObject gun in gunObjects){
			gun.SetActive(false);
		}
		gunObjects[newGunID].SetActive(true);
	}
}
