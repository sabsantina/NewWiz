using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotKeys : MonoBehaviour {
	public ItemClass item;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void useItem()
	{
		if (item != null) {
			GetComponentInParent<PlayerInventory> ().itemEffectUsage (item);
			int numberOfItem = GetComponentInParent<PlayerInventory> ().getNumberItem (item);
			gameObject.transform.parent.GetComponentInChildren<Text> ().text = "" + numberOfItem;
		} else
			Debug.Log ("No item in hotkey");
	}

}
