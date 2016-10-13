using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	float speed = 10;

	// public variables
	[HideInInspector]
	public float moveSpeed = 5.0f;
	[HideInInspector]
	public float gravity = 30.00f;
	public float jumpSpeed = 100.0f;

	private CharacterController controller;

	private Vector3 moveDirection = Vector3.zero;

	void Start()
	{
		controller = gameObject.GetComponent<CharacterController>();
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			moveSpeed = 100.0f;
		}
		else
		{
			moveSpeed = 5.0f;
		}
		//LEFT ,RIGHT , FORWARD,BACK
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		moveDirection = transform.TransformDirection(moveDirection);

		moveDirection *= moveSpeed;

		//GO UP
		if (Input.GetButton("Jump"))
		{
			//moveDirection.y = (moveDirection.y + 1) * jumpSpeed;
			moveDirection.y += jumpSpeed;
		}
		//GO DOWN
		if (Input.GetKey(KeyCode.E))
			moveDirection.y -= jumpSpeed;

		controller.Move(moveDirection * Time.deltaTime);
	}
}
