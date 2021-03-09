using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the script for handling player movement
 * through the use of a character controller.
 * I just personally like CharacterController movement
 * more than rigidbody.
 */
public class CharacterControllerScript : MonoBehaviour
{

    private CharacterController _playerController;  // Character controller component
    [SerializeField]
    private float _movementSpeed = 0f;
    [SerializeField]
    private Transform _camera;
    private float _gravityForce = -.5f; // Value of gravity. For easy manipulating
    //private Vector3 gravity;    // Not needed. Gravity is accounted for in _camRelDirection
    private Animator _animator;
    private bool _isReady;
    public static CharacterControllerScript instance;
	void Awake()
	{
		if(instance==null)
			instance = this;
		else
			Destroy(this);
	}
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
        _playerController = GetComponent<CharacterController>();    // Gets the controller component
        _animator = GetComponent<Animator>();    // Gets the animator component
        if(_playerController.Equals(null))
        {
            Debug.LogWarning("CharacterControllerScript::_playerController is null");
        }
        if(_camera.Equals(null))
        {
            Debug.LogWarning("CharacterControllerScript::_camera is null");
        }
        if(_animator.Equals(null))
        {
            Debug.LogWarning("CharacterControllerScript::_animator is null");
        }
        Cursor.visible = false;
        //Vector3 gravity = new Vector3(0,_gravityForce,0);   // Not needed. Gravity is accounted for in _camRelDirection
    }

    void FixedUpdate()
    {
        if(!_isReady) {return;}
        
        CalculateMovement();
    }

    void CalculateMovement()
    {
        //Gets our input (WASD)
        float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 _velocity = new Vector3(0,0,0); // Sets a base value

        Vector3 _camRelDirection = (_camera.forward * vertical  + _camera.right * horizontal).normalized;   // Gets the camera direction

        if(!_playerController.isGrounded)   //If we are still falling in...
        {

            _camRelDirection = Vector3.zero; // Our only movement is falling from gravity (hopefully fix the bug of falling through the floor)
        }

        _camRelDirection.y = _gravityForce; // Account for the force of gravity...

        _velocity = (_movementSpeed * _camRelDirection);// + gravity;  // Multiply our movement by the direction so we move relative to the camera

        SetPlayerRotation(_velocity);

		_playerController.Move(_velocity * Time.deltaTime);

        
    }

    void SetPlayerRotation(Vector3 _velocity)
    {
        Vector3 _newRotation = _velocity;   // Sets up a vector for a player rotation
        _newRotation.y = 0; // Make sure our Y rotation is 0

        if(_newRotation != Vector3.zero)    // If we are walking...
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_newRotation),1); // Rotate our character to face the direction of our movement
            _animator.SetBool("isWalking",true);    // Sets walking animation
        }
        else
            _animator.SetBool("isWalking",false);   // Disables walking animation which defaults to idle
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EndZone")) // IF we are on the end tile...
            GameManager.instance.EndGame();  // End game
    }

    void ReadyUp()
    {
        _isReady = true;
    }
}
