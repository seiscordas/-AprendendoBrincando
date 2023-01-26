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

        [SerializeField] PointerEventData currentEventData;
        [SerializeField] private string pieceToChoose;

        public string PieceToChoose { get => pieceToChoose; set => pieceToChoose = value; }

        public void OnDrop(PointerEventData eventData)
        {
            DragDrop.IsOverChoiceSlot = true;
            if (eventData.pointerDrag != null)
            {
                eventData.dragging = !CheckProgress(eventData);
            }
        }

        private void AlignGameObject()
        {
            currentEventData.pointerDrag.GetComponent<RectTransform>().transform.position = GetComponent<RectTransform>().transform.position;
        }

        private void Fail(ChoicePiece choicePiece)
        {
            OnChoiceFailChoicePiece?.Invoke(choicePiece);
            OnChoiceFail?.Invoke();
        }

        private void Success(ChoicePiece choicePiece)
        {
            AlignGameObject();
            OnChoiceSuccess?.Invoke();
            OnChoiceSuccessChoicePiece?.Invoke(choicePiece);
        }

        private bool CheckProgress(PointerEventData eventData)
        {
            ChoicePiece choicePiece = eventData.pointerDrag.GetComponent<ChoicePiece>();
            if(choicePiece.nameId == pieceToChoose)
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
