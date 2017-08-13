using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// All of these are set in the inspector
	public Animator anim;
	public GameObject player; // just to keep things clear for me (not ness. as the gamemanager script is 'currently' attached to the player)
	public AudioSource music;
	public AudioSource winner;
	public GameObject clock;

	public Text bestTime;

	// Use this for initialization
	void Start () {
		bestTime.text = PlayerPrefs.GetString ("bestTime");  // if it exists - get the players best time 
		                                                //that was previously stored in player prefs.
	}
	

	void OnCollisionEnter2D (Collision2D col) {
		Debug.Log ("collision " + col.gameObject.tag);
		if (col.gameObject.tag == "hill") {  // if the player's turtle runs into a hill 
			anim.SetTrigger ("isClimbing"); // - set the climbing animation and drop the speed
			player.GetComponent<CharacterMovement>().currentSpeed = 0;
			// TODO it would be cool to have another animation where 
			// depending on the speed of the player when they hit the hill (going too fast)
			// the turtle could roll down the other side inside it's shell.
		}

	}

	void OnTriggerEnter2D (Collider2D other) {
		Debug.Log ("collision " + other.gameObject.tag);
		if (other.gameObject.tag == "lettuce") {  // if the player's turtle runss into some lettuce ...
			anim.SetTrigger ("isEating");  // ...start the eating animation

			player.GetComponent<CharacterMovement>().currentSpeed = 0; // ...drop the speed
			player.GetComponent<CharacterMovement>().playerActive = false;// ...and take away the players control
			StartCoroutine (StarterUpAgain(other.gameObject));/// .. then give it back
		}
		if (other.gameObject.tag == "finish") {
			player.GetComponent<CharacterMovement>().playerActive = false; // take away player control
			anim.SetTrigger ("isCheering");// start the adorable cheering animation
			music.mute = true;// stop the game music
			winner.Play();// play the winner music
			clock.GetComponent<Clock> ().stopWatch.Stop ();// stop the timer

			// TODO there is a problem here - the time stored seems to be off by two seconds????

			string time = clock.GetComponent<Clock> ().time.text.ToString(); // convert the time to store it as a string
			float newtime = clock.GetComponent<Clock> ().stopWatch.ElapsedMilliseconds / 10; // do all the math...
			float oldTime = PlayerPrefs.GetFloat ("bestMilli");                              // ... to see if the new time
			if (oldTime != 0) {																// ... is better than
				if ((oldTime - newtime) > 0) {												//... the old time and
					PlayerPrefs.SetFloat ("bestMilli", newtime);
					PlayerPrefs.SetString ("bestTime", time);								// store it all in the playerprefs
				}
			} else {
				PlayerPrefs.SetFloat ("bestMilli", newtime);
				PlayerPrefs.SetString ("bestTime", time);
			}
			PlayerPrefs.Save ();  // save everything

		}

	}

	IEnumerator StarterUpAgain(GameObject other) {
		Destroy (other);  // get rid of the lettuce sprite
		yield return new WaitForSeconds (2f); 		// stop forward movememnt for two seconds
		player.GetComponent<CharacterMovement> ().playerActive = true; // give the player back control

	}

	public void RestartGame () {
		// restart the whole game (this isn't really in line with the NES - there wouldn't be a button on screen.)
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name); 
	}


}
