// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;

// [System.Serializable]
// public class LevelButtonData
// {
//     public Button button;
//     public string sceneName;
//     public Sprite unlockedSprite;
// }

// public class ChapterSelectManager : MonoBehaviour
// {
//     [Header("Kategori")]
//     public string categoryName = "Mudah";

//     [Header("Sprite Locked")]
//     public Sprite lockedSprite;

//     [Header("Daftar Level")]
//     public LevelButtonData[] levels;

//     void Start()
//     {
//         RefreshButtons();
//     }

//     void OnEnable()
//     {
//         RefreshButtons(); // supaya tombol update saat scene dibuka ulang
//     }
//     public void RefreshButtons()
// {
//     if (GameManager.instance == null)
//     {
//         Debug.LogError("❌ GameManager belum aktif!");
//         return;
//     }

//     for (int i = 0; i < levels.Length; i++)
//     {
//         var level = levels[i];
//         if (level.button == null) continue;

//         int levelIndex = i + 1;
//         bool unlocked = GameManager.instance.IsLevelUnlocked(categoryName, levelIndex);

//         // 🔹 Atur tombol aktif/tidak
//         level.button.interactable = unlocked;
//         Image img = level.button.GetComponent<Image>();
//         if (img != null)
//             img.sprite = unlocked ? level.unlockedSprite : lockedSprite;

//         // 🔹 Bersihkan event lama agar tidak dobel
//         level.button.onClick.RemoveAllListeners();

//         if (unlocked)
//         {
//             string targetScene = level.sceneName;
//             level.button.onClick.AddListener(() =>
//             {
//                 Debug.Log($"▶️ Buka {targetScene}");
//                 PlayerPrefs.SetString("LastSelectScene", SceneManager.GetActiveScene().name);
//                 PlayerPrefs.Save();
//                 SceneManager.LoadScene(targetScene);
//             });
//         }

//         // 🔹 Ambil skor dan tampilkan gambar score (jika ada)
//         int score = GameManager.instance.GetScore(categoryName, levelIndex);
//         Image scoreImage = level.button.transform.Find("ScoreImage")?.GetComponent<Image>();

//         if (scoreImage != null)
//         {
//             // Pastikan sprite di Resources/Score_XX.png
//             string spriteName = "Score_0";

//             if (score >= 90) spriteName = "Score_100";
//             else if (score >= 75) spriteName = "Score_80";
//             else if (score >= 60) spriteName = "Score_60";
//             else if (score >= 40) spriteName = "Score_40";
//             else if (score >= 20) spriteName = "Score_20";
//             else spriteName = "Score_0";

//             Sprite scoreSprite = Resources.Load<Sprite>(spriteName);
//             if (scoreSprite != null)
//             {
//                 scoreImage.sprite = scoreSprite;
//                 scoreImage.enabled = true;
//             }
//             else
//             {
//                 Debug.LogWarning($"⚠️ Sprite '{spriteName}' tidak ditemukan di Resources!");
//                 scoreImage.enabled = false;
//             }
//         }
//     }

//     Debug.Log($"🔄 Semua tombol kategori {categoryName} diperbarui.");
// }

//     // public void RefreshButtons()
//     // {
//     //     if (GameManager.instance == null)
//     //     {
//     //         Debug.LogError("❌ GameManager belum aktif!");
//     //         return;
//     //     }

//     //     for (int i = 0; i < levels.Length; i++)
//     //     {
//     //         var level = levels[i];
//     //         if (level.button == null) continue;

//     //         int levelIndex = i + 1;
//     //         bool unlocked = GameManager.instance.IsLevelUnlocked(categoryName, levelIndex);

//     //         // ubah sprite dan interaksi
//     //         level.button.interactable = unlocked;
//     //         Image img = level.button.GetComponent<Image>();
//     //         if (img != null)
//     //             img.sprite = unlocked ? level.unlockedSprite : lockedSprite;

//     //         // bersihkan event
//     //         level.button.onClick.RemoveAllListeners();

//     //         if (unlocked)
//     //         {
//     //             string targetScene = level.sceneName;
//     //             level.button.onClick.AddListener(() =>
//     //             {
//     //                 Debug.Log($"▶️ Buka {targetScene}");
//     //                 PlayerPrefs.SetString("LastSelectScene", SceneManager.GetActiveScene().name);
//     //                 PlayerPrefs.Save();
//     //                 SceneManager.LoadScene(targetScene);
//     //             });
//     //         }
//     //     }

//     //     Debug.Log($"🔄 Semua tombol kategori {categoryName} diperbarui.");
//     // }
// }
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelButtonData
{
    public Button button;
    public string sceneName;
    public Sprite unlockedSprite;
}

public class ChapterSelectManager : MonoBehaviour
{
    [Header("Kategori")]
    public string categoryName = "Mudah";

    [Header("Sprite Locked")]
    public Sprite lockedSprite;

    [Header("Daftar Level")]
    public LevelButtonData[] levels;

    void Start() => RefreshButtons();

    void OnEnable() => RefreshButtons();

    public void RefreshButtons()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("❌ GameManager belum aktif!");
            return;
        }

        for (int i = 0; i < levels.Length; i++)
        {
            var level = levels[i];
            if (level.button == null) continue;

            int levelIndex = i + 1;
            bool unlocked = GameManager.instance.IsLevelUnlocked(categoryName, levelIndex);

            // 🔹 Atur tampilan tombol
            Image img = level.button.GetComponent<Image>();
            if (img != null)
                img.sprite = unlocked ? level.unlockedSprite : lockedSprite;

            level.button.interactable = unlocked;

            // 🔹 Hapus event lama
            level.button.onClick.RemoveAllListeners();

            // 🔹 Jika level terbuka → bisa diklik untuk buka scene
            if (unlocked)
            {
                string targetScene = level.sceneName;
                level.button.onClick.AddListener(() =>
                {
                    GameManager.instance.SetLastSelectScene(SceneManager.GetActiveScene().name);
                    SceneManager.LoadScene(targetScene);
                    Debug.Log($"▶️ Membuka scene: {targetScene}");
                });
            }
        }

        Debug.Log($"🔄 Semua tombol kategori {categoryName} diperbarui.");
    }
}
