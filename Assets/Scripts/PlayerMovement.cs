using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController controller;
	public float speed = 10f;
	public float gravity = -9.81f;
	Vector3 velocity;
	public Transform groundCheck;
	public float groundDistance = .4f;
	public float jumpPower = 10f;
	bool isGrounded;
	public LayerMask groundMask;
	public bool ableToMove = true;

    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.transform.position = new Vector3(Random.Range(-10, 10), 200, Random.Range(-10, 10));
    }

    // Update is called once per frame
    void Update()
    {
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;

		velocity.y += gravity * Time.deltaTime;

		if(isGrounded && velocity.y < 0){
			velocity.y = 0;
		}

		if (Input.GetKeyDown("space") && isGrounded){
			velocity.y += jumpPower;
		}
		if(ableToMove){
			controller.Move(move * speed * Time.deltaTime);
			controller.Move(velocity * Time.deltaTime);
		}
		else{
			velocity = new Vector3(0, 0, 0);
		}
	}
}
