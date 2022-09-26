using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneAudioController : MonoBehaviour
{
    [Tooltip("���ʉ��̃I�[�f�B�I�\�[�X"), SerializeField]
    AudioSource _seAudioSource;

    [Tooltip("BGM�̃I�[�f�B�I�\�[�X"), SerializeField]
    AudioSource _bgmAudioSource;

    [Tooltip("�{�^����؂�ւ����Ƃ��̌��ʉ�"), SerializeField]
    AudioClip _seSelectClip;

    [Tooltip("�U������SE"), SerializeField]
    AudioClip _attackClip;

    [Tooltip("�񕜎���SE"), SerializeField]
    AudioClip _healClip;

    [Tooltip("GameOver����BGM"), SerializeField]
    AudioClip _gameOverClip;

    private void Start()
    {
        _seAudioSource.volume = TitleUIController.Volume;
        _bgmAudioSource.volume = TitleUIController.Volume / 4f;
    }
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

    public void GameOverBGM()
    {
        _bgmAudioSource.clip = _gameOverClip;
        _bgmAudioSource.Play();
    }
}
