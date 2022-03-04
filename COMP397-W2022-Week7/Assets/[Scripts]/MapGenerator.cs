using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    [Header("Tile Resources")] 
    public List<GameObject> tilePrefabs;
    public GameObject startTile;
    public GameObject goalTile;

    [Header("Map Properties")] 
    [Range(3, 30)]
    public int width = 3;
    [Range(3, 30)] 
    public int depth = 3;
    public Transform parent;

    [Header("Generated Tiles")] 
    public List<GameObject> tiles;

    private int startWidth;
    private int startDepth;

    // Start is called before the first frame update
    void Start()
    {
        startWidth = width;
        startDepth = depth;
        BuildMap();
        BakeNavMeshes();
    }

    // Update is called once per frame
    void Update()
    {
        if (width != startWidth || depth != startDepth)
        {
            ResetMap();
            BuildMap();
            Invoke(nameof(BakeNavMeshes), 0.2f);
            
        }
    }

    public void ResetMap()
    {
        startWidth = width;
        startDepth = depth;
        var size = tiles.Count;

        for (int i = 0; i < size; i++)
        {
            Destroy(tiles[i]);
        }

        tiles.Clear(); // removes all tiles
    }


    public void BuildMap()
    {
        var offset = new Vector3(20.0f, 0.0f, 20.0f);

        // place the start tile
        tiles.Add(Instantiate(startTile, Vector3.zero, Quaternion.identity, parent));

        // choose a random goal position, cannot be equal the start and cannot be larger than the grid
        var randomGoalRow = Random.Range(1, depth + 1);
        var randomGoalCol = Random.Range(1, width + 1);

        // generate more tiles if both width and depth are both greater than 2
        for (int row = 1; row <= depth; row++)
        {
            for (int col = 1; col <= width; col++)
            {
                if (row == 1 && col == 1) {continue;}

                var tilePosition = new Vector3(col * 20.0f, 0.0f, row * 20.0f) - offset;

                if (row == randomGoalRow && col == randomGoalCol)
                {
                    // place the goal tile
                    tiles.Add(Instantiate(goalTile, tilePosition, Quaternion.identity, parent));
                }
                else
                {
                    var randomPrefabIndex = Random.Range(0, 4);
                    var randomRotation = Quaternion.Euler(0.0f, Random.Range(0, 4) * 90.0f, 0.0f);
                    tiles.Add(Instantiate(tilePrefabs[randomPrefabIndex], tilePosition, randomRotation, parent));
                }
                
            }

        }

       
    }

    public void BakeNavMeshes()
    {
        foreach (var tile in tiles)
        {
            tile.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
}
