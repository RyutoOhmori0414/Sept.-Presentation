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
        float testWave = 6969696;

        //�t�F�[�h������Active��false�ɂ��Ă�
        _fadeImage = _fadePanel.GetComponent<Image>();
        _fadePanel.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeImage.DOFade(0f, 1f).OnComplete(() => _fadePanel.SetActive(false)));
        //�t�F�[�h��������X�R�A��\��������
        seq.Append(_scoreTexts[0].DOText(testWave.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //�X�R�A�ɂ��]����ς���
        seq.AppendCallback(() =>
        {
            if (30 < testWave)
            {
                _scoreImage[0].sprite = _scoreSprites[3];
            }
            else if (20 < testWave)
            {
                _scoreImage[0].sprite = _scoreSprites[2];
            }
            else if (10 < testWave)
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
        seq.Append(_scoreTexts[1].DOText(testWave.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //�X�R�A�ɂ��]����ς���
        seq.AppendCallback(() =>
        {
            if (30 < testWave)
            {
                _scoreImage[1].sprite = _scoreSprites[3];
            }
            else if (20 < testWave)
            {
                _scoreImage[1].sprite = _scoreSprites[2];
            }
            else if (10 < testWave)
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
        seq.Append(_scoreTexts[2].DOText(testWave.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //�X�R�A�ɂ��]����ς���
        seq.AppendCallback(() =>
        {
            if (30 < testWave)
            {
                _scoreImage[2].sprite = _scoreSprites[3];
            }
            else if (20 < testWave)
            {
                _scoreImage[2].sprite = _scoreSprites[2];
            }
            else if (10 < testWave)
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
