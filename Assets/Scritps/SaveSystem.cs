using UnityEngine;

// Clase estatica para administrar la persistencia de datos (no requiere estar en un GameObject)
public static class SaveSystem
{
    // Clave unica para guardar y cargar la puntuacion maxima en PlayerPrefs
    private const string HIGH_SCORE_KEY = "HighScore";

    // Guarda el nuevo record de forma permanente en el dispositivo
    public static void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
        PlayerPrefs.Save(); // Forzado de escritura inmediata en disco
    }

    // Devuelve el record guardado (retorna 0 si es la primera partida)
    public static int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    // Metodo utilitario para reiniciar el record durante pruebas
    public static void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
        PlayerPrefs.Save();
    }
}