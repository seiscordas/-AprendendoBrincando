using LearningByPlaying.gameTheme;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LearningByPlaying
{
    public class ImageController : MonoBehaviour
    {
        [Header("Image Config")]
        [SerializeField] private string imagePath;//TODO: MUDAR PARA VARIAVEL GLOBAL

        ListenReadGameController sceneController;

        public static ImageController Instance;

        private void Awake()
        {
            sceneController = GetComponent<ListenReadGameController>();
            Instance = this;
        }

        public Sprite LoadImage(string imgTheme, string imgName)
        {
            return Resources.Load<Sprite>(imagePath + imgTheme + "/" + imgName);
        }

        public void SetImagePieces(List<Piece> piecesList, Transform choicesPlace)
        {
            ChoicePiece[] choicesPlaceList = choicesPlace.GetComponentsInChildren<ChoicePiece>();
            int index = 0;
            foreach (ChoicePiece choicePiece in choicesPlaceList)
            {
                choicePiece.GetComponent<Image>().sprite = LoadImage(CurrentGameTheme.GetGameTheme(), piecesList[index].nameId);
                choicePiece.GetComponent<ChoicePiece>().nameId = piecesList[index].nameId;
                choicePiece.transform.localPosition = Vector3.zero;
                index++;
            }
        }

        public void ResetImagePiecePosition(ChoicePiece choicePiece)
        {
            choicePiece.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
