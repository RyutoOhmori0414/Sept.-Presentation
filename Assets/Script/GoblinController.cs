using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GoblinController : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [Tooltip("���̃G�l�~�[��hp")]
    [SerializeField] float _hp = default;
    [Tooltip("���̃G�l�~�[�̍U����")]
    [SerializeField] float _attack = default;
    [Tooltip("�_���[�W��\�����邽�߂�TextMesh"), SerializeField]
    GameObject _damageText;
    [Tooltip("�U����H��������̒ʏ�G�t�F�N�g"), SerializeField]
    GameObject _HitEffect1;
    [Tooltip("�񕜎��̒ʏ�G�t�F�N�g"), SerializeField]
    GameObject _HealEffect1;
    [Tooltip("�������̃G�t�F�N�g"), SerializeField]
    GameObject _instanceDeathEffect1;
    [Tooltip("�G�l�~�[�����񂾂Ƃ��̃G�t�F�N�g"), SerializeField]
    GameObject _deathEffect;

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
        //����HP��茻�݂�HP�������Ȃ鎞���݂�HP������HP�Ɠ����ɂ���
        if (_currentHP > _hp)
        {
            _currentHP = _hp;
        }

        //�_���[�W��+-�ɂ�菈����ς���
        if (instanteDeath)
        {
            Debug.Log($"Enemy����������");
            Instantiate(_instanceDeathEffect1, this.transform.position, new Quaternion(0, 0, 0, 0));
            _currentHP = 0;
        }
        else if (damage > 0)
        {
            Debug.Log($"Enemy��{damage}�_���[�W�󂯂��I�I");
            Instantiate(_HitEffect1, this.transform.position, new Quaternion(0, 0, 0, 0));
            TextMeshPro DText = Instantiate(_damageText, this.transform). GetComponentInChildren<TextMeshPro>();
            DText.text = damage.ToString();
        }
        else
        {
            Debug.Log($"Enemy��{-damage}�񕜂���");
            Instantiate(_HealEffect1, this.transform.position, new Quaternion(0, 0, 0, 0));
            TextMeshPro DText = Instantiate(_damageText, this.transform).GetComponentInChildren<TextMeshPro>();
            DText.text = damage.ToString();
        }
        _hpSlider.DOValue(_currentHP / _hp, 0.5f);
        if (_currentHP <= 0)
        {
            Debug.Log($"{this.gameObject.name}�͎��ɂ܂���");
            
            Instantiate(_deathEffect, this.transform.position, new Quaternion(0, 0, 0, 0)).GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            Destroy(gameObject);

        }
    }
}
