using LearningByPlaying.AudioProperty;
using LearningByPlaying.gameTheme;
using LearningByPlaying.GameType;
using LearningByPlaying.WordWriterSystem;
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

        private AudioSource audioSource;
        private int goalsQuantity;
        private Piece piece;

        [Header("General Settings")]
        [SerializeField] private string jsonPath;//TODO: MUDAR PARA VARIAVEL GLOBAL
        [Header("SucessScreen Settings")]
        [SerializeField] private GameObject SucessScreen;

        [Header("Word Writer Settings")]
        [SerializeField] private Transform wordSlotPlace;
        [SerializeField] private Transform choicesWordPlace;
        [SerializeField] private GameObject ChoiceSlotAlphabet;
        [SerializeField] private GameObject CharChoiceDragDrop;

        [Header("Image Settings")]
        [SerializeField] private Image image;

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
                CurrentGameType.SetGameType(GameTypes.Write.ToString());
                CurrentAudioProperty.SetAudioProperty(AudioProperties.None.ToString());
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
            piece = SetPieceWord(GetJSONFile());

            SetImage();

            PlayWriteSound();
            StartCoroutine(WriteWord());
            SetGoalsQuantity();
        }

        public void SetImage()
        {
            image.sprite = ImageController.Instance.LoadImage(CurrentGameTheme.GetGameTheme(), piece.nameId);
        }

        private void SetGoalsQuantity()
        {
            goalsQuantity = piece.word.ToCharArray().Length;
        }

        private void PlayPieceSound()
        {
            string audioPath = (CurrentAudioProperty.GetAudioProperty() != AudioProperties.None.ToString()) ? CurrentGameTheme.GetGameTheme() + "/" + CurrentAudioProperty.GetAudioProperty() : CurrentGameTheme.GetGameTheme();
            audioSource.clip = AudioController.Instance.LoadAudio(audioPath, piece.nameId);
            AudioController.Instance.PlaySoundPiece();
        }

        private void PlayWriteSound()
        {
            audioSource.clip = AudioController.Instance.LoadAudio(CurrentGameType.GetGameType());
            AudioController.Instance.PlaySoundPiece();
        }

        private IEnumerator WriteWord()
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            WordWriter.Instance.StartWordWriter(piece.word, CharChoiceDragDrop, choicesWordPlace);
        }

        public void RestartGame()
        {
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
        }

        private void Sucsess()
        {
            ToggleShowSucessScreen();
            ScoreManager.Instance.AddPoint();
            AudioController.Instance.PlaySoundSucess();
        }

        private void StartListening()
        {
            ChoiceSlot.OnChoiceFail += Fail;
            ChoiceSlot.OnChoiceSuccess += CheckForProgresse;
            ChoiceSlot.OnChoiceFailChoicePiece += ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWrite += FinisWrite;
        }

        private void OnDisable()
        {
            ChoiceSlot.OnChoiceFail -= Fail;
            ChoiceSlot.OnChoiceSuccess -= CheckForProgresse;
            ChoiceSlot.OnChoiceFailChoicePiece -= ImageController.Instance.ResetImagePiecePosition;
            WordWriter.OnFinishWrite -= FinisWrite;
        }

        private void FinisWrite()
        {
            ToggleAutoAlign();
            PlayPieceSound();
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
            Piece piece = piecesList.pieces[Random.Range(0, piecesList.pieces.Length)];
            return piece;
        }

        private void AnimateShuffle()
        {
            Transform[] children = choicesWordPlace.GetComponentsInChildren<Transform>();
            ShuffleAnimate.Animate(WordWriter.Instance.CharSlotList);
        }

        public void PlaySound()
        {
            audioSource.Play();
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
    }
}

//TODO:
/////////*---Implementações---*/////////////
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
//

/////////*---Ideias---*/////////////
//Ideias
//sistema de troca de background da cena e das peças, e das fontes de forma aleatoria e de acordo como o tema (USAR PREFABS)
//
//Animar a o menu principal
//Animar os menus do tipo de jogo
//Buscar novas imagens de crianças e backgrounds
//
