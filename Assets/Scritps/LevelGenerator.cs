using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public GameObject floorPrefab;

    [Header("Generator Settings")]
    public float floorWidth = 20f;
    public int totalPoolSize = 3;

    [Header("Randomization Settings")]
    [Range(0f, 1f)]
    public float obstacleSpawnChance = 0.5f; // Probabilidad base
    public int maxObstaclesPerFloor = 2;     // Límite por tramo

    [Header("Dynamic Obstacle Settings")]
    public float baseObstacleChance = 0.4f; // 40% al inicio
    public float maxObstacleChance = 0.85f; // Hasta 85% en niveles altos

    private List<GameObject> floorPool = new List<GameObject>();
    private int oldestFloorIndex = 0;
    private float nextSpawnX = 0f;

    void Start()
    {
        for (int i = 0; i < totalPoolSize; i++)
        {
            Vector3 spawnPosition = new Vector3(nextSpawnX, -3f, 0f);
            GameObject newFloor = Instantiate(floorPrefab, spawnPosition, Quaternion.identity);
            
            if (i == 0)
            {
                ClearAllObstacles(newFloor);
            }
            else
            {
                RandomizeObstaclesInFloor(newFloor);
            }

            floorPool.Add(newFloor);
            nextSpawnX += floorWidth;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;
        if (floorPool == null || floorPool.Count == 0) return;
        if (oldestFloorIndex >= floorPool.Count) return;

        float triggerDistance = floorPool[oldestFloorIndex].transform.position.x + floorWidth;

        if (playerTransform.position.x > triggerDistance)
        {
            RelocateOldestFloor();
        }
    }

    void RelocateOldestFloor()
    {
        GameObject floorToMove = floorPool[oldestFloorIndex];

        floorToMove.transform.position = new Vector3(nextSpawnX, -3f, 0f);

        RandomizeObstaclesInFloor(floorToMove);

        nextSpawnX += floorWidth;
        oldestFloorIndex = (oldestFloorIndex + 1) % totalPoolSize;
    }

    void RandomizeObstaclesInFloor(GameObject floor)
{
    // Primero desactivamos absolutamente todas las rocas de todos los SpawnPoints
    ClearAllObstacles(floor);

    int activeCount = 0;
    float currentChance = (playerTransform != null) ? GetCurrentObstacleChance(playerTransform.position.x) : obstacleSpawnChance;

    // Buscamos cada SpawnPoint dentro del tramo de suelo
    foreach (Transform spawnPoint in floor.transform)
    {
        // Solo procesamos los objetos que se llamen "SpawnPoint"
        if (!spawnPoint.name.StartsWith("SpawnPoint")) continue;

        // Comprobamos si no hemos superado el límite de rocas por tramo y si pasa la probabilidad
        if (activeCount < maxObstaclesPerFloor && Random.value < currentChance)
        {
            // Obtenemos los dos tipos de rocas dentro de este SpawnPoint
            Obstacle[] obstaclesInPoint = spawnPoint.GetComponentsInChildren<Obstacle>(true);

            if (obstaclesInPoint.Length > 0)
            {
                // Elegimos al azar SOLO UNA de las rocas disponibles en este SpawnPoint
                int randomIndex = Random.Range(0, obstaclesInPoint.Length);
                obstaclesInPoint[randomIndex].gameObject.SetActive(true);

                activeCount++;
            }
        }
    }
}

void ClearAllObstacles(GameObject floor)
{
    // Apaga todas las rocas del piso
    Obstacle[] allObstacles = floor.GetComponentsInChildren<Obstacle>(true);
    foreach (Obstacle obs in allObstacles)
    {
        obs.gameObject.SetActive(false);
    }
}

    float GetCurrentObstacleChance(float currentDistance)
    {
        float extraChance = (currentDistance / 100f) * 0.05f; 
        return Mathf.Min(baseObstacleChance + extraChance, maxObstacleChance);
    }
}