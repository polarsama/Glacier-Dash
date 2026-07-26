
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public GameObject groundPrefab;

    [Header("Generator Settings")]
    public float groundWidth = 20f;
    public int totalPoolSize = 3; // Solo existiran 3 suelos en TODO el juego

    private List<GameObject> groundPool = new List<GameObject>();
    private int oldestGroundIndex = 0;
    private float nextSpawnX = 0f;

    void Start()
    {
        // Creamos únicamente la cantidad exacta de suelos que necesitamos para llenar la pantalla
        for (int i = 0; i < totalPoolSize; i++)
        {
            Vector3 spawnPosition = new Vector3(nextSpawnX, -3f, 0f);
            GameObject newGround = Instantiate(groundPrefab, spawnPosition, Quaternion.identity);
            groundPool.Add(newGround);

            nextSpawnX += groundWidth;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Cuando el jugador sobrepasa el punto medio del suelo mas antiguo, movemos ese suelo al frente
        float triggerDistance = groundPool[oldestGroundIndex].transform.position.x + groundWidth;

        if (playerTransform.position.x > triggerDistance)
        {
            RelocateOldestGround();
        }
    }

    void RelocateOldestGround()
    {
        // En lugar de Destroy e Instantiate, solo cambiamos la posicion X
        groundPool[oldestGroundIndex].transform.position = new Vector3(nextSpawnX, -3f, 0f);

        nextSpawnX += groundWidth;

        // Avanzamos el indice circularmente (0 -> 1 -> 2 -> 0)
        oldestGroundIndex = (oldestGroundIndex + 1) % totalPoolSize;
    }
}