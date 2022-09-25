using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEffectController : MonoBehaviour
{
    /// <summary>�G���U�����Ȃ��Ƃ�</summary>
    static bool _noEnemyAttack = false;
    /// <summary>�G���U�����Ȃ��Ƃ��ɓG�̍U�����X�L�b�v</summary>
    public static bool NoEnemyAttack
    { 
        set => _noEnemyAttack = value;
    }

    private void Start()
    {
        _noEnemyAttack = false;
    }
    public void EnemyAttackText()
    {
        if (!_noEnemyAttack)
        {
            Text textUI = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
            textUI.text = "�G�̍U��";
            Sequence seq = DOTween.Sequence();
            seq.Append(textUI.DOFade(1f, 0.3f));
            seq.AppendInterval(0.5f);
            seq.Append(textUI.DOFade(0f, 0.3f).OnComplete(() => FindObjectOfType<UIController>().EffectEnd()));
        }
        Destroy(gameObject);
        _noEnemyAttack = false;
    }

    public void EffectDestroy()
    {
        Destroy(gameObject);
    }
}
