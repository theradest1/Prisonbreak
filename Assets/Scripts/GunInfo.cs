using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour
{
	public float damage;
	public float kickback;
	public float volume;
	public int totalBullets;
	public float reloadTime;
	public float shootDelay;
	public AudioSource shoot;
	public AudioSource reload;
	public bool automatic;
}
