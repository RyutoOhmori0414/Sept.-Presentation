using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject _startTurnButton;
    [SerializeField] GameObject _endTurnButton;


    private void OnEnable()
    {
        GameManager.Instance.OnBeginTurn += BeginTurnUI;
        GameManager.Instance.OnEndTurn += EndTurnUI;
    }

    private void Start()
    {
        _endTurnButton.SetActive(false);
    }

    void BeginTurnUI()
    {
        _startTurnButton.SetActive(false);
        _endTurnButton.SetActive(true);
    }

    void EndTurnUI()
    {
        _startTurnButton.SetActive(true);
        _endTurnButton.SetActive(false);
    }
}
