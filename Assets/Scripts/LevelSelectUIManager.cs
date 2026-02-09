using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectUIManager : MonoBehaviour
{
    [Header("Tombol Pilihan Level")]
    public Button level1;
    public Button level2;
    public Button level3;

    // --- TAMBAHAN BARU ---
    [Header("Tombol Navigasi")]
    [Tooltip("Seret Tombol Home Anda ke sini")]
    public Button homeButton; 
    // ---------------------

    void Start()
    {
        // Hubungkan tombol ke fungsi masing-masing
        level1.onClick.AddListener(() => LoadScene("LevelSelect1"));
        level2.onClick.AddListener(() => LoadScene("LevelSelect2"));
        level3.onClick.AddListener(() => LoadScene("LevelSelect3"));

        // --- TAMBAHAN BARU ---
        // Hubungkan tombol Home ke fungsi OnHomeButtonPressed
        if (homeButton != null)
        {
            homeButton.onClick.AddListener(OnHomeButtonPressed);
        }
        else
        {
            Debug.LogWarning("Tombol Home (homeButton) belum di-set di Inspector!");
        }
        // ---------------------
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // --- FUNGSI BARU YANG ANDA MINTA ---
    /// <summary>
    /// Fungsi ini akan memuat scene "Home".
    /// Pastikan Anda sudah menghubungkan Tombol Home di Inspector.
    /// </summary>
    public void OnHomeButtonPressed()
    {
        LoadScene("Home");
    }
    // ---------------------------------
}