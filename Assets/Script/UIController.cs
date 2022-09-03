using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Tooltip("カードSelectとエンドボタン")]
    [SerializeField] List<GameObject> _selectAndEnd = new List<GameObject>();
    [Tooltip("カード選択から戻るボタン")]
    [SerializeField] GameObject _backButton;
    [Tooltip("あと何枚選べるか表示するテキスト")]
    [SerializeField] Text _selectableCard;
    [SerializeField] List<GameObject> _cardMuzzles = new List<GameObject>();
    List<Image> _cardImages = new List<Image>();
    [Tooltip("カードのスプライト")]
    [SerializeField] List<Sprite> _cardSprite = new List<Sprite>();
    [Tooltip("選ばれたカード")]
    List<GameObject> _selectedCard = new List<GameObject>();
    [Tooltip("攻撃対象")]
    [SerializeField] GameObject _enemy;


    private void OnEnable()
    {
        GameManager.Instance.OnBeginTurn += BeginTurnUI;
        GameManager.Instance.OnEndTurn += EndTurnUI;
    }
    private void Start()
    {
        foreach (var card in _cardMuzzles)
        {
            card.SetActive(false);
            _cardImages.Add(card.GetComponent<Image>());
        }
        ShuffleCard();
        _selectableCard.enabled = false;
    }

    void BeginTurnUI()
    {
        _selectAndEnd.ForEach(i => i.SetActive(true));
        ShuffleCard();
        _selectedCard.Clear();
        _selectableCard.text = $"残り{GameManager.Instance.TurnCount}枚";
        _backButton.SetActive(false);
    }

    public void SelectCard()
    {
        _selectAndEnd.ForEach(i => i.SetActive(false));
        _cardMuzzles.ForEach(i => i.SetActive(true));
        _backButton.SetActive(true);
        _selectableCard.enabled = true;
    }

    public void SelectButton()
    {
        _selectAndEnd.ForEach(i => i.SetActive(true));
        _cardMuzzles.ForEach(i => i.SetActive(false));
        _backButton.SetActive(false);
        _selectableCard.enabled = false;
    }

    void ShuffleCard()
    {
        var copySprite = new List<Sprite>(_cardSprite);
        foreach (var card in _cardImages)
        {
            int RSpriteIndex = UnityEngine.Random.Range(0, copySprite.Count);
            card.sprite = copySprite[RSpriteIndex];
            card.SetNativeSize();
            copySprite.RemoveAt(RSpriteIndex);
        }
    }

    public void ButtonGetGameObject(GameObject go)
    {
        if (!_selectedCard.Contains(go))
        {
            _selectedCard.Add(go);
            go.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            _selectedCard.Remove(go);
            go.GetComponent<Image>().color = Color.white;
        }

        int CurrentSelectableCards = GameManager.Instance.TurnCount % 5;
        _selectableCard.text = $"残り{CurrentSelectableCards - _selectedCard.Count}枚";

        if (_selectedCard.Count > CurrentSelectableCards)
        {
            _enemy.GetComponent<GoblinController>().DecreaseHP(20);
            _selectedCard.ForEach(i => i.GetComponent<Image>().color = Color.white);
            _cardMuzzles.ForEach(i => i.SetActive(false));
            SelectButton();
            GameManager.Instance.EndTurn();
        }
    }

    void EndTurnUI()
    {
        _selectAndEnd.ForEach(i => i.SetActive(false));
        _cardMuzzles.ForEach(i => i.SetActive(false));
    }
}
