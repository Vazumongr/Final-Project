using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AI;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the script for MY PROCEDURAL LABYRINTH GENERATION.
 * This handles the entire generation of the labyrinth and is
 * what I probably spent the most time working on. I don't know 
 * what kind of generation this would be considered. I had no 
 * experience of procedural generation before this.
 */
public class LabyrinthGenerationScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _tilesets;     //THIS IS AN ARRAY TO HOLD THE PREFAB TILESETS I USE TO CREATE MY WORLD
    [SerializeField]
    private GameObject _spawnTile;       // This is the tile you spawn on.
    [SerializeField]
    private GameObject _labyrinthContainer;  //THIS IS TO PARENT ALL THE SPAWNED PREFABS SO THEY DONT FILL MY HEIRARCHY
    private Transform _labyrinthContainerTransform; //This is whats needed to make the spawned tiles a child of the container

    private List<Vector3> _takenLocations = new List<Vector3>();    //A list of all taken locations so that no objects can spawn in the same place
    private List<GameObject> _spawnedTiles = new List<GameObject>();    //A list of all the objects spawned
    private List<Vector3> _usableTakenLocations = new List<Vector3>();  // A list of locations that have room for neighbors

    //public Dictionary<Vector3, GameObject> _spawnedTilesDictionary = new Dictionary<Vector3, GameObject>();   // I needed this for something but I cannot remember. Saving just incase
    [SerializeField]
    private int mazeSize = 5000;    //The amount of tiles to be spawned


    //public TextAsset timesText;     //Text file that gets written to (was used for time testing different methods of generation)

    public Canvas loadingScreen;    // Only useful when generating larger maps, such as 2000+ tiles.

    public NavMeshSurface navMeshSurface;   // The NavMeshSurface component we will need to bake

    public delegate void MazeGeneratedDelagate();   // Delegate for when the Maze(Labyrinth/Map) is done generating
    public static event MazeGeneratedDelagate mazeGeneratedDelagate;

    public delegate void MazeNavMeshBuiltDelegate();    // Delegate for when the NavMesh is been baked
    public static event MazeNavMeshBuiltDelegate mazeNavMeshBuiltDelegate;
    public static LabyrinthGenerationScript instance;   // Singleton. Not utilized in every script yet. Still refactoring.

    void Awake()
    {
        if(instance==null)
            instance = this;
        else
            Destroy(gameObject);

        _labyrinthContainerTransform = _labyrinthContainer.GetComponent<Transform>();   //Gets the transform
        StartCoroutine(GenerateMazeC());    //Start the maze generation!
    }

    
    /*
     * I think using a couroutine is the way I'll have to do 
     * my maze generation so I can have a loading screen
     * during the maze generation. Right now it takes about 30 secs 
     * to generate the maze using the coroutine but that can be 
     * shortened by changed the mod value
     * used towards the end of the method.
     *
     * Essentially how this works is we spawn a block at the origin(0,0,0),
     * then we look to the four directional spaces (left, right, up, down)
     * and spawn a block at one of those spaces chosen at random, if that space is open.
     * if there are no open spaces, we randomly pick another tile in the world and repeat.
     * Check for available spaces->Spawn if we can->Pick new tile if we can't->Check for available spaces...etc.
     */
    IEnumerator GenerateMazeC() // This called GenerateMazeC becasue this is my third iteration of this maze generation method/algorithm(I think I can call it that).
    {
        //int mazeSize = 5000;
        Vector3 oldLocation = new Vector3(0, 0, 0); //This is so we start at the origin.
        Vector3 spawnLocation = new Vector3(0, 0, 0);   //This is so we start at the origin

        float spacing = _tilesets[0].GetComponent<Transform>().localScale.x;    //gets the dimensions of the tile so i know how far apart to spawn them

        for (int i = 0; i < mazeSize; i++)
        {


            GameObject selection = _tilesets[Random.Range(0, _tilesets.Length)];    //Picks a random tileset (they're just color coded blocks right now.)
            //If this is the first one, we skip to the else and spawn at origin
            if (i > 0)
            {
                //List<Vector3> attempts = new List<Vector3>();   //This tracks all our attempts so I know if tile tries the same spot more than once.
                /* PART OF THE SECOND METHOD */
                List<Vector3> availableSpaces = new List<Vector3>();    //Track of all our available spaces that the tile can spawn in
                int[] difference = { (int)spacing * -1, (int)spacing }; //This is used to calculate the possible spawn spaces, as in how far on the x and z axis from this point.
                //These are the possible spawn spaces
                availableSpaces.Add(new Vector3(difference[1], 0, 0) + oldLocation);
                availableSpaces.Add(new Vector3(0, 0, difference[1]) + oldLocation);
                availableSpaces.Add(new Vector3(difference[0], 0, 0) + oldLocation);
                availableSpaces.Add(new Vector3(0, 0, difference[0]) + oldLocation);
                /**/
                //Simply runs a check to make sure we aren't trying to spawn it inside of another object.
                do
                {
                    int index = Random.Range(0, availableSpaces.Count); //Picks a random index
                    spawnLocation = availableSpaces[index]; //Grabs the vector at that index. This becomes the new spawn location.
                    availableSpaces.RemoveAt(index);//Removes the vector at that index so we can't grab it again.

                    /* THIS WAS PART OF MY EFFECIENCY TESTING. KEEPING IT BECAUSE I STILL USE IT WHEN TRYING TO IMRPOVE THIS METHOD
                    //this is used to keep track of how many times a tile tries to spawn in a taken spot.
                    if (attempts.Contains(spawnLocation))
                        Debug.Log("Tried a location more than once");
                    else
                        attempts.Add(spawnLocation);
                    */

                    //If we run out available spaces, we pick a random tile in the world and go to it.
                    if (availableSpaces.Count == 0)
                    {
                        oldLocation = _takenLocations[Random.Range(0, _takenLocations.Count)];  // Locations that have a tile but have room for neighbors
                        availableSpaces.Add(new Vector3(difference[1], 0, 0) + oldLocation);
                        availableSpaces.Add(new Vector3(0, 0, difference[1]) + oldLocation);
                        availableSpaces.Add(new Vector3(difference[0], 0, 0) + oldLocation);
                        availableSpaces.Add(new Vector3(0, 0, difference[0]) + oldLocation);

                    }
                    /**/

                    //if the spot we ended up with is already taken, try it again. When we pick a spot that isn't taken, we will break out of the loop
                } while (_takenLocations.Contains(spawnLocation));
            }
            else
            {
                selection = _spawnTile;
                spawnLocation = new Vector3(0, 0, 0);
            }

            //Create a reference so we can store the position fo the newly spawned object
            GameObject spawn = Instantiate(selection, spawnLocation, new Quaternion(), _labyrinthContainerTransform) as GameObject;

            
            oldLocation = spawn.transform.position; //Gets our old location
            _takenLocations.Add(oldLocation);   //Adds it to our list so we don't spawn blocks on top of eachother
            _spawnedTiles.Add(spawn);   // Keep track of all the tiles we spawned

            //_usableTakenLocations.Add(oldLocation); //Potential Efficiency boost (Not finished with this one yet.)
            //_spawnedTilesDictionary.Add(oldLocation,spawn);   // Can't remember what I was making this for but keeping it just incase

            /*
             * The greater the number I mod i by,
             * the shorter it takes to generate the 
             * maze. But will increase the time that
             * nothing else can be performed (such as a loading screen animation).
             */
            int modVal = 100;

            if (i % modVal == 0)    // The same 'i' used in the for loop.
                yield return null;
        }

        mazeGeneratedDelagate();    // Let subscribes know the maze is done generating
        FindFurthestTile(); // Find the furthest tile, this is the end tile
        navMeshSurface.BuildNavMesh();  // Build the navMesh so our enemies can navigate
        mazeNavMeshBuiltDelegate(); // Let the enemies know whte navMesh has been built
        loadingScreen.enabled = false;  // Disable our loading screen
    }

    // A simple printList method. Debugging purposes
    private void PrintList(List<Vector3> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i]);
        }
    }

    // Getter for the TilesetAdpaterScript
    public List<Vector3> GetTakenLocations()
    {
        return _takenLocations;
    }

    // Finds the furthest tile from origin
    private void FindFurthestTile()
    {
        float furhtestDistance = 0f; // Furthest distance found from origin. Default to 0
        GameObject furthestTile = null; // Set up our object for the furthest tile

        foreach(GameObject i in _spawnedTiles)  // Cycle through all spawned tiles...
        {
            /* Just remembered there's a function for this lol.
            float x = Mathf.Abs(i.transform.position.x);
            float z = Mathf.Abs(i.transform.position.z);
            float distance = Mathf.Sqrt((x * x) + (z * z));
            */
            float distance = Vector3.Distance(Vector3.zero, i.transform.position);  // Finds distance between tile and origin
            if (distance > furhtestDistance)    // If its the greatest distance so far...
            {
                furhtestDistance = distance;    // Cache the distance
                furthestTile = i;               // Cache the tile
            }
        }
        //furthestTile.transform.position = furthestTile.transform.position + new Vector3(0, 10, 0);    // THis was to raise it up so I could easily tell which was the farthest
        BoxCollider col = furthestTile.AddComponent(typeof(BoxCollider)) as BoxCollider;    // Add a collider to the tile
        col.isTrigger = true;      // Make it a trigger
        col.size = new Vector3(1,2,1);  //Scale up the collider so the player can trigger it
        col.tag = "EndZone";    // Give it the appropriate tag
    }

/*  //Part of a more effecient idea
    public void RemoveFromUsableTaken(Vector3 vector)
    {
        _usableTakenLocations.Remove(vector);
    }
*/
    public List<GameObject> GetSpawnedTiles()
    {
        return _spawnedTiles;
    }
}
