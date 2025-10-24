using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        // Pastikan CanvasGroup sudah ada atau ditambahkan
        if (!TryGetComponent<CanvasGroup>(out canvasGroup)) {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        startPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / transform.parent.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData) {
        // Selalu kembalikan properti ini di akhir, tidak peduli apa pun
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter == null || !eventData.pointerEnter.CompareTag("DropSlot")) {
             rectTransform.anchoredPosition = startPosition;
        }
    }

    // FUNGSI YANG DIPERBAIKI
    public void ResetPosition() {
        // Kembalikan posisi ke awal
        rectTransform.anchoredPosition = startPosition;

        // "Hidupkan" kembali objeknya agar bisa di-drag lagi
        if (canvasGroup != null) {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
