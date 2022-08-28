using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));
                if (_instance == null)
                {
                    Debug.LogError("GameManagerをアタッチしているGameObjectがありません");
                }
            }
            return _instance;
        }
    }
    /// <summary>ターンが始まったときに行う</summary>
    public event Action OnBeginTurn;
    /// <summary>ターンが終わったときに行う</summary>
    public event Action OnEndTurn;
    /// <summary>ターンのカウント</summary>
    int _turnCount = 1;

    private void Awake()
    {
        CheckInstance();
    }

    public void BeginTurn()
    {
        _turnCount++;
        OnBeginTurn();
        Debug.Log($"{_turnCount}ターン目開始");
    }

    public void EndTurn()
    {
        OnEndTurn();
        Debug.Log($"{_turnCount}ターン目終了");
        Invoke("BeginTurn", 3f);
    }
    
    void CheckInstance()
    {
        if (_instance == null)
        {
            _instance = this as GameManager;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }
}
