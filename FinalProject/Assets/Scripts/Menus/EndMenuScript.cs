using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the script for handling the EndMenuUI.
 * Serves one purpose: to restart the game.
 */
public class EndMenuScript : MonoBehaviour {

	
	public Button restartButton;	// Our restart button

	// Use this for initialization
	void Start () {
		restartButton.onClick.AddListener(StartGame);	// Listens for the event
	}
	
	void StartGame()
	{
		SceneManager.LoadScene("MainMenu");	// Loads the MainMenu
	}
}
