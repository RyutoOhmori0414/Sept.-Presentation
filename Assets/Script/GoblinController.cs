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

    private void OnEnable()
    {
        GameManager.Instance.OnEndTurn += _enemyAttack;
    }
    void Start()
    {
        _currentHP = _hp;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnEndTurn -= _enemyAttack;
    }

    public void DecreaseEnemyHP(int damage)
    {
        _currentHP -= damage;
        _hpSlider.DOValue(_currentHP / _hp, 0.5f);
        if (_currentHP <= 0)
        {
            Debug.Log($"{this.gameObject.name}は死にました");
            GetComponent<SpriteRenderer>().DOFade(0f, 3f).OnComplete(() => Destroy(this.gameObject));
        }
    }

    void _enemyAttack()
    {
        PlayerController.CurrentPlayerHP -= _attack;
    }
}
