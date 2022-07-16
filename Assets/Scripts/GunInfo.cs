using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour
{
	[Header("Gun settings")]
	public float damage;
	public float kickback;
	public float recoverySpeed;
	public int totalBullets;
	public float reloadTime;
	public float shootDelay;
	public int shootSoundID;
	public int reloadSoundID;
	
	public Vector3 adsPos;
	public Vector3 basePos;

	[Header("In progress (doesn't change anything yet)")]
	public bool automatic;
	public float volume;
}
