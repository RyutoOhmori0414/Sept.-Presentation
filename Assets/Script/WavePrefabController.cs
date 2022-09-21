using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePrefabController : MonoBehaviour
{
    /// <summary>現在のエネミーの数</summary>
    int _enemyCount = default;

    private void OnEnable()
    {
        GameManager.Instance.OnBeginTurn += GetWaveChildCount;
    }

    private void Start()
    {
        _enemyCount = transform.childCount;

    }

    public void GetWaveChildCount()
    {
        _enemyCount = transform.childCount;
        if (_enemyCount <= 0)
        {
            GameManager.Instance._callWave = true;
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.OnBeginTurn -= GetWaveChildCount;
    }
}
