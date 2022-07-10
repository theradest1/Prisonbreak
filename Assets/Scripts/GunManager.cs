using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
	public int bullets;
	public List<GameObject> gunObjects;
	public List<GunInfo> gunInfos = new List<GunInfo>();
	public GameObject gunHolder;
	public GameObject cam;
	Camera camComponent;
	public int gunID = 0;
	public GameManager gameManager;
	float actionTimer;
	public LayerMask hitMask;
	public ServerComm serverComm;
	public Vector3 gunTargetPos;
	public float camTargetFOV = 60f;
	public float camFOVChangeSpeed;
	public bool ads;
	public GameObject bulletHitPrefab;
    // Start is called before the first frame update
    void Start()
    {
		//get gun info ahead of time for performance
        for(int i = 0; i < gunObjects.Count; i++){
			gunInfos.Add(gunObjects[i].GetComponent<GunInfo>());
		}
		gunTargetPos = gunInfos[gunID].basePos;
		camComponent = cam.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {	
		gunObjects[gunID].transform.localPosition = Vector3.MoveTowards(gunObjects[gunID].transform.localPosition, gunTargetPos, gunInfos[gunID].recoverySpeed * Time.deltaTime);
		gunHolder.transform.rotation = Quaternion.RotateTowards(gunHolder.transform.rotation, cam.transform.rotation, gunInfos[gunID].recoverySpeed * Quaternion.Angle(gunHolder.transform.rotation, cam.transform.rotation) * Time.deltaTime);
		if(actionTimer >0){
			actionTimer -= Time.deltaTime;
		}
		Debug.DrawRay(gunHolder.transform.position, gunHolder.transform.forward * 99999f, Color.red);

		camComponent.fieldOfView = Mathf.MoveTowards(camComponent.fieldOfView, camTargetFOV, camFOVChangeSpeed * Time.deltaTime * Mathf.Abs(camComponent.fieldOfView - camTargetFOV));

    }

	public void shoot(){
		if(bullets > 0 && actionTimer <= 0){
			Physics.Raycast(gunHolder.transform.position, gunHolder.transform.forward, out var hit, Mathf.Infinity, hitMask);
			GameObject hitObject = hit.collider.gameObject;

			if(hitObject != null){
				if(hitObject.GetComponent<ClientMovement>() != null){
					Debug.Log("Hit player with ID " + hitObject.name + " for " + gunInfos[gunID].damage.ToString() + " damage");
					StartCoroutine(serverComm.Event("damage " + hitObject.name + " " + gunInfos[gunID].damage.ToString()));
				}
			}

			Instantiate(bulletHitPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)); //dust
			bullets -= 1;
			gameManager.updateGUI();
			actionTimer += gunInfos[gunID].shootDelay;
			if(ads){
				gunHolder.transform.Rotate(-gunInfos[gunID].kickback/2, 0f, 0f, Space.Self);
			}
			else{
				gunHolder.transform.Rotate(-gunInfos[gunID].kickback, 0f, 0f, Space.Self);
			}
			StartCoroutine(serverComm.Event("sound " + gunInfos[gunID].shootSoundID.ToString() + " " + gunObjects[gunID].transform.position.x.ToString() + " " + gunObjects[gunID].transform.position.y.ToString() + " " + gunObjects[gunID].transform.position.z.ToString()));
			return;
		}
		reload();
	}

	public void reload(){
		if(actionTimer <= 0){
			//Debug.Log("Reload");
			bullets = gunInfos[gunID].totalBullets;
			gameManager.updateGUI();
			actionTimer += gunInfos[gunID].reloadTime;
			StartCoroutine(serverComm.Event("sound " + gunInfos[gunID].reloadSoundID.ToString() + " " + gunObjects[gunID].transform.position.x.ToString() + " " + gunObjects[gunID].transform.position.y.ToString() + " " + gunObjects[gunID].transform.position.z.ToString()));
		}
	}

	public void changeGun(int newGunID){
		gunID = newGunID;
		foreach(GameObject gun in gunObjects){
			gun.SetActive(false);
		}
		gunObjects[newGunID].SetActive(true);
		actionTimer = 0;
		gunTargetPos = gunInfos[gunID].basePos;
		reload();
	}
}
