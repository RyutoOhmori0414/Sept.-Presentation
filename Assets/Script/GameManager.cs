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
    int _turnCount = 0;
    /// <summary>�X�e�[�W�R���g���[��</summary>
    StageController _stageController;
    /// <summary>Wave�̃J�E���^�[</summary>
    int _waveCount;
    
    public int TurnCount
    { get => _turnCount; }

    private void Awake()
    {
        CheckInstance();
    }

    private void Start()
    {
        _stageController = GetComponent<StageController>();
        _waveCount = 1;
        _stageController.CallWave1();
        BeginTurn();
    }

    public void BeginTurn()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy") == null && _waveCount == 1)
        {
            _stageController.CallWave2();
            _waveCount++;
        }
        if (GameObject.FindGameObjectsWithTag("Enemy") == null&& _waveCount == 2)
        {
            _stageController.CallWaveBoss();
            _waveCount++;
        }
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
