using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    void Start()
    {
        Invoke("GoToHome", 2.1f);
    }

    void GoToHome()
    {
        SceneManager.LoadScene("Home");
    }
}
