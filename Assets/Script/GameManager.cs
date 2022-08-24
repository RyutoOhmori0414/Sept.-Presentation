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

    private void Awake()
    {
        CheckInstance();
    }

    public void BeginTurn()
    {
        OnBeginTurn();
    }

    public void EndTurn()
    {
        OnEndTurn();
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
