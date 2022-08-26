using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [Tooltip("カードを生成するposition")]
    [SerializeField] List<Transform> _cardMuzzleTransform = new List<Transform>();
    [Tooltip("カードのスプライト")]
    [SerializeField] List<Sprite> _cardSprite = new List<Sprite>();

}
