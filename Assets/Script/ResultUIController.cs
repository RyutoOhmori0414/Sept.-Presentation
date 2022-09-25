using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultUIController : MonoBehaviour
{
    [Header("�t�F�[�h�p�̃p�l��")]
    [Tooltip("�t�F�[�h�p�̃p�l��"), SerializeField]
    GameObject _fadePanel;

    [Header("�e�L�X�g")]
    [Tooltip("�X�R�A�̐��l��\������e�L�X�g"), SerializeField]
    Text[] _scoreTexts;

    [Header("�C���[�W")]
    [Tooltip("�]����\������C���[�W"), SerializeField]
    Image[] _scoreImage;
    [Tooltip("�\������sprite"), SerializeField]
    Sprite[] _scoreSprites;

    Image _fadeImage;

    private void Start()
    {
        //�X�R�A���Z�o���邽�߂ɂ��낢�����Ă���
        float wave = GameManager.TurnCount;
        float totalDamage = GameManager.TotalDamage;
        //�X�R�A�Z�o
        float score = (wave > 30 ? 0 : 30 - wave) * 1000 + totalDamage * 100;

        //�t�F�[�h������Active��false�ɂ��Ă�
        _fadeImage = _fadePanel.GetComponent<Image>();
        _fadePanel.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeImage.DOFade(0f, 1f).OnComplete(() => _fadePanel.SetActive(false)));
        //�t�F�[�h��������X�R�A��\��������
        seq.Append(_scoreTexts[0].DOText(wave.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //�X�R�A�ɂ��]����ς���
        seq.AppendCallback(() =>
        {
            if (30 < wave)
            {
                _scoreImage[0].sprite = _scoreSprites[3];
            }
            else if (20 < wave)
            {
                _scoreImage[0].sprite = _scoreSprites[2];
            }
            else if (10 < wave)
            {
                _scoreImage[0].sprite = _scoreSprites[1];
            }
            else
            {
                _scoreImage[0].sprite = _scoreSprites[0];
            }
        });
        //�]�����t�F�[�h������
        seq.Append(_scoreImage[0].DOFade(1f, 2f));
        //�t�F�[�h��������X�R�A��\��������
        seq.Append(_scoreTexts[1].DOText(totalDamage.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //�X�R�A�ɂ��]����ς���
        seq.AppendCallback(() =>
        {
            if (1000 > totalDamage)
            {
                _scoreImage[1].sprite = _scoreSprites[3];
            }
            else if (2000 > totalDamage)
            {
                _scoreImage[1].sprite = _scoreSprites[2];
            }
            else if (3000 > totalDamage)
            {
                _scoreImage[1].sprite = _scoreSprites[1];
            }
            else
            {
                _scoreImage[1].sprite = _scoreSprites[0];
            }
        });
        //�]�����t�F�[�h������
        seq.Append(_scoreImage[1].DOFade(1f, 2f));
        //�t�F�[�h��������X�R�A��\��������
        seq.Append(_scoreTexts[2].DOText(score.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //�X�R�A�ɂ��]����ς���
        seq.AppendCallback(() =>
        {
            if (20000 > score)
            {
                _scoreImage[2].sprite = _scoreSprites[3];
            }
            else if (20000 > score)
            {
                _scoreImage[2].sprite = _scoreSprites[2];
            }
            else if (30000 > score)
            {
                _scoreImage[2].sprite = _scoreSprites[1];
            }
            else
            {
                _scoreImage[2].sprite = _scoreSprites[0];
            }
        });
        //�]�����t�F�[�h������
        seq.Append(_scoreImage[2].DOFade(1f, 2f));

    }
}
