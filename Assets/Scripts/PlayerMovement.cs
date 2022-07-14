using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController controller;
	public GameManager gameManager;
	GunManager gunManager;
	public float speed = 10f;
	public float gravity = -9.81f;
	Vector3 velocity;
	public Transform groundCheck;
	public float groundDistance = .4f;
	public float jumpPower = 10f;
	bool isGrounded;
	public LayerMask groundMask;

	public bool ableToMove = true;
	public GameObject playerBody;
	public GameObject gunHolder;
	public Vector3 targetPos;
	public float targetPosSpeed;

    // Start is called before the first frame update
    void Start()
    {
        gunManager = gameManager.GetComponent<GunManager>();
    }

    // Update is called once per frame
    void Update()
    {
		if(ableToMove){
			controller.enabled = true;
			playerBody.SetActive(true);
			gunHolder.SetActive(true);

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
			controller.Move(move * speed * Time.deltaTime);
			controller.Move(velocity * Time.deltaTime);
		}
		else{
			controller.enabled = false;
			playerBody.SetActive(false);
			gunHolder.SetActive(false);
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, targetPosSpeed * Time.deltaTime);
			gunManager.actionTimer = 1f;
		}
	}
}
