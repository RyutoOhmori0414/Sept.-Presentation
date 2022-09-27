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
    [Header("プレイヤーのHP")]
    [SerializeField] float _playerHP = default;
    [Header("プレイヤーのHPを表示するスライダー")]
    [SerializeField] Slider _playerHPSlider;
    [Header("プレイヤーの攻撃力"), SerializeField] float _playerAttack = 20f;
    [Tooltip("カメラを振動させるスクリプト"), SerializeField]
    CinemachineImpulseSource _playerImpulseSource;

    [Header("GameOver時に表示するUI")]
    [Tooltip("赤いパネル"), SerializeField]
    GameObject _redPanel;
    [Tooltip("GameOverのロゴ"), SerializeField]
    GameObject _imageLogo;
    [Tooltip("タイトルに戻るためのButton"), SerializeField]
    GameObject _toTitleButton;

    [Tooltip("効果音のためのオーディオソース"), SerializeField]
    GameSceneAudioController _gameSceneAudioController;
    VibrationController _vibrationController;

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
        //もしHPが初期HPより大きくなったらHPに初期HPを代入する
        if (_currentPlayerHP > _playerHP)
        {
            _currentPlayerHP = _playerHP;
        }

        //エフェクトをダメージによって変更する処理
        if (damege > 0)
        {
            Debug.Log($"Playerは{damege}ダメージ受けた");
            _playerImpulseSource.GenerateImpulse();
            //効果音再生
            _gameSceneAudioController.AttackSE();
            _vibrationController.StartVibration();
        }
        else
        {
            Debug.Log($"Playerは{-damege}回復した");
            //効果音再生
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
