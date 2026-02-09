
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ChapterComplete : MonoBehaviour
{
    [Header("Data Chapter")]
    public string categoryName;     // "Mudah", "Sedang", "Sulit"
    public int currentLevelIndex;   // Level terakhir yang diselesaikan

    [Header("UI")]
    public Button nextButton;

    void Start()
    {
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextButtonPressed);
        }
    }

    private void OnNextButtonPressed()
{
    Debug.Log("🖱️ Tombol Next ditekan!");

    if (GameManager.instance == null)
    {
        Debug.LogError("❌ GameManager tidak ditemukan!");
        return;
    }

    // 🔹 Ambil skor terakhir dari PlayerPrefs
    int lastScore = PlayerPrefs.GetInt("FinalScore", 0);
    Debug.Log($"📊 Skor terakhir: {lastScore}");

    // 🔹 Hanya buka level berikutnya jika skor >= 75
    GameManager.instance.UnlockNextLevel(categoryName, currentLevelIndex, lastScore);

    Debug.Log($"✅ Cek level berikutnya dari {categoryName} selesai.");

    // 🔹 Kembali ke scene terakhir (menu level)
    // string lastSelectScene = GameManager.instance.GetLastSelectScene();
    // SceneManager.LoadScene(lastSelectScene);
    // Debug.Log($"↩️ Kembali ke {lastSelectScene}");
}
    // 🔹 Tombol "Main Ulang"
    public void UlangiLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
        Debug.Log("🔁 Mengulang level saat ini.");
    }

    // 🔹 Tombol "Kembali ke Menu Utama"
    public void KembaliKeMenuUtama()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("🏠 Kembali ke menu utama.");
    }
}
