using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	public float maxSpeed = 3f;  // this is as fast as the player's turtle is allowed to go
	public float currentSpeed = 0f;  // holds the current speed

	float yaxis = -.8f; // the starting row position
	public int currentRow = 1;  // the starting row number

	public bool playerActive = false;  // turn off player control of turtle
	public Animator anim;// the animator

	
	void Update () {
		// if the turtle is moving, start the walking animation
		if ((Input.GetAxis ("Horizontal")) > 0)
			anim.SetBool ("isWalking", true);
		if ((Input.GetAxis ("Horizontal")) <= 0 || currentSpeed == 0) {
			anim.SetBool ("isWalking", false);
			currentSpeed = 0;
		}
	}

	void FixedUpdate ()
	{
		if (playerActive == true) {
			currentSpeed = Mathf.Lerp (currentSpeed, maxSpeed, Time.deltaTime); // gradually increase the turtles speed
			
			// If the up or down arrows (or the w or s keys) are pressed - move the character to the next lane.
			// this looks like overkill - but I only want to get the initial key press
			// not the whole time the player is holding down the key
			if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {
				if ((Input.GetAxis ("Vertical")) < 0) {// if player hits down - go down a row - unless it's at the bottom
					if (currentRow != 1)
						currentRow = currentRow - 1;
				} else if ((Input.GetAxis ("Vertical")) > 0) {// if player hits up - go up a row - unless it's at the top
					if (currentRow != 4)
						currentRow = currentRow + 1;
				}

				switch (currentRow) { // find the correct y-axis based n the row
				case 1:
					yaxis = -0.8f;
					break;
				case 2:
					yaxis = -0.32f;
					break;
				case 3:
					yaxis = 0.16f;
					break;
				case 4:
					yaxis = 0.6f;
					break;
				}
				transform.position = new Vector3 (transform.position.x, yaxis, 0); // move the player to the correct row
				//TODO make this a nice smooth move instead of a instant jump
			}
				
			transform.Translate (Input.GetAxis ("Horizontal") * currentSpeed * Time.deltaTime, 0, 0); // move the turtle
			transform.localEulerAngles = new Vector3 (0, 0, 0); // keep it steady on and after the hills
		}
	}
}
