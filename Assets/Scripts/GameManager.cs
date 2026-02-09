
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
    public void UnlockNextLevel(string categoryName, int currentLevelIndex, int lastScore)
{
    int nextLevel = currentLevelIndex + 1;
    string nextKey = $"{categoryName}_LevelUnlocked_{nextLevel}";

    if (lastScore >= 75)
    {
        PlayerPrefs.SetInt(nextKey, 1);
        PlayerPrefs.Save();
        Debug.Log($"✅ Level berikutnya terbuka karena skor {lastScore} ≥ 75: {nextKey}");
    }
    else
    {
        Debug.Log($"❌ Level berikutnya tetap terkunci karena skor {lastScore} < 75");
    }
}

    // 🔹 Reset semua level
public void ResetAllLevels()
{
    string[] categories = { "Mudah", "Sedang", "Sulit" };

    foreach (string category in categories)
    {
        string catLower = category.ToLower();

        for (int i = 1; i <= 10; i++)
        {
            // 1) Reset unlock flag (tetap format yang Tuan pakai)
            string unlockKey = $"{category}_LevelUnlocked_{i}";
            PlayerPrefs.SetInt(unlockKey, i == 1 ? 1 : 0);
            Debug.Log($"🔁 Reset unlock key: {unlockKey}");

            // 2) Hapus kemungkinan key skor sesuai dua pola yang mungkin ada

            // Pola A: scene-based seperti yang terlihat di log: "level1mudah_Score"
            string sceneStyleKey = $"level{i}{catLower}_Score";
            if (PlayerPrefs.HasKey(sceneStyleKey))
            {
                PlayerPrefs.DeleteKey(sceneStyleKey);
                Debug.Log($"🗑️ Hapus scene-style score key: {sceneStyleKey}");
            }

            // Pola B: kategori-style (fallback) "Mudah_Level1_Score" (jika ada)
            string categoryStyleKey = $"{category}_Level{i}_Score";
            if (PlayerPrefs.HasKey(categoryStyleKey))
            {
                PlayerPrefs.DeleteKey(categoryStyleKey);
                Debug.Log($"🗑️ Hapus category-style score key: {categoryStyleKey}");
            }
        }
    }

    // Hapus FinalScore juga kalau ada
    if (PlayerPrefs.HasKey("FinalScore"))
    {
        PlayerPrefs.DeleteKey("FinalScore");
        Debug.Log("🗑️ Hapus FinalScore");
    }

    PlayerPrefs.Save();
    Debug.Log("🔁 Semua level dan skor direset: hanya level 1 yang terbuka.");
}
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