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
            audioSource.clip = AudioController.Instance.LoadAudio(CurrentGameType.GetGameType(), CurrentGameTheme.GetGameTheme(), PieceToChoose.nameId);
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
            WordWriter.Instance.StartWordWriter(PieceToChoose.word);
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
            WordWriter.OnFinishWrite += PlayPieceSound;
        }

        private void OnDisable()
        {
            ChoiceSlot.OnChoiceFail -= ScreenShaker.Instance.ShakeIt;
            ChoiceSlot.OnChoiceFail -= AudioController.Instance.PlaySoundFail;
            ChoiceSlot.OnChoiceSuccess -= ToggleShowSucessScreen;
            ChoiceSlot.OnChoiceSuccess -= ScoreManager.Instance.AddPoint;
            ChoiceSlot.OnChoiceSuccess -= AudioController.Instance.PlaySoundSucess;
            ChoiceSlot.OnChoiceFailChoicePiece -= ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWrite -= PlayPieceSound;
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
/////////*---Implementações---*/////////////
//colocar botão de fechar o jogo
//colocar botão de voltar para o menu principal nos menus de tipo de jogo
//animar os botões saindo do HUD e indo para o centro da tela quando ativar tela sucesso
//Adicionar sistema de som em cada letra e ao cliclar falar o nome da letra
//Gravar os audios
//
//
//
//sistema de idioma
//
/////////*---Gugs conhecidos---*/////////////
//Palavras muito grade estrapolando a tela
//Quando clica mais de uma vez sobre recarregar as peças no tipo: leitura, escreve mais de uma vez a palavra
//

/////////*---Ideias---*/////////////
//Ideias
//sistema de troca de background da cena e das peças, e das fontes de forma aleatoria e de acordo como o tema (USAR PREFABS)
//
//Animar a o menu principal
//Animar os menus do tipo de jogo
//Buscar novas imagens de crianças e backgrounds
//