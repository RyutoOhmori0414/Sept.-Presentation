using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Tooltip("�v���C���[��HP")]
    [SerializeField] float _playerHP = default;
    [Tooltip("�v���C���[��HP��\������X���C�_�[")]
    [SerializeField] Slider _playerHPSlider;
    /// <summary>���݂�HP</summary>
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
