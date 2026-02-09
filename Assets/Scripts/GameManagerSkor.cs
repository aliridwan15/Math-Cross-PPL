using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// Otomatis menambahkan AudioSource ke GameObject
[RequireComponent(typeof(AudioSource))] 
public class GameManagerSkor : MonoBehaviour
{
    [Header("Referensi UI")]
    public TextMeshProUGUI scoreText;
    public Image stageCompleteImage;
    public Button nextButton; 
    public GameObject lowScorePopup; // Drag pop-up "Skor Rendah" Anda ke sini

    [Header("Pengaturan Animasi")]
    public float durasiAnimasiSkor = 2.0f;
    public float durasiAnimasiGambar = 0.5f;
    public float delaySebelumSkor = 0.3f;
    
    [Header("Pengaturan Scene")]
    public string chapterSelectSceneName = "PilihChapter"; 

    [Header("Pengaturan Audio")]
    public AudioClip levelCompleteSound; // Suara jika skor >= 70
    public AudioClip levelFailSound;     // Suara jika skor < 70

    private int finalScore;
    private string lastPuzzleSceneName; 
    private string nextPuzzleSceneName; 
    private AudioSource audioSource; 

    private const string SCORE_KEY = "FinalScore";
    private const string LAST_SCENE_KEY = "LastPuzzleScene"; 
    private const string NEXT_SCENE_KEY = "NextPuzzleScene"; 

    // ==========================
    // 🔶 LIFECYCLE
    // ==========================

    private void Awake()
    {
        // Mengambil komponen AudioSource yang sudah otomatis ditambahkan
        audioSource = GetComponent<AudioSource>();
        // Pastikan tidak 3D (Spatial Blend) dan tidak looping
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0.0f; // 0.0f = 2D Sound
        }
    }

    private void Start()
    {
        // Pastikan pop-up tersembunyi di awal
        lowScorePopup?.SetActive(false);
        
        // 1. Ambil skor DULU
        finalScore = PlayerPrefs.GetInt(SCORE_KEY, 0);

        // Ambil data scene
        lastPuzzleSceneName = PlayerPrefs.GetString(LAST_SCENE_KEY, chapterSelectSceneName); 
        nextPuzzleSceneName = PlayerPrefs.GetString(NEXT_SCENE_KEY); 

        // 2. Set kondisi awal gambar
        if (stageCompleteImage != null)
        {
            stageCompleteImage.rectTransform.sizeDelta = Vector2.zero;
            stageCompleteImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Referensi 'stageCompleteImage' belum di-assign di Inspector!");
        }

        // 3. Mulai animasi
        if (scoreText != null)
        {
            StartCoroutine(AnimasiGambarMuncul());
            StartCoroutine(HitungSkorAnimasi());
        }
        else
        {
            Debug.LogError("Referensi 'scoreText' belum di-assign di Inspector!");
        }

        // 4. Cek Skor untuk Audio, Tombol Next, dan Pop-up
        if (finalScore < 70)
        {
            // --- SKOR RENDAH (< 70) ---
            
            // Mainkan suara GAGAL
            if (audioSource != null && levelFailSound != null)
            {
                audioSource.PlayOneShot(levelFailSound);
            }
            
            // Sembunyikan tombol "Next"
            if (nextButton != null)
            {
                nextButton.gameObject.SetActive(false);
            }

            // Tampilkan pop-up skor rendah
            if (lowScorePopup != null)
            {
                lowScorePopup.SetActive(true);
            }
        }
        else
        {
            // --- SKOR TINGGI (>= 70) ---

            // Mainkan suara BERHASIL
            if (audioSource != null && levelCompleteSound != null)
            {
                audioSource.PlayOneShot(levelCompleteSound);
            }

            // Cek tombol "Next" (Logika untuk level terakhir)
            if (nextButton != null)
            {
                bool isLastLevel = string.IsNullOrEmpty(nextPuzzleSceneName);
                if (isLastLevel)
                {
                    // Ini level terakhir, sembunyikan tombol Next
                    nextButton.gameObject.SetActive(false);
                }
                else
                {
                    // Bukan level terakhir & skor cukup, pastikan tombol Next aktif
                    nextButton.gameObject.SetActive(true);
                }
            }
        }
    }

    // ==========================
    // 🔶 FUNGSI ANIMASI (Tidak Berubah)
    // ==========================

    private IEnumerator AnimasiGambarMuncul()
    {
        float elapsedTime = 0f;
        Vector2 sizeAwal = Vector2.zero;
        Vector2 targetImageSize = new Vector2(600, 400); 
        RectTransform imageRect = stageCompleteImage.rectTransform;

        while (elapsedTime < durasiAnimasiGambar)
        {
            float progres = elapsedTime / durasiAnimasiGambar;
            imageRect.sizeDelta = Vector2.Lerp(sizeAwal, targetImageSize, progres);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        imageRect.sizeDelta = targetImageSize;
    }

    private IEnumerator HitungSkorAnimasi()
    {
        yield return new WaitForSeconds(delaySebelumSkor);

        float elapsedTime = 0f;
        int skorTampilanSaatIni = 0;
        
        scoreText.text = "Skor Anda: 0";

        while (elapsedTime < durasiAnimasiSkor)
        {
            float progres = elapsedTime / durasiAnimasiSkor;
            skorTampilanSaatIni = (int)Mathf.Lerp(0f, (float)finalScore, progres);
            
            scoreText.text = $"Skor Anda: {skorTampilanSaatIni}";
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scoreText.text = $"Skor Anda: {finalScore}";
    }

    // ==========================
    // 🔶 FUNGSI TOMBOL NAVIGASI
    // ==========================

    /// <summary>
    /// Fungsi untuk tombol "Replay"
    /// </summary>
    public void OnReplayButton()
    {
        SceneManager.LoadScene(lastPuzzleSceneName);
    }

    /// <summary>
    /// Fungsi untuk tombol "Next"
    /// </summary>
    public void OnNextButton()
    {
        SceneManager.LoadScene(nextPuzzleSceneName);
    }

    /// <summary>
    /// Fungsi untuk tombol "Pilih Chapter"
    /// </summary>
    public void OnChapterSelectButton()
    {
        if (!string.IsNullOrEmpty(chapterSelectSceneName))
        {
            SceneManager.LoadScene(chapterSelectSceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene ('chapterSelectSceneName') belum diatur di Inspector!");
        }
    }

    /// <summary>
    /// Fungsi untuk dihubungkan ke tombol "OK" atau "Tutup" pada pop-up skor rendah
    /// </summary>
    public void OnLowScorePopupClose()
    {
        lowScorePopup?.SetActive(false);
    }
}