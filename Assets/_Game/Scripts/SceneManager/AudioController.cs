using UnityEngine;

namespace LearningByPlaying
{
    public class AudioController : MonoBehaviour
    {
        [Header("Audio Config")]
        [SerializeField] private string audioPath;
        [SerializeField] private AudioClip audioSucess;
        [SerializeField] private AudioClip audioFail;

        public AudioSource AudioSource;

        public static AudioController Instance;

        private void Awake()
        {            
            Instance = this;
        }

        public AudioClip LoadAudio(string audioTheme, string audioName)
        {
            return Resources.Load<AudioClip>(audioPath + audioTheme + "/" + audioName);
        }

        public void PlaySoundPiece()
        {
            AudioSource.Play();
        }

        public void PlaySoundSucess()
        {
            AudioSource.PlayOneShot(audioSucess);
        }

        public void PlaySoundFail()
        {
            AudioSource.PlayOneShot(audioFail);
        }
    }
}
