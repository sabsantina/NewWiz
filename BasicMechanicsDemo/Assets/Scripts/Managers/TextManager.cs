using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] private RPGTalk _rpgTalkContainer;
    [SerializeField] private List<RPGTalkArea> _talkAreas;
    [SerializeField] private List<TextAsset> _textAssets;
    // Use this for initialization
    private void Start()
    {
        _rpgTalkContainer = Instantiate(_rpgTalkContainer);
        _rpgTalkContainer.dialoger = true;
        _rpgTalkContainer.textUI = _rpgTalkContainer.gameObject.FindComponentsInChildrenWithTag<Text>("Text")[0];
        _rpgTalkContainer.dialogerUI = _rpgTalkContainer.gameObject.FindComponentsInChildrenWithTag<Text>("Name")[0];
        _rpgTalkContainer.passWithMouse = false;
        _rpgTalkContainer.enableQuickSkip = true;
        _rpgTalkContainer.passWithInputButton = "Interact";
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

public static class Helper
{
    public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
    {
        if(parent == null) { throw new System.ArgumentNullException(); }
        if(string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }
        List<T> list = new List<T>(parent.GetComponentsInChildren<T>(forceActive));
        if(list.Count == 0) { return null; }
 
        for(int i = list.Count - 1; i >= 0; i--) 
        {
            if (list[i].CompareTag(tag) == false)
            {
                list.RemoveAt(i);
            }
        }
        return list.ToArray();
    }
 
    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
    {
        if (parent == null) { throw new System.ArgumentNullException(); }
        if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }
 
        T [] list = parent.GetComponentsInChildren<T>(forceActive);
        foreach(T t in list)
        {
            if (t.CompareTag(tag) == true)
            {
                return t;
            }
        }
        return null;
    }
}