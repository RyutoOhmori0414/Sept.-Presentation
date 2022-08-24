using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPicturesTest : MonoBehaviour
{
    [SerializeField] Image[] _characterPictures;
    [SerializeField] RectTransform[] _anchors;

    public void SetCharacters(int n)
    {
        for (int i = 0; i < n; i++)
        {
            var anchor = _anchors[n - 1].GetChild(i);
            var character = Instantiate(_characterPictures[i % _characterPictures.Length]);
            character.transform.SetParent(anchor);
            character.rectTransform.anchoredPosition = Vector3.zero;
            character.rectTransform.localScale = Vector3.one;
        }
    }

    public void RemoveCharacters()
    {
        Array.ForEach(GameObject.FindGameObjectsWithTag("Player"), x => Destroy(x));
    }
}
