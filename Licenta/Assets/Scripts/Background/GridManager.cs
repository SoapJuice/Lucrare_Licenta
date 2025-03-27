using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private int width, height;
    [SerializeField]
    private Tile tile;
    [SerializeField]
    private Component player;
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private WallTile wall;

    private Tile endTile;

    private void Start()
    {
        GenerateBase(new Vector3(7,0,0), new Vector3(7,8,0));
    }
    void GenerateBase(Vector3 StartPosition, Vector3 EndPosition)
    {
        for (int i = 0; i < width; i++)
        {
            
            for(int j = 0; j < height; j++)
            {
             
                var spawnedTile = Instantiate(tile, new Vector3(i,j), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j}";


                spawnedTile.PatternColor(i % 2 == 0 && j % 2 != 0 || i % 2 != 0 && j % 2 == 0);
            }
        }

        player.transform.position = StartPosition;

        endTile = Instantiate(tile, EndPosition, Quaternion.identity);
        endTile.SingleColor(Color.green);

        cam.transform.position = new Vector3 ((float) width/2 - 0.5f, (float) height/2 - 0.5f, -10);

        for (int i = -1;  i <= width; i++)
        {
            var spawnedNordWalls = Instantiate(wall, new Vector3(i, height), Quaternion.identity);
            spawnedNordWalls.name = $"Wall {i} {height}";
            spawnedNordWalls.SingleColor(Color.black);

            var spawnedSouthWalls = Instantiate(wall, new Vector3(i, -1), Quaternion.identity);
            spawnedSouthWalls.name = $"Wall {i} {-1}";
            spawnedSouthWalls.SingleColor(Color.black);
        }

        for (int i = -1; i <= height; i++)
        {
            var spawnedEastWalls = Instantiate(wall, new Vector3(width, i), Quaternion.identity);
            spawnedEastWalls.name = $"Wall {i} {-1}";
            spawnedEastWalls.SingleColor(Color.black);

            var spawnedWestWalls = Instantiate(wall, new Vector3(-1, i), Quaternion.identity);
            spawnedWestWalls.name = $"Wall {i} {-1}";
            spawnedWestWalls.SingleColor(Color.black);
        }
    }

    private void Update()
    {
        if (player.transform.position.x >= endTile.transform.position.x - 0.3f &&
            player.transform.position.x <= endTile.transform.position.x + 0.3f &&
            player.transform.position.y >= endTile.transform.position.y - 0.3f &&
            player.transform.position.y <= endTile.transform.position.y + 0.3f) { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
