using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GoblinController : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [Tooltip("このエネミーのhp")]
    [SerializeField] float _hp = default;
    [Tooltip("このエネミーの攻撃力")]
    [SerializeField] float _attack = default;
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
        set => _currentAttack = _currentAttack * value;
    }

    PlayerController _playerController;

    void Start()
    {
        _currentHP = _hp;
        _currentAttack = _attack;
        _playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    public void DecreaseEnemyHP(float damage)
    {
        _currentHP -= damage;
        Debug.Log($"{damage}ダメージ与えた！！");
        _hpSlider.DOValue(_currentHP / _hp, 0.5f);
        if (_currentHP <= 0)
        {
            Debug.Log($"{this.gameObject.name}は死にました");
            GetComponent<SpriteRenderer>().DOFade(0f, 3f).OnComplete(() => Destroy(this.gameObject));
        }
    }
}
