using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class LevelButtonData
{
    public Button button;
    public string sceneName;
    public Sprite unlockedSprite;
    public TextMeshProUGUI scoreText;
}

public class ChapterSelectManager : MonoBehaviour
{
    [Header("Kategori")]
    public string categoryName = "Mudah";

    [Header("Sprite Locked")]
    public Sprite lockedSprite;

    [Header("Daftar Level")]
    public LevelButtonData[] levels;

    void Start()
    {
        // 🔹 Pastikan PlayerPrefs sudah siap sebelum dipanggil
        Invoke(nameof(RefreshButtons), 0.05f);
    }

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

            // 🔹 Bersihkan event lama
            level.button.onClick.RemoveAllListeners();

            // 🔹 Jika level terbuka → bisa diklik
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

            // ✅ Tampilkan skor terakhir (fix nama key dan format UI)
            if (level.scoreText != null)
            {
                string key = $"{level.sceneName}_Score";
                int savedScore = PlayerPrefs.GetInt(key, -999); // ubah default jadi -999 biar mudah debug

                if (savedScore == -999)
                    Debug.LogWarning($"⚠️ Skor belum tersimpan untuk {key}");
                else
                    Debug.Log($"✅ Menampilkan skor {savedScore} untuk {key}");

                // Tampilkan angka tanpa label tambahan
                level.scoreText.text = savedScore <= 0 ? "0" : savedScore.ToString();
            }
        }

        Debug.Log($"🔄 Semua tombol kategori {categoryName} diperbarui.");
    }

    /// <summary>
    /// Fungsi untuk kembali ke scene "LevelSelect".
    /// Hubungkan ini ke tombol "Back" di Inspector.
    /// </summary>
    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    // ---------------------------------
}