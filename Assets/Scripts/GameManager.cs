// using UnityEngine;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager instance;

//     void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//             Debug.Log("✅ GameManager aktif dan tidak akan dihancurkan antar scene.");
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     // 🔹 Mengecek apakah level tertentu sudah terbuka
//     public bool IsLevelUnlocked(string categoryName, int levelIndex)
//     {
//         string key = $"{categoryName}_LevelUnlocked_{levelIndex}";
//         int defaultValue = (levelIndex == 1) ? 1 : 0;
//         int value = PlayerPrefs.GetInt(key, defaultValue);
//         return value == 1;
//     }

//     // 🔹 Membuka level berikutnya setelah menyelesaikan level sekarang
//     public void UnlockNextLevel(string categoryName, int currentLevelIndex)
//     {
//         int nextLevel = currentLevelIndex + 1;
//         string nextKey = $"{categoryName}_LevelUnlocked_{nextLevel}";
//         PlayerPrefs.SetInt(nextKey, 1);
//         PlayerPrefs.Save();
//         Debug.Log($"✅ Level berikutnya terbuka: {nextKey}");
//     }

//     // 🔹 Reset semua level
//     public void ResetAllLevels()
//     {
//         string[] categories = { "Mudah", "Sedang", "Sulit" };

//         foreach (string category in categories)
//         {
//             for (int i = 1; i <= 10; i++)
//             {
//                 string key = $"{category}_LevelUnlocked_{i}";
//                 PlayerPrefs.SetInt(key, i == 1 ? 1 : 0);

//                 string scoreKey = $"{category}_LevelScore_{i}";
//                 PlayerPrefs.DeleteKey(scoreKey);
//             }
//         }

//         PlayerPrefs.Save();
//         Debug.Log("🔁 Semua level direset: hanya level 1 yang terbuka dan skor dihapus.");
//     }

//     // 🔹 Simpan skor level tertentu
//     public void SaveScore(string categoryName, int levelIndex, int score)
//     {
//         string scoreKey = $"{categoryName}_LevelScore_{levelIndex}";
//         PlayerPrefs.SetInt(scoreKey, score);
//         PlayerPrefs.Save();
//         Debug.Log($"💾 Skor tersimpan: {scoreKey} = {score}");
//     }

//     // 🔹 Ambil skor level tertentu
//     public int GetScore(string categoryName, int levelIndex)
//     {
//         string scoreKey = $"{categoryName}_LevelScore_{levelIndex}";
//         return PlayerPrefs.GetInt(scoreKey, 0);
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("✅ GameManager aktif dan tidak akan dihancurkan antar scene.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔹 Mengecek apakah level tertentu sudah terbuka
    public bool IsLevelUnlocked(string categoryName, int levelIndex)
    {
        string key = $"{categoryName}_LevelUnlocked_{levelIndex}";
        int defaultValue = (levelIndex == 1) ? 1 : 0; // Level 1 selalu terbuka
        int value = PlayerPrefs.GetInt(key, defaultValue);
        return value == 1;
    }

    // 🔹 Membuka level berikutnya setelah menyelesaikan level sekarang
    public void UnlockNextLevel(string categoryName, int currentLevelIndex)
    {
        int nextLevel = currentLevelIndex + 1;
        string nextKey = $"{categoryName}_LevelUnlocked_{nextLevel}";
        PlayerPrefs.SetInt(nextKey, 1);
        PlayerPrefs.Save();
        Debug.Log($"✅ Level berikutnya terbuka: {nextKey}");
    }

    // 🔹 Reset semua level
    public void ResetAllLevels()
    {
        string[] categories = { "Mudah", "Sedang", "Sulit" };

        foreach (string category in categories)
        {
            for (int i = 1; i <= 10; i++)
            {
                string key = $"{category}_LevelUnlocked_{i}";
                PlayerPrefs.SetInt(key, i == 1 ? 1 : 0);
            }
        }

        PlayerPrefs.Save();
        Debug.Log("🔁 Semua level direset: hanya level 1 yang terbuka.");
    }

    // 🔹 Menyimpan scene terakhir (misal: "SelectLevel_Mudah")
    public void SetLastSelectScene(string sceneName)
    {
        PlayerPrefs.SetString("LastSelectScene", sceneName);
        PlayerPrefs.Save();
        Debug.Log($"💾 Scene terakhir disimpan: {sceneName}");
    }

    // 🔹 Mendapatkan scene terakhir
    public string GetLastSelectScene()
    {
        return PlayerPrefs.GetString("LastSelectScene", "MainMenu");
    }
}