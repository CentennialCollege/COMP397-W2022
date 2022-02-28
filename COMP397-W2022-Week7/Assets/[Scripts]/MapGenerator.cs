using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Tile Resources")] 
    public List<GameObject> tilePrefabs;

    [Header("Map Properties")] 
    [Range(2, 30)]
    public int width = 2;
    [Range(2, 30)] 
    public int depth = 2;

    [Header("Generated Tiles")] 
    public List<GameObject> tiles;

    // Start is called before the first frame update
    void Start()
    {
        BuildMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildMap()
    {

        // Generate first 3 tile
        for (int i = 0; i < 3; i++)
        {
            var randomPrefabIndex = Random.Range(0, 4);
            var randomRotation = Quaternion.Euler(0.0f, Random.Range(0, 4) * 90.0f, 0.0f);
            tiles.Add(Instantiate(tilePrefabs[randomPrefabIndex], Vector3.zero, randomRotation));
        }

        tiles[0].transform.position = new Vector3(0.0f, 0.0f, 20.0f);
        tiles[1].transform.position = new Vector3(20.0f, 0.0f, 20.0f);
        tiles[2].transform.position = new Vector3(20.0f, 0.0f, 0.0f);





        for (int row = 0; row < depth; row++)
        {
            for (int col = 0; col < width; col++)
            {
                


            }
        }
    }
}
