using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement; 

public class TutorialManager : MonoBehaviour {
    
    [Header("UI Elements")]
    public GameObject handPointer;
    public GameObject feedbackPanelBenar;
    public GameObject feedbackPanelSalah;
    public Button submitButton;
    public GameObject replayPanel;
    public Animator handPointerAnimator;

    [Header("Game Logic References")]
    public DropSlot slotVertikal;
    public DropSlot slotHorizontal;
    public Draggable jawaban10;
    public Draggable jawaban2;

    [Header("Hint System")]
    public Button hintButton;
    public GameObject hintPanel;
    public GameObject hintImageSoal1;
    public GameObject hintImageSoal2;

    // Variabel privat untuk mengelola state tutorial
    private DropSlot pendingSlot;
    private int pendingValue;
    private Draggable pendingObject;
    private int tutorialStep = 0;

    void Start() {
        handPointer.SetActive(false);
        feedbackPanelBenar.SetActive(false);
        feedbackPanelSalah.SetActive(false);
        submitButton.gameObject.SetActive(false);
        replayPanel.SetActive(false);
        hintPanel.SetActive(false);
        
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial() {
        Debug.Log("Memulai panduan drag...");
        yield return StartCoroutine(ShowDragGuide(jawaban10.transform, slotVertikal.transform));
        Debug.Log("Panduan drag selesai, menunggu input pemain.");
    }

    IEnumerator ShowDragGuide(Transform startObject, Transform endObject) {
        handPointer.SetActive(true);
        handPointer.transform.position = startObject.position;
        yield return new WaitForSeconds(0.5f); 
        
        handPointerAnimator.SetTrigger("DoClick");
        yield return new WaitForSeconds(0.5f); 
        
        float duration = 1.5f; 
        float elapsedTime = 0f;
        Vector3 startingPos = startObject.position;
        Vector3 endingPos = endObject.position; 
        
        while (elapsedTime < duration) {
            handPointer.transform.position = Vector3.Lerp(startingPos, endingPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime; 
            yield return null; 
        }

        handPointer.transform.position = endingPos;
        yield return new WaitForSeconds(0.5f);
        handPointer.SetActive(false);
    }
    
    // --- FUNGSI BARU: Panduan untuk menekan tombol Hint ---
    IEnumerator ShowHintGuide() {
        Debug.Log("Memulai panduan hint...");
        
        // 1. Gerakkan pointer ke tombol Hint
        handPointer.SetActive(true);
        float duration = 1.0f;
        float elapsedTime = 0f;
        Vector3 startingPos = handPointer.transform.position; // Mulai dari posisi terakhir
        if (!handPointer.activeSelf) startingPos = new Vector3(-100, -100, 0); // Atau dari luar layar
        Vector3 endingPos = hintButton.transform.position;

        while (elapsedTime < duration) {
            handPointer.transform.position = Vector3.Lerp(startingPos, endingPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        handPointer.transform.position = endingPos;

        // 2. Mainkan animasi klik
        handPointerAnimator.SetTrigger("DoClick");
        yield return new WaitForSeconds(0.5f);

        // 3. Panggil fungsi ShowHint() seolah-olah tombolnya ditekan
        ShowHint();
        
        // 4. Beri waktu pemain untuk membaca hint
        yield return new WaitForSeconds(2.0f);

        // 5. Tutup panel hint
        CloseHintPanel();
        handPointer.SetActive(false); // Sembunyikan pointer setelahnya
        Debug.Log("Panduan hint selesai.");
    }

    public void ReceivePendingAnswer(DropSlot slot, int value, Draggable obj) {
        pendingSlot = slot;
        pendingValue = value;
        pendingObject = obj;
        submitButton.gameObject.SetActive(true);
    }

    public void SubmitAnswer() {
        submitButton.gameObject.SetActive(false);
        if (pendingSlot != null) ProcessAnswer(pendingSlot, pendingValue, pendingObject);
        pendingSlot = null;
        pendingValue = 0;
        pendingObject = null;
    }

    private void ProcessAnswer(DropSlot slot, int value, Draggable obj) {
        if (tutorialStep == 0) {
            if (slot == slotVertikal && value == 10) StartCoroutine(ShowWrongAnswerFeedback(obj, slot));
            else StartCoroutine(ShowWrongAnswerFeedback(obj, slot));
        } else if (tutorialStep == 1) {
            if (slot == slotVertikal && value == 2) {
                slotVertikal.GetComponent<Image>().color = Color.green;
                tutorialStep = 2;
                StartCoroutine(ShowDragGuide(jawaban10.transform, slotHorizontal.transform));
            } else StartCoroutine(ShowWrongAnswerFeedback(obj, slot));
        } else if (tutorialStep == 2) {
            if (slot == slotHorizontal && value == 10) {
                slotHorizontal.GetComponent<Image>().color = Color.green;
                tutorialStep = 3;
                StartCoroutine(ShowCorrectAnswerFeedback());
            } else StartCoroutine(ShowWrongAnswerFeedback(obj, slot));
        }
    }

    // --- FUNGSI INI DIMODIFIKASI ---
    IEnumerator ShowWrongAnswerFeedback(Draggable wrongObject, DropSlot wrongSlot) {
        feedbackPanelSalah.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        feedbackPanelSalah.SetActive(false);

        if (wrongObject != null) {
            wrongObject.gameObject.SetActive(true);
            wrongObject.ResetPosition();
        }
        if (wrongSlot != null) wrongSlot.ClearSlot();

        if (tutorialStep == 0) { // Hanya tunjukkan panduan hint saat salah pertama kali
             tutorialStep = 1;
             // Panggil panduan hint SEBELUM menunjukkan jawaban benar
             yield return StartCoroutine(ShowHintGuide());
             // Setelah hint selesai, baru tunjukkan jawaban benar
             StartCoroutine(ShowDragGuide(jawaban2.transform, slotVertikal.transform));
        } else if (tutorialStep == 1) { // Jika salah lagi di tahap 1
            tutorialStep = 1;
            StartCoroutine(ShowDragGuide(jawaban2.transform, slotVertikal.transform));
        } else if (tutorialStep == 2) { // Jika salah di tahap 2
             StartCoroutine(ShowDragGuide(jawaban10.transform, slotHorizontal.transform));
        }
    }
    
    IEnumerator ShowCorrectAnswerFeedback() {
        feedbackPanelBenar.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        feedbackPanelBenar.SetActive(false);
        replayPanel.SetActive(true);
    }
    
    public void ReplayTutorial() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseReplayPanel() {
        replayPanel.SetActive(false);
    }

    public void ShowHint() {
        hintImageSoal1.SetActive(false);
        hintImageSoal2.SetActive(false);
        if (tutorialStep <= 1) hintImageSoal1.SetActive(true);
        else if (tutorialStep == 2) hintImageSoal2.SetActive(true);
        hintPanel.SetActive(true);
    }

    public void CloseHintPanel() {
        hintPanel.SetActive(false);
    }

    // --- FUNGSI BARU UNTUK TOMBOL HOME ---
    public void GoToHome()
    {
        // Ganti "HomeScene" dengan nama scene menu utama Anda
        SceneManager.LoadScene("Home");
    }
}

