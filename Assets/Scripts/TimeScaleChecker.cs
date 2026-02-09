using UnityEngine;

public class TimeScaleChecker : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Time Scale Sekarang: " + Time.timeScale);
        }
    }
}
