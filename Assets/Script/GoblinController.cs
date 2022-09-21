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

    public void DecreaseEnemyHP(float damage)
    {
        _currentHP -= damage;
        if (damage > 0)
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
            GetComponent<SpriteRenderer>().DOFade(0f, 3f).OnComplete(() => Destroy(this.gameObject));
        }
    }
}
