using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LearningByPlaying
{
    public class ChoiceSlot : MonoBehaviour, IDropHandler
    {
        public static event Action OnChoiceFail;
        public static event Action<ChoicePiece> OnChoiceFailChoicePiece;
        public static event Action<ChoicePiece> OnChoiceSuccess;

        PointerEventData currentEventData;

        public void OnDrop(PointerEventData eventData)
        {
            DragDrop.IsOverChoiceSlot = true;
            if (eventData.pointerDrag != null)
            {
                CheckProgress(eventData);                
            }
        }

        private void AlignGameObject()
        {
            currentEventData.pointerDrag.GetComponent<Transform>().parent = GetComponent<Transform>().parent;
            currentEventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }

        private void Fail(ChoicePiece choicePiece)
        {
            Debug.Log("falhou...");
            OnChoiceFailChoicePiece?.Invoke(choicePiece);
            OnChoiceFail?.Invoke();
        }

        private void Success(ChoicePiece choicePiece)
        {
            Debug.Log("Acertou...");
            AlignGameObject();
            OnChoiceSuccess?.Invoke(choicePiece);
        }

        private bool CheckProgress(PointerEventData eventData)
        {
            ChoicePiece choicePiece = eventData.pointerDrag.GetComponent<ChoicePiece>();
            if(choicePiece.nameId == SceneController.Piece.nameId)
            {
                currentEventData = eventData;
                Success(choicePiece);
                return true;
            }
            
            Fail(choicePiece);
            return false;
        }
    }
}
