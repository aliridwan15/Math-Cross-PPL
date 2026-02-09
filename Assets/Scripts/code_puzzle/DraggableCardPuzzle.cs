using UnityEngine;
using UnityEngine.EventSystems;

// Mengimplementasikan SEMUA interface yang dibutuhkan:
// IBeginDragHandler, IDragHandler, IEndDragHandler (untuk di-drag)
// IDropHandler (untuk menangani jika ada kartu lain yang di-drop di atasnya)
public class DraggableCardPuzzle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Tooltip("Nilai angka dari kartu ini")]
    public int cardValue;

    [HideInInspector]
    public Vector3 originalPosition; // Posisi awal untuk 'replay'
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [HideInInspector]
    public DropSlotPuzzle currentSlot = null; // Slot tempat kartu ini berada

    // Variabel untuk perbaikan Layer/Canvas
    private Transform originalParent; // Untuk menyimpan parent asli (bank kartu)
    private Canvas rootCanvas; // Untuk menyimpan Canvas utama

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = rectTransform.position;

        // Simpan parent asli (bank kartu)
        originalParent = transform.parent;
        // Cari Canvas utama
        rootCanvas = GetComponentInParent<Canvas>();

        // Pastikan DARI AWAL, kartu sudah block raycast
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // --- Perbaikan Layer/Canvas ---
        // 1. Pindahkan parent ke root canvas agar tidak terpotong oleh UI lain
        transform.SetParent(rootCanvas.transform, true);
        // 2. Pindahkan ke layer paling atas (agar dirender di atas semua UI)
        transform.SetAsLastSibling();

        // --- Perbaikan Raycast ---
        // Beri tahu GameManager untuk menonaktifkan raycast SEMUA kartu
        GameManagerPuzzle.instance.SetAllCardsRaycast(false);

        // --- Logika Skor (saat mengambil dari slot) ---
        // Cek apakah kartu ini diambil dari slot yang valid
        if (currentSlot != null)
        {
            // Beri tahu baris yang terhubung
            if (currentSlot.parentRows != null && currentSlot.parentRows.Count > 0)
            {
                foreach (EquationRowPuzzle row in currentSlot.parentRows)
                {
                    row.NotifyCardRemoved();
                }
            }
            // Kosongkan slot
            currentSlot.ClearSlot();
            currentSlot = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Mengikuti posisi mouse/jari
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // --- Perbaikan Raycast ---
        // Beri tahu GameManager untuk mengaktifkan kembali raycast SEMUA kartu
        GameManagerPuzzle.instance.SetAllCardsRaycast(true);

        // --- Perbaikan Layer/Canvas ---
        if (currentSlot == null)
        {
            // Drop GAGAL (tidak di atas slot), panggil Reset
            ResetToOriginalPosition();
        }
        else
        {
            // Drop BERHASIL (di atas slot), jadikan slot sebagai parent baru
            transform.SetParent(currentSlot.transform, true);
            // Posisikan kartu tepat di tengah slot
            rectTransform.position = currentSlot.transform.position;
        }
    }

    // Fungsi untuk mengembalikan kartu ke posisi awal (untuk Replay atau drop gagal)
    public void ResetToOriginalPosition()
    {
        // Kembalikan ke parent asli (bank)
        transform.SetParent(originalParent, true);
        // Kembalikan ke posisi asli
        rectTransform.position = originalPosition;

        // Jika kartu ini masih mengira ada di dalam slot,
        // paksa slot itu untuk mengosongkan dirinya (ClearSlot).
        if (currentSlot != null)
        {
            currentSlot.ClearSlot();
        }

        currentSlot = null; // Kartu ini sekarang tidak ada di slot manapun
    }

    // --- Perbaikan Stacking/Menimpa ---
    // Fungsi ini dipanggil jika ada kartu lain (Kartu B) yang di-drop
    // DI ATAS kartu ini (Kartu A)
    public void OnDrop(PointerEventData eventData)
    {
        // Jika kartu ini sedang berada di slot...
        if (currentSlot != null)
        {
            // ...maka "teruskan" event OnDrop ini ke SLOT di bawahnya.
            // Ini akan memicu logika "menimpa" di DropSlotPuzzle.cs
            currentSlot.OnDrop(eventData);
        }
    }

    // --- FUNGSI HELPER BARU ---
    // Dipanggil oleh GameManagerPuzzle untuk mengatur raycast
    public void SetRaycastBlock(bool state)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = state;
        }
    }
}