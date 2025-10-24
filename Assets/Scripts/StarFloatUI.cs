using UnityEngine;

public class StarFloatUI : MonoBehaviour
{
    [Header("Gerakan Naik Turun")]
    public float floatSpeed = 1f;        
    public float floatHeight = 30f;      

    [Header("Rotasi Bintang")]
    public float rotationSpeed = 40f;    

    [Header("Efek Berdenyut")]
    public float pulseSpeed = 2f;        
    public float pulseScale = 0.1f;      

    private RectTransform rectTransform;
    private Vector3 startPos;
    private Vector3 originalScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;
    }

    void Update()
    {
        // Gerak naik-turun
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        rectTransform.anchoredPosition = new Vector2(startPos.x, newY);

        // Rotasi pelan
        rectTransform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Efek berdenyut (pulse)
        float scaleChange = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        rectTransform.localScale = originalScale * scaleChange;
    }
}
