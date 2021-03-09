using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the script for billboarding
 * the enemy healthbars so you can see them
 * in game and they always face you.
 */
public class Billboard : MonoBehaviour
{
    public Transform cameraTransform;

    void Start()
    {
        cameraTransform = GameObject.Find("Main Camera").transform;
    }
     void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTransform.forward); // Makes it look at the camera(you)
    }
}
