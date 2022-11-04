using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AudioController : MonoBehaviour
{
    public string audioName = "abelha.mp3";

    [Header("Audio Stuff")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private string soundPath;

    [SerializeField] private AudioClip teste;
    public string testeName = "abelha";
    private void Awake()
    {
        teste = Resources.Load<AudioClip>(testeName);

        audioSource = gameObject.AddComponent<AudioSource>();
        soundPath = Application.dataPath + "/_Game/Resources/Sounds/Animals/abelha.mp3";

        //StartCoroutine(LoadAudio());
    }

    private IEnumerator LoadAudio()
    {
        if (System.IO.File.Exists(soundPath))
        {
            using (var audio = UnityWebRequestMultimedia.GetAudioClip(soundPath, AudioType.MPEG))
            {
                ((DownloadHandlerAudioClip)audio.downloadHandler).streamAudio = true;

                yield return audio.SendWebRequest();

                if(audio.result == UnityWebRequest.Result.ConnectionError || audio.result == UnityWebRequest.Result.ProtocolError)
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
        }
    }

    private void PlayAudioFile()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        audioSource.loop = true;
    }

    //private UnityWebRequest GetAudioFromFile(string path, string filename)
    //{
    //    string audioToLoad = string.Format(path + "{0}", filename);
    //    UnityWebRequest request = UnityWebRequestMultimedia. (audioToLoad);
    //    return request;
    //}
}
