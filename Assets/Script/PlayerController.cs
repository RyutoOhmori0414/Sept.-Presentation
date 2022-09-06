using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[��HP")]
    [SerializeField] float _playerHP = default;
    [Header("�v���C���[��HP��\������X���C�_�[")]
    [SerializeField] Slider _playerHPSlider;
    [Header("�v���C���[�̍U����"), SerializeField] static float _playerAttack = 20f;


    public static float PlayerAttack
    {
        get => _playerAttack;
    }
    /// <summary>���݂�HP</summary>
    static float _currentPlayerHP = default;

    private void Start()
    {
        _currentPlayerHP = _playerHP;
    }

    private void Update()
    {
        _playerHPSlider.value = _currentPlayerHP / _playerHP;
    }

    public static void PlayerDamage(float damege)
    {
        _currentPlayerHP -= damege;
        Debug.Log($"{damege}�_���[�W�H�����");
    }
}
