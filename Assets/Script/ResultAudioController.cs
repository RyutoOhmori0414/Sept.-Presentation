using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultAudioController : MonoBehaviour
{
    [Tooltip("BGMのオーディオソース"), SerializeField]
    AudioSource _bgmAudioSource;
    void Start()
    {
        _bgmAudioSource.volume = TitleUIController.Volume / 4f;
    }
}
