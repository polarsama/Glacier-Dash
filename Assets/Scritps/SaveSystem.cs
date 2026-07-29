using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class SaveSystem
{
    private const string HIGH_SCORE_KEY = "HighScore";

    public static void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
        PlayerPrefs.Save();
    }

    public static int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    // Al agregar [MenuItem], crea una pestaña en el menu superior de Unity
#if UNITY_EDITOR
    [MenuItem("Glacier Dash/Borrar Record (Reset High Score)")]
    public static void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
        PlayerPrefs.Save();
        Debug.Log("¡El High Score ha sido reiniciado a 0 con exito!");
    }
#endif
}