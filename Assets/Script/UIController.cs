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

    /// <summary>����</summary>
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
        _selectableCard.text = $"�c��{GameManager.Instance.TurnCount}��";
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
        }

        int CurrentSelectableCards = GameManager.Instance.TurnCount % 5;
        _selectableCard.text = $"�c��{CurrentSelectableCards - _selectedCard.Count + 1}��";

        if (_selectedCard.Count > CurrentSelectableCards)
        {
            float damage = PlayerController.PlayerAttack;
            //�_���[�W�␳��ǉ�
            //�t���O�����Ƒ������ʂ��m���Ŕ���
            if(_instantDeath > 0)
            {
                damage = 3000f;
                _instantDeath = 0;
                Debug.Log("�����I�I");
            }
            //�t���O�����ƃ_���[�W�A�b�v
            else if (_powerUp)
            {
                damage = damage * 1.5f;
                _powerUp = false;
                Debug.Log("�N���e�B�J���I�I");
            }
            //�t���O�����ƃK�[�h�A�b�v
            else if (_guardUp)
            {
                GoblinController.CurrentAttack = 0.5f;
                Debug.Log("�K�[�h�A�b�v�I�I");
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
