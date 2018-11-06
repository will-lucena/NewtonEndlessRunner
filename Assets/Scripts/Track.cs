using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public GameObject[] obstacles;
    public List<GameObject> currentObstacles;
    public int minObstaclesAmount;
    public int maxObstaclesAmount;
    public float trackLength;

    private void Awake()
    {
        int obstaclesAmount = Random.Range(minObstaclesAmount, maxObstaclesAmount);

        for (int i = 0; i < obstaclesAmount; i++)
        {
            currentObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length-1)], transform));
            currentObstacles[i].SetActive(false);
        }
        spawnObstacles();
    }

    private void spawnObstacles()
    {
        for (int i = 0; i < currentObstacles.Count; i++)
        {
            float posZ = (trackLength / currentObstacles.Count) * 2 * i;
            currentObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZ, posZ + 1));
            currentObstacles[i].SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.position = new Vector3(0, 0, transform.position.z + trackLength * 2);
            Invoke("spawnObstacles", 5f);
        }
    }
}
