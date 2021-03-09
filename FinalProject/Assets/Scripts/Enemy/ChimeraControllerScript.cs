using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the script for the enemy Chimeras.
 * The only enemy type at the moment. Handles movement
 * and the chimera state machine.
 * Movement is done through the NavAgent.
 */
public class ChimeraControllerScript : MonoBehaviour {


	public NavMeshAgent agent;
	public Transform player;
    public Slider healthBar;
    public float maxHealth = 100;
    public float activeHealth;
	private float attackRange = 8;	// The range before chimera attacks
	private Animator _anim;
	private bool _inRange = false;
	private float damage = 10;
	//public float _speed = 10;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;	// Finds the player
        healthBar = GetComponentInChildren<Slider>();	// Fetches our healthbar object
		_anim = GetComponent<Animator>();
        activeHealth = maxHealth;	// Sets health to maxHealth value
	}
	
	// Update is called once per frame
	void Update () {
		//Super simple state machine
		if(_inRange)	// If we are in attack range of the player...
		{
			Attack();	// We are attacking.
		}
		else
		{
			Chase();	// We are chasing.
		}
	}

	void Chase()
	{

		if(agent.isStopped)	// If we are not moving...
		{
			agent.isStopped = false;	// START MOVING
			//_anim.SetBool("isChasing", true);
		}
			

		if(agent.isOnNavMesh)
		{
			agent.SetDestination(player.position);	// Chase the player
		}
	}

	//GOING TO CHANGE. JUST STOPPING MOVEMENT FOR SUBMISSION
	void Attack()
	{
		agent.isStopped = true;	// Stop Moving
		agent.velocity = Vector3.zero;
		_anim.SetTrigger("Attack");
	}

	public void TakeDamage(float damageValue)
    {
        activeHealth -= damageValue;	// Reduce my health value
        healthBar.value = activeHealth / maxHealth;	// Update my health bar
        if(activeHealth <= 0)	// If I am dead...
        {
            Destroy(gameObject,.5f);	// Destroy me. I am dead.
        }
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			_inRange = true;
			// agent.isStopped = true;
			// _anim.SetTrigger("Attack");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			_inRange = false;
			// agent.isStopped = false;
		}
	}

	void DealDamage()
	{
		if(_inRange)
		{
			PlayerHealthScript.instance.TakeDamage(damage);
		}
	}
}
