using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningEffectController : MonoBehaviour
{
    public void EffectEnd()
    {
        Array.ForEach(FindObjectsOfType<Button>(), i => i.interactable = true);
    }
}
