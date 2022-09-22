using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーのHP")]
    [SerializeField] float _playerHP = default;
    [Header("プレイヤーのHPを表示するスライダー")]
    [SerializeField] Slider _playerHPSlider;
    [Header("プレイヤーの攻撃力"), SerializeField] float _playerAttack = 20f;
    public float PlayerAttack
    {
        get => _playerAttack;
    }

    /// <summary>現在のHP</summary>
    float _currentPlayerHP = default;
    public float CurrentPlayerHP
    {
        get => _currentPlayerHP;
    }

    private void Start()
    {
        _currentPlayerHP = _playerHP;
    }

    private void Update()
    {
        _playerHPSlider.value = _currentPlayerHP / _playerHP;
    }

    public void PlayerDamage(float damege)
    {
        _currentPlayerHP -= damege;
        //もしHPが初期HPより大きくなったらHPに初期HPを代入する
        if (_currentPlayerHP > _playerHP)
        {
            _currentPlayerHP = _playerHP;
        }

        //エフェクトをダメージによって変更する処理
        if (damege > 0)
        {
            Debug.Log($"Playerは{damege}ダメージ受けた");
            //被弾エフェクト
        }
        else
        {
            Debug.Log($"Playerは{-damege}回復した");
            //回復エフェクト
        }
        _playerHPSlider.DOValue(_currentPlayerHP / _playerHP, 1f);
    }
}
