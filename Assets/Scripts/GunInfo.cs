using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour
{
	[Header("Gun settings")]
	public float damage;
	public float kickbackVertical;
	public float kickbackVerticalVariation;
	public float kickbackHorizontal;
	public float reductionOfRecoilWhenADS;
	public float changeInFOVOnShot;
	public float reductionOfFOVChangeWhenADS;
	public float recoverySpeed;
	public int totalBullets;
	public float reloadTime;
	public float shootDelay;
	public int shootSoundID;
	public int reloadSoundID;
	
	public Vector2 kickback(bool ads){
		if(!ads){
			return new Vector2(kickbackVertical + Random.Range(-kickbackVerticalVariation, kickbackVerticalVariation), Random.Range(-kickbackHorizontal, kickbackHorizontal));
		}
		else{
			return new Vector2((kickbackVertical + Random.Range(-kickbackVerticalVariation, kickbackVerticalVariation)) * reductionOfRecoilWhenADS, Random.Range(-kickbackHorizontal, kickbackHorizontal) * reductionOfRecoilWhenADS);
		}
	}

	public float FOVOnShot(bool ads){
		if(!ads){
			return changeInFOVOnShot;
		}
		else{
			return changeInFOVOnShot * reductionOfFOVChangeWhenADS;
		}
	}

	public Vector3 adsPos;
	public Vector3 basePos;

	[Header("In progress (doesn't change anything yet)")]
	public bool automatic;
	public float volume;
}
