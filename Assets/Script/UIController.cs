using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject _endTurnButton;


    private void OnEnable()
    {
        GameManager.Instance.OnBeginTurn += BeginTurnUI;
        GameManager.Instance.OnEndTurn += EndTurnUI;
    }

    void BeginTurnUI()
    {
        _endTurnButton.SetActive(true);
    }

    void EndTurnUI()
    {
        _endTurnButton.SetActive(false);
    }
}
