using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [Header("ボタン関係")]
    [Tooltip("カードSelectとエンドボタン")]
    [SerializeField] List<GameObject> _selectAndEnd = new List<GameObject>();
    [Tooltip("カード選択から戻るボタン")]
    [SerializeField] GameObject _backButton;

    [Header("テキスト関係")]
    [Tooltip("あと何枚選べるか表示するテキスト")]
    [SerializeField] Text _selectableCard;
    [Tooltip("Waveが変わった際に現在のWave数を表示する"), SerializeField]
    GameObject _waveTextgo;
    [Tooltip("選んでいるカードの効果を表示するテキストボックス"), SerializeField]
    GameObject _cardStateTextBox;

    [Header("カード関係")]
    [Tooltip("カード"), SerializeField]
    List<GameObject> _cardMuzzles = new List<GameObject>();
    [Tooltip("カードのアニメーション"), SerializeField]
    Animator _cardAnim;
    List<Image> _cardImages = new List<Image>();
    List<Button> _cardButtons = new List<Button>();
    [Tooltip("カードのスプライト")]
    [SerializeField] List<Sprite> _cardSprite = new List<Sprite>();
    [Tooltip("選ばれたカード")]
    List<GameObject> _selectedCard = new List<GameObject>();
    [Header("選べるカードの枚数"), SerializeField] int _cards = default;
    
    /// <summary>選んでいるカードの効果を表示するテキスト</summary>
    Text _cardStateText;

    PlayerController _playerController;
    EventSystem _currentES = default;

    GameObject _lastSelectedObj;

    StateFlag _fool = StateFlag.Normal;
    List<StateFlag> _stateFlags = new List<StateFlag>();

    /// <summary>プレイヤーがエネミーに与えるダメージ</summary>
    float _gDamage = default;
    /// <summary>エネミーがプレイヤーに与えるダメージ</summary>
    float _pDamage = default;

    private void Update()
    {
        if (_lastSelectedObj != _currentES.currentSelectedGameObject && _currentES.currentSelectedGameObject)
        {
            OnSelect(_currentES.currentSelectedGameObject);
        }
        _lastSelectedObj = _currentES.currentSelectedGameObject;
    }
    private void OnEnable()
    {
        GameManager.Instance.OnBeginTurn += BeginTurnUI;
        GameManager.Instance.OnEndTurn += EndTurnUI;
    }
    private void Start()
    {
        foreach (var card in _cardMuzzles)
        {
            _cardImages.Add(card.GetComponent<Image>());
            Button cardButton = card.GetComponent<Button>();
            cardButton.interactable = false;
            _cardButtons.Add(cardButton);
        }
        ShuffleCard();
        _selectableCard.enabled = false;

        _currentES = EventSystem.current;

        _cardStateTextBox.SetActive(false);

        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _cardStateText = _cardStateTextBox.GetComponentInChildren<Text>();
    }

    void BeginTurnUI()
    {
        _selectAndEnd.ForEach(i => i.SetActive(true));
        _cardMuzzles.ForEach(i => i.SetActive(true));
        ShuffleCard();
        _selectedCard.Clear();
        _selectableCard.text = $"残り{_cards}枚";
        _backButton.SetActive(false);
    }

    public void SelectCard()
    {
        _selectAndEnd.ForEach(i => i.SetActive(false));
        _cardButtons.ForEach(i => i.interactable = true);
        _cardAnim.SetBool("ZoomIn", true);
        _backButton.SetActive(true);
        _selectableCard.enabled = true;
        _currentES.SetSelectedGameObject(_cardMuzzles[0]);
        _cardStateTextBox.SetActive(true);
    }

    public void SelectButton()
    {
        _selectAndEnd.ForEach(i => i.SetActive(true));
        _cardButtons.ForEach(i => i.interactable = false);
        _cardAnim.SetBool("ZoomIn", false);
        _backButton.SetActive(false);
        _selectableCard.enabled = false;
        _currentES.SetSelectedGameObject(_selectAndEnd[0]);
        _cardStateTextBox.SetActive(false);
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
    public void OnTestSelected(BaseEventData eventData)
    {
        Debug.Log("Test動いた");
        Debug.Log(eventData.selectedObject.name);
    }

    //カードをSelectしている際にSelectしているカードの効果を出力する
    public void OnSelect(GameObject sgameObject)
    {
        if (sgameObject.CompareTag("Card"))
        {
            string selectedSpriteName = sgameObject.GetComponent<Image>().sprite.name;

            if (selectedSpriteName.Contains("死神") || selectedSpriteName.Contains("吊るされた男"))
            {
                _cardStateText.text = "確率で即死効果が付与される";
            }
            else if (selectedSpriteName.Contains("力"))
            {
                _cardStateText.text = "自身の攻撃力が一時的に上昇する！";
            }
            else if (selectedSpriteName.Contains("戦車"))
            {
                _cardStateText.text = "自身の防御力が一時的に上昇する！";
            }
            else if (selectedSpriteName.Contains("節制"))
            {
                _cardStateText.text = "攻撃対象と、自分のHPを等しく配分しなおす";
            }
            else if (selectedSpriteName.Contains("女帝"))
            {
                _cardStateText.text = "自身の攻撃に回復効果が付与される";
            }
            else if (selectedSpriteName.Contains("愚者"))
            {
                _cardStateText.text = "ランダムで追加効果が発生する";
            }
            else
            {
                _cardStateText.text = "特になし";
            }
        }
        else
        {
            _cardStateText.text = "";
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
            if (image.sprite.name.Contains("死神") || image.sprite.name.Contains("吊るされた男"))
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
                int randomValue = UnityEngine.Random.Range(0, 5);
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
            //攻撃するキャラクターを選択する場面に移動
            _cardStateTextBox.SetActive(false);
            _backButton.SetActive(false);
            _cardMuzzles.ForEach(i => i.SetActive(false));

            //直接選べないようにする
            var enemyTargets = GameObject.FindGameObjectsWithTag("ArrowMark");
            Array.ForEach(Array.ConvertAll(enemyTargets, i => i.GetComponent<Button>()), i => i.interactable = true);
                
            _currentES.SetSelectedGameObject(enemyTargets[0]);
        }
    }

    public void PlayerAttack(GoblinController gc)
    {
        _gDamage = _playerController.PlayerAttack;
        _pDamage = gc.Attack;
        bool instanceDeath = false;
        //ダメージ補正を追加
        //フラグが立つと即死効果を確率で発生
        if (_fool == StateFlag.InstantDeath || _stateFlags.Contains(StateFlag.InstantDeath))
        {
            if (UnityEngine.Random.Range(0, 10) < 5)
            {
                instanceDeath = true;
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
            float av = (_playerController.CurrentPlayerHP + gc.CurrentEnemyHP) / 2;
            _gDamage = -(_playerController.CurrentPlayerHP - av);
            _pDamage = -(gc.CurrentEnemyHP - av);
        }
        //フラグが立つとダメージアップ
        else if (_stateFlags.Contains(StateFlag.PowerUp) || _fool == StateFlag.PowerUp)
        {
            _gDamage = _gDamage * 1.5f;
            Debug.Log("クリティカル！！");
        }
        //フラグが立つと回復
        if ((_stateFlags.Contains(StateFlag.Heal) || _fool == StateFlag.Heal) && !_stateFlags.Contains(StateFlag.Average))
        {
            _playerController.PlayerDamage(-10);
            _pDamage = 0;
        }
        //フラグが立つとガードアップ
        if ((_stateFlags.Contains(StateFlag.GuardUp) || _fool == StateFlag.GuardUp) && !_stateFlags.Contains(StateFlag.Average))
        {
            _pDamage = _pDamage / 2f;
            Debug.Log("ガードアップ！！");
        }

        // フラグの初期化
        _stateFlags.Clear();
        _fool = StateFlag.Normal;

        gc.DecreaseEnemyHP(_gDamage, instanceDeath);
        //_playerController.PlayerDamage(_pDamage);
        _selectedCard.ForEach(i => i.GetComponent<Image>().color = Color.white);
        _cardMuzzles.ForEach(i => i.SetActive(false));
        SelectButton();
        GameManager.Instance.EndTurn();
    }


    void EndTurnUI()
    {
        _selectAndEnd.ForEach(i => i.SetActive(false));
        _cardMuzzles.ForEach(i => i.SetActive(false));
    }

    public void WaveStartUIText(int currentWave)
    {
        Text waveText =_waveTextgo.GetComponent<Text>();
        waveText.text = $"Wave {currentWave.ToString()}/3";
        var seq = DOTween.Sequence();
        seq.Append(waveText.DOFade(1f, 1f));
        seq.AppendInterval(1f);
        seq.Append(waveText.DOFade(0f, 1f));    
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

    public void EffectEnd()
    {
        _playerController.PlayerDamage(_pDamage);
        Array.ForEach(GameObject.FindGameObjectsWithTag("Enemy"), i => i.GetComponent<Animator>().Play("EnemyAttack"));
    }

    private void OnDisable()
    {
        GameManager.Instance.OnBeginTurn -= BeginTurnUI;
        GameManager.Instance.OnEndTurn -= EndTurnUI;
    }
}
