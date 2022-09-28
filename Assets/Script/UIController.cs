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
    [SerializeField] 
    List<GameObject> _selectAndEnd = new List<GameObject>();
    [Tooltip("カード選択から戻るボタン")]
    [SerializeField] 
    GameObject _backButton;

    [Header("テキスト関係")]
    [Tooltip("あと何枚選べるか表示するテキスト")]
    [SerializeField] 
    Text _selectableCard;
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
    [Tooltip("選べるカードの枚数"), SerializeField] 
    int _cards = default;

    [Header("その他")]
    [Tooltip("フェードに使うパネル"), SerializeField]
    GameObject _fadePanel;
    
    /// <summary>選んでいるカードの効果を表示するテキスト</summary>
    Text _cardStateText;

    PlayerController _playerController;
    GameManager _gameManager;
    EventSystem _currentES = default;
    GameSceneAudioController _gameSceneAudioController;
    GameObject _lastSelectedObj;

    StateFlag _fool = StateFlag.Normal;
    List<StateFlag> _stateFlags = new List<StateFlag>();

    /// <summary>プレイヤーがエネミーに与えるダメージ</summary>
    float _gDamage = default;
    /// <summary>エネミーがプレイヤーに与えるダメージ</summary>
    float _pDamage = default;
    /// <summary>自分がカードを選んでいるときtrue</summary>
    bool _select = false;

    private void Update()
    {
        //フォーカスが外れた場合、一個前にフォーカスしていたものにフォーカスを戻す
        if (!_currentES.currentSelectedGameObject)
        {
            _currentES.SetSelectedGameObject(_lastSelectedObj);
        }
        //フォーカスしているものが変わったときに行われる処理
        else if (_lastSelectedObj != _currentES.currentSelectedGameObject)
        {
            OnSelect(_currentES.currentSelectedGameObject);
            if (_select)
            {
                _gameSceneAudioController.OnButtonSelectedSE();
            }
            _lastSelectedObj = _currentES.currentSelectedGameObject;
        }
    }
    private void OnEnable()
    {
        _gameManager = GetComponent<GameManager>();
        _gameSceneAudioController = FindObjectOfType<GameSceneAudioController>();
        _gameManager.OnBeginTurn += BeginTurnUI;
        _gameManager.OnEndTurn += EndTurnUI;
    }
    private void Start()
    {
        //カードのボタンとイメージを配列に入れる
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

    /// <summary>ターンが始まった際のUIの管理</summary>
    void BeginTurnUI()
    {
        _select = true;
        _selectAndEnd.ForEach(i => i.SetActive(true));
        _cardMuzzles.ForEach(i => i.SetActive(true));
        ShuffleCard();
        _selectedCard.Clear();
        _selectableCard.text = $"残り{_cards}枚";
        _backButton.SetActive(false);
    }

    /// <summary>カードSelect画面に移るときに行うUIの管理</summary>
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

    /// <summary>カードSelectをやめるときのUIの管理</summary>
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

    /// <summary>選択するカードをシャッフルする</summary>
    void ShuffleCard()
    {
        //カードのスプライトをコピーする
        var copySprite = new List<Sprite>(_cardSprite);
        //カードのイメージをランダムで被らないように表示させる
        _cardImages.ForEach(i =>
        {
            int rSpriteIndex = UnityEngine.Random.Range(0, copySprite.Count);
            i.sprite = copySprite[rSpriteIndex];
            i.SetNativeSize();
            copySprite.RemoveAt(rSpriteIndex);
        });
    }
    
    /// <summary>
    /// カードをSelectしている際にSelectしているカードの効果を出力する
    /// </summary>
    /// <param name="sGameObject"></param>
    public void OnSelect(GameObject sGameObject)
    {
        //フォーカスが当たっているのがカードかどうか確認
        if (sGameObject.CompareTag("Card"))
        {
            string selectedSpriteName = sGameObject.GetComponent<Image>().sprite.name;

            //カードのスプライトの名前で判定して効果のテキストを変える
            if (selectedSpriteName.Contains("Death") || selectedSpriteName.Contains("HangedMan"))
            {
                _cardStateText.text = "確率で即死効果が付与される";
            }
            else if (selectedSpriteName.Contains("Magician") || selectedSpriteName.Contains("Strength") || selectedSpriteName.Contains("Emperor"))
            {
                _cardStateText.text = "自身の攻撃力が一時的に上昇する！";
            }
            else if (selectedSpriteName.Contains("Chariot"))
            {
                _cardStateText.text = "自身の防御力が一時的に上昇する！";
            }
            else if (selectedSpriteName.Contains("Temperance") || selectedSpriteName.Contains("Judgement"))
            {
                _cardStateText.text = "攻撃対象と、自分のHPを等しく配分しなおす";
            }
            else if (selectedSpriteName.Contains("Empress") || selectedSpriteName.Contains("Lovers"))
            {
                _cardStateText.text = "自身の攻撃に回復効果が付与される";
            }
            else if (selectedSpriteName.Contains("Hierophant"))
            {
                _cardStateText.text = "体力を全回復";
            }
            else if (selectedSpriteName.Contains("Priestes"))
            {
                _cardStateText.text = "体力上限アップ";
            }
            else if (selectedSpriteName.Contains("Fool") || selectedSpriteName.Contains("Fortune"))
            {
                _cardStateText.text = "ランダムで追加効果が発生する";
            }
            else if (selectedSpriteName.Contains("Tower"))
            {
                _cardStateText.text = "体力が減る代わりに強力な攻撃";
            }
            else if (selectedSpriteName.Contains("Justice") || selectedSpriteName.Contains("World"))
            {
                _cardStateText.text = "1ターン無敵になる";
            }
            else if (selectedSpriteName.Contains("Hermit"))
            {
                _cardStateText.text = "ランダムで1ターン無敵になる";
            }
            else if (selectedSpriteName.Contains("Devil"))
            {
                _cardStateText.text = "自分にダメージが入ります";
            }
            else if (selectedSpriteName.Contains("Moon") || selectedSpriteName.Contains("Star") || selectedSpriteName.Contains("Sun"))
            {
                _cardStateText.text = "星と月と太陽をとると超絶ダメージ";
            }
            else
            {
                _cardStateText.text = "特になし";
            }
        }
        //カードが選ばれていないときは何も表示しない
        else
        {
            _cardStateText.text = "";
        }
    }

    /// <summary>
    /// カード（ボタン）が押されたときにそのカードのGameObjectを取ってくる
    /// </summary>
    /// <param name="go"></param>
    public void ButtonGetGameObject(GameObject go)
    {
        //選んだカードが入れられている配列にgoが入っていないときフラグを立てて、その配列に加える
        if (!_selectedCard.Contains(go))
        {
            _selectedCard.Add(go);
            Image image = go.GetComponent<Image>();
            image.color = Color.gray;

            //スプライトの名前で判断してフラグを立てる
            if (image.sprite.name.Contains("Death") || image.sprite.name.Contains("HangedMan"))
            {
                _stateFlags.Add(StateFlag.InstantDeath);
            }
            else if (image.sprite.name.Contains("Magician") || image.sprite.name.Contains("Strength") || image.sprite.name.Contains("Emperor"))
            {
                _stateFlags.Add(StateFlag.PowerUp);
            }
            else if (image.sprite.name.Contains("Priestes"))
            {
                _stateFlags.Add(StateFlag.HPUp);
            }
            else if (image.sprite.name.Contains("Chariot"))
            {
                _stateFlags.Add(StateFlag.GuardUp);
            }
            else if (image.sprite.name.Contains("Temperance") || image.sprite.name.Contains("Judgement"))
            {
                _stateFlags.Add(StateFlag.Average);
            }
            else if (image.sprite.name.Contains("Empress") || image.sprite.name.Contains("Lovers"))
            {
                _stateFlags.Add(StateFlag.Heal);
            }
            else if (image.sprite.name.Contains("Hierophant"))
            {
                _stateFlags.Add(StateFlag.PowerHeal);
            }
            else if (image.sprite.name.Contains("Fool") || image.sprite.name.Contains("Fortune"))
            {
                //ランダム効果の場合一括管理するとフラグを取り消す際に難しくなるため別で変数を用意してそこで管理する
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
            else if (image.sprite.name.Contains("Tower"))
            {
                _stateFlags.Add(StateFlag.RiskyAttack);
            }
            else if (image.sprite.name.Contains("Justice") || image.sprite.name.Contains("World"))
            {
                _stateFlags.Add(StateFlag.ParfectGuard);
            }
            else if (image.sprite.name.Contains("Hermit"))
            {
                _stateFlags.Add(StateFlag.RandomGuard);
            }
            else if (image.sprite.name.Contains("Devil"))
            {
                _stateFlags.Add(StateFlag.SelfHarm);
            }
            else if (image.sprite.name.Contains("Star"))
            {
                _stateFlags.Add(StateFlag.Star);
            }
            else if (image.sprite.name.Contains("Moon"))
            {
                _stateFlags.Add(StateFlag.Moon);
            }
            else if (image.sprite.name.Contains("Sun"))
            {
                _stateFlags.Add(StateFlag.Sun);
            }
        }
        //すでにそのカードが選ばれている場合は選択状態を解除して、立てられたフラグを消す
        else
        {
            _selectedCard.Remove(go);
            Image image = go.GetComponent<Image>();
            image.color = Color.white;

            //選ばれたカードの選択が解除された際フラグも取り消す
            if (image.sprite.name.Contains("Death") || image.sprite.name.Contains("HangedMan"))
            {
                _stateFlags.Remove(StateFlag.InstantDeath);
            }
            else if (image.sprite.name.Contains("Magician") || image.sprite.name.Contains("Strength") || image.sprite.name.Contains("Emperor"))
            {
                _stateFlags.Remove(StateFlag.PowerUp);
            }
            else if (image.sprite.name.Contains("Priestes"))
            {
                _stateFlags.Remove(StateFlag.HPUp);
            }
            else if (image.sprite.name.Contains("Chariot"))
            {
                _stateFlags.Remove(StateFlag.GuardUp);
            }
            else if (image.sprite.name.Contains("Temperance") || image.sprite.name.Contains("Judgement"))
            {
                _stateFlags.Remove(StateFlag.Average);
            }
            else if (image.sprite.name.Contains("Empress") || image.sprite.name.Contains("Lovers"))
            {
                _stateFlags.Remove(StateFlag.Heal);
            }
            else if (image.sprite.name.Contains("Hierophant"))
            {
                _stateFlags.Remove(StateFlag.PowerHeal);
            }
            else if (image.sprite.name.Contains("Fool") || image.sprite.name.Contains("Fortune"))
            {
                _fool = StateFlag.Normal;
            }
            else if (image.sprite.name.Contains("Tower"))
            {
                _stateFlags.Remove(StateFlag.RiskyAttack);
            }
            else if (image.sprite.name.Contains("Justice") || image.sprite.name.Contains("World"))
            {
                _stateFlags.Remove(StateFlag.ParfectGuard);
            }
            else if (image.sprite.name.Contains("Hermit"))
            {
                _stateFlags.Remove(StateFlag.RandomGuard);
            }
            else if (image.sprite.name.Contains("Devil"))
            {
                _stateFlags.Remove(StateFlag.SelfHarm);
            }
            else if (image.sprite.name.Contains("Star"))
            {
                _stateFlags.Remove(StateFlag.Star);
            }
            else if (image.sprite.name.Contains("Moon"))
            {
                _stateFlags.Remove(StateFlag.Moon);
            }
            else if (image.sprite.name.Contains("Sun"))
            {
                _stateFlags.Remove(StateFlag.Sun);
            }
        }

        //残り選択可能枚数を表示している
        _selectableCard.text = $"残り{_cards - _selectedCard.Count}枚";

        //選択可能枚数
        if (_selectedCard.Count + 1 > _cards)
        {
            //攻撃するキャラクターを選択する場面に移動
            _cardStateTextBox.SetActive(false);
            _backButton.SetActive(false);
            _cardMuzzles.ForEach(i => i.SetActive(false));

            //エネミーを直接選べないようにする
            var enemyTargets = GameObject.FindGameObjectsWithTag("ArrowMark");
            Array.ForEach(Array.ConvertAll(enemyTargets, i => i.GetComponent<Button>()), i => i.interactable = true);
                
            _currentES.SetSelectedGameObject(enemyTargets[0]);
        }
    }

    /// <summary>
    /// 立っていたフラグを使いダメージを計算する
    /// </summary>
    /// <param name="gc"></param>
    public void PlayerAttack(GoblinController gc)
    {
        _gDamage = _playerController.PlayerAttack;
        _pDamage = gc.Attack;
        bool instanceDeath = false;
        //ダメージ補正を追加
        //星と太陽と月のフラグが立っていた場合に動く
        if (_stateFlags.Contains(StateFlag.Star) && _stateFlags.Contains(StateFlag.Sun) && _stateFlags.Contains(StateFlag.Moon))
        {
            _gDamage = _gDamage * 20;
        }
        //フラグが立つと即死効果を確率で発生
        else if (_fool == StateFlag.InstantDeath || _stateFlags.Contains(StateFlag.InstantDeath))
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
            _gDamage = (_playerController.CurrentPlayerHP - av) * -1f;
            _playerController.NoEffectHPChange((gc.CurrentEnemyHP - av) * -1f);
        }
        //フラグが立つとダメージを受けるが攻撃力が超アップ
        else if (_stateFlags.Contains(StateFlag.RiskyAttack) || _fool == StateFlag.RiskyAttack)
        {
            _playerController.NoEffectHPChange(40);
            _gDamage = _gDamage * 4;
        }
        //フラグが立つとダメージアップ
        else if (_stateFlags.Contains(StateFlag.PowerUp) || _fool == StateFlag.PowerUp)
        {
            _gDamage = _gDamage * 1.5f;
            Debug.Log("クリティカル！！");
        }

        //フラグが立つとHPアップ
        if ((_stateFlags.Contains(StateFlag.HPUp) || _fool == StateFlag.HPUp))
        {
            _playerController.PlayerHPUp(20f);
        }

        //全回復
        if ((_stateFlags.Contains(StateFlag.PowerHeal) || _fool == StateFlag.PowerHeal))
        {
            _playerController.NoEffectHPChange(-10000);
            Debug.Log("全回復");
        }
        //フラグが立つと回復
        else if ((_stateFlags.Contains(StateFlag.Heal) || _fool == StateFlag.Heal) && !_stateFlags.Contains(StateFlag.Average))
        {
            _playerController.NoEffectHPChange(-40);
        }

        //無敵
        if (_stateFlags.Contains(StateFlag.ParfectGuard) || _fool == StateFlag.ParfectGuard)
        {
            _pDamage = 0;
        }
        //フラグが立つとガードアップ
        else if ((_stateFlags.Contains(StateFlag.GuardUp) || _fool == StateFlag.GuardUp) && !_stateFlags.Contains(StateFlag.Average))
        {
            _pDamage = _pDamage / 2f;
            Debug.Log("ガードアップ！！");
        }

        //ランダムで受けるダメージを0にする
        if (_stateFlags.Contains(StateFlag.RandomGuard) || _fool == StateFlag.RandomGuard)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                _pDamage = 0;
                Debug.Log("guard成功");
            }
        }
        //自傷damage
        if ((_stateFlags.Contains(StateFlag.SelfHarm) || _fool == StateFlag.SelfHarm))
        {
            _playerController.PlayerHPUp(20f);
        }

        // フラグの初期化
        _stateFlags.Clear();
        _fool = StateFlag.Normal;

        //エネミーにダメージを与える
        gc.DecreaseEnemyHP(_gDamage, instanceDeath);
        
        //UIなどの後処理
        _selectedCard.ForEach(i => i.GetComponent<Image>().color = Color.white);
        _cardMuzzles.ForEach(i => i.SetActive(false));
        SelectButton();
    }

    /// <summary>エンドターンで行うUIの操作</summary>
    void EndTurnUI()
    {
        _select = false;
        _selectAndEnd.ForEach(i => i.SetActive(false));
        _cardMuzzles.ForEach(i => i.SetActive(false));
        _gameManager.EndTurn();
    }

    /// <summary>
    /// 現在のWaveを表示する
    /// </summary>
    /// <param name="currentWave"></param>
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
        Heal,
        RiskyAttack,
        ParfectGuard,
        HPUp,
        PowerHeal,
        RandomGuard,
        SelfHarm,
        Star,
        Moon,
        Sun
    }

    /// <summary>
    /// プレイヤーが攻撃を受ける際に呼び出す
    /// </summary>
    public void EffectEnd()
    {
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            _playerController.PlayerDamage(_pDamage);
            Array.ForEach(GameObject.FindGameObjectsWithTag("Enemy"), i => i.GetComponent<Animator>().Play("EnemyAttack"));
        }
    }

    /// <summary>fadeで使える関数</summary>
    public void Fade(float fadeEndValue, Color panelColor, TweenCallback fadeOnComplete = null)
    {
        fadeOnComplete += () => _fadePanel.SetActive(false);
        _fadePanel.SetActive(true);
        Image fadeImage = _fadePanel.GetComponent<Image>();
        fadeImage.color = panelColor;
        fadeImage.DOFade(fadeEndValue, 1).OnComplete(fadeOnComplete);
    }

    private void OnDisable()
    {
        _gameManager.OnBeginTurn -= BeginTurnUI;
        _gameManager.OnEndTurn -= EndTurnUI;
    }
}
