using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [Tooltip("�J�[�h�𐶐�����position")]
    [SerializeField] List<Transform> _cardMuzzleTransform = new List<Transform>();
    [Tooltip("�J�[�h�̃X�v���C�g")]
    [SerializeField] List<Sprite> _cardSprite = new List<Sprite>();

}
