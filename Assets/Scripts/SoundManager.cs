using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Komponen Audio")]
    public AudioSource backgroundMusic;

    private const string MusicStateKey = "MusicState";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadMusicState();
    }

    public void SetMusicOn()
    {
        backgroundMusic.enabled = true;
        SaveMusicState(true);
    }

    public void SetMusicOff()
    {
        backgroundMusic.enabled = false;
        SaveMusicState(false);
    }

    public bool IsMusicEnabled()
    {
        return backgroundMusic.enabled;
    }

    private void LoadMusicState()
    {
        int state = PlayerPrefs.GetInt(MusicStateKey, 1);
        backgroundMusic.enabled = (state == 1);
    }

    private void SaveMusicState(bool isOn)
    {
        PlayerPrefs.SetInt(MusicStateKey, isOn ? 1 : 0);
    }

    // --- LOGIKA BARU DITAMBAHKAN DI SINI ---
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cari script animator di scene yang baru dimuat
        SimpleToggleAnimator animator = FindFirstObjectByType<SimpleToggleAnimator>(FindObjectsInactive.Include);

        if (animator != null)
        {
            // Beri tahu animator posisi awalnya berdasarkan status musik saat ini
            animator.SetInitialState(backgroundMusic.enabled);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}