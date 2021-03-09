using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the script for handling player
 * ability usage.
 */
public class Effects : MonoBehaviour
{

    public Camera cam;
    public float castRange = 1000f; // Max range we can cast at
    public GameObject[] abilities;    // Our array of equipped abilites
    private string _input;  // Cache our input
    public Animator animator;   // Player animator component
    private bool _isReady;
    void OnEnable()
    {
        LabyrinthGenerationScript.mazeNavMeshBuiltDelegate += ReadyUp;	// Subscribe so we know when the navMesh is ready
    }

    void OnDisable()
    {
        LabyrinthGenerationScript.mazeNavMeshBuiltDelegate -= ReadyUp;	// Desubscribe(is that word?)
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(animator.Equals(null))
        {
            Debug.LogWarning("Effects::animator is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isReady) {return;}
        _input = "";    // Reset the cache every frame

        try
        {
            _input = Regex.Replace(Input.inputString, "[^0-9]", "");    // Remove everything that isn't a number from the input
        }
        catch(Exception e)
        {
            Debug.Log(e.StackTrace);
        }

        if(_input.Length == 0)  // If no input on this frame...
            return; // Do nothing
        switch(_input[0])
        {
            /* THIS IS FOR DEVELOPMENT PURPOSES TO SAMPLE DIFFERENT ABILITIES IN REAL TIME
            case '0':
                Debug.Log("Skill 0");
                SampleSkill(0);
                break;
            */
            case '1':
                Debug.Log("Skill 1");
                animator.SetInteger("SkillNumber",1);
                animator.SetTrigger("UseSkill");
                break;
            case '2':
                Debug.Log("Skill 2");
                animator.SetInteger("SkillNumber",2);
                animator.SetTrigger("UseSkill");
                break;
            default:
                Debug.Log("No skill cast");
                break;
        }
    }

    public void UseFireRing()
    {
        GameObject instance = Instantiate(abilities[0],transform.position,transform.rotation);  // Spawn ability at our feet
        //instance.transform.localScale = new Vector3(1,1,1);   //THIS WAS FOR MESSING WITH DIFFERENT SIZES AND SEEING HOW IT SCALED
    }

/* THIS IS FOR DEVELOPMENT PURPOSES TO SAMPLE DIFFERENT ABILITIES IN REAL TIME
    void SampleSkill(int num)
    {
        //GameObject instance = Instantiate(effects[num],transform.position + (transform.up * 3) + (transform.forward * 5),transform.rotation);
        RaycastHit hit;
        Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, castRange);
        Debug.DrawLine(cam.transform.position,hit.point,Color.green,5f);
        GameObject instance = Instantiate(abilities[num], hit.point,transform.rotation);
        //UIManager.instance.DarkenSkill(1);
    }
*/

    public void UseFireDragon()
    {
        RaycastHit hit;
        if(Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, castRange)) // Raycast to where we are looking...
        {
            //Debug.DrawLine(cam.transform.position,hit.point,Color.green,5f); Debugging the raycasting to make sure it was working. 
            GameObject instance = Instantiate(abilities[1], hit.point,transform.rotation);  //Spawn the fire dragon where we are looking
        }
    }

    void ReadyUp()
    {
        _isReady = true;
    }
}
