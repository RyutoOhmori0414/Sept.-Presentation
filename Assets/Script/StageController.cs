using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Tooltip("1Wave‚Ì“G"), SerializeField]
    GameObject[] _wave1Enemies= default;
    [Tooltip("2Wave‚Ì“G"), SerializeField]
    GameObject[] _wave2Enemies = default;
    [Tooltip("BossWave‚Ì“G"), SerializeField]
    GameObject[] _waveBossEnemies = default;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
