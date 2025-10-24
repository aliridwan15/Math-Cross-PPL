// using UnityEngine;
// using UnityEngine.UI;

// public class LevelScoreDisplay : MonoBehaviour
// {
//     public string categoryName;     // contoh: "Mudah"
//     public int levelIndex;          // contoh: 1, 2, 3 ...
//     public Image scoreImage;        // drag komponen image dari Inspector
//     public Sprite score20, score40, score60, score80, score100;

//     void Start()
//     {
//         string key = $"Score_{categoryName}_Level{levelIndex}";
//         if (!PlayerPrefs.HasKey(key))
//         {
//             // Belum pernah main → sembunyikan icon score
//             scoreImage.gameObject.SetActive(false);
//         }
//         else
//         {
//             // Sudah main → tampilkan sesuai score
//             scoreImage.gameObject.SetActive(true);
//             int score = PlayerPrefs.GetInt(key);
//             UpdateScoreSprite(score);
//         }
//     }

//     void UpdateScoreSprite(int score)
//     {
//         if (score >= 80) scoreImage.sprite = score100;
//         else if (score >= 60) scoreImage.sprite = score80;
//         else if (score >= 40) scoreImage.sprite = score60;
//         else if (score >= 20) scoreImage.sprite = score40;
//         else scoreImage.sprite = score20;
//     }
// }
using UnityEngine;
using UnityEngine.UI;

public class LevelScoreDisplay : MonoBehaviour
{
    public string categoryName;
    public int levelIndex;
    public Image scoreImage;
    public Sprite score20, score40, score60, score80, score100, noScore;

    void Start()
    {
        string key = $"{categoryName}_LevelScore_{levelIndex}";
        if (!PlayerPrefs.HasKey(key))
        {
            scoreImage.sprite = noScore;
            return;
        }

        int score = PlayerPrefs.GetInt(key);
        UpdateScoreSprite(score);
    }

    void UpdateScoreSprite(int score)
    {
        if (score >= 90) scoreImage.sprite = score100;
        else if (score >= 75) scoreImage.sprite = score80;
        else if (score >= 60) scoreImage.sprite = score60;
        else if (score >= 40) scoreImage.sprite = score40;
        else if (score >= 20) scoreImage.sprite = score20;
        else scoreImage.sprite = noScore;
    }
}

