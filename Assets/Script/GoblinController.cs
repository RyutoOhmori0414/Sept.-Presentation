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
    public float Attack
    {
        get => _attack;
    }

    /// <summary>���݂�hp</summary>
    float _currentHP = default;
    public float CurrentEnemyHP
    {
        get => _currentHP;
    }
    /// <summary>���݂̍U����</summary>
    float _currentAttack = default;
    /// <summary>�S�u�����̍U���͂ɂ�����{���������Ă�</summary>
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
        Debug.Log($"{damage}�_���[�W�^�����I�I");
        _hpSlider.DOValue(_currentHP / _hp, 0.5f);
        if (_currentHP <= 0)
        {
            Debug.Log($"{this.gameObject.name}�͎��ɂ܂���");
            GetComponent<SpriteRenderer>().DOFade(0f, 3f).OnComplete(() => Destroy(this.gameObject));
        }
    }
}
