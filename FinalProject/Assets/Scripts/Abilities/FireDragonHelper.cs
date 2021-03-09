using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is a helper script for FireDragon.cs
 * This one gets attached to the object that has
 * the animator in the ability prefab and is used
 * to call methods in FireDragon.cs from the animation.
 */
public class FireDragonHelper : MonoBehaviour
{

    FireDragon parentScript;    //The FireDragon.cs

    void Start()
    {
        parentScript = GetComponentInParent<FireDragon>();
    }
    public void CreateHitBoxHelper()
    {
        parentScript.CreateHitBox();
    }

    public void DestroyMe()
    {
        parentScript.DestroyMe();
    }
}
