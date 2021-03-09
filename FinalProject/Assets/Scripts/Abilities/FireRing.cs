using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the script for handling the 
 * FireRing ability. (Prefab made by Kripto)
 * This handles damage, scaling, duration,
 *  tick rates, and enemy interaction.
 */
public class FireRing : MonoBehaviour
{
    //All our base values
    private float _baseDuration = 15f;
    private float _baseDamage = 10f;
    private float _baseScale = 1f;
    private float _baseTickRate = .5f;

    private float _activeDuration,_activeDamage,_activeScale,_activeTickRate;

    private List<GameObject> _collidingEnemies = new List<GameObject>();    // A list to store all our colliding enemies

    // Start is called before the first frame update
    void Start()
    {
        // All of these are set up to multiplied by modifiers from the player script. Was not implemented.
        _activeDuration = _baseDuration;
        _activeDamage = _baseDamage;
        _activeScale = _baseScale;
        _activeTickRate = _baseTickRate;
        StartCoroutine(Duration());
        StartCoroutine(DamageTicks());
    }

    // Basically a timer to delete the object when it's duration is over
    IEnumerator Duration()
    {
        yield return new WaitForSecondsRealtime(_activeDuration);   //Based off skills duration
        Destroy(gameObject);
    }

    // Responsiuble for handling our ticks of damage
    IEnumerator DamageTicks()
    {
        while(true)
        {
            foreach(GameObject go in _collidingEnemies) // For every enemy we are colliding with...
            {
                if(go.Equals(null)) // If they enemy has been removed from the game...
                {
                    _collidingEnemies.Remove(go);   // Remove them from our list
                    break;
                }

                //Only do damage to enemies
                if(go.CompareTag("Enemies"))    // If it is an enemy in our list... (should do this check when adding them to the list instead)
                {
                    go.GetComponent<ChimeraControllerScript>().TakeDamage(_activeDamage);   // Tell the enemy to take damage

                }
                
            }
            yield return new WaitForSecondsRealtime(_activeTickRate);   // Wait till we can tick again...
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if(!_collidingEnemies.Contains(other.gameObject))   // If the object is not already in our list...
        {
            _collidingEnemies.Add(other.gameObject);    // Add them to our list
        }
            
    }

    void OnTriggerExit(Collider other)
    {
        if(_collidingEnemies.Contains(other.gameObject))    // If the object was in our list...
        {
            _collidingEnemies.Remove(other.gameObject); // Remove them from our list
        }
            
    }
}
