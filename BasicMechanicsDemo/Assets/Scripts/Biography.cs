#define testing

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Biography : MonoBehaviour 
{
	#if testing
	string part1 = "";

	#endif
	
	Text bioText;
	RectTransform rectTrans;
	// Use this for initialization
	void Start () 
	{
		bioText = gameObject.GetComponentInChildren<Text> ();
		rectTrans = gameObject.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		#if testing
		if(Input.GetKeyDown(KeyCode.F1))
		{
			
		}
		#endif
	}



}
