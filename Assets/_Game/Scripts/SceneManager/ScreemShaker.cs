using UnityEngine;

public class ScreemShaker : MonoBehaviour
{
    [Header("Screem Shaker Config")]
    private Vector3 screemInitalPosition;
    [SerializeField] private float shakeMagnitude = 0.05f;
    [SerializeField] private float shakeTime = 0.5f;
    [SerializeField] private GameObject elementToShake;

    public static ScreemShaker instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShakeIt()
    {
        screemInitalPosition = elementToShake.transform.position;
        InvokeRepeating(nameof(StartScreemShaking), 0, 0.005f);
        Invoke(nameof(StopScreemShaking), shakeTime);
    }

    void StartScreemShaking()
    {
        float screemShakeOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float screemShakeOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        Vector3 screemIntermediatePosition = elementToShake.transform.position;

        screemIntermediatePosition.x = screemShakeOffsetX;
        screemIntermediatePosition.y = screemShakeOffsetY;
        elementToShake.transform.position = screemIntermediatePosition;
    }

    void StopScreemShaking()
    {
        CancelInvoke(nameof(StartScreemShaking));
        elementToShake.transform.position = screemInitalPosition;
    }
}
