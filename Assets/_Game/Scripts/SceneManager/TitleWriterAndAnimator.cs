using DG.Tweening;
using LearningByPlaying.WordWriterSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleWriterAndAnimator : MonoBehaviour
{
    [SerializeField] private Transform titlePlace;
    [SerializeField] private GameObject gameObjectChar;
    [SerializeField] private string sceneTitle;


    private void Start()
    {
        WordWriter.Instance.StartWordWriter(sceneTitle, gameObjectChar, titlePlace, 0.1f);
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
        for (int i = 0; i < titlePlace.childCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 position = titlePlace.GetChild(i).localPosition;
            titlePlace.GetChild(i).transform.DOLocalJump(new Vector3(position.x, 3f, position.z), 1f, 1, 0.2f).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void StartJumpCoroutine()
    {
        StartCoroutine(Jump());
    }

    private void DisableAutoAlign()
    {
        titlePlace.GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
    }
}

