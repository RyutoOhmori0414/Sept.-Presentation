using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleUIController : MonoBehaviour
{
    [Header("�ʏ�")]
    [Tooltip("�ʏ펞�\�������Button"), SerializeField]
    Button[] _nomalButton;

    [Tooltip("�t�F�[�h�Ɏg���p�l��"), SerializeField]
    GameObject _fadeImage;

    [SerializeField]
    TitleAudioController _audioController;
    //[Header("�w���v")]
    //[Tooltip("�w���v���ɕ\�������Panel"), SerializeField]
    //GameObject _helpPanel;

    //[Header("�ݒ�")]
    //[Tooltip("�ݒ莞�ɕ\�������Panel")]
    //GameObject _settingPanel;

    GameObject _lastSelectedObj;

    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject)
        {
            EventSystem.current.SetSelectedGameObject(_lastSelectedObj);
        }
        else if (_lastSelectedObj != EventSystem.current.currentSelectedGameObject)
        {
            _audioController.ChooseSEPlay(true);
            _lastSelectedObj = EventSystem.current.currentSelectedGameObject;
        }
    }
    public void ButtonUsable(bool isUsed)
    {
        Array.ForEach(_nomalButton, i => i.interactable = isUsed);
    }

    public void FadeStart()
    {
        _fadeImage.SetActive(true);
        _fadeImage.GetComponent<Image>().DOFade(1f, 1f).OnComplete(() => SceneManager.LoadScene("GameScene"));
    }
}
