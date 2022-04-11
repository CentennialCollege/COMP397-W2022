using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class WorldMaker : MonoBehaviour
{
    [Header("Player Properties")] 
    public GameObject playerPrefab;

    [Header("World Properties")] 
    [Range(8, 128)]
    public int height = 1;
    [Range(8, 128)]
    public int width = 1;
    [Range(8, 128)]
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

    private Queue<GameObject> pool;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = new List<GameObject>(); // creates a new empty container

        BuildPool();
        Generate();
    }

    private void CreateTile()
    {
        var tile = Instantiate(threeDTile, Vector3.zero, Quaternion.identity);
        tile.SetActive(false);
        tile.transform.SetParent(tileParent);
        pool.Enqueue(tile);
    }

    private void BuildPool()
    {
        pool = new Queue<GameObject>();

        for (int i = 0; i < 80000; i++)
        {
            CreateTile();
        }
    }

    private GameObject GetTile(Vector3 position = new Vector3())
    {
        GameObject tile = null;
        if (pool.Count < 1)
        {
            tile = Instantiate(threeDTile, Vector3.zero, Quaternion.identity);
            tile.SetActive(false);
            tile.transform.SetParent(tileParent);
        }
        else
        {
            tile = pool.Dequeue();
            tile.SetActive(true);
        }

        tile.transform.position = position;
        return tile;
    }

    private void ReleaseTile(GameObject tile)
    {
        this.AddComponent<BoxCollider>();
        tile.SetActive(false);
        pool.Enqueue(tile);
    }

    private void Generate()
    {
        Initialize();
        Regenerate();
        Invoke("RemoveInternalTiles", 0.1f);
        Invoke("CombineMeshes", 0.2f);
        Invoke("ResetMap",  0.3f);
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
                    var perlinValue = Mathf.PerlinNoise((x + offsetX) / rand, (z + offsetZ) / rand) * depth * 0.5f;

                    if (y < perlinValue)
                    {
                        var tile = GetTile(new Vector3(x, y, z));
                        grid.Add(tile);
                    }
                }
            }
        }
    }

    private void ResetMap()
    {
        var size = grid.Count;
        for (int i = 0; i < size; i++)
        {
            ReleaseTile(grid[i]);
        }

        grid.Clear();
    }

    private void RemoveInternalTiles()
    {
        var normalArray = new Vector3[] { Vector3.up, Vector3.down, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
        List<GameObject> tilesToBeRemoved = new List<GameObject>();

        foreach (var tile in grid)
        {
            int collisionCounter = 0;
            for (int i = 0; i < normalArray.Length; i++)
            {
                if (Physics.Raycast(tile.transform.position, normalArray[i], tile.transform.localScale.magnitude * 0.3f))
                {
                    collisionCounter++;
                }
            }

            if (collisionCounter > 5)
            {
                tilesToBeRemoved.Add(tile);
            }
        }

        foreach (var tile in grid)
        {
            Destroy(tile.GetComponent<BoxCollider>());
        }

        var size = tilesToBeRemoved.Count;
        for (int i = 0; i < size; i++)
        {
            grid.Remove(tilesToBeRemoved[i]);
            ReleaseTile(tilesToBeRemoved[i]);
        }

        tilesToBeRemoved.Clear();
    }

    private void CombineMeshes()
    {
        var meshFilter = tileParent.GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh {indexFormat = UnityEngine.Rendering.IndexFormat.UInt32};

        List<MeshFilter> meshFilters = new List<MeshFilter>(); // all mesh filters from the tiles
        foreach (var tile in grid)
        {
            meshFilters.Add(tile.GetComponent<MeshFilter>());
        }

        CombineInstance[] combine = new CombineInstance[meshFilters.Count];

        int i = 0;
        while (i < meshFilters.Count)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        meshFilter.mesh.CombineMeshes(combine);
        tileParent.GetComponent<MeshCollider>().sharedMesh = meshFilter.sharedMesh;
    }

    private void PositionPlayer()
    {
        playerPrefab.GetComponent<CharacterController>().enabled = false;
        playerPrefab.transform.position = new Vector3(width * 0.5f, height + 10.0f, +depth *  0.5f);
        playerPrefab.GetComponent<CharacterController>().enabled = true;
    }
}
