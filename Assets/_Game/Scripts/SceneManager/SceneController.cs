using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LearningByPlaying
{
    public class SceneController : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private TextAsset jsonScene;
        [SerializeField] private Transform choicesPlace;
        [SerializeField] private List<Piece> piecesSet;

        [Header("Audio Config")]
        [SerializeField] private string audioPath;
        private AudioSource audioSource;
        private AudioClip audioClip;

        [Header("Image Config")]
        [SerializeField] private string imagePath;

        public static Piece Piece { get; private set; }

        private void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            StartGame();
        }

        private void StartGame()
        {
            piecesSet = Shuffle(CreatePiecesSet(GetJSONFile()));
            SetImagePieces(piecesSet);

            Piece = piecesSet[UnityEngine.Random.Range(0, piecesSet.Count)];
            this.audioClip = LoadAudio(Piece.theme, Piece.nameId);
            PlayAudioFile();
        }

        public void RestartGame()
        {
            print("Restart");
            StartGame();
        }

        public void PlaySound()
        {
            audioSource.Play();
        }

        private void OnEnable()
        {
            ChoiceSlot.OnChoiceFailChoicePiece += ResetImagePosition;
            ChoiceSlot.OnChoiceFail += ScreemShaker.instance.ShakeIt;
        }

        private void OnDisable()
        {
            ChoiceSlot.OnChoiceFailChoicePiece -= ResetImagePosition;
            ChoiceSlot.OnChoiceFail -= ScreemShaker.instance.ShakeIt;
        }

        private PiecesList GetJSONFile()
        {
            //var jsonFile = Resources.Load("Pieces").ToString();
            return JsonUtility.FromJson<PiecesList>(jsonScene.ToString());
        }

        private void SetImagePieces(List<Piece> piecesList)
        {
            ChoicePiece[] choicesPlace = this.choicesPlace.GetComponentsInChildren<ChoicePiece>();
            int index = 0;
            foreach (ChoicePiece choicePiece in choicesPlace)
            {
                choicePiece.GetComponent<Image>().sprite = LoadImage(piecesList[index].theme, piecesList[index].nameId);
                choicePiece.GetComponent<ChoicePiece>().nameId = piecesList[index].nameId;
                index++;
            }
        }

        private List<Piece> CreatePiecesSet(PiecesList piecesList)
        {
            List<Piece> pl = new();
            int index = 0;
            while (index < 3)
            {
                Piece piece = piecesList.pieces[UnityEngine.Random.Range(0, piecesList.pieces.Length)];
                if (pl.Find(x => x.nameId == piece.nameId) == null)
                {
                    pl.Add(piece);
                    index++;
                }
            }
            return pl;
        }

        private List<Piece> Shuffle(List<Piece> piecesList)
        {
            return piecesList.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public static void ResetImagePosition(ChoicePiece choicePiece)
        {
            choicePiece.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        #region load audio     
        public AudioClip LoadAudio(string audioTheme, string audioName)
        {
            return Resources.Load<AudioClip>(audioPath + audioTheme + "/" + audioName);
        }        

        private void PlayAudioFile()
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            audioSource.loop = false;
        }
        #endregion

        #region load image
        public Sprite LoadImage(string imgTheme, string imgName)
        {
            return Resources.Load<Sprite>(imagePath + imgTheme + "/" + imgName);
        }
        #endregion
    }
    enum GameType { audio, read }
    enum GameTheme { animals, objects }
}