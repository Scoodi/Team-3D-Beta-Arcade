using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Biome
{
    public Biome(string _name, GameObject[] _tiles, string _biomeBGMEvent)
    {
        name = _name;
        tiles = _tiles;
        biomeBGMEvent = _biomeBGMEvent;

        biomeBGM = FMODUnity.RuntimeManager.CreateInstance(biomeBGMEvent);
    }

    public string name;
    public GameObject[] tiles;

    public string biomeBGMEvent;
    public FMOD.Studio.EventInstance biomeBGM;
}

[Serializable]
public class BiomeS
{
    public BiomeS(Biome _biome, int _tileCount, float _startX) { biome = _biome; tileCount = _tileCount; tilesToSpawn = tileCount; startX = _startX; endx = startX + 45 * tileCount; }

    public Biome biome;
    public int tileCount, tilesToSpawn;
    public float startX, endx;

    public List<GameObject> spawnedTiles = new List<GameObject>();

}

public class WorldGenScript : MonoBehaviour
{
    //public int numToBuild;

    public Transform origin;
    public float distanceBetween;
    //public GameObject[] tiles;

    public Biome[] biomes;
    public List<BiomeS> game_biomes = new List<BiomeS>();

    public float lastBiomeEndx = 0;
    Vector3 currentPos;
    float spawnStartX = 0;
    public PlayerScript player;

    void SpawnTiles(ref List<BiomeS> biomes, ref int toSpawn)
    {
        for (int i = 0; i < biomes.Count; i++)
        {
            var biome = biomes[i];
            if (biome.tilesToSpawn <= 0)
            {
                continue;
            }

            while (toSpawn > 0)
            {
                GameObject go = new GameObject();

                if (biome.tilesToSpawn <= 0)
                {
                    break;
                }

                BuildWorld(1, biome.biome.tiles, ref go);
                biome.spawnedTiles.Add(go);

                toSpawn--;
                biome.tilesToSpawn--;
            }
        }
    }

    void Start()
    {
        BuildWorld(2, biomes[0].tiles);
        spawnStartX = currentPos.x;
    }

    int toSpawn;
    BiomeS currentBiome;

    bool e = false;

    void check_to_del_biomes()
    {
        //var dist = Vector2.Distance(
        //    Vector2.right * game_biomes[0].startX + Vector2.right * (game_biomes[0].spawnedTiles.Count * distanceBetween),
        //    Vector2.right * player.transform.position.x);

        // print($" {currentPos.x}, {dist}, {lastBiomeEndx}, {player.transform.position.x}");

        //print($"{lastBiomeEndx}, {player.transform.position.x + distanceBetween * 1.55f}");
        //Debug.DrawLine(new Vector3(lastBiomeEndx, 0, 0), new Vector3(lastBiomeEndx, 10, 0), Color.red);
        //Debug.DrawLine(new Vector3(lastBiomeEndx + distanceBetween * 2, 0, 0), new Vector3(lastBiomeEndx + distanceBetween * 2, 10, 0), Color.red);

        if (lastBiomeEndx + distanceBetween <= player.transform.position.x)
        {
            var b = game_biomes[0];

            for (int k = 0; k < b.spawnedTiles.Count; k++)
            {
                Destroy(b.spawnedTiles[k].gameObject);
            }

            game_biomes.RemoveAt(0);
            lastBiomeEndx = game_biomes[0].endx;
        }
    }
    float test = 0;

    void Update()
    {
        if (game_biomes.Count < 50)
        {
            int i = e ? 1 : 0;
            game_biomes.Add(new BiomeS(biomes[i], UnityEngine.Random.Range(2, 5), origin.position.x + currentPos.x));
            //currentPos.x += game_biomes[game_biomes.Count - 1].tileCount * 45;

            lastBiomeEndx = game_biomes[0].endx;

            e = !e;
        }

        toSpawn = 2;

        var dist = Vector2.Distance(
          Vector2.right * game_biomes[0].startX + Vector2.right * (game_biomes[0].spawnedTiles.Count * distanceBetween),
          Vector2.right * player.transform.position.x);

        //print("t " + currentPos.x + " y " + player.transform.position.x);
        //print("b " + Vector2.right * game_biomes[0].startX + Vector2.right * (game_biomes[0].spawnedTiles.Count * distanceBetween));
        //print("dist " + (game_biomes[0].startX + game_biomes[0].spawnedTiles.Count * distanceBetween));

        print($"current pos x {origin.position.x + currentPos.x} " +
            $"player transform {player.transform.position.x}");

        if (origin.position.x + currentPos.x <= player.transform.position.x + distanceBetween * 1.55f)
        {
            SpawnTiles(ref game_biomes, ref toSpawn);
        }

        //Debug.DrawLine(new Vector3(lastBiomeEndx, 0, 0), new Vector3(lastBiomeEndx, 10, 0), Color.red);

        //check_to_del_biomes();
    }


    void BuildWorld(int numOfTiles, GameObject[] tilesToUse)
    {
        currentPos = origin.position + Vector3.right * currentPos.x;
        //posToSpawn += currentPos;

        for (int i = 0; i < numOfTiles; i++)
        {
            Instantiate(tilesToUse[UnityEngine.Random.Range(0, tilesToUse.Length)], currentPos, Quaternion.identity);
            currentPos.x = currentPos.x + distanceBetween;
        }

        //posToSpawn += Vector3.right * distanceBetween * numOfTiles;
    }

    void BuildWorld(int numOfTiles, GameObject[] tilesToUse, ref GameObject _go)
    {
        currentPos = origin.position + Vector3.right * currentPos.x;
        //posToSpawn += currentPos;

        for (int i = 0; i < numOfTiles; i++)
        {
            GameObject go = Instantiate(tilesToUse[UnityEngine.Random.Range(0, tilesToUse.Length)], currentPos, Quaternion.identity);
            _go = go;
            currentPos.x = currentPos.x + distanceBetween;
        }

        //posToSpawn += Vector3.right * distanceBetween * numOfTiles;
    }

    private void OnDrawGizmos()
    {
    }
}
