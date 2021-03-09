using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour {

	private float _maxHealth = 100;
	private float _currentHealth;
	
	public static PlayerHealthScript instance;
	void Awake()
	{
		if(instance==null)
			instance = this;
		else
			Destroy(this);
	}

	// Use this for initialization
	void Start () {
		_currentHealth = _maxHealth;
	}
	
	public void TakeDamage(float damage)
	{
		_currentHealth -= damage;
		UIManager.instance.UpdatePlayerHealthBar(_currentHealth/_maxHealth);
		if(_currentHealth <= 0)
		{
			GameManager.instance.EndGame();
		}

	}
}
