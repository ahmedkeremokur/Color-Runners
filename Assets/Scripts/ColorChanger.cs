using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Renderer objRenderer; // Objeyi renklendirmek i�in Renderer
    public float transitionSpeed = 1f; // Renk de�i�tirme ge�i� h�z�

    private Color currentColor; // Mevcut renk
    private Color targetColor; // Ge�ilecek yeni renk

    void Start()
    {
        objRenderer = GetComponent<Renderer>();

        // Ba�lang�� ve hedef renkleri ayarla
        currentColor = objRenderer.material.color;
        targetColor = GetRandomColor();
    }

    void Update()
    {
        // Mevcut renk ile hedef renk aras�nda ge�i� yap
        currentColor = Color.Lerp(
            currentColor,
            targetColor,
            transitionSpeed * Time.deltaTime
        );

        objRenderer.material.color = currentColor;

        // E�er hedef renge yakla�t�ysak yeni bir hedef renk belirle
        if (Vector4.Distance(currentColor, targetColor) < 0.1f)
        {
            targetColor = GetRandomColor();
        }
    }

    // Rastgele bir renk �ret
    private Color GetRandomColor()
    {
        return new Color(
            Random.Range(0f, 1f), // R, G, B i�in rastgele de�erler
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
    }
}
