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

    
    StateFlag _fool = StateFlag.Normal;
    List<StateFlag> _stateFlags = new List<StateFlag>();


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
                _stateFlags.Add(StateFlag.InstantDeath);
            }
            else if (image.sprite.name.Contains("力"))
            {
                _stateFlags.Add(StateFlag.PowerUp);
            }
            else if (image.sprite.name.Contains("戦車"))
            {
                _stateFlags.Add(StateFlag.GuardUp);
            }
            else if (image.sprite.name.Contains("節制"))
            {
                _stateFlags.Add(StateFlag.Average);
            }
            else if (image.sprite.name.Contains("女帝"))
            {
                _stateFlags.Add(StateFlag.Heal);
            }
            else if (image.sprite.name.Contains("愚者"))
            {
                int randomValue = Random.Range(0, 5);
                if (randomValue == 0)
                {
                    _fool = StateFlag.InstantDeath;
                }
                else if (randomValue == 1)
                {
                    _fool = StateFlag.PowerUp;
                }
                else if (randomValue == 2)
                {
                    _fool = StateFlag.GuardUp;
                }
                else if (randomValue == 3)
                {
                    _fool = StateFlag.Average;
                }
                else if (randomValue == 4)
                {
                    _fool = StateFlag.Heal;
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
                _stateFlags.Remove(StateFlag.InstantDeath);
            }
            else if (image.sprite.name.Contains("力"))
            {
                _stateFlags.Remove(StateFlag.PowerUp);
            }
            else if (image.sprite.name.Contains("戦車"))
            {
                _stateFlags.Remove(StateFlag.GuardUp);
            }
            else if (image.sprite.name.Contains("節制"))
            {
                _stateFlags.Remove(StateFlag.Average);
            }
            else if (image.sprite.name.Contains("女帝"))
            {
                _stateFlags.Remove(StateFlag.Heal);
            }
            else if (image.sprite.name.Contains("愚者"))
            {
                _fool = StateFlag.Normal;
            }
        }

        _selectableCard.text = $"残り{_cards - _selectedCard.Count}枚";

        if (_selectedCard.Count + 1 > _cards)
        {
            float gDamage = _playerController.PlayerAttack;
            float pDamage = _goblinController.Attack;
            //ダメージ補正を追加
            //フラグが立つと即死効果を確率で発生
            if (_fool == StateFlag.InstantDeath || _stateFlags.Contains(StateFlag.InstantDeath))
            {
                if (Random.Range(0, 10) < 5)
                {
                    gDamage = 3000f;
                    Debug.Log("即死！！");
                }
                else
                {
                    Debug.Log("即死失敗");
                }
            }
            //フラグが立つと敵と自分のHPが二人の平均になる
            else if ((_stateFlags.Contains(StateFlag.Average) || _fool == StateFlag.Average))
            {
                float av = (_playerController.CurrentPlayerHP + _goblinController.CurrentEnemyHP) / 2;
                gDamage = -(_playerController.CurrentPlayerHP - av);
                pDamage = -(_goblinController.CurrentEnemyHP - av);
            }
            //フラグが立つとダメージアップ
            else if (_stateFlags.Contains(StateFlag.PowerUp) || _fool == StateFlag.PowerUp)
            {
                gDamage = gDamage * 1.5f;
                Debug.Log("クリティカル！！");
            }
            //フラグが立つと回復
            if ((_stateFlags.Contains(StateFlag.Heal) || _fool == StateFlag.Heal) && !_stateFlags.Contains(StateFlag.Average))
            {
                _playerController.PlayerDamage(-10);
                gDamage = 0f;
            }
            //フラグが立つとガードアップ
            if ((_stateFlags.Contains(StateFlag.GuardUp) || _fool == StateFlag.GuardUp) && !_stateFlags.Contains(StateFlag.Average))
            {
                pDamage = pDamage / 2f;
                Debug.Log("ガードアップ！！");
            }

            // フラグの初期化
            _stateFlags.Clear();
            _fool = StateFlag.Normal;

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

    enum StateFlag
    {
        Normal,
        InstantDeath,
        PowerUp,
        GuardUp,
        Average,
        Heal
    }
}
