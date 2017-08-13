using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform targetRight;

	public GameObject player;

	float speed = 1;

	void Update () {

		// I didn't want to just attach the camera to the player because when the turtle
		// goes up and down hills - the camera rotates too - not good.
		// Another way to do this would be to lock the rotation of the camera.  

		speed = player.GetComponent<CharacterMovement>().currentSpeed; // keep up with the turtle

		targetRight.position = new Vector3 (targetRight.position.x, 0, -4);// aim for a spot just in front of the turtle

		transform.position = Vector3.MoveTowards (transform.position, targetRight.position, speed * Time.deltaTime);// keeps it moving steady

	}


}