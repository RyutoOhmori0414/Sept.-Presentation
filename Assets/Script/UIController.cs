using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject _endTurnButton;

    Animator _buttonAnimation;


    private void OnEnable()
    {
        GameManager.Instance.OnBeginTurn += BeginTurnUI;
        GameManager.Instance.OnEndTurn += EndTurnUI;
    }
    private void Start()
    {
        _buttonAnimation = GameObject.Find("Canvas").GetComponent<Animator>();
    }

    void BeginTurnUI()
    {
        _buttonAnimation.SetBool("Turn", true);
    }

    public void SelectCard()
    {
        _buttonAnimation.SetBool("Fadeout", true);
    }

    public void SelectButton()
    {
        _buttonAnimation.SetBool("Fadeout", false);
    }

    void EndTurnUI()
    {
        _buttonAnimation.SetBool("Turn", false);
    }
}
