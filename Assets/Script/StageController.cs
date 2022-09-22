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

    public void CallWave1()
    {
        Instantiate(_wave1Enemies, new Vector3(0, -2, 0), Quaternion.identity);
        FindObjectOfType<UIController>().WaveStartUIText(1);
    }

    public void CallWave2()
    {
        Instantiate(_wave2Enemies, new Vector3(0, -2, 0), Quaternion.identity);
        FindObjectOfType<UIController>().WaveStartUIText(2);
    }

    public void CallWaveBoss()
    {
        Instantiate(_waveBossEnemies, new Vector3(0, -2, 0), Quaternion.identity);
        FindObjectOfType<UIController>().WaveStartUIText(3);
    }
}
