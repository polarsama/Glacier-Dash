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
    public float obstacleSpawnChance = 0.5f; // 50% de probabilidad de que aparezca una roca en un punto
    public int maxObstaclesPerFloor = 2;     // Para no saturar el tramo y dar tiempo a reaccionar

    private List<GameObject> floorPool = new List<GameObject>();
    private int oldestFloorIndex = 0;
    private float nextSpawnX = 0f;

    void Start()
    {
        for (int i = 0; i < totalPoolSize; i++)
        {
            Vector3 spawnPosition = new Vector3(nextSpawnX, -3f, 0f);
            GameObject newFloor = Instantiate(floorPrefab, spawnPosition, Quaternion.identity);
            
            // Si es el primer suelo (donde aparece el oso), lo dejamos sin rocas para no chocar de entrada
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

        // VALIDACIÓN DE SEGURIDAD: Evita el error si la lista no esta lista o esta vacia
        if (floorPool == null || floorPool.Count == 0) return;

        // Aseguramos que el indice no se salga de los limites de la lista
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

        // 1. Teletransportamos el tramo de suelo a la nueva posicion X
        floorToMove.transform.position = new Vector3(nextSpawnX, -3f, 0f);

        // 2. Aleatorizamos cuales rocas se activan en esta nueva vuelta
        RandomizeObstaclesInFloor(floorToMove);

        // 3. Avanzamos X e indice circular
        nextSpawnX += floorWidth;
        oldestFloorIndex = (oldestFloorIndex + 1) % totalPoolSize;
    }

    void RandomizeObstaclesInFloor(GameObject floor)
    {
        // Obtiene todos los obstaculos del tramo
        Obstacle[] obstacles = floor.GetComponentsInChildren<Obstacle>(true);

        int activeCount = 0;

        foreach (Obstacle obs in obstacles)
        {
            // Apagamos todas de entrada
            obs.gameObject.SetActive(false);

            // Verificamos probabilidad y que no superemos el limite por tramo
            if (activeCount < maxObstaclesPerFloor && Random.value < obstacleSpawnChance)
            {
                obs.gameObject.SetActive(true);
                activeCount++;
            }
        }
    }

    void ClearAllObstacles(GameObject floor)
    {
        Obstacle[] obstacles = floor.GetComponentsInChildren<Obstacle>(true);
        foreach (Obstacle obs in obstacles)
        {
            obs.gameObject.SetActive(false);
        }
    }
}