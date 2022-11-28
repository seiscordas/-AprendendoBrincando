using LearningByPlaying.AudioProperty;
using LearningByPlaying.gameTheme;
using LearningByPlaying.GameType;
using LearningByPlaying.WordWriterSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LearningByPlaying
{
    public class ListenReadGameController : MonoBehaviour
    {
        public static event Action OnFinishPlayAudioClip;
        public static Piece PieceToChoose { get; private set; }

        [Header("General Settings")]
        [SerializeField] private string jsonPath;//TODO: MUDAR PARA VARIAVEL GLOBAL
        [SerializeField] private GameObject LockSceen;
        [Header("Game Pieces Settings")]
        [SerializeField] private ChoiceSlot choiceSlot;
        [SerializeField] private Transform choicesPlace;
        [Header("Word Writer Settings")]
        [SerializeField] private GameObject SucessScreen;
        [Header("Word Writer Settings")]
        [SerializeField] private Transform wordPlace;
        [SerializeField] private GameObject charSlot;
        [SerializeField] private float wordWiretDelay = 0.5f;

        private List<Piece> piecesSet;
        private AudioSource audioSource;

        private void Awake()
        {
            //somente para executar teste direto na cena.
            if (CurrentGameTheme.GetGameTheme() == GameThemes.None.ToString())
            {
                CurrentGameTheme.SetGameTheme(GameThemes.Fruits.ToString());
                CurrentGameType.SetGameType(GameTypes.Read.ToString());
                CurrentAudioProperty.SetAudioProperty(AudioProperties.None.ToString());
            }
            print("GameTheme: " + CurrentGameTheme.GetGameTheme().ToString() + " | GameType: " + CurrentGameType.GetGameType().ToString() + " | AudioProperty: " + CurrentAudioProperty.GetAudioProperty().ToString());
        }

        private void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            AudioController.Instance.AudioSource = audioSource;
            StartGame();
        }

        private void StartGame()
        {
            ToggleShowLockScreen();

            piecesSet = Shuffle(CreatePiecesSet(GetJSONFile()));//TODO: COLOCAR METODOS EM CADEIA CreatePiecesSet->Shuffle
            ImageController.Instance.SetImagePieces(piecesSet, choicesPlace);

            PieceToChoose = piecesSet[UnityEngine.Random.Range(0, piecesSet.Count)];
            choiceSlot.PieceToChoose = PieceToChoose.nameId;

            if (CurrentGameType.GetGameType() == GameTypes.Read.ToString())
            {
                PlayReadSound();
                StartCoroutine(WriteWord());
            }
            else
            {
                StartCoroutine(PlayPieceSound());
            }
            StartListening();
        }

        private void PlayPieceSoundCoroutine()
        {
            StartCoroutine(PlayPieceSound());
        }

        private IEnumerator PlayPieceSound()
        {
            string audioPath = (CurrentAudioProperty.GetAudioProperty() != AudioProperties.None.ToString()) ? CurrentGameTheme.GetGameTheme() + "/" + CurrentAudioProperty.GetAudioProperty() : CurrentGameTheme.GetGameTheme();
            audioSource.clip = AudioController.Instance.LoadAudio(audioPath, PieceToChoose.nameId);
            AudioController.Instance.PlaySoundPiece();
            yield return new WaitForSeconds(audioSource.clip.length);
            OnFinishPlayAudioClip?.Invoke();
        }

        private void PlayReadSound()
        {
            audioSource.clip = AudioController.Instance.LoadAudio(CurrentGameType.GetGameType());
            AudioController.Instance.PlaySoundPiece();
        }

        private IEnumerator WriteWord()
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            WordWriter.Instance.StartWordWriter(PieceToChoose.word, charSlot, wordPlace, wordWiretDelay);
        }

        public void RestartGame()
        {
            StopListening();
            StopAllCoroutines();
            CleanWordFromScene();
            StartGame();
            SucessScreen.SetActive(false);
        }

        private void StartListening()
        {
            ChoiceSlot.OnChoiceFail += Fail;
            ChoiceSlot.OnChoiceSuccess += Sucsess;
            ChoiceSlot.OnChoiceFailChoicePiece += ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWriteWord += PlayPieceSoundCoroutine;
            OnFinishPlayAudioClip += ToggleShowLockScreen;
        }

        private void OnDisable()
        {
            StopListening();
        }

        private void StopListening()
        {
            ChoiceSlot.OnChoiceFail -= Fail;
            ChoiceSlot.OnChoiceSuccess -= Sucsess;
            ChoiceSlot.OnChoiceFailChoicePiece -= ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWriteWord -= PlayPieceSoundCoroutine;
            OnFinishPlayAudioClip -= ToggleShowLockScreen;
        }

        private void Sucsess()
        {
            SucessScreen.SetActive(true);
            ScoreManager.Instance.AddPoint();
            AudioController.Instance.PlaySoundSucess();
            WordWriter.OnFinishWriteWord -= PlayPieceSoundCoroutine;
        }

        private void Fail()
        {
            ScreenShaker.Instance.ShakeIt();
            AudioController.Instance.PlaySoundFail();
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

        private void CleanWordFromScene()
        {
            WordWriter.Instance.CleanCharSlotList();
        }

        private void ToggleShowLockScreen()
        {
            LockSceen.SetActive(!LockSceen.activeSelf);
        }
    }
}

//TODO: trocar contagem de sucesso neste arquivo
/////////*---Implementações---*/////////////
//carregar background de acordo com thema
//animar os botões saindo do HUD e indo para o centro da tela quando ativar tela sucesso
//Gravar os audios
//
//
//sistema de idioma
//
/////////*---Gugs conhecidos---*/////////////
//Repete peça com muita frequencia
//

/////////*---Ideias---*/////////////
//Ideias
//sistema de troca de background da cena e das peças, e das fontes de forma aleatoria e de acordo como o tema (USAR PREFABS)
//
//Animar a o menu principal
//Animar os menus do tipo de jogo
//Buscar novas imagens de crianças e backgrounds
//