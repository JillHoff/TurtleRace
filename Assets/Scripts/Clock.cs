using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System;

public class Clock : MonoBehaviour {

	public Stopwatch stopWatch = new Stopwatch();  //a simple timer
	public Text time;  // the text field on the UI to show the timer
	public Text startgametext; // the text field on the UI that propts the player to start the game
	public GameObject player; // the player (set in inspector)
	public AudioSource startClip;// the start music (set in inspector)
	public GameObject[] AI;// and array of all the NPCs (length and game objects set in inspector)


	// Use this for initialization
	void Start () {
		startgametext.GetComponent<Text> ().enabled = true; // this is here for when the player plays again
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return))
			StartGame ();  // waits to start til the player hits enter
		TimeSpan ts = TimeSpan.FromMilliseconds (stopWatch.ElapsedMilliseconds); 
					// automaticly converts the stopwatch time into minutes and seconds
		time.text = string.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
					// make it look pretty

	}

	void StartGame () {
		startgametext.GetComponent<Text> ().enabled = false;  // turn off the player prompt
		StartCoroutine (FlashCountdown()); // start the game
	}

	IEnumerator FlashCountdown() {
		startClip.Play();  // play the silly start sounds

		// this whole long thing just flashes the clock from red to yellow to green to white
		time.color = Color.red;
		yield return new WaitForSeconds (.2f);
		time.color = Color.white;
		yield return new WaitForSeconds (.2f);
		time.color = Color.red;
		yield return new WaitForSeconds (.2f);
		time.color = Color.white;
		yield return new WaitForSeconds (.3f);

		time.color = Color.yellow;
		yield return new WaitForSeconds (.2f);
		time.color = Color.white;
		yield return new WaitForSeconds (.2f);
		time.color = Color.yellow;
		yield return new WaitForSeconds (.2f);
		time.color = Color.white;
		yield return new WaitForSeconds (.2f);
		time.color = Color.green;

		stopWatch.Start ();  // start the timer
		player.GetComponent<CharacterMovement> ().playerActive = true;  // let the player take over
		for (int x = 0; x < AI.Length; x++) {
			AI[x].GetComponent<AIMovement> ().playerActive = true;  // get the NPC turtles moving
		}

		yield return new WaitForSeconds (.2f);
		time.color = Color.white;  // set the timer back to white - it's easier to see.
	}


}
