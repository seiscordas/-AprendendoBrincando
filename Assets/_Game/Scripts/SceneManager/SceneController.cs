using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LearningByPlaying
{
    public class SceneController : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private TextAsset jsonScene;
        [SerializeField] private Transform choicesPlace;
        [SerializeField] private List<Piece> piecesSet;
        
        private AudioSource audioSource;

        public static Piece PieceToChoose { get; private set; }
        public Transform ChoicesPlace { get => choicesPlace; }

        private void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            AudioController.Instance.AudioSource = audioSource;
            StartGame();
        }

        private void StartGame()
        {
            piecesSet = Shuffle(CreatePiecesSet(GetJSONFile()));
            ImageController.Instance.SetImagePieces(piecesSet);

            PieceToChoose = piecesSet[UnityEngine.Random.Range(0, piecesSet.Count)];
            audioSource.clip = AudioController.Instance.LoadAudio(PieceToChoose.theme, PieceToChoose.nameId);
            AudioController.Instance.PlaySoundPiece();
        }

        public void RestartGame()
        {
            StartGame();
        }

        private void OnEnable()
        {
            ChoiceSlot.OnChoiceFailChoicePiece += ImageController.Instance.ResetImagePiecePosition;
            ChoiceSlot.OnChoiceFail += ScreenShaker.Instance.ShakeIt;
            ChoiceSlot.OnChoiceSuccess += AudioController.Instance.PlaySoundSucess;
            ChoiceSlot.OnChoiceFail += AudioController.Instance.PlaySoundFail;
        }

        private void OnDisable()
        {
            ChoiceSlot.OnChoiceFailChoicePiece -= ImageController.Instance.ResetImagePiecePosition;
            ChoiceSlot.OnChoiceFail -= ScreenShaker.Instance.ShakeIt;
            ChoiceSlot.OnChoiceSuccess -= AudioController.Instance.PlaySoundSucess;
            ChoiceSlot.OnChoiceFail -= AudioController.Instance.PlaySoundFail;
        }

        private PiecesList GetJSONFile()
        {
            //var jsonFile = Resources.Load("Pieces").ToString();
            return JsonUtility.FromJson<PiecesList>(jsonScene.ToString());
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

        public void PlaySound()
        {
            audioSource.Play();
        }

        public static bool PieceCompare(ChoicePiece chosenPiece)
        {
            if(chosenPiece.nameId == PieceToChoose.nameId)
                return true;
            return false;
        }
    }
    enum GameType { audio, read }
    enum GameTheme { animals, objects }
}
//TODO:
//Add som de erro quando falha
//Add som de sucesso quando acerta
//Acrecentar acertos no Score
//Add tela de sucesso com os botões de menu proximo
//
//sistema de troca de background aleatorio e de acordo como o tema
//sistema de troca de background das peças aleatorio
//