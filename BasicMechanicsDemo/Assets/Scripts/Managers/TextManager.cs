using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private RPGTalk _RPGTalkContainer;
    [SerializeField] private List<RPGTalkArea> talkAreas;

    // Use this for initialization
    private void Start()
    {
        _RPGTalkContainer = Instantiate(_RPGTalkContainer);
        _RPGTalkContainer.GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        foreach (RPGTalkArea talkArea in talkAreas)
        {
            talkArea.rpgtalkTarget = _RPGTalkContainer;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}