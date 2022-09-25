using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAudioController : MonoBehaviour
{
    [Tooltip("�g�p����I�[�f�B�I�\�[�X"), SerializeField]
    AudioSource _seAudioSource;

    [Tooltip("Select���؂�ւ�����Ƃ��̉�"), SerializeField]
    AudioClip _chooseSE;

    [Tooltip("���艹"), SerializeField]
    AudioClip _selectSE;

    public void ChooseSEPlay()
    {
        _seAudioSource.clip = _chooseSE;
        _seAudioSource.Play();
    }

    public void SelectSEPlay()
    {
        _seAudioSource.clip = _selectSE;
        _seAudioSource.Play();
    }
}
