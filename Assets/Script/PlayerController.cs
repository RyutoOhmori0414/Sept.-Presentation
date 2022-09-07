using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーのHP")]
    [SerializeField] float _playerHP = default;
    [Header("プレイヤーのHPを表示するスライダー")]
    [SerializeField] Slider _playerHPSlider;
    [Header("プレイヤーの攻撃力"), SerializeField] static float _playerAttack = 20f;
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
        if (damege > 0)
        {
            Debug.Log($"{damege}ダメージ食らった");
        }
        else
        {
            Debug.Log($"{-damege}回復した");
        }
    }
}
