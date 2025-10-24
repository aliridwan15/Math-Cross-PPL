using UnityEngine;
using UnityEngine.UI;

public class HomeUIManager : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button buttonMulai;
    public Button buttonTutorial;

    [Header("Scene Loader")]
    public SceneLoader sceneLoader;

    void Start()
    {
        buttonMulai.onClick.AddListener(sceneLoader.LoadLevelSelect);
        buttonTutorial.onClick.AddListener(sceneLoader.LoadTutorial);
    }
}
