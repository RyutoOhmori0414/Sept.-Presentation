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
    [Header("通常")]
    [Tooltip("通常時表示されるButton"), SerializeField]
    Button[] _nomalButton;

    [Tooltip("フェードに使うパネル"), SerializeField]
    GameObject _fadeImage;

    [SerializeField]
    TitleAudioController _audioController;
    //[Header("ヘルプ")]
    //[Tooltip("ヘルプ時に表示されるPanel"), SerializeField]
    //GameObject _helpPanel;

    //[Header("設定")]
    //[Tooltip("設定時に表示されるPanel")]
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
