using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Blastproof/Systems/Audio/MixerVolumeControl")]
public class MixerVolumeControl : ScriptableObject
{
    public AudioMixer MainMixer;
    public string MixerValueToControl;
    public PlayerPrefsInt Trigger;
    public float LowerLimit = -80f;
    public float UpperLimit = 0f;

    private void OnEnable()
    {
        Trigger.onValueChanged += TriggerChanged;
    }

    void TriggerChanged()
    {
        MainMixer.SetFloat(MixerValueToControl, Mathf.Lerp(LowerLimit, UpperLimit, Trigger.Value));
    }

    private void OnDisable()
    {
        Trigger.onValueChanged -= TriggerChanged;
    }
}
