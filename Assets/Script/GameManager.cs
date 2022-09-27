using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    /// <summary>ターンが始まったときに行う</summary>
    public event Action OnBeginTurn;
    /// <summary>ターンが終わったときに行う</summary>
    //public event Action OnEndTurn;
    /// <summary>ターンのカウント</summary>
    static int _turnCount = 0;
    /// <summary>経過したターン数</summary>
    public static int TurnCount
    {
        get => _turnCount;
        set => _turnCount = value;
    }
    /// <summary>与えたダメージの合計</summary>
    static float _totalDamage;
    /// <summary>与えたダメージの合計</summary>
    public static float TotalDamage
    {
        get => _totalDamage;
        set => _totalDamage = value;
    }
    /// <summary>ステージコントローラ</summary>
    StageController _stageController;
    /// <summary>Waveのカウンター</summary>
    int _waveCount;
    /// <summary>trueになった場合次のWaveを呼ぶ</summary>
    public bool _callWave = false;
    

    private void Start()
    {
        _totalDamage = 0;
        _callWave = false;
        _stageController = GetComponent<StageController>();
        _waveCount = 1;
        _stageController.CallWave1();
        GetComponent<UIController>().Fade(0f, Color.black, () => BeginTurn());
    }

    public void BeginTurn()
    {
        _turnCount++;
        OnBeginTurn();
        Debug.Log($"{_turnCount}ターン目開始");
        if (_callWave && _waveCount == 1)
        {
            _stageController.CallWave2();
            _waveCount++;
            _callWave = false;
        }
        else if (_callWave && _waveCount == 2)
        {
            _stageController.CallWaveBoss();
            _waveCount++;
            _callWave = false;
        }
    }

    public void EndTurn()
    {
        //OnEndTurn();
        Debug.Log($"{_turnCount}ターン目終了");
        Invoke("BeginTurn", 3f);
    }
}
