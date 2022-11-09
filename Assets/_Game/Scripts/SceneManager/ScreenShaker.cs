using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [Header("Screen Shaker Config")]
    private Vector3 screenInitalPosition;
    [SerializeField] private float shakeMagnitude = 0.05f;
    [SerializeField] private float shakeTime = 0.5f;
    [SerializeField] private GameObject elementToShake;

    public static ScreenShaker Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ShakeIt()
    {
        screenInitalPosition = elementToShake.transform.position;
        InvokeRepeating(nameof(StartScreenShaking), 0, 0.005f);
        Invoke(nameof(StopScreenShaking), shakeTime);
    }

    void StartScreenShaking()
    {
        float screenShakeOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float screenShakeOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        Vector3 screenIntermediatePosition = elementToShake.transform.position;

        screenIntermediatePosition.x = screenShakeOffsetX;
        screenIntermediatePosition.y = screenShakeOffsetY;
        elementToShake.transform.position = screenIntermediatePosition;
    }

    void StopScreenShaking()
    {
        CancelInvoke(nameof(StartScreenShaking));
        elementToShake.transform.position = screenInitalPosition;
    }
}
