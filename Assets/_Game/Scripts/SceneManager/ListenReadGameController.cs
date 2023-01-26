using LearningByPlaying.AudioProperty;
using LearningByPlaying.gameTheme;
using LearningByPlaying.GameType;
using LearningByPlaying.WordWriterSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        [SerializeField] private PiecesList piecesSetDebug;

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
            ShowLockScreen();
            PiecesList piecesList = GetJSONFile();

            piecesList = RemovePiecesSaved(piecesList);
            List<Piece> pieces = CreatePiecesSet(piecesList);
            piecesSet = Shuffle(pieces);//TODO: COLOCAR METODOS EM CADEIA CreatePiecesSet->Shuffle
            ImageController.Instance.SetImagePieces(piecesSet, choicesPlace);

            PieceToChoose = piecesSet[UnityEngine.Random.Range(0, piecesSet.Count)];
            choiceSlot.PieceToChoose = PieceToChoose.nameId;

            Debug.Log("PieceToChoose: " + choiceSlot.PieceToChoose);

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

        private PiecesList RemovePiecesSaved(PiecesList piecesList)
        {
            PiecesList piecesListCompleted = GameDataManager.ReadFile(CurrentGameTheme.GetGameTheme().ToString() + SceneManager.GetActiveScene().name);
            if (piecesListCompleted != null)
            {
                foreach (var item in piecesListCompleted.pieces.ToList())
                {
                    List<Piece> pieces = piecesList.pieces.ToList();
                    pieces.RemoveAll(piece => piece.nameId == item.nameId);
                    piecesList.pieces = pieces.ToArray();
                }
            }
            if (piecesList.pieces.Length < 3)
            {
                piecesList.pieces = new List<Piece>().ToArray();
                GameDataManager.WriteFile(piecesList, CurrentGameTheme.GetGameTheme().ToString() + SceneManager.GetActiveScene().name);
                return piecesListCompleted;
            }
            return piecesList;
        }
        private void PlayPieceSoundCoroutine()
        {
            StartCoroutine(PlayPieceSound());
        }

        private IEnumerator PlayPieceSound()
        {
            string audioPath = CurrentGameTheme.GetGameTheme() + "/" + CurrentAudioProperty.GetAudioProperty();
            Debug.Log("audioPath:" + audioPath + "/" + PieceToChoose.nameId);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //StopListening();
            //StopAllCoroutines();
            //CleanWordFromScene();
            //StartGame();
            //SucessScreen.SetActive(false);
            //Scema
        }

        private void StartListening()
        {
            ChoiceSlot.OnChoiceFail += Fail;
            ChoiceSlot.OnChoiceSuccess += Sucsess;
            ChoiceSlot.OnChoiceSuccessChoicePiece += ChoicePiece;
            ChoiceSlot.OnChoiceFailChoicePiece += ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWriteWord += PlayPieceSoundCoroutine;
            OnFinishPlayAudioClip += HideLockScreen;
        }

        private void OnDisable()
        {
            StopListening();
        }

        private void StopListening()
        {
            ChoiceSlot.OnChoiceFail -= Fail;
            ChoiceSlot.OnChoiceSuccess -= Sucsess;
            ChoiceSlot.OnChoiceSuccessChoicePiece -= ChoicePiece;
            ChoiceSlot.OnChoiceFailChoicePiece -= ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWriteWord -= PlayPieceSoundCoroutine;
            OnFinishPlayAudioClip -= HideLockScreen;
        }

        private void ChoicePiece(ChoicePiece choicePiece)
        {
            PiecesList p = GameDataManager.ReadFile(CurrentGameTheme.GetGameTheme().ToString() + SceneManager.GetActiveScene().name);
            Piece item = new Piece();
            item.nameId = choicePiece.nameId;
            if (p == null)
            {
                p = new PiecesList();
                p.pieces = new List<Piece>().ToArray();
            }
            List<Piece> pieces = p.pieces.ToList();
            pieces.Add(item);

            p.pieces = pieces.ToArray();

            GameDataManager.WriteFile(p, CurrentGameTheme.GetGameTheme().ToString() + SceneManager.GetActiveScene().name);
        }
        private void Sucsess()
        {
            SucessScreen.SetActive(true);
            ScoreManager.Instance.AddPoint();
            AudioController.Instance.PlaySoundSucess();
            WordWriter.OnFinishWriteWord -= PlayPieceSoundCoroutine;
            //GameDataManager.ReadFile(CurrentGameTheme.GetGameTheme().ToString());
        }

        private void Fail()
        {
            ScreenShaker.Instance.ShakeIt();
            AudioController.Instance.PlaySoundFail();
        }

        private PiecesList GetJSONFile()
        {
            var jsonFile = Resources.Load(jsonPath + CurrentGameTheme.GetGameTheme()).ToString();
            piecesSetDebug = JsonUtility.FromJson<PiecesList>(jsonFile);
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

        private void ShowLockScreen()
        {
            Debug.Log("ATIVAR ");
            LockSceen.SetActive(true);
        }
        private void HideLockScreen()
        {
            Debug.Log("Desativar ");
            LockSceen.SetActive(false);
        }
    }
}

//TODO: trocar contagem de sucesso neste arquivo
/////////*---Implementações---*/////////////
//Gravar os audios
//sistema de idioma

/////////*---Gugs conhecidos---*/////////////
//Repete peça com muita frequencia

/////////*---Ideias---*/////////////
//Criar sistema de escolha de thema
//sistema de troca de background da cena e das peças, e das fontes de acordo como o tema (USAR PREFABS)
//Animar os menus do tipo de jogo
//animar os botões saindo do HUD e indo para o centro da tela quando ativar tela sucesso