using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectUIManager : MonoBehaviour
{
    [Header("Tombol Pilihan Level")]
    public Button level1;
    public Button level2;
    public Button level3;

    void Start()
    {
        // Hubungkan tombol ke fungsi masing-masing
        level1.onClick.AddListener(() => LoadScene("LevelSelect1"));
        level2.onClick.AddListener(() => LoadScene("LevelSelect2"));
        level3.onClick.AddListener(() => LoadScene("LevelSelect3"));
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
