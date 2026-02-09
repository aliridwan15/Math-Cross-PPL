using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EquationRowPuzzle : MonoBehaviour
{
    [Tooltip("DAFTAR slot jawaban milik baris ini")]
    public List<DropSlotPuzzle> answerSlots;
    
    [Tooltip("DAFTAR nilai angka yang benar (HARUS URUT SESUAI DAFTAR SLOT)")]
    public List<int> correctAnswers;

    [Tooltip("Audio source untuk memainkan sound effect")]
    public AudioSource audioSource;
    public AudioClip correctSound;
     public AudioClip wrongSound;

    [Header("Animasi Elemen")]
    [Tooltip("Elemen yang HANYA ada di baris ini (TIDAK BERPOTONGAN)")]
    public List<Transform> rowElements;

    [Tooltip("Elemen yang BERPOTONGAN / dibagi dengan baris lain")]
    public List<Transform> sharedElements;

    private bool isSolved = false;

    // Simpan posisi/skala asli untuk animasi
    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();

    void Start()
    {
        CacheOriginalTransforms(rowElements);
        CacheOriginalTransforms(sharedElements);
    }

    void CacheOriginalTransforms(List<Transform> elements)
    {
        if (elements == null) return;
        foreach (Transform element in elements)
        {
            if (element != null && !originalPositions.ContainsKey(element))
            {
                originalPositions[element] = element.position;
                originalScales[element] = element.localScale;
            }
        }
    }

    // Dipanggil oleh DropSlotPuzzle setelah kartu dijatuhkan
    public void CheckAnswer()
    {
        // 1. Cek apakah SEMUA slot di baris ini sudah terisi
        foreach (DropSlotPuzzle slot in answerSlots)
        {
            if (slot == null || !slot.IsOccupied())
            {
                return;
            }
        }

        // 2. Jika semua slot terisi, cek jawabannya
        bool allCorrect = true;
        for (int i = 0; i < answerSlots.Count; i++)
        {
            if (answerSlots[i].GetCardValue() != correctAnswers[i])
            {
                allCorrect = false;
                break;
            }
        }

        // 3. Gabungkan SEMUA elemen untuk dianimasikan
        List<Transform> allElementsToAnimate = new List<Transform>(rowElements);
        if (sharedElements != null && sharedElements.Count > 0)
        {
            allElementsToAnimate.AddRange(sharedElements);
        }

        // 4. Berikan feedback
        if (allCorrect)
        {
            if (!isSolved)
            {
                isSolved = true;
                GameManagerPuzzle.instance.AddScore();
            }
            audioSource.PlayOneShot(correctSound);
            StartCoroutine(WobbleAnimation(allElementsToAnimate));
        }
        else
        {
            audioSource.PlayOneShot(wrongSound);
            StartCoroutine(ShakeAnimation(allElementsToAnimate));
        }
    }

    IEnumerator WobbleAnimation(List<Transform> elements)
    {
        for (float t = 0; t < 1f; t += Time.deltaTime * 2f)
        {
            float scaleAmount = 1 + Mathf.Sin(t * Mathf.PI) * 0.1f;
            foreach (Transform element in elements)
            {
                if (element != null && originalScales.ContainsKey(element))
                {
                    element.localScale = originalScales[element] * scaleAmount;
                }
            }
            yield return null;
        }

        foreach (Transform element in elements)
        {
            if (element != null && originalScales.ContainsKey(element))
            {
                element.localScale = originalScales[element];
            }
        }
    }

    IEnumerator ShakeAnimation(List<Transform> elements)
    {
        float duration = 0.5f;
        float magnitude = 3f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float xOffset = Mathf.Sin(t * 50f) * magnitude;
            foreach (Transform element in elements)
            {
                if (element != null && originalPositions.ContainsKey(element))
                {
                    element.position = new Vector3(
                        originalPositions[element].x + xOffset,
                        originalPositions[element].y,
                        originalPositions[element].z
                    );
                }
            }
            yield return null;
        }

        foreach (Transform element in elements)
        {
            if (element != null && originalPositions.ContainsKey(element))
            {
                element.position = originalPositions[element];
            }
        }
    }
    
    // Dipanggil oleh DraggableCardPuzzle saat kartu diambil dari slot
    public void NotifyCardRemoved()
    {
        if (isSolved)
        {
            isSolved = false;
            GameManagerPuzzle.instance.SubtractScore();
        }
    }
    
    public void ResetState()
    {
        isSolved = false;
    }
    
    public bool GetIsSolved()
    {
        return isSolved;
    }
}
    