using UnityEngine;

public class ButtonIdlePulse : MonoBehaviour
{
    [Header("Efek Denyut Halus (Pulse)")]
    [Tooltip("Kecepatan denyut (semakin besar semakin cepat)")]
    public float pulseSpeed = 1.8f;

    [Tooltip("Seberapa besar perubahan skala (0.01 = sangat halus)")]
    public float pulseAmount = 1.1f;

    private Vector3 originalScale;

    void Start()
    {
        // Simpan ukuran awal tombol
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Hitung perubahan skala berdasarkan waktu
        float scaleChange = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        // Terapkan perubahan skala lembut ke tombol
        transform.localScale = originalScale * scaleChange;
    }
}
