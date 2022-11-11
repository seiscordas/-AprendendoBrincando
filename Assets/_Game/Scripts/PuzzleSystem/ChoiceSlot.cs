using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LearningByPlaying
{
    public class ChoiceSlot : MonoBehaviour, IDropHandler
    {
        public static event Action OnChoiceFail;
        public static event Action OnChoiceSuccess;
        public static event Action<ChoicePiece> OnChoiceFailChoicePiece;
        public static event Action<ChoicePiece> OnChoiceSuccessChoicePiece;

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
            currentEventData.pointerDrag.GetComponent<RectTransform>().transform.position = GetComponent<RectTransform>().transform.position;
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
            OnChoiceSuccessChoicePiece?.Invoke(choicePiece);
            OnChoiceSuccess?.Invoke();
        }

        private bool CheckProgress(PointerEventData eventData)
        {
            ChoicePiece choicePiece = eventData.pointerDrag.GetComponent<ChoicePiece>();
            if(SoundGameController.PieceCompare(choicePiece))
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
