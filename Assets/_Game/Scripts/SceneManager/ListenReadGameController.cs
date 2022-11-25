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
        public static ListenReadGameController Instance;
        public static Piece PieceToChoose { get; private set; }

        [Header("General Settings")]
        [SerializeField] private string jsonPath;//TODO: MUDAR PARA VARIAVEL GLOBAL
        [Header("Game Pieces Settings")]
        [SerializeField] private ChoiceSlot choiceSlot;
        [SerializeField] private Transform choicesPlace;
        [Header("Word Writer Settings")]
        [SerializeField] private GameObject SucessScreen;
        [Header("Word Writer Settings")]
        [SerializeField] private Transform wordPlace;
        [SerializeField] private GameObject charSlot;

        private List<Piece> piecesSet;
        private AudioSource audioSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            //somente para executar teste direto na cena.
            if (CurrentGameTheme.GetGameTheme() == GameThemes.None.ToString())
            {
                CurrentGameTheme.SetGameTheme(GameThemes.Fruits.ToString());
                CurrentGameType.SetGameType(GameTypes.Read.ToString());
                CurrentAudioProperty.SetAudioProperty(AudioProperties.Sound.ToString());
            }
            print("GameTheme: " + CurrentGameTheme.GetGameTheme().ToString() + " | GameType: " + CurrentGameType.GetGameType().ToString() + " | AudioProperty: " + CurrentAudioProperty.GetAudioProperty().ToString());
        }

        private void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            AudioController.Instance.AudioSource = audioSource;
            StartGame();
            StartListening();
        }

        private void StartGame()
        {
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
                PlayPieceSound();
            }
        }

        private void PlayPieceSound()
        {
            //audioSource.clip = AudioController.Instance.LoadAudio(CurrentGameTheme.GetGameTheme(), PieceToChoose.nameId);
            string audioPath = (CurrentAudioProperty.GetAudioProperty() != AudioProperties.None.ToString()) ? CurrentGameTheme.GetGameTheme() + "/" + CurrentAudioProperty.GetAudioProperty() : CurrentGameTheme.GetGameTheme();
            audioSource.clip = AudioController.Instance.LoadAudio(audioPath, PieceToChoose.nameId);
            AudioController.Instance.PlaySoundPiece();
        }

        private void PlayReadSound()
        {
            audioSource.clip = AudioController.Instance.LoadAudio(CurrentGameType.GetGameType());
            AudioController.Instance.PlaySoundPiece();
        }

        private IEnumerator WriteWord()
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            WordWriter.Instance.StartWordWriter(PieceToChoose.word, charSlot, wordPlace);
        }

        public void RestartGame()
        {
            StopAllCoroutines();
            CleanWordFromScene();
            StartGame();
            SucessScreen.SetActive(false);
        }

        private void StartListening()
        {
            ChoiceSlot.OnChoiceFail += ScreenShaker.Instance.ShakeIt;
            ChoiceSlot.OnChoiceFail += AudioController.Instance.PlaySoundFail;
            ChoiceSlot.OnChoiceSuccess += Sucsess;
            ChoiceSlot.OnChoiceFailChoicePiece += ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWrite += PlayPieceSound;
        }

        private void OnDisable()
        {
            ChoiceSlot.OnChoiceFail -= Fail;
            ChoiceSlot.OnChoiceSuccess -= ToggleShowSucessScreen;
            ChoiceSlot.OnChoiceSuccess -= ScoreManager.Instance.AddPoint;
            ChoiceSlot.OnChoiceSuccess -= AudioController.Instance.PlaySoundSucess;
            ChoiceSlot.OnChoiceFailChoicePiece -= ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWrite -= PlayPieceSound;
        }

        private void Sucsess()
        {
            ToggleShowSucessScreen();
            ScoreManager.Instance.AddPoint();
            AudioController.Instance.PlaySoundSucess();
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

        public void PlaySound()
        {
            audioSource.Play();
        }

        private void CleanWordFromScene()
        {
            WordWriter.Instance.CleanCharSlotList();
        }

        private void ToggleShowSucessScreen()
        {
            SucessScreen.SetActive(!SucessScreen.activeSelf);
        }
    }
}

//TODO: trocar contagem de sucesso neste arquivo
/////////*---Implementações---*/////////////
//Não deixar interagir com a cena enquanto não for carregada por completa (colocar um painel transparente na frente de tudo)
//colocar botão de fechar o jogo
//colocar botão de voltar para o menu principal nos menus de tipo de jogo
//animar os botões saindo do HUD e indo para o centro da tela quando ativar tela sucesso
//Gravar os audios
//
//
//
//sistema de idioma
//
/////////*---Gugs conhecidos---*/////////////
//Palavras muito grade estrapolando a tela
//Quando clica mais de uma vez sobre recarregar as peças no tipo: leitura, escreve mais de uma vez a palavra
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