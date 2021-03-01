using System.Collections;
using Blastproof.Systems.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Blastproof.Systems.Audio
{
    [CreateAssetMenu(menuName = "Blastproof/Systems/Audio/AudioSystem")]
    public class AudioSystem : SerializedScriptableObject
    {
        [BoxGroup("References"), Required, AssetsOnly] public AudioData audioData;
        [BoxGroup("References"), Required, SerializeField] private AudioSource sourcePrefab;
        [BoxGroup("References"), Required, SerializeField] private AudioMixer audioMixer;

        //private int _isTurnOn;
        
        private AudioSourcePlayer _audioSources;
        
        //[BoxGroup("Info"), ShowInInspector] public bool IsTurnOn => _isTurnOn == 1;
        
        public AudioSourcePlayer AudioSources
        {
            get
            {
                if (_audioSources == null)
                    _audioSources = FindObjectOfType<AudioSourcePlayer>();
                return _audioSources;
            }
        }

        //private void Awake()
        //{
        //    Initialize();
        //}

        //private void Initialize()
        //{
        //    _isTurnOn = UnityEngine.PlayerPrefs.GetInt("AUDIO", 1);
        //    audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80f, 0f, 1f));
        //}

        //public void TurnOn()
        //{
        //    _isTurnOn = 1;
        //    UnityEngine.PlayerPrefs.SetInt("AUDIO", 1);
        //    audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80f, 0f, 1f));
        //}

        //public void TurnOff()
        //{
        //    _isTurnOn = 0;
        //    UnityEngine.PlayerPrefs.SetInt("AUDIO", 0);
        //    audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80f, 0f, 0f));
        //}

        public void PlaySoundData(Sounds data)
        {
            //if (_isTurnOn == 0) return;
            
            var clip = data.clips.Random();
            PlayClip(clip);
        }

        private void PlayClip(ClipData clip)
        {
            AudioSources.StartCoroutine(PlayClipRoutine(clip));
        }

        private IEnumerator PlayClipRoutine(ClipData clip)
        {
            if (clip.hasAudioSource)
            {
                if (clip.audioSource == null)
                {
                    clip.audioSource = Instantiate(sourcePrefab, _audioSources.transform);
                }
                clip.audioSource.clip = clip.clip;
                clip.audioSource.outputAudioMixerGroup = clip.MixerGroup;
                clip.audioSource.Play();
            }
            else
            {
                var newSource = Instantiate(sourcePrefab, _audioSources.transform);
                newSource.clip = clip.clip;
                newSource.outputAudioMixerGroup = clip.MixerGroup;
                newSource.loop = clip.IsLooping;
                newSource.Play();
                if(!clip.IsLooping)
                {
                    yield return new WaitForSeconds(clip.clip.length);
                    Destroy(newSource.gameObject);
                }
            }
        }
    }
}