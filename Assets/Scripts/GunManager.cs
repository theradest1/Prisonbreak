using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
	[HideInInspector]
	public int bullets;
	public List<GameObject> gunObjects;
	[HideInInspector]
	public List<GunInfo> gunInfos = new List<GunInfo>();
	GameObject gunHolder;
	GameObject cam;
	Camera camComponent;

	[HideInInspector]
	public int gunID = 0;
	GameManager gameManager;
	[HideInInspector]
	public float actionTimer;
	public LayerMask hitMask;
	ServerComm serverComm;
	[HideInInspector]
	public Vector3 gunTargetPos;

	[HideInInspector]
	public float camTargetFOV = 90f;
	public float camFOVChangeSpeed;

	[HideInInspector]
	public bool ads;
	public GameObject bulletHitPrefab;
    // Start is called before the first frame update
    void Start()
    {
		gunHolder = GameObject.Find("guns");
		cam = GameObject.Find("Main Camera");
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		serverComm = GameObject.Find("Server").GetComponent<ServerComm>();
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
		camComponent.fieldOfView = Mathf.MoveTowards(camComponent.fieldOfView, camTargetFOV, camFOVChangeSpeed * Time.deltaTime * Mathf.Abs(camComponent.fieldOfView - camTargetFOV));
		
		if(actionTimer > 0){
			actionTimer -= Time.deltaTime;
		}
    }

	public void shoot(){
		if(bullets > 0 && actionTimer <= 0){
			if(Physics.Raycast(gunHolder.transform.position, gunHolder.transform.forward, out var hit, Mathf.Infinity, hitMask)){
				GameObject hitObject = hit.collider.gameObject;
				if(hitObject.GetComponent<ClientMovement>() != null){
					Debug.Log("Hit player with ID " + hitObject.name + " for " + gunInfos[gunID].damage.ToString() + " damage");
					StartCoroutine(serverComm.Event("damage " + hitObject.name + " " + gunInfos[gunID].damage.ToString()));
				}
				else{
					Instantiate(bulletHitPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)); //dust
				}
			}

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

		StartCoroutine(serverComm.Event("changeheld " + serverComm.ID.ToString() + " " + newGunID.ToString()));
	}
}
