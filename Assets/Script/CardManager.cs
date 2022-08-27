using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [Tooltip("�J�[�h�𐶐�����position")]
    [SerializeField] List<Image> _cardMuzzleSprite = new List<Image>();
    [Tooltip("�J�[�h�̃X�v���C�g")]
    [SerializeField] List<Sprite> _cardSprite = new List<Sprite>();

    private void OnEnable()
    {
        GameManager.Instance.OnBeginTurn += ShuffleCard;
    }

    private void Start()
    {
        ShuffleCard();
    }
    void ShuffleCard()
    {
        var copySprite = _cardSprite;
        foreach (var card in _cardMuzzleSprite)
        {
            int RSpriteIndex = Random.Range(0, copySprite.Count);
            card.sprite = copySprite[RSpriteIndex];
            card.SetNativeSize();
            copySprite.RemoveAt(RSpriteIndex);
        }
    }
}
