using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIController : MonoBehaviour
{
    [Header("通常")]
    [Tooltip("通常時表示されるButton"), SerializeField]
    Button[] _nomalButton;

    //[Header("ヘルプ")]
    //[Tooltip("ヘルプ時に表示されるPanel"), SerializeField]
    //GameObject _helpPanel;

    //[Header("設定")]
    //[Tooltip("設定時に表示されるPanel")]
    //GameObject _settingPanel;

    void Start()
    {
        
    }

    public void ButtonUsable(bool isUsed)
    {
        Array.ForEach(_nomalButton, i => i.interactable = isUsed);
    }
}
