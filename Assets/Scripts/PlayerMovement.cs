using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController controller;
	GameManager gameManager;
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
	public bool drivingCar = false;
	public Vector3 targetPos; //this only works when able to move is false, also this is local
	public float targetPosSpeed;

	public GameObject playerBody;
	public GameObject gunHolder;
	public GameObject activeCar;

    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.transform.position = new Vector3(Random.Range(-10, 10), 200, Random.Range(-10, 10));
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		gunManager = gameManager.gameObject.GetComponent<GunManager>();
    }

    // Update is called once per frame
    void Update()
    {
		if(ableToMove && !drivingCar){
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
			controller.enabled = true;
			playerBody.SetActive(true);
			gunHolder.SetActive(true);
		}
		else{
			velocity = new Vector3(0, 0, 0);
			gunManager.actionTimer = 1f;

			transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, targetPosSpeed * Time.deltaTime);

			if(drivingCar){
				controller.enabled = false;
				playerBody.SetActive(false);
				gunHolder.SetActive(false);
			}
		}
	}
}
