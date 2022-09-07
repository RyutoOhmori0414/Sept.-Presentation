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
    [Header("選べるカードの枚数"), SerializeField] int _cards = default;

    PlayerController _playerController;
    GoblinController _goblinController;

    /// <summary>即死</summary>
    int _instantDeath = 0;
    bool _powerUp = false;
    bool _guardUp = false;
    bool _average = false;
    bool _heal = false;
    RandomFlag _fool = RandomFlag.Normal;


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

        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _goblinController = _enemy.GetComponent<GoblinController>();
    }

    void BeginTurnUI()
    {
        _selectAndEnd.ForEach(i => i.SetActive(true));
        ShuffleCard();
        _selectedCard.Clear();
        _selectableCard.text = $"残り{_cards}枚";
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
            else if (image.sprite.name.Contains("節制"))
            {
                _average = true;
            }
            else if (image.sprite.name.Contains("女帝"))
            {
                _heal = true;
            }
            else if (image.sprite.name.Contains("愚者"))
            {
                int randomValue = Random.Range(0, 5);
                if (randomValue == 0)
                {
                    _fool = RandomFlag.InstantDeath;
                }
                else if (randomValue == 1)
                {
                    _fool = RandomFlag.PowerUp;
                }
                else if (randomValue == 2)
                {
                    _fool = RandomFlag.GuardUp;
                }
                else if (randomValue == 3)
                {
                    _fool = RandomFlag.Average;
                }
                else if (randomValue == 4)
                {
                    _fool = RandomFlag.Heal;
                }
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
            else if (image.sprite.name.Contains("節制"))
            {
                _average = false;
            }
            else if (image.sprite.name.Contains("女帝"))
            {
                _heal = false;
            }
            else if (image.sprite.name.Contains("愚者"))
            {
                _fool = RandomFlag.Normal;
            }
        }

        _selectableCard.text = $"残り{_cards - _selectedCard.Count}枚";

        if (_selectedCard.Count + 1 > _cards)
        {
            float gDamage = _playerController.PlayerAttack;
            float pDamage = _goblinController.Attack;
            //ダメージ補正を追加
            //フラグが立つと即死効果を確率で発生
            if (_instantDeath > 0 || _fool == RandomFlag.InstantDeath)
            {
                if (Random.Range(0, 10) < 5)
                {
                    gDamage = 3000f;
                    _instantDeath = 0;
                    Debug.Log("即死！！");
                }
                else
                {
                    _instantDeath = 0;
                    Debug.Log("即死失敗");
                }
            }
            //フラグが立つと敵と自分のHPが二人の平均になる
            else if ((_average || _fool == RandomFlag.Average) && _instantDeath == 0)
            {
                float av = (_playerController.CurrentPlayerHP + _goblinController.CurrentEnemyHP) / 2;
                gDamage = -(_playerController.CurrentPlayerHP - av);
                pDamage = -(_goblinController.CurrentEnemyHP - av);
            }
            //フラグが立つとダメージアップ
            else if (_powerUp || _fool == RandomFlag.PowerUp)
            {
                gDamage = gDamage * 1.5f;
                _powerUp = false;
                Debug.Log("クリティカル！！");
            }
            //フラグが立つと回復
            if ((_heal || _fool == RandomFlag.Heal) && !_average)
            {
                _playerController.PlayerDamage(-10);
                gDamage = 0f;
            }
            //フラグが立つとガードアップ
            if ((_guardUp || _fool == RandomFlag.GuardUp) && !_average)
            {
                pDamage = pDamage / 2f;
                Debug.Log("ガードアップ！！");
            }

            // フラグの初期化
            _instantDeath = 0;
            _average = false;
            _powerUp = false;
            _guardUp = false;
            _heal = false;
            _fool = RandomFlag.Normal;

            _goblinController.DecreaseEnemyHP(gDamage);
            _playerController.PlayerDamage(pDamage);
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

    enum RandomFlag
    {
        Normal,
        InstantDeath,
        PowerUp,
        GuardUp,
        Average,
        Heal
    }
}
