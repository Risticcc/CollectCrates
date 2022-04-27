using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SpawnManager : MonoBehaviour
{
    public Action<string> TimeToMerge;

    [SerializeField] private Button spawnButton;
    [SerializeField]private int level;
    [SerializeField]private bool spanAllow;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] ParticleSystem particle;
  

    private Tile[] tiles;
    private float lerpDuration = 1;
    private string cubeLevel;

    private  int spawnedPrefabs = 0;
    private  int numberOfMerges = 0;

    public  int SpawnedPrefabs { get => spawnedPrefabs; set=> spawnedPrefabs = value; }
    public  int NumberOfMerges { get => numberOfMerges; set => numberOfMerges = value; }

    private void Update()
    {
        level = SpawnedPrefabs.ToString().Length;
    }


    private void Start()
    {
        CreateTable();
        spanAllow = true;
        TimeToMerge = Merge;
    }

    private void SpawnPrefab()
    {
        if (spanAllow == false)
            return;


        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].Free == true)
            {
                tiles[i].Prefab = Instantiate(prefabs[0], tiles[i].Postion, prefabs[0].transform.rotation);
                tiles[i].Free = false;
                SpawnedPrefabs++;
                break;
            }
        }
        if (SpawnedPrefabs % 10 == 0)
        {
            spanAllow = false;
            int num = SpawnedPrefabs;
            int lvl = 0;
            while (num != 0)
            {
                if (num % 10 == 0)
                {
                    lvl++;
                    TimeToMerge.Invoke("level" + lvl);
                    
                }
                num = num / 10;
            }
        }
    }

    public void CreateTable()
    {

        float columns = 4;
        float rows = 6;

        Vector3 startPoition = new Vector3(-12.5f, 6f, 9.5f);
        Vector3 nextColumn = new Vector3(5f, 0, 0);
        Vector3 nextRow = new Vector3(0, 0, -5);
        Vector3 position = startPoition;

        tiles = new Tile[24];
        int current = 0;
        for (int i = 0; i< rows;i++)
        {
            position = startPoition + nextRow*i; 
            for(int j = 0; j< columns; j++)
            {
                position = position + nextColumn;
                tiles[current] = new Tile(position, true);
                current++;
            }
        }
    }

    private void Merge(string cubeLevel)
    {
        level = SpawnedPrefabs.ToString().Length;
        NumberOfMerges++;

        foreach (Tile tile in tiles)
        {
            if (tile.Free == false && tile.Prefab.tag.Equals(cubeLevel))
            {
                StartCoroutine(Lerp(tile.Prefab.transform.position, tiles[(NumberOfMerges - 1) % 10].Postion, tile));
            }
        }

        StartCoroutine(Delete(cubeLevel));
        StartCoroutine(LevelUp(cubeLevel));
        AnalyticsManager.analyticsManager.MergeEvent(NumberOfMerges);
    }

    IEnumerator Lerp(Vector3 startValue, Vector3 endValue, Tile tile)
    {    
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            tile.Prefab.transform.position = Vector3.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Delete(string cubeLevel)
    {
        yield return new WaitForSeconds(1);

        foreach (Tile tile in tiles)
        {
            if (tile.Free == false && tile.Prefab != null)
            {
                if (tile.Prefab.gameObject.tag.Equals(cubeLevel))
                {
                    Destroy(tile.Prefab.gameObject);
                    tile.Prefab = null;
                    tile.Free = true;
                }
            }
        }
    }

    IEnumerator LevelUp(string cubeLevel)
    {
        yield return new WaitForSeconds(1);

        //0 -> crvene level1
        //1 -> plave level2
        int levellIndex = cubeLevel[cubeLevel.Length - 1] - '0';
       
        foreach(Tile tile in tiles)
        {
            if(tile.Free ==true)
            {
                particle.transform.position = tile.Postion;
              
                if(!particle.isPlaying)
                    particle.Play();
  
               
                tile.Prefab = Instantiate(prefabs[levellIndex], tile.Postion, prefabs[levellIndex].transform.rotation);
                tile.Free = false;

                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, tile.Postion.z-12f);
                spanAllow = true;
                break;
            }
        }

    }  
}