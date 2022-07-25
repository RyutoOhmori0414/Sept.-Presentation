using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class TestTextController : MonoBehaviour
{
    [SerializeField] TextAsset _text = default;
    [SerializeField] Text _nameText = default;
    [SerializeField] Text _lineText = default;
    List<string> _name = new List<string>();
    List<string> _line = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        SplitText(_text);
    }

    // Update is called once per frame
    void Update()
    {
        _nameText.text = _name[0];
        _lineText.text = _line[0];
    }

    void SplitText (TextAsset text)
    {
        string[] str = text.text.Split(",");

        foreach(var SText in str)
        {
            if(SText.IndexOf("@name") >= 0)
            {
                _name.Add(SText.Remove(0, 5));
            }
            else if (SText.IndexOf("@line") >= 0)
            {
                _line.Add(SText.Remove(0, 8));
            }
        }
    }
    
    void NextLine()
    {

    }
}
