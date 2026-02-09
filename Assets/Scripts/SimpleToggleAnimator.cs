using UnityEngine;
using System.Collections;

public class SimpleToggleAnimator : MonoBehaviour
{
    [Header("Referensi Objek")]
    public RectTransform starHandle;
    public Transform onPositionMarker;
    public Transform offPositionMarker;

    [Header("Pengaturan Animasi")]
    public float animationDuration = 0.2f;
    public float rotationAmount = 360f;

    private Coroutine runningCoroutine;

    // Start() function dihapus karena SoundManager akan mengatur posisi awal

    public void AnimateToOn()
    {
        if (starHandle.position != onPositionMarker.position)
        {
            StartAnimation(true);
        }
    }

    public void AnimateToOff()
    {
        if (starHandle.position != offPositionMarker.position)
        {
            StartAnimation(false);
        }
    }
    
    // --- FUNGSI BARU ---
    // Fungsi ini akan dipanggil oleh SoundManager saat scene baru dimuat
    public void SetInitialState(bool isOn)
    {
        if (starHandle == null || onPositionMarker == null || offPositionMarker == null) return;
        starHandle.position = isOn ? onPositionMarker.position : offPositionMarker.position;
        starHandle.localEulerAngles = Vector3.zero;
    }

    private void StartAnimation(bool isOn)
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(AnimateStar(isOn));
    }

    private IEnumerator AnimateStar(bool isOn)
    {
        if (starHandle == null || onPositionMarker == null || offPositionMarker == null) yield break;

        Vector3 startPosition = starHandle.position;
        Vector3 targetPosition = isOn ? onPositionMarker.position : offPositionMarker.position;

        float startRotation = starHandle.localEulerAngles.z;
        float targetRotation = isOn ? 0f : rotationAmount;

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            t = t * t * (3f - 2f * t);

            starHandle.position = Vector3.Lerp(startPosition, targetPosition, t);
            starHandle.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(startRotation, targetRotation, t));
            yield return null;
        }

        starHandle.position = targetPosition;
        starHandle.localEulerAngles = Vector3.zero;
        runningCoroutine = null;
    }
}