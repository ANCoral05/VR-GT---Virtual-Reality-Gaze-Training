using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject[] tiles;
    public GameObject goal;
    public GameObject environment;

    public GameObject spawner;
    public GameObject startSpawner;
    private GameObject tile;
    public GameObject[] obstacleSpawner;
    private int obstacleCounter;
    private bool findObstacles;

    public List<string> tileNames = new List<string>();
    public List<string> obstacleNames = new List<string>();

    public int tileCounter;

    public GameObject[] obstacles;

    private List<GameObject> obstacleList = new List<GameObject>();

    private GameObject[] obstacleMarkers;

    bool obstaclesSpawned;

    public int currentStep;

    public bool finished;

    void Start()
    {
        
    }

    void Update()
    {
        if(currentStep == 1)
        {
            StageZero();
        }

        if(currentStep == 2)
        {
            StageOne();
        }

        if(currentStep == 3)
        {
            StageTwo();
        }

        
    }

    void StageZero()
    {
        obstaclesSpawned = false;

        finished = false;

        tileCounter = 0;

        for(int i = 0; i<tiles.Length; i++)
        {
            tiles[i].SetActive(false);
        }

        for(int i = 0; i<obstacles.Length; i++)
        {
            obstacles[i].SetActive(false);
        }
        
        spawner = startSpawner;

        currentStep = 2;
    }

    void StageOne()
    {
        while (tileCounter < tiles.Length)
        {
            int random = Random.Range(0, tiles.Length);

            if (!tiles[random].activeSelf)
            {
                tile = tiles[random];

                tile.SetActive(true);

                tile.transform.position = spawner.transform.position;

                tile.transform.rotation = spawner.transform.rotation;

                tile.transform.parent = environment.transform;
                //tile = Instantiate(tiles[random], spawner.transform.position, spawner.transform.rotation, environment.transform);

                tileNames.Add(tile.name);

                tileCounter += 1;

                foreach (Transform child in tile.transform)
                {
                    if (child.tag == "TileSpawner")
                        spawner = child.gameObject;
                }

                //tiles[random] = null;
            }

        }

        if (tileCounter == tiles.Length)
        {
            foreach (Transform child in tile.transform)
            {
                if (child.tag == "TileSpawner")
                    spawner = child.gameObject;
            }

            tile = goal;

            tile.transform.position = spawner.transform.position;
            tile.transform.rotation = spawner.transform.rotation;
            tile.transform.parent = environment.transform;

            //tile = Instantiate(goal, spawner.transform.position, spawner.transform.rotation, environment.transform);

            tileNames.Add("goal");

            //directory = rotationMeasurement.directory;

            //WriteHeader();

            //goal = null;
        }

        //if (goal == null && !findObstacles)
        //{
        //    obstacleSpawner = GameObject.FindGameObjectsWithTag("ObstacleSpawner");

        //    foreach (GameObject singleObstacleSpawner in obstacleSpawner)
        //    {
        //        //obstacleNames.Add(singleObstacleSpawner.transform.GetChild(0).name);
        //    }

        //    //WriteTrackingData();

        //    findObstacles = true;
        //}

        if (!obstaclesSpawned && tileCounter == tiles.Length)
        {
            SpawnObstacles();

            finished = true;

            currentStep = 3;

            obstaclesSpawned = true;
        }
    }

    void StageTwo()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            currentStep = 1;
        }
    }

    void SpawnObstacles()
    {
        obstacleMarkers = GameObject.FindGameObjectsWithTag("ObstacleMarker");

        print("Markers: " + obstacleMarkers.Length + "; " + obstacles.Length);

        for (int i = 0; i < obstacles.Length; i++)
        {
            print("counting: " + i);
            obstacleList.Add(obstacles[i]);
        }

        for (int i = 0; i < obstacleList.Count; i++)
        {
            GameObject temp = obstacleList[i];
            int randomIndex = Random.Range(i, obstacleList.Count);
            obstacleList[i] = obstacleList[randomIndex];
            obstacleList[randomIndex] = temp;
        }

        for (int i = 0; i < obstacleMarkers.Length; i++)
        {
            GameObject obstacle = obstacleList[0];

            obstacle.SetActive(true);

            obstacle.transform.position = obstacleMarkers[i].transform.position;
            obstacle.transform.rotation = obstacleMarkers[i].transform.rotation;
            obstacle.transform.parent = obstacleMarkers[i].transform;

            //Instantiate(obstacleList[0], obstacleMarkers[i].transform.position, obstacleMarkers[i].transform.rotation, obstacleMarkers[i].transform);

            obstacleList.RemoveAt(0);
        }
    }
}
