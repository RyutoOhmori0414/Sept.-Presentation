using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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

    [Header("GameOver���ɕ\������UI")]
    [Tooltip("�Ԃ��p�l��"), SerializeField]
    GameObject _redPanel;
    [Tooltip("GameOver�̃��S"), SerializeField]
    GameObject _imageLogo;
    [Tooltip("�^�C�g���ɖ߂邽�߂�Button"), SerializeField]
    GameObject _toTitleButton;

    [Tooltip("���ʉ��̂��߂̃I�[�f�B�I�\�[�X"), SerializeField]
    GameSceneAudioController _gameSceneAudioController;
    VibrationController _vibrationController;

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
        _vibrationController = GetComponent<VibrationController>();
        _currentPlayerHP = _playerHP;
    }

    private void Update()
    {
        _playerHPSlider.value = _currentPlayerHP / _playerHP;
    }

    public void PlayerHPUp(float upHP)
    {
        _playerHP += upHP;
        _playerHPSlider.DOValue(_currentPlayerHP / _playerHP, 1f);
    }

    public void NoEffectHPChange(float damage)
    {
        _currentPlayerHP -= damage;
        if (_currentPlayerHP > _playerHP)
        {
            _currentPlayerHP = _playerHP;
        }
        _playerHPSlider.DOValue(_currentPlayerHP / _playerHP, 1f);
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
            //���ʉ��Đ�
            _gameSceneAudioController.AttackSE();
            _vibrationController.StartVibration();
        }
        else
        {
            Debug.Log($"Player��{-damege}�񕜂���");
            //���ʉ��Đ�
            _gameSceneAudioController.HealSE();
        }
        _playerHPSlider.DOValue(_currentPlayerHP / _playerHP, 1f);

        if (_currentPlayerHP < 0)
        {
            GetComponent<UIController>().Fade(1f, new Color (1f, 0f, 0f, 0f),() => 
            {
                _redPanel.SetActive(true);
                _imageLogo.SetActive(true);
                _gameSceneAudioController.GameOverBGM();
                _imageLogo.GetComponent<Image>().DOFade(1f, 1f).OnComplete(() =>
                {
                    _toTitleButton.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(_toTitleButton);
                });
            });
        }
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
