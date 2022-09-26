using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneAudioController : MonoBehaviour
{
    [Tooltip("���ʉ��̃I�[�f�B�I�\�[�X"), SerializeField]
    AudioSource _seAudioSource;

    [Tooltip("�{�^����؂�ւ����Ƃ��̌��ʉ�"), SerializeField]
    AudioClip _seSelectClip;

    [Tooltip("�U������SE"), SerializeField]
    AudioClip _attackClip;

    [Tooltip("�񕜎���SE"), SerializeField]
    AudioClip _healClip;

    public void OnButtonSelectedSE()
    {
        _seAudioSource.clip = _seSelectClip;
        _seAudioSource.Play();
    }

    public void AttackSE()
    {
        _seAudioSource.clip = _attackClip;
        _seAudioSource.Play();
    }

    public void HealSE()
    {
        _seAudioSource.clip = _healClip;
        _seAudioSource.Play();
    }
}
