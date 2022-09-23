using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[��HP")]
    [SerializeField] float _playerHP = default;
    [Header("�v���C���[��HP��\������X���C�_�[")]
    [SerializeField] Slider _playerHPSlider;
    [Header("�v���C���[�̍U����"), SerializeField] float _playerAttack = 20f;
    [Tooltip("�J������U��������X�N���v�g"), SerializeField]
    CinemachineImpulseSource _playerImpulseSource;

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
        //����HP������HP���傫���Ȃ�����HP�ɏ���HP��������
        if (_currentPlayerHP > _playerHP)
        {
            _currentPlayerHP = _playerHP;
        }

        //�G�t�F�N�g���_���[�W�ɂ���ĕύX���鏈��
        if (damege > 0)
        {
            Debug.Log($"Player��{damege}�_���[�W�󂯂�");
            _playerImpulseSource.GenerateImpulse();
        }
        else
        {
            Debug.Log($"Player��{-damege}�񕜂���");
            //�񕜃G�t�F�N�g
        }
        _playerHPSlider.DOValue(_currentPlayerHP / _playerHP, 1f);

        if (_currentPlayerHP < 0)
        {
            GetComponent<UIController>().Fade(1f, new Color (1f, 0f, 0f, 0f));
        }
    }
}
