using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LearningByPlaying
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private TextAsset jsonScene;

        [SerializeField] private GameObject pieceSlot;
        [SerializeField] private Transform choicesPlace;

        [SerializeField] private PiecesList choicePieces;

        private void Start()
        {
            GetJSONFile();

            audioSource = gameObject.AddComponent<AudioSource>();

            //carrega audio
            StartCoroutine(LoadAudio(Application.dataPath + soundPath + "Animals" + "/" + "abelha.mp3"));

            //seta imagens das peças
            SetImagePieces();
        }

        private void GetJSONFile()
        {
            choicePieces = JsonUtility.FromJson<PiecesList>(jsonScene.text);
            //print("choicePieces: " + choicePieces.Pieces.Length);
        }

        private void GetPiecesComponents()
        {

        }

        private void SetImagePieces()
        {
            ChoicePiece[] choicesPlace = this.choicesPlace.GetComponentsInChildren<ChoicePiece>();
            int index = 0;
            foreach (ChoicePiece choicePiece in choicesPlace)
            {
                choicePiece.GetComponent<Image>().sprite = LoadImage("Animals", "abelha");
                index++;
            }
        }

        #region load audio

        [Header("Audio Stuff")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private string soundPath;

        private IEnumerator LoadAudio(string soundPath)
        {
            if (System.IO.File.Exists(soundPath))
            {
                using (var audio = UnityWebRequestMultimedia.GetAudioClip(soundPath, AudioType.MPEG))
                {
                    ((DownloadHandlerAudioClip)audio.downloadHandler).streamAudio = true;

                    yield return audio.SendWebRequest();

                    if (audio.result == UnityWebRequest.Result.ConnectionError || audio.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.Log(audio.error);
                        yield break;
                    }
                    DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)audio.downloadHandler;
                    if (dlHandler.isDone)
                    {
                        AudioClip audioClip = dlHandler.audioClip;

                        if (audioClip != null)
                        {
                            this.audioClip = DownloadHandlerAudioClip.GetContent(audio);
                            PlayAudioFile();
                            Debug.Log("Playing song using Audio Source!");
                        }
                        else
                        {
                            Debug.Log("Couldn't find a valid AudioClip :(");
                        }
                    }
                    else
                    {
                        Debug.Log("The download process is not completely finished.");
                    }
                }
            }
            else
            {
                Debug.Log("Unable to locate converted song file.");
                print(soundPath);
            }
        }

        private void PlayAudioFile()
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            audioSource.loop = false;
        }
        #endregion

        #region load image
        [Header("Image Stuff")]
        [SerializeField] private string imagePath;

        public Sprite LoadImage(string imgTheme, string imgName)
        {
            return Resources.Load<Sprite>(imagePath + imgTheme + "/" + imgName);
        }
        #endregion

    }
    enum GameType { audio, read }
    enum GameTheme { animals, objects }
}