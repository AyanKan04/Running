using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TileManager : MonoBehaviour
{
    public static string currentScene;
    private bool isSurMode = false;
    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float tileLength = 30;
    public int numberOfTiles = 5;
    private List<GameObject> activeTiles = new List<GameObject>();

    
    public static int indexLevel;

    public Transform playerTransform;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case ("Level1"): indexLevel = 1; break;
            case ("Level2"): indexLevel = 2; break;
            case ("Level3"): indexLevel = 3; break;
            case ("Level4"): indexLevel = 4; break;
            case ("Level5"): indexLevel = 5; break;
        }
        if (currentScene == "Play")
        {
            isSurMode = true;
            for (int i = 0; i < numberOfTiles; i++)
            {
                if (i == 0)
                {
                    SpawnTile(0);
                }
                else
                {
                    int randomIndex = Random.Range(0, tilePrefabs.Length);
                    SpawnTile(randomIndex);
                }
            }
        }
        else
        {
            isSurMode = false;
            //int maxTiles = Mathf.Min(numberOfTiles, tilePrefabs.Length);
            for (int i = 0; i < tilePrefabs.Length; i++)
            {
                SpawnTile(i);
            }               
        }

        
    }

    private void Update()
    {
        if (isSurMode)
        {
            if (playerTransform.position.z - 35 > zSpawn - numberOfTiles * tileLength)
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length));
                DeleteTile();
            }
        }
        
    }
    public void SpawnTile(int tileIndex)
    {
        if (tileIndex >= 0 && tileIndex < tilePrefabs.Length)
        {
            GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
            activeTiles.Add(go);
            zSpawn += tileLength;
        }
        else
        {
            Debug.LogError("Tile index out of range: " + tileIndex);
        }

    }
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
