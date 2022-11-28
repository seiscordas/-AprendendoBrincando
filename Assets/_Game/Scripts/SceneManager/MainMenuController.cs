using DG.Tweening;
using LearningByPlaying.WordWriterSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Transform titleText;
    [SerializeField] private GameObject gameObjectChar;

    private void OnEnable()
    {
        WordWriter.Instance.StartWordWriter("aprendendo brincando", gameObjectChar, titleText, 0.1f);
        WordWriter.OnFinishWriteWord += DisableAutoAlign;
        WordWriter.OnFinishWriteWord += StartJumpCoroutine;
    }

    private void OnDisable()
    {
        WordWriter.OnFinishWriteWord -= StartJumpCoroutine;
        WordWriter.OnFinishWriteWord -= DisableAutoAlign;
    }

    private IEnumerator Jump()
    {
        for (int i = 0; i < titleText.childCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 position = titleText.GetChild(i).localPosition;
            titleText.GetChild(i).transform.DOLocalJump(new Vector3(position.x, 3f, position.z), 1f, 1, 0.2f).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void StartJumpCoroutine()
    {
        StartCoroutine(Jump());
    }

    private void DisableAutoAlign()
    {
        titleText.GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
    }
}

