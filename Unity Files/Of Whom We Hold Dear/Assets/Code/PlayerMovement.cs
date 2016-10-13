﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	public float speed = 20.0f;

	void Start () 
	{
		Cursor.lockState = CursorLockMode.Locked;
		Physics.gravity = new Vector3 (0, -35.0F, 0);

	}

	void Update () 
	{
		float translation = Input.GetAxis ("Vertical") * speed;
		float straffe = Input.GetAxis ("Horizontal") * speed;
		translation *= Time.deltaTime;
		straffe *= Time.deltaTime;

		transform.Translate (straffe, 0, translation);

		if (Input.GetKeyDown ("escape")) {
			Cursor.lockState = CursorLockMode.None;
		}
	
	}
}