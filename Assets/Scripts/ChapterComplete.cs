// // using UnityEngine;
// // using UnityEngine.SceneManagement;

// // public class ChapterComplete : MonoBehaviour
// // {
// //     [Header("Set Manual di Inspector")]
// //     public string categoryName = "Mudah"; // "Mudah", "Sedang", atau "Sulit"
// //     public int currentLevelIndex = 1;     // misalnya 1 untuk level1mudah

// //     public void LevelSelesai()
// //     {
// //         if (GameManager.instance == null)
// //         {
// //             Debug.LogError("❌ GameManager tidak ditemukan di scene!");
// //             return;
// //         }

// //         Debug.Log($"🎯 Level {categoryName} {currentLevelIndex} selesai!");
// //         GameManager.instance.UnlockNextLevel(categoryName, currentLevelIndex);

// //         // Kembali ke scene terakhir (Select Chapter)
// //         string lastSelect = PlayerPrefs.GetString("LastSelectScene", "MainMenu");
// //         Debug.Log($"🔙 Kembali ke scene: {lastSelect}");
// //         SceneManager.LoadScene(lastSelect);
// //     }
// // }
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class ChapterComplete : MonoBehaviour
// {
//     [Header("Set Manual di Inspector")]
//     public string categoryName = "Mudah"; // "Mudah", "Sedang", "Sulit"
//     public int currentLevelIndex = 1;     // level sekarang
//     public int score = 100;               // isi dengan skor aktual setelah main

//     public void LevelSelesai()
//     {
//         if (GameManager.instance == null)
//         {
//             Debug.LogError("❌ GameManager tidak ditemukan di scene!");
//             return;
//         }

//         Debug.Log($"🎯 Level {categoryName} {currentLevelIndex} selesai dengan skor {score}!");

//         // Simpan skor & buka level berikutnya
//         GameManager.instance.SaveScore(categoryName, currentLevelIndex, score);
//         GameManager.instance.UnlockNextLevel(categoryName, currentLevelIndex);

//         // Kembali ke scene Select
//         string lastSelect = PlayerPrefs.GetString("LastSelectScene", "MainMenu");
//         SceneManager.LoadScene(lastSelect);
//     }

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        // Selalu buka level berikutnya setelah selesai
        GameManager.instance.UnlockNextLevel(categoryName, currentLevelIndex);
        Debug.Log($"✅ Level berikutnya dari {categoryName} terbuka.");

        // Kembali ke scene terakhir (menu level)
        string lastSelectScene = GameManager.instance.GetLastSelectScene();
        SceneManager.LoadScene(lastSelectScene);
        Debug.Log($"↩️ Kembali ke {lastSelectScene}");
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
