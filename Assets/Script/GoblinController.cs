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
    /// <summary>現在のhp</summary>
    float _currentHP = default;
    /// <summary>現在の攻撃力</summary>
    public static float _currentAttack = default;
    /// <summary>ゴブリンの攻撃力にかける倍率を代入してね</summary>
    public static float CurrentAttack
    {
        set => _currentAttack = _currentAttack * value;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnEndTurn += _enemyAttack;
    }
    void Start()
    {
        _currentHP = _hp;
        _currentAttack = _attack;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnEndTurn -= _enemyAttack;
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

    void _enemyAttack()
    {
        PlayerController.PlayerDamage(_currentAttack);
        _currentAttack = _attack;
    }
}
