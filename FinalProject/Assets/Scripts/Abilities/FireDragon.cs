using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the main script for handling the FireDragon
 * ability. (Prefab made by Kripto) This controls damage, enemy interaction, 
 * and self destruction.
 */
public class FireDragon : MonoBehaviour
{
    private float _baseDamage = 100f;   //BaseDamage value
    private float _baseScale = 1f;  //Base size.
    private float _activeDamage,_activeScale;   //Scaling factors for damage and size.

    CapsuleCollider col;    //Collider reference for collisions.


    // Start is called before the first frame update
    void Start()
    {
        _activeDamage = _baseDamage; // * damageModifer but no damageModifier implemented yet.
        _activeScale = _baseScale; // * scaleModifier but no scaleModifier implemented yet.
        
    }

    /*
     * This creates a hit box at a specific point in the animation
     * so that ability can collide with enemies.
     * This is called by the FireDragonHelper.cs file during the
     * FireDragonAnim.anim animation.
     */
    public void CreateHitBox()
    {
        col = gameObject.AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;  //Creates our capsule collider
        col.isTrigger = true;   //Makes it a trigger because we don't want collision to restrict movement
        col.radius = 5f;    //Sets the appropriate radius
    }

    /*
     * The nullcheck is there because somehow I was getting TriggerEnters on 
     * objects that didn't have colliders and it would throw errors. Weird.
     */
    void OnTriggerEnter(Collider other)
    {
        if(other.Equals(null)) { return; }   
        other.GetComponent<ChimeraControllerScript>().TakeDamage(_activeDamage);    //Tell the enemy that we hit to take damage.
    }

    /*
     * Destroys this object and collider.
     * Was having issues with collider not being destroyed 
     * when destroying this object. Had to specify collider
     * directly.
     */
    public void DestroyMe()
    {
        Destroy(col);
        Destroy(gameObject,3f);
    }
}
