using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        var enemyTargets = GameObject.FindGameObjectsWithTag("ArrowMark");
        Array.ForEach(Array.ConvertAll(enemyTargets, i => i.GetComponent<Button>()), i => i.interactable = false);
    }
}
