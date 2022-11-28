using System;
using UnityEngine;

namespace LearningByPlaying
{
    public class AudioController : MonoBehaviour
    {
        public static AudioController Instance;

        [Header("Audio Config")]
        [SerializeField] private AudioClip audioSucess;
        [SerializeField] private AudioClip audioSucessProgress;
        [SerializeField] private AudioClip audioFail;

        public AudioSource AudioSource;

        private void Awake()
        {            
            Instance = this;
        }

        public AudioClip LoadAudio(string gameType, string gameTheme, string audioName)
        {
            return Resources.Load<AudioClip>("Audios/" + gameType + "/" + gameTheme + "/" + audioName);
        }

        public AudioClip LoadAudio(string path, string audioName)
        {
            return Resources.Load<AudioClip>("Audios/" + path + "/" + audioName);
        }

        public AudioClip LoadAudio(string audioName)
        {
            return Resources.Load<AudioClip>("Audios/Miscellaneous/" + audioName);
        }

        public void PlaySoundPiece()
        {
            AudioSource.Play();
        }

        public void PlaySoundSucess()
        {
            AudioSource.PlayOneShot(audioSucess);
        }

        public void PlaySoundSucessProgress()
        {
            AudioSource.PlayOneShot(audioSucessProgress);
        }

        public void PlaySoundFail()
        {
            AudioSource.PlayOneShot(audioFail);
        }
    }
}
