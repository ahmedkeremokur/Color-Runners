using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Renderer objRenderer; // Objeyi renklendirmek için Renderer
    public float transitionSpeed = 1f; // Renk deðiþtirme geçiþ hýzý

    private Color currentColor; // Mevcut renk
    private Color targetColor; // Geçilecek yeni renk

    void Start()
    {
        objRenderer = GetComponent<Renderer>();

        // Baþlangýç ve hedef renkleri ayarla
        currentColor = objRenderer.material.color;
        targetColor = GetRandomColor();
    }

    void Update()
    {
        // Mevcut renk ile hedef renk arasýnda geçiþ yap
        currentColor = Color.Lerp(
            currentColor,
            targetColor,
            transitionSpeed * Time.deltaTime
        );

        objRenderer.material.color = currentColor;

        // Eðer hedef renge yaklaþtýysak yeni bir hedef renk belirle
        if (Vector4.Distance(currentColor, targetColor) < 0.1f)
        {
            targetColor = GetRandomColor();
        }
    }

    // Rastgele bir renk üret
    private Color GetRandomColor()
    {
        return new Color(
            Random.Range(0f, 1f), // R, G, B için rastgele deðerler
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
    }
}
