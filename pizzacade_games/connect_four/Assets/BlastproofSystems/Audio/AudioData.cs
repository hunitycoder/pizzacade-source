using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;


namespace Blastproof.Systems.Audio
{
    [Serializable]
    public class ClipData
    {
        public AudioClip clip;
        public bool hasAudioSource;
        public AudioSource audioSource;
        public AudioMixerGroup MixerGroup;
        public bool IsLooping;
        [MinMaxSlider(0, 1, true), SerializeField] private Vector2 VolumeRange = Vector2.one;

        public float Volume => Random.Range(VolumeRange.x, VolumeRange.y);
    }

    [Serializable]
    public class Sounds
    {
        public ClipData[] clips;
    }

    [CreateAssetMenu(menuName = "Blastproof/Systems/Audio/AudioData")]
    public class AudioData : ScriptableObject
    {
        [BoxGroup("Music", true, true)] public Sounds MusicIntro;
        [BoxGroup("Music", true, true)] public Sounds MusicLoop;
        [BoxGroup("SFX", true, true)] public Sounds SelectBall;
        [BoxGroup("SFX", true, true)] public Sounds DeselectBall;
        [BoxGroup("SFX", true, true)] public Sounds Win;
        [BoxGroup("SFX", true, true)] public Sounds Lose;

        [BoxGroup("SFX", true, true)] public Sounds ButtonClick;
    }
}