using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the script for adapting the tilesets
 * based on how many tiles are around them.
 * Each tileset is a prefab with 4 walls. Each wall
 * is deactivated if it sits between two tiles.
 */
public class TilesetAdapterScript : MonoBehaviour
{

    public GameObject positiveXWall,negativeXWall,positiveZWall,negativeZWall;  // References to the walls
    private List<Vector3> _takenLocations = new List<Vector3>();    // Going to store the _takenLocations from labyrinthGenerationScript
    [SerializeField]
    private GameObject _labyrinthGenerator; // Reference to the object
    private LabyrinthGenerationScript _labyrinthGenerationScript;   // Reference to the script

    public int neighbors = 0;   // Our neighbors!

    void OnEnable()
    {
        LabyrinthGenerationScript.mazeGeneratedDelagate += CheckForNeighbors;   // Subscribe so we know when the maze is done generating
    }

    void OnDisable()
    {
        LabyrinthGenerationScript.mazeGeneratedDelagate -= CheckForNeighbors;   // Unsubscribe(that's the word!) when disabled
    }

    // Start is called before the first frame update
    void Start()
    {

        _labyrinthGenerator = GameObject.Find("LabyrinthGenerator");    // Get the generator
        _labyrinthGenerationScript = _labyrinthGenerator.GetComponent<LabyrinthGenerationScript>(); // Get the script component

        if (_labyrinthGenerationScript == null)
            Debug.LogError("ITS NULL");

    }

    public void CheckForNeighbors()
    {
        LabyrinthGenerationScript.mazeGeneratedDelagate -= CheckForNeighbors;   //Unsubscribe because we only need called once.
        
        //One tile will throw an error if these two lines aren't here. not sure why yet.
        _labyrinthGenerator = GameObject.Find("LabyrinthGenerator");
        _labyrinthGenerationScript = _labyrinthGenerator.GetComponent<LabyrinthGenerationScript>();

        if (!_takenLocations.Equals(_labyrinthGenerationScript.GetTakenLocations()))    // Make sure we have an updated list
        {
            _takenLocations = _labyrinthGenerationScript.GetTakenLocations();   // Get that updated list

            // Checks the neighboring locations for a tile
            if (_takenLocations.Contains(transform.position + new Vector3(10, 0, 0)))   // If tile found...
            {
                positiveXWall.SetActive(false); // Deactivate appropriate wall
                neighbors++;    // Increase neighbor count
                                // Repeat for all four...
            }
            if (_takenLocations.Contains(transform.position + new Vector3(-10, 0, 0)))
            {
                negativeXWall.SetActive(false);
                neighbors++;

            }
            if (_takenLocations.Contains(transform.position + new Vector3(0, 0, 10)))
            {
                positiveZWall.SetActive(false);
                neighbors++;

            }
            if (_takenLocations.Contains(transform.position + new Vector3(0, 0, -10)))
            {
                negativeZWall.SetActive(false);
                neighbors++;

            }
        }
        /* This is useless right now since this script isnt activated until after the maze is done generating.
        if(neighbors==4)
        {
            _labyrinthGenerationScript.RemoveFromUsableTaken(transform.position);
        }
        */
    }
}
