using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAudioController : MonoBehaviour
{
    [Tooltip("使用するオーディオソース"), SerializeField]
    AudioSource _seAudioSource;

    [Tooltip("Selectが切り替わったときの音"), SerializeField]
    AudioClip _chooseSE;

    [Tooltip("決定音"), SerializeField]
    AudioClip _selectSE;

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
