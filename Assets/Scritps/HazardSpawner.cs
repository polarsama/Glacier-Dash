using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject fallingIcePrefab; // Prefab de la estalactita
    public Transform playerTransform;   // Referencia al jugador

    [Header("Spawn Settings")]
    public float minSpawnInterval = 3f;  // Intervalo inicial de caida (segundos)
    public float maxSpawnInterval = 6f;
    public float spawnHeight = 8f;       // Altura desde la que caen
    public float leadDistance = 5f;      // Que tan adelante del jugador caera el proyectil

    private float timer;
    private float nextSpawnTime;

    void Start()
    {
        if (playerTransform == null)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null) playerTransform = player.transform;
        }

        SetNextSpawnTime();
    }

    void Update()
    {
        if (playerTransform == null) return;

        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnHazard();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    void SpawnHazard()
    {
        // Calcula la posicion en X donde impactara (un poco por delante del jugador)
        float spawnX = playerTransform.position.x + leadDistance;
        Vector3 spawnPosition = new Vector3(spawnX, playerTransform.position.y + spawnHeight, 0f);

        // Instancia o activa el peligro
        Instantiate(fallingIcePrefab, spawnPosition, fallingIcePrefab.transform.rotation);
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}