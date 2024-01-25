using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstacles;

    private List<GameObject> obstacleList = new List<GameObject>();

    private GameObject[] obstacleMarkers;

    // Start is called before the first frame update
    void Start()
    {
        obstacleMarkers = GameObject.FindGameObjectsWithTag("ObstacleMarker");

        print(obstacleMarkers.Length);

        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacleList.Add(obstacles[i]);
        }

        for (int i = 0; i < obstacleList.Count; i++)
        {
            GameObject temp = obstacleList[i];
            int randomIndex = Random.Range(i, obstacleList.Count);
            obstacleList[i] = obstacleList[randomIndex];
            obstacleList[randomIndex] = temp;
        }

        for(int i = 0; i < obstacleMarkers.Length; i++)
        {
            Instantiate(obstacleList[0], obstacleMarkers[i].transform.position, obstacleMarkers[i].transform.rotation, obstacleMarkers[i].transform);

            obstacleList.RemoveAt(0);
        }
    }
}
