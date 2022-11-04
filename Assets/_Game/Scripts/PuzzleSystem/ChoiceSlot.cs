using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LearningByPlaying
{
    public class ChoiceSlot : MonoBehaviour, IDropHandler
    {
        public static event Action OnFail;
        public static event Action OnSuccess;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                AlignGameObject(eventData);
            }
        }

        private void AlignGameObject(PointerEventData eventData)
        {
            Debug.Log(eventData.pointerDrag.name);
            eventData.pointerDrag.GetComponent<Transform>().parent = GetComponent<Transform>().parent;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }

        private void Fail()
        {
            OnFail?.Invoke();
        }

        private void Success()
        {
            OnSuccess?.Invoke();
        }

        private bool CheckProgress(PointerEventData eventData)
        {
            //TODO: PEGAR CRIAR UMA CLASSE QUE TENHA STATUS DO ITEM A SER ESCOLHIDO
            //eventData.pointerDrag.GetComponent<Transform>().parent = GetComponent<Transform>().parent;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            return true;
        }
    }
}
