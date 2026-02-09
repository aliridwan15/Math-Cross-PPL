using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class GameManagerPuzzle : MonoBehaviour
{
    // ==========================
    // 🔷 SINGLETON
    // ==========================
    public static GameManagerPuzzle instance;

    [Header("Referensi UI")]
    public GameObject replayPopup;
    public GameObject submitPopup;
    public GameObject hintHabisPopup;
    public TextMeshProUGUI hintText;
    public Button hintButton;
    public TextMeshProUGUI scoreText;

    [Header("Data Game")]
    public int maxScore = 100;
    public string scoreSceneName = "SkorAkhir";
    public string nextLevelSceneName;
    public string previousSceneName; 

    [Header("Referensi Puzzle")]
    public List<EquationRowPuzzle> allPuzzleRows;
    public List<DraggableCardPuzzle> allAnswerCards; // <-- DAFTAR INI YANG KITA PAKAI

    private int hintCount = 3;
    private int totalRows;
    private int correctRows = 0;
    private int currentScore = 0;

    // ==========================
    // 🔶 LIFECYCLE
    // ==========================
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        replayPopup?.SetActive(false);
        submitPopup?.SetActive(false);
        hintHabisPopup?.SetActive(false);

        totalRows = allPuzzleRows?.Count ?? 0;

        UpdateHintUI();
        UpdateScoreUI();
    }

    // ======================================
    // 🔶 FUNGSI BARU UNTUK KARTU (PERMINTAAN ANDA)
    // ======================================

    /// <summary>
    /// Mengatur status blocksRaycasts untuk SEMUA kartu jawaban.
    /// Dipanggil oleh DraggableCardPuzzle saat drag mulai/selesai.
    /// </summary>
    /// <param name="state">True = BISA diklik, False = TIDAK BISA diklik</param>
    public void SetAllCardsRaycast(bool state)
    {
        if (allAnswerCards == null) return;

        foreach (DraggableCardPuzzle card in allAnswerCards)
        {
            if (card != null)
            {
                card.SetRaycastBlock(state);
            }
        }
    }

    // ==========================
    // 🔶 FUNGSI REPLAY
    // ==========================
    public void OnReplayButtonPressed() => replayPopup?.SetActive(true);

    public void OnReplayConfirm()
    {
        foreach (DraggableCardPuzzle card in allAnswerCards)
            card.ResetToOriginalPosition();

        foreach (EquationRowPuzzle row in allPuzzleRows)
            row.ResetState();

        correctRows = 0;
        currentScore = 0;
        hintCount = 3;

        UpdateScoreUI();
        UpdateHintUI();

        replayPopup?.SetActive(false);
    }

    public void OnReplayCancel() => replayPopup?.SetActive(false);

    // ==========================
    // 🔶 FUNGSI HINT
    // ==========================
    public void OnHintButtonPressed()
    {
        if (hintCount <= 0)
        {
            hintHabisPopup?.SetActive(true);
            return;
        }

        EquationRowPuzzle targetRow = allPuzzleRows
            .FirstOrDefault(row => !row.GetIsSolved() && row.answerSlots.Any(slot => !slot.IsOccupied()));

        if (targetRow == null) return;

        DropSlotPuzzle targetSlot = targetRow.answerSlots.FirstOrDefault(slot => !slot.IsOccupied());
        if (targetSlot == null) return;

        int targetSlotIndex = targetRow.answerSlots.IndexOf(targetSlot);
        int correctAnswerForSlot = targetRow.correctAnswers[targetSlotIndex];

        DraggableCardPuzzle correctCard = allAnswerCards
            .FirstOrDefault(card => card.cardValue == correctAnswerForSlot && card.currentSlot == null);

        if (correctCard != null)
        {
            PlaceCardWithHint(targetSlot, correctCard);
            hintCount--;
            UpdateHintUI();
        }
    }

    private void PlaceCardWithHint(DropSlotPuzzle slot, DraggableCardPuzzle card)
    {
        if (slot == null || card == null) return;

        card.GetComponent<RectTransform>().position = slot.GetComponent<RectTransform>().position;
        card.currentSlot = slot;
        slot.SetOccupiedCard(card);

        if (slot.parentRows != null && slot.parentRows.Count > 0)
        {
            foreach (EquationRowPuzzle row in slot.parentRows)
                row.CheckAnswer();
        }
    }

    private void UpdateHintUI()
    {
        hintText.text = hintCount switch
        {
            3 => "III",
            2 => "II",
            1 => "I",
            _ => ""
        };
    }

    // ==========================
    // 🔶 SKOR
    // ==========================
    public void AddScore()
    {
        correctRows++;
        UpdateCurrentScore();
    }

    public void SubtractScore()
    {
        correctRows = Mathf.Max(0, correctRows - 1);
        UpdateCurrentScore();
    }

    private void UpdateCurrentScore()
    {
        if (totalRows > 0)
        {
            float scorePerItem = (float)maxScore / totalRows;
            currentScore = Mathf.RoundToInt(scorePerItem * correctRows);
        }
        else
        {
            currentScore = 0;
        }

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();
    }

    // ==========================
    // 🔶 FUNGSI SUBMIT
    // ==========================
    public void OnSubmitButtonPressed() => submitPopup?.SetActive(true);

    public void OnSubmitConfirm()
    {
        PlayerPrefs.SetInt("FinalScore", currentScore);
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt($"{currentSceneName}_Score", currentScore);
        PlayerPrefs.SetString("LastPuzzleScene", currentSceneName);
        PlayerPrefs.SetString("NextPuzzleScene", nextLevelSceneName);
        PlayerPrefs.Save();
        SceneManager.LoadScene(scoreSceneName);
        Debug.Log($"💾 Menyimpan skor {currentScore} untuk {currentSceneName}_Score");
    }

    public void OnSubmitCancel() => submitPopup?.SetActive(false);

    // ==========================
    // 🔶 POPUP HINT HABIS
    // ==========================
    public void OnHintHabisPopupClose() => hintHabisPopup?.SetActive(false);

    // ==========================
    // 🔶 NAVIGASI BACK
    // ==========================
    public void OnBackButtonPressed()
    {
        if (!string.IsNullOrEmpty(previousSceneName))
        {
            SceneManager.LoadScene(previousSceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene sebelumnya (previousSceneName) belum di-set di Inspector!");
        }
    }
}