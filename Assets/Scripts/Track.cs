using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public static System.Action<float> increaseSpeed;

    public GameObject[] obstacles;
    public GameObject coinPrefab;
    public List<GameObject> currentObstacles;
    public List<GameObject> currentCoins;
    public int minObstaclesAmount;
    public int maxObstaclesAmount;
    public int maxCoins;
    public int minCoins;
    public float trackLength;
    public float speedModifier;

    private void Awake()
    {
        int obstaclesAmount = Random.Range(minObstaclesAmount, maxObstaclesAmount);
        int coinsAmount = Random.Range(minCoins, maxCoins);

        for (int i = 0; i < obstaclesAmount; i++)
        {
            GameObject selectedPrefab = obstacles[Random.Range(0, obstacles.Length)];
            GameObject go = Instantiate(selectedPrefab, transform);
            go.SetActive(false);
            go.name = selectedPrefab.name;
            currentObstacles.Add(go);
        }

        for (int i = 0; i < coinsAmount; i++)
        {
            GameObject go = Instantiate(coinPrefab, transform);
            go.SetActive(false);
            currentCoins.Add(go);
        }

        spawnObstacles();
        spawnCoins();
    }

    private void spawnObstacles()
    {
        for (int index = 1; index < currentObstacles.Count; index++)
        {
            float posZ = (trackLength / currentObstacles.Count) * 2 * index;
            Vector3 spawnPosition = new Vector3(0, 0, Random.Range(posZ, posZ + 1));

            if (currentObstacles[index].name.Equals("ObstacleBin"))
            {
                spawnPosition.x = (int)Random.Range(-1, 2);
            }

            currentObstacles[index].transform.localPosition = spawnPosition;
            currentObstacles[index].SetActive(true);
        }
    }

    private void spawnCoins()
    {
        float minZ = 10f;
        for (int i = 0; i < currentCoins.Count; i++)
        {
            float posZ = (trackLength / currentCoins.Count) * 2 * i;
            currentCoins[i].transform.localPosition = new Vector3((int)Random.Range(-1, 2), 0, Random.Range(minZ, minZ + 5));
            currentCoins[i].SetActive(true);
            minZ = posZ + 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            increaseSpeed?.Invoke(speedModifier);
            transform.position = new Vector3(0, 0, transform.position.z + trackLength * 2);
            spawnObstacles();
            spawnCoins();
        }
    }
}
