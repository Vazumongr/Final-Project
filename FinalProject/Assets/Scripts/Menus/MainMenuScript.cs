using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the script for the MainMenuUI.
 * Serves one purpose: Start the game.
 */
public class MainMenuScript : MonoBehaviour {

	public Button playButton;

	// Use this for initialization
	void Start () {
		playButton.onClick.AddListener(StartGame);	// Listens for the event.
	}
	
	void StartGame()
	{
		SceneManager.LoadScene("Main");	// Loads the main level
	}
}
