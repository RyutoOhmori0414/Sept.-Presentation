using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectController : MonoBehaviour
{
    GoblinController _gc;

    private void Start()
    {
        _gc = GetComponent<GoblinController>();
    }
    public void EnemySelected ()
    {
        FindObjectOfType<UIController>().PlayerAttack(_gc);
    }
}
