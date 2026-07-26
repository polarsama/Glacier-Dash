using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public GameObject groundPrefab;

    [Header("Generator Settings")]
    public float groundWidth = 20f;       // Ancho exacto del prefab en X
    public float spawnDistance = 30f;     // Distancia a la que se genera el nuevo suelo antes de llegar
    public int initialGroundCount = 3;    // Tramos iniciales al arrancar

    private float nextSpawnX = 0f;
    private List<GameObject> activeGrounds = new List<GameObject>();

    void Start()
    {
        // Generar los primeros tramos de suelo al iniciar el juego
        for (int i = 0; i < initialGroundCount; i++)
        {
            SpawnGround();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Si el jugador se aproxima al final del ultimo suelo generado, creamos otro
        if (playerTransform.position.x + spawnDistance > nextSpawnX)
        {
            SpawnGround();
            DestroyOldGround();
        }
    }

    void SpawnGround()
    {
        // Instancia el prefab en la siguiente posicion X correspondiente
        Vector3 spawnPosition = new Vector3(nextSpawnX, -3f, 0f);
        GameObject newGround = Instantiate(groundPrefab, spawnPosition, Quaternion.identity);
        
        activeGrounds.Add(newGround);

        // Avanza el punto de origen para el siguiente tramo
        nextSpawnX += groundWidth;
    }

    void DestroyOldGround()
    {
        // Si hay mas de 3 tramos en pantalla, elimina el mas antiguo que quedo atras
        if (activeGrounds.Count > 3)
        {
            Destroy(activeGrounds[0]);
            activeGrounds.RemoveAt(0);
        }
    }
}