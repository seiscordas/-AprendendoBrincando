using LearningByPlaying.gameTheme;
using LearningByPlaying.GameType;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace LearningByPlaying
{
    public class SoundGameController : MonoBehaviour
    {
        public static SoundGameController Instance;

        private AudioSource audioSource;
        public static Piece PieceToChoose { get; private set; }
        public Transform ChoicesPlace { get => choicesPlace; }

        [Header("General Settings")]
        [SerializeField] private string jsonPath;
        [SerializeField] private TextMeshProUGUI WordsPlace;
        [SerializeField] private Transform choicesPlace;
        [SerializeField] private GameObject SucessScreen;
        [SerializeField] private List<Piece> piecesSet;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            //if (CurrentGameTheme.GetGameTheme() == null)
            //    CurrentGameTheme.SetGameTheme("read");
            print("Game Theme: " + CurrentGameTheme.GetGameTheme());
            print("Game Type: " + CurrentGameType.GetGameType());

            audioSource = gameObject.AddComponent<AudioSource>();
            AudioController.Instance.AudioSource = audioSource;
            StartGame();
            StartListening();
        }

        private void StartGame()
        {
            piecesSet = Shuffle(CreatePiecesSet(GetJSONFile()));
            ImageController.Instance.SetImagePieces(piecesSet);

            PieceToChoose = piecesSet[UnityEngine.Random.Range(0, piecesSet.Count)];
            audioSource.clip = AudioController.Instance.LoadAudio(CurrentGameType.GetGameType(), CurrentGameTheme.GetGameTheme(), PieceToChoose.nameId);
            AudioController.Instance.PlaySoundPiece();
        }

        public void RestartGame()
        {
            StartGame();
            SucessScreen.SetActive(false);
        }

        private void StartListening()
        {
            ChoiceSlot.OnChoiceFail += ScreenShaker.Instance.ShakeIt;
            ChoiceSlot.OnChoiceFail += AudioController.Instance.PlaySoundFail;
            ChoiceSlot.OnChoiceSuccess += ToggleShowSucessScreen;
            ChoiceSlot.OnChoiceSuccess += ScoreManager.Instance.AddPoint;
            ChoiceSlot.OnChoiceSuccess += AudioController.Instance.PlaySoundSucess;
            ChoiceSlot.OnChoiceFailChoicePiece += ImageController.Instance.ResetImagePiecePosition;
        }

        private void OnDisable()
        {
            ChoiceSlot.OnChoiceFail -= ScreenShaker.Instance.ShakeIt;
            ChoiceSlot.OnChoiceFail -= AudioController.Instance.PlaySoundFail;
            ChoiceSlot.OnChoiceSuccess -= ToggleShowSucessScreen;
            ChoiceSlot.OnChoiceSuccess -= ScoreManager.Instance.AddPoint;
            ChoiceSlot.OnChoiceSuccess -= AudioController.Instance.PlaySoundSucess;
            ChoiceSlot.OnChoiceFailChoicePiece -= ImageController.Instance.ResetImagePiecePosition;
        }

        private PiecesList GetJSONFile()
        {
            var jsonFile = Resources.Load(jsonPath + CurrentGameType.GetGameType().ToString() + "/" + CurrentGameTheme.GetGameTheme()).ToString();
            return JsonUtility.FromJson<PiecesList>(jsonFile);
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
            if (chosenPiece.nameId == PieceToChoose.nameId)
                return true;
            return false;
        }

        private void ToggleShowSucessScreen()
        {
            SucessScreen.SetActive(!SucessScreen.activeSelf);
        }
    }
    //enum GameType { audio, read }
    //enum GameTheme { animals, objects }
}
//Duvidas
//
//
//
//
//TODO:
//
//sistema de troca de background aleatorio e de acordo como o tema
//sistema de troca de background das peças aleatorio
//sistema de idioma