using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIController : MonoBehaviour
{
    [Header("�ʏ�")]
    [Tooltip("�ʏ펞�\�������Button"), SerializeField]
    Button[] _nomalButton;

    //[Header("�w���v")]
    //[Tooltip("�w���v���ɕ\�������Panel"), SerializeField]
    //GameObject _helpPanel;

    //[Header("�ݒ�")]
    //[Tooltip("�ݒ莞�ɕ\�������Panel")]
    //GameObject _settingPanel;

    void Start()
    {
        
    }

    public void ButtonUsable(bool isUsed)
    {
        Array.ForEach(_nomalButton, i => i.interactable = isUsed);
    }
}
