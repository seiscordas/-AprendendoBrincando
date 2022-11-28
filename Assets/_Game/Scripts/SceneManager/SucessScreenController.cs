using DG.Tweening;
using LearningByPlaying.WordWriterSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SucessScreenController : MonoBehaviour
{
    [SerializeField] private Transform CongratsText;
    [SerializeField] private GameObject gameObjectChar;

    private void OnEnable()
    {
        EnableAutoAlign();
        WordWriter.Instance.CleanCharSlotList();
        WordWriter.Instance.StartWordWriter("parabens", gameObjectChar, CongratsText, 0.1f);
        WordWriter.OnFinishWriteWord += DisableAutoAlign;
        WordWriter.OnFinishWriteWord += StartJumpCoroutine;
    }

    private void OnDisable()
    {
        WordWriter.OnFinishWriteWord -= StartJumpCoroutine;
        WordWriter.OnFinishWriteWord -= DisableAutoAlign;
    }

    private void StartJumpCoroutine()
    {
        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        for (int i = 0; i < CongratsText.childCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 position = CongratsText.GetChild(i).localPosition;
            CongratsText.GetChild(i).transform.DOLocalJump(new Vector3(position.x, 3f, position.z), 1f, 1, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }

    private void EnableAutoAlign()
    {
        CongratsText.GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
    }

    private void DisableAutoAlign()
    {
        CongratsText.GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
    }
}
