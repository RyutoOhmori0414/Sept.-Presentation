using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [Header("�{�^���֌W")]
    [Tooltip("�J�[�hSelect�ƃG���h�{�^��")]
    [SerializeField] 
    List<GameObject> _selectAndEnd = new List<GameObject>();
    [Tooltip("�J�[�h�I������߂�{�^��")]
    [SerializeField] 
    GameObject _backButton;

    [Header("�e�L�X�g�֌W")]
    [Tooltip("���Ɖ����I�ׂ邩�\������e�L�X�g")]
    [SerializeField] 
    Text _selectableCard;
    [Tooltip("Wave���ς�����ۂɌ��݂�Wave����\������"), SerializeField]
    GameObject _waveTextgo;
    [Tooltip("�I��ł���J�[�h�̌��ʂ�\������e�L�X�g�{�b�N�X"), SerializeField]
    GameObject _cardStateTextBox;

    [Header("�J�[�h�֌W")]
    [Tooltip("�J�[�h"), SerializeField]
    List<GameObject> _cardMuzzles = new List<GameObject>();
    [Tooltip("�J�[�h�̃A�j���[�V����"), SerializeField]
    Animator _cardAnim;
    List<Image> _cardImages = new List<Image>();
    List<Button> _cardButtons = new List<Button>();
    [Tooltip("�J�[�h�̃X�v���C�g")]
    [SerializeField] List<Sprite> _cardSprite = new List<Sprite>();
    [Tooltip("�I�΂ꂽ�J�[�h")]
    List<GameObject> _selectedCard = new List<GameObject>();
    [Tooltip("�I�ׂ�J�[�h�̖���"), SerializeField] 
    int _cards = default;

    [Header("���̑�")]
    [Tooltip("�t�F�[�h�Ɏg���p�l��"), SerializeField]
    GameObject _fadePanel;
    
    /// <summary>�I��ł���J�[�h�̌��ʂ�\������e�L�X�g</summary>
    Text _cardStateText;

    PlayerController _playerController;
    GameManager _gameManager;
    EventSystem _currentES = default;
    GameSceneAudioController _gameSceneAudioController;
    GameObject _lastSelectedObj;

    StateFlag _fool = StateFlag.Normal;
    List<StateFlag> _stateFlags = new List<StateFlag>();

    /// <summary>�v���C���[���G�l�~�[�ɗ^����_���[�W</summary>
    float _gDamage = default;
    /// <summary>�G�l�~�[���v���C���[�ɗ^����_���[�W</summary>
    float _pDamage = default;
    /// <summary>�������J�[�h��I��ł���Ƃ�true</summary>
    bool _select = false;

    private void Update()
    {
        //�t�H�[�J�X���O�ꂽ�ꍇ�A��O�Ƀt�H�[�J�X���Ă������̂Ƀt�H�[�J�X��߂�
        if (!_currentES.currentSelectedGameObject)
        {
            _currentES.SetSelectedGameObject(_lastSelectedObj);
        }
        //�t�H�[�J�X���Ă�����̂��ς�����Ƃ��ɍs���鏈��
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
        //�J�[�h�̃{�^���ƃC���[�W��z��ɓ����
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

    /// <summary>�^�[�����n�܂����ۂ�UI�̊Ǘ�</summary>
    void BeginTurnUI()
    {
        _select = true;
        _selectAndEnd.ForEach(i => i.SetActive(true));
        _cardMuzzles.ForEach(i => i.SetActive(true));
        ShuffleCard();
        _selectedCard.Clear();
        _selectableCard.text = $"�c��{_cards}��";
        _backButton.SetActive(false);
    }

    /// <summary>�J�[�hSelect��ʂɈڂ�Ƃ��ɍs��UI�̊Ǘ�</summary>
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

    /// <summary>�J�[�hSelect����߂�Ƃ���UI�̊Ǘ�</summary>
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

    /// <summary>�I������J�[�h���V���b�t������</summary>
    void ShuffleCard()
    {
        //�J�[�h�̃X�v���C�g���R�s�[����
        var copySprite = new List<Sprite>(_cardSprite);
        //�J�[�h�̃C���[�W�������_���Ŕ��Ȃ��悤�ɕ\��������
        _cardImages.ForEach(i =>
        {
            int rSpriteIndex = UnityEngine.Random.Range(0, copySprite.Count);
            i.sprite = copySprite[rSpriteIndex];
            i.SetNativeSize();
            copySprite.RemoveAt(rSpriteIndex);
        });
    }
    
    /// <summary>
    /// �J�[�h��Select���Ă���ۂ�Select���Ă���J�[�h�̌��ʂ��o�͂���
    /// </summary>
    /// <param name="sGameObject"></param>
    public void OnSelect(GameObject sGameObject)
    {
        //�t�H�[�J�X���������Ă���̂��J�[�h���ǂ����m�F
        if (sGameObject.CompareTag("Card"))
        {
            string selectedSpriteName = sGameObject.GetComponent<Image>().sprite.name;

            //�J�[�h�̃X�v���C�g�̖��O�Ŕ��肵�Č��ʂ̃e�L�X�g��ς���
            if (selectedSpriteName.Contains("Death") || selectedSpriteName.Contains("HangedMan"))
            {
                _cardStateText.text = "�m���ő������ʂ��t�^�����";
            }
            else if (selectedSpriteName.Contains("Magician") || selectedSpriteName.Contains("Strength") || selectedSpriteName.Contains("Emperor"))
            {
                _cardStateText.text = "���g�̍U���͂��ꎞ�I�ɏ㏸����I";
            }
            else if (selectedSpriteName.Contains("Chariot"))
            {
                _cardStateText.text = "���g�̖h��͂��ꎞ�I�ɏ㏸����I";
            }
            else if (selectedSpriteName.Contains("Temperance") || selectedSpriteName.Contains("Judgement"))
            {
                _cardStateText.text = "�U���ΏۂƁA������HP�𓙂����z�����Ȃ���";
            }
            else if (selectedSpriteName.Contains("Empress") || selectedSpriteName.Contains("Lovers"))
            {
                _cardStateText.text = "���g�̍U���ɉ񕜌��ʂ��t�^�����";
            }
            else if (selectedSpriteName.Contains("Hierophant"))
            {
                _cardStateText.text = "�̗͂�S��";
            }
            else if (selectedSpriteName.Contains("Priestes"))
            {
                _cardStateText.text = "�̗͏���A�b�v";
            }
            else if (selectedSpriteName.Contains("Fool") || selectedSpriteName.Contains("Fortune"))
            {
                _cardStateText.text = "�����_���Œǉ����ʂ���������";
            }
            else if (selectedSpriteName.Contains("Tower"))
            {
                _cardStateText.text = "�̗͂��������ɋ��͂ȍU��";
            }
            else if (selectedSpriteName.Contains("Justice") || selectedSpriteName.Contains("World"))
            {
                _cardStateText.text = "1�^�[�����G�ɂȂ�";
            }
            else if (selectedSpriteName.Contains("Hermit"))
            {
                _cardStateText.text = "�����_����1�^�[�����G�ɂȂ�";
            }
            else if (selectedSpriteName.Contains("Devil"))
            {
                _cardStateText.text = "�����Ƀ_���[�W������܂�";
            }
            else if (selectedSpriteName.Contains("Moon") || selectedSpriteName.Contains("Star") || selectedSpriteName.Contains("Sun"))
            {
                _cardStateText.text = "���ƌ��Ƒ��z���Ƃ�ƒ���_���[�W";
            }
            else
            {
                _cardStateText.text = "���ɂȂ�";
            }
        }
        //�J�[�h���I�΂�Ă��Ȃ��Ƃ��͉����\�����Ȃ�
        else
        {
            _cardStateText.text = "";
        }
    }

    /// <summary>
    /// �J�[�h�i�{�^���j�������ꂽ�Ƃ��ɂ��̃J�[�h��GameObject������Ă���
    /// </summary>
    /// <param name="go"></param>
    public void ButtonGetGameObject(GameObject go)
    {
        //�I�񂾃J�[�h��������Ă���z���go�������Ă��Ȃ��Ƃ��t���O�𗧂ĂāA���̔z��ɉ�����
        if (!_selectedCard.Contains(go))
        {
            _selectedCard.Add(go);
            Image image = go.GetComponent<Image>();
            image.color = Color.gray;

            //�X�v���C�g�̖��O�Ŕ��f���ăt���O�𗧂Ă�
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
                //�����_�����ʂ̏ꍇ�ꊇ�Ǘ�����ƃt���O���������ۂɓ���Ȃ邽�ߕʂŕϐ���p�ӂ��Ă����ŊǗ�����
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
        //���łɂ��̃J�[�h���I�΂�Ă���ꍇ�͑I����Ԃ��������āA���Ă�ꂽ�t���O������
        else
        {
            _selectedCard.Remove(go);
            Image image = go.GetComponent<Image>();
            image.color = Color.white;

            //�I�΂ꂽ�J�[�h�̑I�����������ꂽ�ۃt���O��������
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

        //�c��I���\������\�����Ă���
        _selectableCard.text = $"�c��{_cards - _selectedCard.Count}��";

        //�I���\����
        if (_selectedCard.Count + 1 > _cards)
        {
            //�U������L�����N�^�[��I�������ʂɈړ�
            _cardStateTextBox.SetActive(false);
            _backButton.SetActive(false);
            _cardMuzzles.ForEach(i => i.SetActive(false));

            //�G�l�~�[�𒼐ڑI�ׂȂ��悤�ɂ���
            var enemyTargets = GameObject.FindGameObjectsWithTag("ArrowMark");
            Array.ForEach(Array.ConvertAll(enemyTargets, i => i.GetComponent<Button>()), i => i.interactable = true);
                
            _currentES.SetSelectedGameObject(enemyTargets[0]);
        }
    }

    /// <summary>
    /// �����Ă����t���O���g���_���[�W���v�Z����
    /// </summary>
    /// <param name="gc"></param>
    public void PlayerAttack(GoblinController gc)
    {
        _gDamage = _playerController.PlayerAttack;
        _pDamage = gc.Attack;
        bool instanceDeath = false;
        //�_���[�W�␳��ǉ�
        //���Ƒ��z�ƌ��̃t���O�������Ă����ꍇ�ɓ���
        if (_stateFlags.Contains(StateFlag.Star) && _stateFlags.Contains(StateFlag.Sun) && _stateFlags.Contains(StateFlag.Moon))
        {
            _gDamage = _gDamage * 20;
        }
        //�t���O�����Ƒ������ʂ��m���Ŕ���
        else if (_fool == StateFlag.InstantDeath || _stateFlags.Contains(StateFlag.InstantDeath))
        {
            if (UnityEngine.Random.Range(0, 10) < 5)
            {
                instanceDeath = true;
                Debug.Log("�����I�I");
            }
            else
            {
                Debug.Log("�������s");
            }
        }
        //�t���O�����ƓG�Ǝ�����HP����l�̕��ςɂȂ�
        else if ((_stateFlags.Contains(StateFlag.Average) || _fool == StateFlag.Average))
        {
            float av = (_playerController.CurrentPlayerHP + gc.CurrentEnemyHP) / 2;
            _gDamage = (_playerController.CurrentPlayerHP - av) * -1f;
            _playerController.NoEffectHPChange((gc.CurrentEnemyHP - av) * -1f);
        }
        //�t���O�����ƃ_���[�W���󂯂邪�U���͂����A�b�v
        else if (_stateFlags.Contains(StateFlag.RiskyAttack) || _fool == StateFlag.RiskyAttack)
        {
            _playerController.NoEffectHPChange(40);
            _gDamage = _gDamage * 4;
        }
        //�t���O�����ƃ_���[�W�A�b�v
        else if (_stateFlags.Contains(StateFlag.PowerUp) || _fool == StateFlag.PowerUp)
        {
            _gDamage = _gDamage * 1.5f;
            Debug.Log("�N���e�B�J���I�I");
        }

        //�t���O������HP�A�b�v
        if ((_stateFlags.Contains(StateFlag.HPUp) || _fool == StateFlag.HPUp))
        {
            _playerController.PlayerHPUp(20f);
        }

        //�S��
        if ((_stateFlags.Contains(StateFlag.PowerHeal) || _fool == StateFlag.PowerHeal))
        {
            _playerController.NoEffectHPChange(-10000);
            Debug.Log("�S��");
        }
        //�t���O�����Ɖ�
        else if ((_stateFlags.Contains(StateFlag.Heal) || _fool == StateFlag.Heal) && !_stateFlags.Contains(StateFlag.Average))
        {
            _playerController.NoEffectHPChange(-40);
        }

        //���G
        if (_stateFlags.Contains(StateFlag.ParfectGuard) || _fool == StateFlag.ParfectGuard)
        {
            _pDamage = 0;
        }
        //�t���O�����ƃK�[�h�A�b�v
        else if ((_stateFlags.Contains(StateFlag.GuardUp) || _fool == StateFlag.GuardUp) && !_stateFlags.Contains(StateFlag.Average))
        {
            _pDamage = _pDamage / 2f;
            Debug.Log("�K�[�h�A�b�v�I�I");
        }

        //�����_���Ŏ󂯂�_���[�W��0�ɂ���
        if (_stateFlags.Contains(StateFlag.RandomGuard) || _fool == StateFlag.RandomGuard)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                _pDamage = 0;
                Debug.Log("guard����");
            }
        }
        //����damage
        if ((_stateFlags.Contains(StateFlag.SelfHarm) || _fool == StateFlag.SelfHarm))
        {
            _playerController.PlayerHPUp(20f);
        }

        // �t���O�̏�����
        _stateFlags.Clear();
        _fool = StateFlag.Normal;

        //�G�l�~�[�Ƀ_���[�W��^����
        gc.DecreaseEnemyHP(_gDamage, instanceDeath);
        
        //UI�Ȃǂ̌㏈��
        _selectedCard.ForEach(i => i.GetComponent<Image>().color = Color.white);
        _cardMuzzles.ForEach(i => i.SetActive(false));
        SelectButton();
    }

    /// <summary>�G���h�^�[���ōs��UI�̑���</summary>
    void EndTurnUI()
    {
        _select = false;
        _selectAndEnd.ForEach(i => i.SetActive(false));
        _cardMuzzles.ForEach(i => i.SetActive(false));
        _gameManager.EndTurn();
    }

    /// <summary>
    /// ���݂�Wave��\������
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
    /// �v���C���[���U�����󂯂�ۂɌĂяo��
    /// </summary>
    public void EffectEnd()
    {
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            _playerController.PlayerDamage(_pDamage);
            Array.ForEach(GameObject.FindGameObjectsWithTag("Enemy"), i => i.GetComponent<Animator>().Play("EnemyAttack"));
        }
    }

    /// <summary>fade�Ŏg����֐�</summary>
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
