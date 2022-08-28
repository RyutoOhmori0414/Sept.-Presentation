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
                    Debug.LogError("GameManager���A�^�b�`���Ă���GameObject������܂���");
                }
            }
            return _instance;
        }
    }
    /// <summary>�^�[�����n�܂����Ƃ��ɍs��</summary>
    public event Action OnBeginTurn;
    /// <summary>�^�[�����I������Ƃ��ɍs��</summary>
    public event Action OnEndTurn;
    /// <summary>�^�[���̃J�E���g</summary>
    int _turnCount = 1;

    private void Awake()
    {
        CheckInstance();
    }

    public void BeginTurn()
    {
        _turnCount++;
        OnBeginTurn();
        Debug.Log($"{_turnCount}�^�[���ڊJ�n");
    }

    public void EndTurn()
    {
        OnEndTurn();
        Debug.Log($"{_turnCount}�^�[���ڏI��");
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
