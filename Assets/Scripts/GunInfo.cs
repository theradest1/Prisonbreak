using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour
{
	public float damage;
	public float kickback;
	public float recoverySpeed;
	public float volume;
	public int totalBullets;
	public float reloadTime;
	public float shootDelay;
	public int shootSoundID;
	public int reloadSoundID;
	public bool automatic;
	public Vector3 adsPos;
	public Vector3 basePos;
}
