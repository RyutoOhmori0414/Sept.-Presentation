using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Tooltip("�J�[�hSelect�ƃG���h�{�^��")]
    [SerializeField] List<GameObject> _selectAndEnd = new List<GameObject>();
    [Tooltip("�J�[�h�I������߂�{�^��")]
    [SerializeField] GameObject _backButton;
    [Tooltip("���Ɖ����I�ׂ邩�\������e�L�X�g")]
    [SerializeField] Text _selectableCard;
    [SerializeField] List<GameObject> _cardMuzzles = new List<GameObject>();
    List<Image> _cardImages = new List<Image>();
    [Tooltip("�J�[�h�̃X�v���C�g")]
    [SerializeField] List<Sprite> _cardSprite = new List<Sprite>();
    [Tooltip("�I�΂ꂽ�J�[�h")]
    List<GameObject> _selectedCard = new List<GameObject>();
    [Tooltip("�U���Ώ�")]
    [SerializeField] GameObject _enemy;
    [Header("�I�ׂ�J�[�h�̖���"), SerializeField] int _cards = default;

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
        _selectableCard.text = $"�c��{_cards}��";
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

            //������ʂ�����J�[�h���I�΂ꂽ�ۃt���O�𗧂Ă�
            if (image.sprite.name.Contains("���_") || image.sprite.name.Contains("��"))
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
            float gDamage = _playerController.PlayerAttack;
            float pDamage = _goblinController.Attack;
            //�_���[�W�␳��ǉ�
            //�t���O�����Ƒ������ʂ��m���Ŕ���
            if (_fool == StateFlag.InstantDeath || _stateFlags.Contains(StateFlag.InstantDeath))
            {
                if (Random.Range(0, 10) < 5)
                {
                    gDamage = 3000f;
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
                float av = (_playerController.CurrentPlayerHP + _goblinController.CurrentEnemyHP) / 2;
                gDamage = -(_playerController.CurrentPlayerHP - av);
                pDamage = -(_goblinController.CurrentEnemyHP - av);
            }
            //�t���O�����ƃ_���[�W�A�b�v
            else if (_stateFlags.Contains(StateFlag.PowerUp) || _fool == StateFlag.PowerUp)
            {
                gDamage = gDamage * 1.5f;
                Debug.Log("�N���e�B�J���I�I");
            }
            //�t���O�����Ɖ�
            if ((_stateFlags.Contains(StateFlag.Heal) || _fool == StateFlag.Heal) && !_stateFlags.Contains(StateFlag.Average))
            {
                _playerController.PlayerDamage(-10);
                gDamage = 0f;
            }
            //�t���O�����ƃK�[�h�A�b�v
            if ((_stateFlags.Contains(StateFlag.GuardUp) || _fool == StateFlag.GuardUp) && !_stateFlags.Contains(StateFlag.Average))
            {
                pDamage = pDamage / 2f;
                Debug.Log("�K�[�h�A�b�v�I�I");
            }

            // �t���O�̏�����
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
