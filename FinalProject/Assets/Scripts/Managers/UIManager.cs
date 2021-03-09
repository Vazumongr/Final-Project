using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance;
	
	public Slider playerHealthBar;

	void Awake()
	{
		if(instance==null)
			instance = this;
		else
			Destroy(this);
	}
	
	public void UpdatePlayerHealthBar(float value)
	{
		playerHealthBar.value = value;
	}
}
