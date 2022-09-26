using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAudioController : MonoBehaviour
{
    [Tooltip("使用するSEオーディオソース"), SerializeField]
    AudioSource _seAudioSource;

    [Tooltip("使用するBGMオーディオソース"), SerializeField]
    AudioSource _bgmAudioSource;

    [Tooltip("Selectが切り替わったときの音"), SerializeField]
    AudioClip _chooseSE;

    [Tooltip("決定音"), SerializeField]
    AudioClip _selectSE;

    private void Update()
    {
        _seAudioSource.volume = TitleUIController.Volume;
        _bgmAudioSource.volume = TitleUIController.Volume / 4f;
    }

    public void ChooseSEPlay(bool choose = false)
    {
        if (choose)
        {
            _seAudioSource.clip = _chooseSE;
        }
        else
        {
            _seAudioSource.clip = _selectSE;
        }
        _seAudioSource.Play();
    }
}
