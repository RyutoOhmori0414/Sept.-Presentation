using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GoblinController : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [Tooltip("このエネミーのhp")]
    [SerializeField] float _hp = default;
    [Tooltip("このエネミーの攻撃力")]
    [SerializeField] float _attack = default;
    [Tooltip("ダメージを表示するためのTextMesh"), SerializeField]
    GameObject _damageText;
    [Tooltip("攻撃を食らった時の通常エフェクト"), SerializeField]
    GameObject _HitEffect1;
    [Tooltip("回復時の通常エフェクト"), SerializeField]
    GameObject _HealEffect1;
    [Tooltip("即死時のエフェクト"), SerializeField]
    GameObject _instanceDeathEffect1;
    [Tooltip("エネミーが死んだときのエフェクト"), SerializeField]
    GameObject _deathEffect;

    public float Attack
    {
        get => _attack;
    }

    /// <summary>現在のhp</summary>
    float _currentHP = default;
    public float CurrentEnemyHP
    {
        get => _currentHP;
    }
    /// <summary>現在の攻撃力</summary>
    float _currentAttack = default;
    /// <summary>ゴブリンの攻撃力にかける倍率を代入してね</summary>
    public float CurrentAttack
    {
        set => _currentAttack = value;
    }

    PlayerController _playerController;

    void Start()
    {
        _currentHP = _hp;
        _currentAttack = _attack;
        _playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    public void DecreaseEnemyHP(float damage, bool instanteDeath = false)
    {
        GetComponentInChildren<Button>().interactable = false;
        _currentHP -= damage;
        //初期HPより現在のHPが多くなる時現在のHPを初期HPと同じにする
        if (_currentHP > _hp)
        {
            _currentHP = _hp;
        }

        //ダメージの+-により処理を変える
        if (instanteDeath)
        {
            Debug.Log($"Enemyが即死した");
            Instantiate(_instanceDeathEffect1, this.transform.position, new Quaternion(0, 0, 0, 0));
            _currentHP = 0;
        }
        else if (damage > 0)
        {
            Debug.Log($"Enemyは{damage}ダメージ受けた！！");
            Instantiate(_HitEffect1, this.transform.position, new Quaternion(0, 0, 0, 0));
            TextMeshPro DText = Instantiate(_damageText, this.transform). GetComponentInChildren<TextMeshPro>();
            DText.text = damage.ToString();
        }
        else
        {
            Debug.Log($"Enemyは{-damage}回復した");
            Instantiate(_HealEffect1, this.transform.position, new Quaternion(0, 0, 0, 0));
            TextMeshPro DText = Instantiate(_damageText, this.transform).GetComponentInChildren<TextMeshPro>();
            DText.text = damage.ToString();
        }
        _hpSlider.DOValue(_currentHP / _hp, 0.5f);
        if (_currentHP <= 0)
        {
            Debug.Log($"{this.gameObject.name}は死にました");
            
            Instantiate(_deathEffect, this.transform.position, new Quaternion(0, 0, 0, 0)).GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            Destroy(gameObject);

        }
    }
}
