using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenScript : MonoBehaviour
{
    public int numToBuild;

    public Transform origin;
    public float distanceBetween;
    public GameObject[] tiles;
    // Start is called before the first frame update
    void Start()
    {
        BuildWorld(numToBuild);
        //SpawnBatteries();
    }

    private void SpawnBatteries()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildWorld (int numOfTiles)
    {
        Vector3 currentPos = origin.position;
        for (int i = 0; i < numOfTiles; i++)
        {
            Instantiate(tiles[Random.Range(0,tiles.Length)], currentPos, Quaternion.identity);
            currentPos.x = currentPos.x + distanceBetween;
        }
    }
}
