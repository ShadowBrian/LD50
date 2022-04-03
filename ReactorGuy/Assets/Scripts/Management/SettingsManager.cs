using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static System.Action<float> OnSensitivityChange;
    public static System.Action<float> OnVolumeChange;

    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider audioSlider;

    private void Awake()
    {
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        audioSlider.onValueChanged.RemoveAllListeners();
        sensitivitySlider.onValueChanged.AddListener(SetNewSensitivity);
        audioSlider.onValueChanged.AddListener(SetNewAudio);
    }


    private void SetNewSensitivity(float sensitivity)
    {
        OnSensitivityChange?.Invoke(sensitivity);
    }
    private void SetNewAudio(float volume)
    {
        OnVolumeChange?.Invoke(volume);
    }
}
