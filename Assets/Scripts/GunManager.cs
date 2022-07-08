using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
	public int bullets;
	public List<GameObject> gunObjects;
	List<GunInfo> gunInfos = new List<GunInfo>();
	public GameObject gunHolder;
	public GameObject cam;
	public float gunLerpSpeed;
	public int gunID = 0;
	public GameManager gameManager;
	float actionTimer;
	public LayerMask hitMask;
	public ServerComm serverComm;
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
		gunHolder.transform.rotation = Quaternion.RotateTowards(gunHolder.transform.rotation, cam.transform.rotation, gunLerpSpeed * Time.deltaTime);
		if(actionTimer >0){
			actionTimer -= Time.deltaTime;
		}
    }

	public void shoot(){
		if(bullets > 0 && actionTimer <= 0){
			//Debug.Log("Bang!!!!");
			GameObject hitObject = gameManager.getLookedAtObject(cam, hitMask);
			if(hitObject != null){
				if(hitObject.GetComponent<ClientMovement>() != null){
					Debug.Log("Hit player with ID " + hitObject.name + " for " + gunInfos[gunID].damage.ToString() + " damage");
					StartCoroutine(serverComm.Event("damage " + hitObject.name + " " + gunInfos[gunID].damage.ToString()));
				}
			}
			bullets -= 1;
			gameManager.updateGUI();
			gunInfos[gunID].shoot.Play();
			actionTimer += gunInfos[gunID].shootDelay;
			return;
		}
		reload();
	}

	public void reload(){
		if(actionTimer <= 0){
			//Debug.Log("Reload");
			bullets = gunInfos[gunID].totalBullets;
			gameManager.updateGUI();
			gunInfos[gunID].reload.Play();
			actionTimer += gunInfos[gunID].reloadTime;
		}
	}

	public void changeGun(int newGunID){
		gunID = newGunID;
		foreach(GameObject gun in gunObjects){
			gun.SetActive(false);
		}
		gunObjects[newGunID].SetActive(true);
		reload();
	}
}
