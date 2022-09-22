using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEffectController : MonoBehaviour
{

    public void EnemyAttackText()
    {
        Text textUI = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        textUI.text = "ìGÇÃçUåÇ";
        Sequence seq = DOTween.Sequence();
        seq.Append(textUI.DOFade(1f, 0.3f));
        seq.AppendInterval(0.5f);
        seq.Append(textUI.DOFade(0f, 0.3f));
    }

    public void EffectDestroy()
    {
        Destroy(gameObject);
    }

    public void CallEffectEnd()
    {
        FindObjectOfType<UIController>().EffectEnd();
        Destroy(gameObject);
    }
}
