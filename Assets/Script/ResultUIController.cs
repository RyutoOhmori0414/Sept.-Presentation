using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultUIController : MonoBehaviour
{
    [Header("フェード用のパネル")]
    [Tooltip("フェード用のパネル"), SerializeField]
    GameObject _fadePanel;

    [Header("テキスト")]
    [Tooltip("スコアの数値を表示するテキスト"), SerializeField]
    Text[] _scoreTexts;

    [Header("イメージ")]
    [Tooltip("評価を表示するイメージ"), SerializeField]
    Image[] _scoreImage;
    [Tooltip("表示するsprite"), SerializeField]
    Sprite[] _scoreSprites;

    Image _fadeImage;

    private void Start()
    {
        float testWave = 6969696;

        //フェードさせてActiveをfalseにしてる
        _fadeImage = _fadePanel.GetComponent<Image>();
        _fadePanel.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeImage.DOFade(0f, 1f).OnComplete(() => _fadePanel.SetActive(false)));
        //フェードさせたらスコアを表示させる
        seq.Append(_scoreTexts[0].DOText(testWave.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //スコアにより評価を変える
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
        //評価をフェードさせる
        seq.Append(_scoreImage[0].DOFade(1f, 2f));
        //フェードさせたらスコアを表示させる
        seq.Append(_scoreTexts[1].DOText(testWave.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //スコアにより評価を変える
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
        //評価をフェードさせる
        seq.Append(_scoreImage[1].DOFade(1f, 2f));
        //フェードさせたらスコアを表示させる
        seq.Append(_scoreTexts[2].DOText(testWave.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //スコアにより評価を変える
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
        //評価をフェードさせる
        seq.Append(_scoreImage[2].DOFade(1f, 2f));

    }
}
