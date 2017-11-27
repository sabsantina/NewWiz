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
        _rpgTalkContainer.GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        int x;
        foreach (RPGTalkArea talkArea in _talkAreas)
        {
            talkArea.rpgtalkTarget = _rpgTalkContainer;
            talkArea.txtToParse = _textAssets[0];
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}