using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Tooltip("1Wave�̓G"), SerializeField]
    GameObject[] _wave1Enemies= default;
    [Tooltip("2Wave�̓G"), SerializeField]
    GameObject[] _wave2Enemies = default;
    [Tooltip("BossWave�̓G"), SerializeField]
    GameObject[] _waveBossEnemies = default;

    void CallWave1()
    {

    }

    void CallWave2()
    {

    }

    void CallWaveBoss()
    {

    }
}
