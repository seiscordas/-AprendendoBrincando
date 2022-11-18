using LearningByPlaying.gameTheme;
using LearningByPlaying.GameType;
using LearningByPlaying.WordWriterSystem;
using System;
using System.Collections.Generic;
using System.Linq;
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

            //somente para executar teste direto na cena.
            if (CurrentGameTheme.GetGameTheme() == null)
            {
                CurrentGameTheme.SetGameTheme(GameThemes.Fruits.ToString());
                CurrentGameType.SetGameType(GameTypes.Read.ToString());
            }
        }

        private void Start()
        {
            print("Game Theme: " + CurrentGameTheme.GetGameTheme());//debug
            print("Game Type: " + CurrentGameType.GetGameType());//debug

            audioSource = gameObject.AddComponent<AudioSource>();
            AudioController.Instance.AudioSource = audioSource;
            StartGame();
            StartListening();
        }

        private void StartGame()
        {
            piecesSet = Shuffle(CreatePiecesSet(GetJSONFile()));//TODO: COLOCAR METODOS EM CADEIA CreatePiecesSet->Shuffle
            ImageController.Instance.SetImagePieces(piecesSet);

            PieceToChoose = piecesSet[UnityEngine.Random.Range(0, piecesSet.Count)];
            audioSource.clip = AudioController.Instance.LoadAudio(CurrentGameType.GetGameType(), CurrentGameTheme.GetGameTheme(), PieceToChoose.nameId);
            AudioController.Instance.PlaySoundPiece();

            if (CurrentGameType.GetGameType() == GameTypes.Read.ToString())
            {
                WordWriter.Instance.StartWordWriter(PieceToChoose.word);
            }
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
}

//TODO:
//Adicionar sistema de som em cada letra e ao cliclar falar o nome da letra
//Trocar de ListToPopup (lista de cena) para Enum. Testar
//sistema de troca de background da cena e das peças de forma aleatoria e de acordo como o tema (USAR PREFABS)
//sistema de idioma
//Duvidas
//
//
//
//