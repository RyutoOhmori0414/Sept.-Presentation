using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Tooltip("1Wave‚Ì“G"), SerializeField]
    GameObject _wave1Enemies= default;
    [Tooltip("2Wave‚Ì“G"), SerializeField]
    GameObject _wave2Enemies = default;
    [Tooltip("BossWave‚Ì“G"), SerializeField]
    GameObject _waveBossEnemies = default;

    void CallWave1()
    {
        Instantiate(_wave1Enemies, Vector3.zero, Quaternion.identity);
    }

    void CallWave2()
    {
        Instantiate(_wave2Enemies, Vector3.zero, Quaternion.identity);
    }

    void CallWaveBoss()
    {
        Instantiate(_waveBossEnemies, Vector3.zero, Quaternion.identity);
    }
}
