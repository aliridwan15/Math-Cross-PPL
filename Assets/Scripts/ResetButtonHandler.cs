using UnityEngine;

public class ResetButtonHandler : MonoBehaviour
{
    public ChapterSelectManager chapterSelectManager;

    public void OnClickReset()
{
    if (chapterSelectManager == null)
    {
        Debug.LogWarning("⚠️ ChapterSelectManager belum di-assign di Inspector!");
        return;
    }

    GameManager.instance.ResetAllLevels();

    // 🔄 Perbarui UI tombol setelah reset
    chapterSelectManager.RefreshButtons();
}

}
