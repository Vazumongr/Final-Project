using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Troy Records Jr.
 * Last Updated: April 19th, 2020
 * This is the script for handling
 * the enemy spawning. Enemy spawn locations
 * are random (they have to be) but 
 * the amount is predetermined.
 */
public class EnemySpawner : MonoBehaviour {

	
	public GameObject enemyPrefab;	// The prefab of the enemy we want to spawn
	public GameObject enemyContainer;	// The conatainer to house all the enemies so the heirarchy isn't chaos
	public int EnemyCount = 5;	// The amount of enemies we want to spawn

	void OnEnable()
    {
        LabyrinthGenerationScript.mazeNavMeshBuiltDelegate += SpawnEnemies;	// Subscribe so we know when the navMesh is ready
    }

    void OnDisable()
    {
        LabyrinthGenerationScript.mazeNavMeshBuiltDelegate -= SpawnEnemies;	// Desubscribe(is that word?)
    }

	void SpawnEnemies()
	{
		List<GameObject> _spawnedTiles = LabyrinthGenerationScript.instance.GetSpawnedTiles();	// Gets all the tiles in the world from the LabyrinthGenerationScript
		List<GameObject> _usedTiles = new List<GameObject>();	// Sets up a list to keep track of which tiles we used

		GameObject _targetTile;	// Sets up an object to hold the tile we choose

		for(int i = 0; i < EnemyCount; i++)	// Spawn as many enemies as we need...
		{
			do
			{
				_targetTile = _spawnedTiles[Random.Range(0,_spawnedTiles.Count)];	//Pick a random tile
			}while(_usedTiles.Contains(_targetTile));	// If it's already been used, pick a new one

			GameObject instance = Instantiate(enemyPrefab, _targetTile.transform.position, _targetTile.transform.rotation, enemyContainer.transform);	// Spawn the enemy on that tile
			_usedTiles.Add(_targetTile);	// Add it to our usedTiles list
		}
		
	}
	
}
