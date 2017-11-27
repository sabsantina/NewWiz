using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private RPGTalk _rpgTalkContainer;
    [SerializeField] private List<RPGTalkArea> _talkAreas;
    [SerializeField] private List<TextAsset> _textAssets;
    // Use this for initialization
    private void Start()
    {
        _rpgTalkContainer = Instantiate(_rpgTalkContainer);
        
        _rpgTalkContainer.GetComponentInChildren<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _rpgTalkContainer.txtToParse = _textAssets[0];

        print(_talkAreas.Count);
        for(int i = 0 ; i < _talkAreas.Count; ++i)
        {
            _talkAreas[i] = Instantiate(_talkAreas[i]);
            _talkAreas[i].rpgtalkTarget = _rpgTalkContainer;
            _talkAreas[i].txtToParse = _textAssets[1];
        }
    }
}