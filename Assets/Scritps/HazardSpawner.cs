using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject fallingIcePrefab; // Prefab de la estalactita
    public Transform playerTransform;   // Referencia al jugador

    [Header("Base Spawn Settings")]
    public float baseMinInterval = 3.5f; // Intervalo de generación inicial
    public float baseMaxInterval = 6.0f;
    public float spawnHeight = 8f;       // Altura desde la que caen
    public float leadDistance = 5f;      // Distancia por delante del oso

    [Header("Difficulty Scaling Limits")]
    public float minAllowedInterval = 1.2f; // Límite de seguridad: nunca saldrán con menos tiempo que este
    public float baseFallSpeed = 12f;       // Velocidad de caída inicial
    public float maxFallSpeed = 22f;        // Velocidad de caída máxima
    public float doubleIceThreshold = 1000f;// Puntos necesarios para que puedan caer 2 picos

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
            SpawnSequence();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    void SpawnSequence()
    {
        // 1. Obtener la puntuación/distancia actual para calcular el nivel de dificultad
        PlayerController player = playerTransform.GetComponent<PlayerController>();
        float score = (player != null) ? player.GetCurrentScore() : playerTransform.position.x * 10f;

        // 2. Spawnear el primer pico de hielo
        SpawnSingleIce(leadDistance, score);

        // 3. Si se supera el umbral de puntos, hay un 25% de probabilidad de lanzar un segundo pico
        if (score >= doubleIceThreshold && Random.value <= 0.25f)
        {
            // El segundo pico cae un poco más adelante o atrás (+3 o +7) para no quedar encimados
            float secondLead = leadDistance + Random.Range(2.5f, 4.5f);
            SpawnSingleIce(secondLead, score);
        }
    }

    void SpawnSingleIce(float offsetX, float currentScore)
    {
        float spawnX = playerTransform.position.x + offsetX;
        Vector3 spawnPosition = new Vector3(spawnX, playerTransform.position.y + spawnHeight, 0f);

        GameObject ice = Instantiate(fallingIcePrefab, spawnPosition, fallingIcePrefab.transform.rotation);

        // Ajustar la velocidad de caída de este pico según la puntuación acumulada
        FallingObstacle obstacleScript = ice.GetComponent<FallingObstacle>();
        if (obstacleScript != null)
        {
            // Aumenta progresivamente la velocidad de caída (1 unidad por cada 200 puntos)
            float scaledSpeed = baseFallSpeed + (currentScore / 200f);
            obstacleScript.fallSpeed = Mathf.Min(scaledSpeed, maxFallSpeed);
        }
    }

    void SetNextSpawnTime()
    {
        // Calcular qué tan lejos ha llegado el jugador
        PlayerController player = playerTransform.GetComponent<PlayerController>();
        float score = (player != null) ? player.GetCurrentScore() : playerTransform.position.x * 10f;

        // Reducir los intervalos de tiempo progresivamente según la puntuación
        float speedUpFactor = (currentScoreScaling(score));
        float currentMin = Mathf.Max(baseMinInterval - speedUpFactor, minAllowedInterval);
        float currentMax = Mathf.Max(baseMaxInterval - speedUpFactor, minAllowedInterval + 1f);

        nextSpawnTime = Random.Range(currentMin, currentMax);
    }

    float currentScoreScaling(float score)
    {
        // Resta 0.3 segundos al tiempo entre spawns por cada 300 puntos de partida
        return (score / 300f) * 0.3f;
    }
}