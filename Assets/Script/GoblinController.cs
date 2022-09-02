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
    /// <summary>現在のhp</summary>
    float _currentHP = default;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentHP = _hp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseHP(int damage)
    {
        _currentHP -= damage;
        _hpSlider.DOValue(_currentHP / _hp, 0.5f);
        if (_currentHP <= 0)
        {
            Debug.Log($"{this.gameObject.name}は死にました");
        }
    }
}
