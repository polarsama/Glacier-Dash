using System.Collections;
using UnityEngine;
using TMPro; // Usamos TextMeshPro

public class CanaModeUI : MonoBehaviour
{
    public TextMeshProUGUI canaText; // Arrastra tu componente TextMeshPro aquí
    public float displayDuration = 1.5f;

    private Coroutine activeRoutine;

    private void Awake()
    {
        if (canaText != null)
        {
            canaText.gameObject.SetActive(false);
        }
    }

    public void ShowCanaText()
    {
        if (canaText == null) return;

        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        activeRoutine = StartCoroutine(ShowTextRoutine());
    }

    private IEnumerator ShowTextRoutine()
    {
        canaText.gameObject.SetActive(true);
        canaText.text = "¡MODO CAÑA!";
        
        // Efecto de pulso / resplandor ligero de tamaño
        canaText.transform.localScale = Vector3.one * 1.5f;
        float elapsed = 0f;

        while (elapsed < 0.2f)
        {
            elapsed += Time.deltaTime;
            canaText.transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, elapsed / 0.2f);
            yield return null;
        }

        yield return new WaitForSeconds(displayDuration);

        canaText.gameObject.SetActive(false);
    }
}