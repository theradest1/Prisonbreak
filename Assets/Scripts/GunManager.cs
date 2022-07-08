using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
	public int bullets;
	public int totalBullets;
	public List<GameObject> gunObjects;
	public List<GunInfo> gunInfos;
	public GameObject gunHolder;
	public GameObject cam;
	public int gunID = 0;
	public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < gunObjects.Count; i++){
			gunInfos.Add(gunObjects[i].GetComponent<GunInfo>());
			Debug.Log(gunObjects[i]);
		}
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
			gunInfos[gunID].shoot.Play();
			return;
		}
		reload();
	}

	public void reload(){
		Debug.Log("Reload");
		bullets = totalBullets;
		gameManager.updateGUI();
		gunInfos[gunID].reload.Play();
	}

	public void changeGun(int newGunID){
		gunID = newGunID;
		foreach(GameObject gun in gunObjects){
			gun.SetActive(false);
		}
		gunObjects[newGunID].SetActive(true);
	}
}
