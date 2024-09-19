using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private Slider _slider;

    public void OnChangeSlider(float value) {
        _source.volume = value;
        _slider.value = value;
    }
}
