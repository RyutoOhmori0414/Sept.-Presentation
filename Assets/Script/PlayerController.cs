using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Tooltip("プレイヤーのHP")]
    [SerializeField] float _playerHP = default;
    [Tooltip("プレイヤーのHPを表示するスライダー")]
    [SerializeField] Slider _playerHPSlider;
    /// <summary>現在のHP</summary>
    static float _currentPlayerHP = default;
    public static float CurrentPlayerHP
    {
        get => _currentPlayerHP;
        set => _currentPlayerHP = value;
    }

    private void Start()
    {
        _currentPlayerHP = _playerHP;
    }

    private void Update()
    {
        _playerHPSlider.value = _currentPlayerHP / _playerHP;
    }
}
