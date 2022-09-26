using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneAudioController : MonoBehaviour
{
    [Tooltip("効果音のオーディオソース"), SerializeField]
    AudioSource _seAudioSource;

    [Tooltip("ボタンを切り替えたときの効果音"), SerializeField]
    AudioClip _seSelectClip;

    [Tooltip("攻撃時のSE"), SerializeField]
    AudioClip _attackClip;

    [Tooltip("回復時のSE"), SerializeField]
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
