using LearningByPlaying.AudioProperty;
using LearningByPlaying.gameTheme;
using LearningByPlaying.GameType;
using LearningByPlaying.WordWriterSystem;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LearningByPlaying
{
    public class ListenWriteGameController : MonoBehaviour
    {
        //public static ListenWriteGameController Instance;
        //public static Piece PieceToChoose { get; private set; }
        public static event Action OnFinishPlayAudioClip;

        private AudioSource audioSource;
        private int goalsQuantity;
        private Piece piece;

        [Header("General Settings")]
        [SerializeField] private string jsonPath;//TODO: MUDAR PARA VARIAVEL GLOBAL
        [SerializeField] private GameObject LockSceen;
        [Header("SucessScreen Settings")]
        [SerializeField] private GameObject SucessScreen;

        [Header("Word Writer Settings")]
        [SerializeField] private Transform wordSlotPlace;
        [SerializeField] private Transform choicesWordPlace;
        [SerializeField] private GameObject ChoiceSlotAlphabet;
        [SerializeField] private GameObject CharChoiceDragDrop;
        [SerializeField] private float wordWiretDelay = 0.5f;

        [Header("Image Settings")]
        [SerializeField] private Image wordImage;

        private void Awake()
        {
            OnlyForTest();
        }

        private void OnlyForTest()
        {
            //somente para executar teste direto na cena.
            if (CurrentGameTheme.GetGameTheme() == GameThemes.None.ToString())
            {
                CurrentGameTheme.SetGameTheme(GameThemes.Fruits.ToString());
                CurrentGameType.SetGameType(GameTypes.Listen.ToString());
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

        private IEnumerator CreateChoiceSlots()
        {
            foreach (GameObject charSlot in WordWriter.Instance.CharSlotList)
            {
                GameObject choiceSlot = Instantiate(ChoiceSlotAlphabet, wordSlotPlace);
                choiceSlot.GetComponent<ChoiceSlot>().PieceToChoose = SetAndReturnPieceNameId(charSlot);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private string SetAndReturnPieceNameId(GameObject charSlot)
        {
            string nameId = charSlot.GetComponentInChildren<TextMeshProUGUI>().text;
            charSlot.GetComponentInChildren<ChoicePiece>().nameId = nameId;
            return nameId;
        }

        private void CleanWordFromScene()
        {
            WordWriter.Instance.CleanCharSlotList();
            while (wordSlotPlace.childCount > 0)
            {
                DestroyImmediate(wordSlotPlace.GetChild(0).gameObject);
            }
        }

        private void StartGame()
        {
            ToggleShowLockScreen();

            piece = SetPieceWord(GetJSONFile());

            SetImage();

            PlayWriteSound();
            StartCoroutine(WriteWord());
            SetGoalsQuantity();
            StartListening();
        }

        public void SetImage()
        {
            wordImage.sprite = ImageController.Instance.LoadImage(CurrentGameTheme.GetGameTheme(), piece.nameId);
        }

        private void SetGoalsQuantity()
        {
            goalsQuantity = piece.word.ToCharArray().Length;
        }

        private void PlayPieceSoundCoroutine()
        {
            StartCoroutine(PlayPieceSound());
        }

        private IEnumerator PlayPieceSound()
        {
            string audioPath = CurrentGameTheme.GetGameTheme() + "/" + CurrentAudioProperty.GetAudioProperty();
            audioSource.clip = AudioController.Instance.LoadAudio(audioPath, piece.nameId);
            AudioController.Instance.PlaySoundPiece();
            yield return new WaitForSeconds(audioSource.clip.length);
            OnFinishPlayAudioClip?.Invoke();
        }

        private void PlayWriteSound()
        {
            audioSource.clip = AudioController.Instance.LoadAudio(CurrentGameType.GetGameType());
            AudioController.Instance.PlaySoundPiece();
        }

        private IEnumerator WriteWord()
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            WordWriter.Instance.StartWordWriter(piece.word, CharChoiceDragDrop, choicesWordPlace, wordWiretDelay);
        }

        public void RestartGame()
        {
            StopListening();
            StopAllCoroutines();
            CleanWordFromScene();
            ToggleAutoAlign();
            StartGame();
            SucessScreen.SetActive(false);
        }

        private void StartCreateChoiceSlots()
        {
            StartCoroutine(CreateChoiceSlots());
        }

        private void Fail()
        {
            ScreenShaker.Instance.ShakeIt();
            AudioController.Instance.PlaySoundFail();
        }

        private void CheckForProgresse()
        {
            goalsQuantity -= 1;

            if (goalsQuantity <= 0)
                Sucsess();
            else
                AudioController.Instance.PlaySoundSucessProgress();
        }

        private void Sucsess()
        {
            ToggleShowSucessScreen();
            ScoreManager.Instance.AddPoint();
            AudioController.Instance.PlaySoundSucess();
            StopListening();
        }

        private void StartListening()
        {
            ChoiceSlot.OnChoiceFail += Fail;
            ChoiceSlot.OnChoiceSuccess += CheckForProgresse;
            ChoiceSlot.OnChoiceFailChoicePiece += ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWriteWord += WordWriterFinished;
            OnFinishPlayAudioClip += ToggleShowLockScreen;
        }

        private void OnDisable()
        {
            StopListening();
        }

        private void StopListening()
        {
            ChoiceSlot.OnChoiceFail -= Fail;
            ChoiceSlot.OnChoiceSuccess -= CheckForProgresse;
            ChoiceSlot.OnChoiceFailChoicePiece -= ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWriteWord -= WordWriterFinished;
            OnFinishPlayAudioClip -= ToggleShowLockScreen;
        }

        private void WordWriterFinished()
        {
            ToggleAutoAlign();
            PlayPieceSoundCoroutine();
            StartCreateChoiceSlots();
            AnimateShuffle();
        }

        private PiecesList GetJSONFile()
        {
            var jsonFile = Resources.Load(jsonPath + CurrentGameTheme.GetGameTheme()).ToString();
            return JsonUtility.FromJson<PiecesList>(jsonFile);
        }

        private Piece SetPieceWord(PiecesList piecesList)
        {
            Piece piece = piecesList.pieces[UnityEngine.Random.Range(0, piecesList.pieces.Length)];
            return piece;
        }

        private void AnimateShuffle()
        {
            Transform[] children = choicesWordPlace.GetComponentsInChildren<Transform>();
            ShuffleAnimate.Animate(WordWriter.Instance.CharSlotList);
        }

        private void ToggleShowSucessScreen()
        {
            SucessScreen.SetActive(!SucessScreen.activeSelf);
        }

        private void ToggleAutoAlign()
        {
            HorizontalLayoutGroup hlg = choicesWordPlace.GetComponentInParent<HorizontalLayoutGroup>();
            hlg.enabled = !hlg.enabled;
        }

        private void ToggleShowLockScreen()
        {
            LockSceen.SetActive(!LockSceen.activeSelf);
        }
    }
}
//TODO:
//erro conhecido
//quando arrasta uma letra e contabiliza acerto, se arrastar a mesma letra contabiliza acerto novamente