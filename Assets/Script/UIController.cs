using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject _endTurnButton;
    [SerializeField] List<GameObject>　_cardMuzzles = new List<GameObject>();
    List<Image> _cardImages = new List<Image>();

    Animator _buttonAnimation;
    [Tooltip("カードのスプライト")]
    [SerializeField] List<Sprite> _cardSprite = new List<Sprite>();


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
            _cardImages.Add(card.GetComponent<Image>());
        }
        ShuffleCard();
    }

    void BeginTurnUI()
    {
        _buttonAnimation.SetBool("Turn", true);
        ShuffleCard();
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

    void ShuffleCard()
    {
        var copySprite = _cardSprite;
        foreach (var card in _cardImages)
        {
            int RSpriteIndex = Random.Range(0, copySprite.Count);
            card.sprite = copySprite[RSpriteIndex];
            card.SetNativeSize();
            copySprite.RemoveAt(RSpriteIndex);
        }
    }

    void EndTurnUI()
    {
        _buttonAnimation.SetBool("Turn", false);
    }
}
