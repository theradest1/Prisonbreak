using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
	public float mouseSense = 100f;
	public Transform player;
	float xRot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

		player.Rotate(Vector3.up * mouseX);
		
		xRot -= mouseY;
		xRot = Mathf.Clamp(xRot, -90, 90);
		transform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }
}
