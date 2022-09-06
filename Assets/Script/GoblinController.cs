using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GoblinController : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [Tooltip("���̃G�l�~�[��hp")]
    [SerializeField] float _hp = default;
    [Tooltip("���̃G�l�~�[�̍U����")]
    [SerializeField] float _attack = default;
    /// <summary>���݂�hp</summary>
    float _currentHP = default;
    /// <summary>���݂̍U����</summary>
    public static float _currentAttack = default;
    /// <summary>�S�u�����̍U���͂ɂ�����{���������Ă�</summary>
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
        Debug.Log($"{damage}�_���[�W�^�����I�I");
        _hpSlider.DOValue(_currentHP / _hp, 0.5f);
        if (_currentHP <= 0)
        {
            Debug.Log($"{this.gameObject.name}�͎��ɂ܂���");
            GetComponent<SpriteRenderer>().DOFade(0f, 3f).OnComplete(() => Destroy(this.gameObject));
        }
    }

    void _enemyAttack()
    {
        PlayerController.PlayerDamage(_currentAttack);
        _currentAttack = _attack;
    }
}
