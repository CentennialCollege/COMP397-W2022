using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class WorldMaker : MonoBehaviour
{
    [Header("Player Properties")] 
    public GameObject playerPrefab;

    [Header("World Properties")] 
    [Range(1, 64)]
    public int height = 1;
    [Range(1, 64)]
    public int width = 1;
    [Range(1, 64)]
    public int depth = 1;

    [Header("Scaling Values")] 
    public float min = 16.0f;
    public float max = 24.0f;

    [Header("Tile Properties")] 
    public Transform tileParent;
    public GameObject threeDTile;

    [Header("Grid")] 
    public List<GameObject> grid;

    // starting values
    private int startHeight;
    private int startWidth;
    private int startDepth;
    private float startMin;
    private float startMax;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = new List<GameObject>(); // creates a new empty container

        Generate();
    }

    private void Generate()
    {
        Initialize();
        Reset();
        Regenerate();
        PositionPlayer();
    }

    void Update()
    {
        if (height != startHeight || depth != startDepth || width != startWidth || min != startMin || max != startMax)
        {
            Generate();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Generate();
        }
    }

    private void Initialize()
    {
    startHeight = height;
    startWidth = width;
    startDepth = depth;
    startMin = min;
    startMax = max;
    }

    private void Regenerate()
    {
        // generation

        // perlin noise texture that we will sample
        float rand = Random.Range(min, max);

        float offsetX = Random.Range(-1024.0f, 1024.0f);
        float offsetZ = Random.Range(-1024.0f, 1024.0f);

        for (int y = 0; y < height; y++)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y < Mathf.PerlinNoise((x + offsetX) / rand, (z + offsetZ) / rand) * depth * 0.5f)
                    {
                        var tile = Instantiate(threeDTile, new Vector3(x, y, z), Quaternion.identity);
                        tile.transform.parent = tileParent;
                        grid.Add(tile);
                    }
                }
            }
        }
    }

    private void Reset()
    {
        foreach (var tile in grid)
        {
            Destroy(tile);
        }

        grid.Clear();
    }

    private void PositionPlayer()
    {
        playerPrefab.GetComponent<CharacterController>().enabled = false;
        playerPrefab.transform.position = new Vector3(width * 0.5f, height + 10.0f, +depth *  0.5f);
        playerPrefab.GetComponent<CharacterController>().enabled = true;
    }
}
