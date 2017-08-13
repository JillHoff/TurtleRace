using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
	public float maxSpeed = 3f;  //this is as fast as the Turtle can ever move
	public float currentSpeed = 0f; //this holds the current speed of the Turtle

	public bool playerActive = true; // this determines if the move code in fixed update can work
	public Animator anim; // the animator for all the Turtles

	public int currentRow = 1;  //for the NPC Turtles, this is actually set in the inspector - not here.
								//TODO - could have the NPC Turtles try to switch rows - maybe...


	void Update () {

		if (currentSpeed == 0) {  // if there is forward movement, trigger the walking animation
			anim.SetBool ("isWalking", false);
		} else
			anim.SetBool ("isWalking", true);
	}

	void FixedUpdate ()
	{
		if (playerActive == true) { // if the Turtle is allowed to move right now...
			currentSpeed = Mathf.Lerp (currentSpeed, maxSpeed, Time.deltaTime); // ...gradually increase the speed variable
							
			transform.Translate (currentSpeed * Time.deltaTime, 0, 0);// ... then set the turtle moving
			transform.localEulerAngles = new Vector3 (0, 0, 0); // the hill colliders will force the turtles to move at an angle...
																// ...this gets them back on track
		}
	}

	void OnCollisionEnter2D (Collision2D col) { // if the turtle hits something...
		Debug.Log ("collision " + col.gameObject.tag);
		if (col.gameObject.tag == "hill") { // ... and if it's a hill
			anim.SetTrigger ("isClimbing"); // ... trigger the climbing animation (runs once)
			currentSpeed = 0; // ... and drop the turtle's speed to 0
		}

	}

	void OnTriggerEnter2D (Collider2D other) {
		Debug.Log ("collision " + other.gameObject.tag);
		if (other.gameObject.tag == "lettuce") {  // if the turtle triggers the lettuce...
			anim.SetTrigger ("isEating");// ... the turtle will stop and eat

			currentSpeed = 0;// kills the speed
			playerActive = false;  // stops it from moving forward while eating
			StartCoroutine (StarterUpAgain(other.gameObject)); // calls another function - waits for it to finish
		}
		if (other.gameObject.tag == "finish") {
			playerActive = false;  // if the turtle hits the finish line collider ...
			currentSpeed = 0;      // ... game is over - shut it all down
		}

	}

	IEnumerator StarterUpAgain(GameObject other) {
		Destroy (other); // destroys the head of lettuce
		yield return new WaitForSeconds (2f); 	// stop forward movememnt for two seconds
		playerActive = true; // and now turtle can move again

	}
}
