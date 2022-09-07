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

    /// <summary>����</summary>
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
                _instantDeath++;
            }
            else if (image.sprite.name.Contains("��"))
            {
                _powerUp = true;
            }
            else if (image.sprite.name.Contains("���"))
            {
                _guardUp = true;
            }
            else if (image.sprite.name.Contains("�ߐ�"))
            {
                _average = true;
            }
            else if (image.sprite.name.Contains("����"))
            {
                _heal = true;
            }
            else if (image.sprite.name.Contains("����"))
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

            //������ʂ�����J�[�h�̑I�����������ꂽ�ۃt���O��������
            if (image.sprite.name.Contains("���_") || image.sprite.name.Contains("��"))
            {
                _instantDeath--;
            }
            else if (image.sprite.name.Contains("��"))
            {
                _powerUp = false;
            }
            else if (image.sprite.name.Contains("���"))
            {
                _guardUp = false;
            }
            else if (image.sprite.name.Contains("�ߐ�"))
            {
                _average = false;
            }
            else if (image.sprite.name.Contains("����"))
            {
                _heal = false;
            }
            else if (image.sprite.name.Contains("����"))
            {
                _fool = RandomFlag.Normal;
            }
        }

        _selectableCard.text = $"�c��{_cards - _selectedCard.Count}��";

        if (_selectedCard.Count + 1 > _cards)
        {
            float gDamage = _playerController.PlayerAttack;
            float pDamage = _goblinController.Attack;
            //�_���[�W�␳��ǉ�
            //�t���O�����Ƒ������ʂ��m���Ŕ���
            if (_instantDeath > 0 || _fool == RandomFlag.InstantDeath)
            {
                if (Random.Range(0, 10) < 5)
                {
                    gDamage = 3000f;
                    _instantDeath = 0;
                    Debug.Log("�����I�I");
                }
                else
                {
                    _instantDeath = 0;
                    Debug.Log("�������s");
                }
            }
            //�t���O�����ƓG�Ǝ�����HP����l�̕��ςɂȂ�
            else if ((_average || _fool == RandomFlag.Average) && _instantDeath == 0)
            {
                float av = (_playerController.CurrentPlayerHP + _goblinController.CurrentEnemyHP) / 2;
                gDamage = -(_playerController.CurrentPlayerHP - av);
                pDamage = -(_goblinController.CurrentEnemyHP - av);
            }
            //�t���O�����ƃ_���[�W�A�b�v
            else if (_powerUp || _fool == RandomFlag.PowerUp)
            {
                gDamage = gDamage * 1.5f;
                _powerUp = false;
                Debug.Log("�N���e�B�J���I�I");
            }
            //�t���O�����Ɖ�
            if ((_heal || _fool == RandomFlag.Heal) && !_average)
            {
                _playerController.PlayerDamage(-10);
                gDamage = 0f;
            }
            //�t���O�����ƃK�[�h�A�b�v
            if ((_guardUp || _fool == RandomFlag.GuardUp) && !_average)
            {
                pDamage = pDamage / 2f;
                Debug.Log("�K�[�h�A�b�v�I�I");
            }

            // �t���O�̏�����
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
