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
    [SerializeField] List<GameObject> _selectAndEnd = new List<GameObject>();
    [Tooltip("�J�[�h�I������߂�{�^��")]
    [SerializeField] GameObject _backButton;

    [Header("�e�L�X�g�֌W")]
    [Tooltip("���Ɖ����I�ׂ邩�\������e�L�X�g")]
    [SerializeField] Text _selectableCard;
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
    [Header("�I�ׂ�J�[�h�̖���"), SerializeField] int _cards = default;
    
    /// <summary>�I��ł���J�[�h�̌��ʂ�\������e�L�X�g</summary>
    Text _cardStateText;

    PlayerController _playerController;
    EventSystem _currentES = default;

    GameObject _lastSelectedObj;

    StateFlag _fool = StateFlag.Normal;
    List<StateFlag> _stateFlags = new List<StateFlag>();

    /// <summary>�v���C���[���G�l�~�[�ɗ^����_���[�W</summary>
    float _gDamage = default;
    /// <summary>�G�l�~�[���v���C���[�ɗ^����_���[�W</summary>
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
        _selectableCard.text = $"�c��{_cards}��";
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
        Debug.Log("Test������");
        Debug.Log(eventData.selectedObject.name);
    }

    //�J�[�h��Select���Ă���ۂ�Select���Ă���J�[�h�̌��ʂ��o�͂���
    public void OnSelect(GameObject sgameObject)
    {
        if (sgameObject.CompareTag("Card"))
        {
            string selectedSpriteName = sgameObject.GetComponent<Image>().sprite.name;

            if (selectedSpriteName.Contains("���_") || selectedSpriteName.Contains("�݂邳�ꂽ�j"))
            {
                _cardStateText.text = "�m���ő������ʂ��t�^�����";
            }
            else if (selectedSpriteName.Contains("��"))
            {
                _cardStateText.text = "���g�̍U���͂��ꎞ�I�ɏ㏸����I";
            }
            else if (selectedSpriteName.Contains("���"))
            {
                _cardStateText.text = "���g�̖h��͂��ꎞ�I�ɏ㏸����I";
            }
            else if (selectedSpriteName.Contains("�ߐ�"))
            {
                _cardStateText.text = "�U���ΏۂƁA������HP�𓙂����z�����Ȃ���";
            }
            else if (selectedSpriteName.Contains("����"))
            {
                _cardStateText.text = "���g�̍U���ɉ񕜌��ʂ��t�^�����";
            }
            else if (selectedSpriteName.Contains("����"))
            {
                _cardStateText.text = "�����_���Œǉ����ʂ���������";
            }
            else
            {
                _cardStateText.text = "���ɂȂ�";
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

            //������ʂ�����J�[�h���I�΂ꂽ�ۃt���O�𗧂Ă�
            if (image.sprite.name.Contains("���_") || image.sprite.name.Contains("�݂邳�ꂽ�j"))
            {
                _stateFlags.Add(StateFlag.InstantDeath);
            }
            else if (image.sprite.name.Contains("��"))
            {
                _stateFlags.Add(StateFlag.PowerUp);
            }
            else if (image.sprite.name.Contains("���"))
            {
                _stateFlags.Add(StateFlag.GuardUp);
            }
            else if (image.sprite.name.Contains("�ߐ�"))
            {
                _stateFlags.Add(StateFlag.Average);
            }
            else if (image.sprite.name.Contains("����"))
            {
                _stateFlags.Add(StateFlag.Heal);
            }
            else if (image.sprite.name.Contains("����"))
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

            //������ʂ�����J�[�h�̑I�����������ꂽ�ۃt���O��������
            if (image.sprite.name.Contains("���_") || image.sprite.name.Contains("��"))
            {
                _stateFlags.Remove(StateFlag.InstantDeath);
            }
            else if (image.sprite.name.Contains("��"))
            {
                _stateFlags.Remove(StateFlag.PowerUp);
            }
            else if (image.sprite.name.Contains("���"))
            {
                _stateFlags.Remove(StateFlag.GuardUp);
            }
            else if (image.sprite.name.Contains("�ߐ�"))
            {
                _stateFlags.Remove(StateFlag.Average);
            }
            else if (image.sprite.name.Contains("����"))
            {
                _stateFlags.Remove(StateFlag.Heal);
            }
            else if (image.sprite.name.Contains("����"))
            {
                _fool = StateFlag.Normal;
            }
        }

        _selectableCard.text = $"�c��{_cards - _selectedCard.Count}��";

        if (_selectedCard.Count + 1 > _cards)
        {
            //�U������L�����N�^�[��I�������ʂɈړ�
            _cardStateTextBox.SetActive(false);
            _backButton.SetActive(false);
            _cardMuzzles.ForEach(i => i.SetActive(false));

            //���ڑI�ׂȂ��悤�ɂ���
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
        //�_���[�W�␳��ǉ�
        //�t���O�����Ƒ������ʂ��m���Ŕ���
        if (_fool == StateFlag.InstantDeath || _stateFlags.Contains(StateFlag.InstantDeath))
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
            _gDamage = -(_playerController.CurrentPlayerHP - av);
            _pDamage = -(gc.CurrentEnemyHP - av);
        }
        //�t���O�����ƃ_���[�W�A�b�v
        else if (_stateFlags.Contains(StateFlag.PowerUp) || _fool == StateFlag.PowerUp)
        {
            _gDamage = _gDamage * 1.5f;
            Debug.Log("�N���e�B�J���I�I");
        }
        //�t���O�����Ɖ�
        if ((_stateFlags.Contains(StateFlag.Heal) || _fool == StateFlag.Heal) && !_stateFlags.Contains(StateFlag.Average))
        {
            _playerController.PlayerDamage(-10);
            _pDamage = 0;
        }
        //�t���O�����ƃK�[�h�A�b�v
        if ((_stateFlags.Contains(StateFlag.GuardUp) || _fool == StateFlag.GuardUp) && !_stateFlags.Contains(StateFlag.Average))
        {
            _pDamage = _pDamage / 2f;
            Debug.Log("�K�[�h�A�b�v�I�I");
        }

        // �t���O�̏�����
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
