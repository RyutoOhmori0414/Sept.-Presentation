using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePrefabController : MonoBehaviour
{
    GameManager _gameManager;
    private void Update()
    {
        if (transform.childCount <= 0)
        {
            EnemyEffectController.NoEnemyAttack = true;
        }
    }

    private void OnEnable()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.OnBeginTurn += GetWaveChildCount;
    }

    public void GetWaveChildCount()
    {
        if (transform.childCount <= 0)
        {
            _gameManager._callWave = true;
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        _gameManager.OnBeginTurn -= GetWaveChildCount;
    }
}
