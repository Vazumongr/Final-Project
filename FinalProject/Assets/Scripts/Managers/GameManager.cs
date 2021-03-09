using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	void Awake()
	{
		if(instance==null)
			instance = this;
		else
			Destroy(this);
	}
	
	public void EndGame()
	{
		SceneManager.LoadScene("End");
	}
}
