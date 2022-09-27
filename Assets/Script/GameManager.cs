using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    /// <summary>�^�[�����n�܂����Ƃ��ɍs��</summary>
    public event Action OnBeginTurn;
    /// <summary>�^�[�����I������Ƃ��ɍs��</summary>
    //public event Action OnEndTurn;
    /// <summary>�^�[���̃J�E���g</summary>
    static int _turnCount = 0;
    /// <summary>�o�߂����^�[����</summary>
    public static int TurnCount
    {
        get => _turnCount;
        set => _turnCount = value;
    }
    /// <summary>�^�����_���[�W�̍��v</summary>
    static float _totalDamage;
    /// <summary>�^�����_���[�W�̍��v</summary>
    public static float TotalDamage
    {
        get => _totalDamage;
        set => _totalDamage = value;
    }
    /// <summary>�X�e�[�W�R���g���[��</summary>
    StageController _stageController;
    /// <summary>Wave�̃J�E���^�[</summary>
    int _waveCount;
    /// <summary>true�ɂȂ����ꍇ����Wave���Ă�</summary>
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
        Debug.Log($"{_turnCount}�^�[���ڊJ�n");
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
        Debug.Log($"{_turnCount}�^�[���ڏI��");
        Invoke("BeginTurn", 3f);
    }
}
