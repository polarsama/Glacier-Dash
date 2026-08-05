using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource; // Canal exclusivo para música de fondo

    [Header("SFX Clips")]
    public AudioClip penaltySFX;     // Sonido al chocar con espinas / penalización
    public AudioClip rockBreakSFX;   // Sonido al romper una roca normal con Dash
    public AudioClip canaActivateSFX;// Sonido al activar el Modo Caña

    [Header("Music Clips")]
    public AudioClip backgroundMusic;// Tu pista de música de fondo

    private void Awake()
    {
        // Patrón Singleton para mantener el AudioManager vivo entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        // Se suscribe al evento de cambio de escena para reiniciar la música automáticamente al recargar
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Desuscribe el evento para evitar fugas de memoria
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    // Se ejecuta automáticamente cada vez que se carga o reinicia una escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBackgroundMusic();
    }

    /// <summary>
    /// Reproduce un efecto de sonido puntual.
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    /// <summary>
    /// Inicia la música de fondo en bucle.
    /// </summary>
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null || musicSource == null) return;

        // Si la música ya está sonando el mismo clip, no la reinicia bruscamente; 
        // pero si se detuvo (al morir), la vuelve a reproducir desde el inicio.
        if (musicSource.clip != backgroundMusic || !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Detiene la música de fondo (Ideal para cuando el jugador muere/pierde).
    /// </summary>
   public void StopBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop(); // Detiene la reproducción
            musicSource.clip = null; // Remueve la pista para evitar que vuelva a sonar por loop o disparo automático
        }
    }
}   