// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;


// using TMPro;
// public class DropSlot : MonoBehaviour, IDropHandler {
//     public int correctValue; // Atur di Inspector (misal: 2 untuk soal vertikal)
//     public TutorialManager tutorialManager; // Hubungkan ke TutorialManager

//     public void OnDrop(PointerEventData eventData) {
//     if (eventData.pointerDrag != null) {
//         GameObject droppedObject = eventData.pointerDrag;

//         // 1. DEKLARASIKAN VARIABEL DI SINI (DI AWAL)
//         Draggable draggableComp = droppedObject.GetComponent<Draggable>();
//         int droppedValue = int.Parse(droppedObject.GetComponentInChildren<TextMeshProUGUI>().text);

//         // 2. ATUR POSISI
//         droppedObject.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

//         // 3. BARU GUNAKAN VARIABELNYA DI SINI (DI AKHIR)
//         tutorialManager.ReceivePendingAnswer(this, droppedValue, draggableComp);
//     }
// }
// }

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro; // Pastikan ini ada

public class DropSlot : MonoBehaviour, IDropHandler {
    
    public int correctValue;
    public TutorialManager tutorialManager;
    private TextMeshProUGUI myText; // Variabel baru untuk teks di kotak ini

    void Awake() {
        // Ambil komponen teks di dalam objek ini
        myText = GetComponentInChildren<TextMeshProUGUI>();
        
        // Pastikan teksnya kosong di awal
        if (myText != null) {
            myText.text = "";
        }
    }

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            GameObject droppedObject = eventData.pointerDrag;
            Draggable draggableComp = droppedObject.GetComponent<Draggable>();
            
            // Ambil teks dari objek jawaban
            string droppedText = droppedObject.GetComponentInChildren<TextMeshProUGUI>().text;
            int droppedValue = int.Parse(droppedText);

            // --- PERUBAHAN UTAMA DI SINI ---
            
            // 1. Tampilkan teks jawaban di kotak ini
            if (myText != null) {
                myText.text = droppedText;
            }

            // 2. Sembunyikan objek jawaban yang di-drag
            droppedObject.SetActive(false); 
            
            // 3. Laporkan ke manajer (tanpa mengubah posisi)
            tutorialManager.ReceivePendingAnswer(this, droppedValue, draggableComp);
        }
    }

    // FUNGSI BARU: Untuk mengosongkan slot jika jawaban salah
    public void ClearSlot() {
        if (myText != null) {
            myText.text = "";
        }
    }
}