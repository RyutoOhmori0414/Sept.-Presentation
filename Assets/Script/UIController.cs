using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject _endTurnButton;
    [SerializeField] List<GameObject>Å@_cardMuzzles = new List<GameObject>();

    Animator _buttonAnimation;


    private void OnEnable()
    {
        GameManager.Instance.OnBeginTurn += BeginTurnUI;
        GameManager.Instance.OnEndTurn += EndTurnUI;
    }
    private void Start()
    {
        _buttonAnimation = GameObject.Find("Canvas").GetComponent<Animator>();
        foreach (var card in _cardMuzzles)
        {
            card.SetActive(false);
        }

    }

    void BeginTurnUI()
    {
        _buttonAnimation.SetBool("Turn", true);
    }

    public void SelectCard()
    {
        _buttonAnimation.SetBool("Fadeout", true);
        foreach (var card in _cardMuzzles)
        {
            card.SetActive(true);
        }
    }

    public void SelectButton()
    {
        _buttonAnimation.SetBool("Fadeout", false);
        foreach (var card in _cardMuzzles)
        {
            card?.SetActive(false);
        }
    }

    void EndTurnUI()
    {
        _buttonAnimation.SetBool("Turn", false);
    }
}
