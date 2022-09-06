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

    /// <summary>即死</summary>
    int _instantDeath = 0;
    bool _powerUp = false;
    bool _guardUp = false;


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
            Image image = go.GetComponent<Image>();
            image.color = Color.gray;

            //特殊効果があるカードが選ばれた際フラグを立てる
            if (image.sprite.name.Contains("死神") || image.sprite.name.Contains("死"))
            {
                _instantDeath++;
            }
            else if (image.sprite.name.Contains("力"))
            {
                _powerUp = true;
            }
            else if (image.sprite.name.Contains("戦車"))
            {
                _guardUp = true;
            }
        }
        else
        {
            _selectedCard.Remove(go);
            Image image = go.GetComponent<Image>();
            image.color = Color.white;

            //特殊効果があるカードの選択が解除された際フラグも取り消す
            if (image.sprite.name.Contains("死神") || image.sprite.name.Contains("死"))
            {
                _instantDeath--;
            }
            else if (image.sprite.name.Contains("力"))
            {
                _powerUp = false;
            }
            else if (image.sprite.name.Contains("戦車"))
            {
                _guardUp = false;
            }
        }

        int CurrentSelectableCards = GameManager.Instance.TurnCount % 5;
        _selectableCard.text = $"残り{CurrentSelectableCards - _selectedCard.Count + 1}枚";

        if (_selectedCard.Count > CurrentSelectableCards)
        {
            float damage = PlayerController.PlayerAttack;
            //ダメージ補正を追加
            //フラグが立つと即死効果を確率で発生
            if(_instantDeath > 0)
            {
                damage = 3000f;
                _instantDeath = 0;
                Debug.Log("即死！！");
            }
            //フラグが立つとダメージアップ
            else if (_powerUp)
            {
                damage = damage * 1.5f;
                _powerUp = false;
                Debug.Log("クリティカル！！");
            }
            //フラグが立つとガードアップ
            else if (_guardUp)
            {
                GoblinController.CurrentAttack = 0.5f;
                Debug.Log("ガードアップ！！");
            }

            _enemy.GetComponent<GoblinController>().DecreaseEnemyHP(damage);
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
