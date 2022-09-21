using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[��HP")]
    [SerializeField] float _playerHP = default;
    [Header("�v���C���[��HP��\������X���C�_�[")]
    [SerializeField] Slider _playerHPSlider;
    [Header("�v���C���[�̍U����"), SerializeField] float _playerAttack = 20f;
    public float PlayerAttack
    {
        get => _playerAttack;
    }

    /// <summary>���݂�HP</summary>
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
            Debug.Log($"Player��{damege}�_���[�W�󂯂�");
        }
        else
        {
            Debug.Log($"Player��{-damege}�񕜂���");
        }
        _playerHPSlider.DOValue(_currentPlayerHP / _playerHP, 1f);
    }
}
