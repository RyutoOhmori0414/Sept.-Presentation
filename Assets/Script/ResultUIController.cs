using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
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

    [Header("ボタン")]
    [Tooltip("タイトルに遷移するボタン"), SerializeField]
    GameObject _toTitleButton;
    Image _fadeImage;

    private void Start()
    {
        //スコアを算出するためにいろいろ取ってくる
        float wave = GameManager.TurnCount;
        float totalDamage = GameManager.TotalDamage;
        //スコア算出
        float score = (wave > 30 ? 0 : 30 - wave) * 1000 + totalDamage * 300;

        //フェードさせてActiveをfalseにしてる
        _fadeImage = _fadePanel.GetComponent<Image>();
        _fadePanel.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeImage.DOFade(0f, 1f).OnComplete(() => _fadePanel.SetActive(false)));
        //フェードさせたらスコアを表示させる
        seq.Append(_scoreTexts[0].DOText(wave.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //スコアにより評価を変える
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
        //評価をフェードさせる
        seq.Append(_scoreImage[0].DOFade(1f, 2f));
        //フェードさせたらスコアを表示させる
        seq.Append(_scoreTexts[1].DOText(totalDamage.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //スコアにより評価を変える
        seq.AppendCallback(() =>
        {
            if (400 > totalDamage)
            {
                _scoreImage[1].sprite = _scoreSprites[3];
            }
            else if (700 > totalDamage)
            {
                _scoreImage[1].sprite = _scoreSprites[2];
            }
            else if (1000 > totalDamage)
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
        seq.Append(_scoreTexts[2].DOText(score.ToString("0000000"), 2f, scrambleMode: ScrambleMode.Numerals));
        //スコアにより評価を変える
        seq.AppendCallback(() =>
        {
            if (200000 > score)
            {
                _scoreImage[2].sprite = _scoreSprites[3];
            }
            else if (300000 > score)
            {
                _scoreImage[2].sprite = _scoreSprites[2];
            }
            else if (400000 > score)
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
        seq.AppendCallback(() => _toTitleButton.SetActive(true));
    }

    public void ToTitleScene()
    {
        _fadePanel.SetActive(true);
        var panelImage = _fadePanel.GetComponent<Image>();
        panelImage.color = Color.clear;
        panelImage.DOFade(1f, 1f).OnComplete(() => SceneManager.LoadScene("TitleScene"));
    }
}
