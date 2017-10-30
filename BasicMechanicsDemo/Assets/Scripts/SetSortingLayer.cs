using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**Script to be used on objects which draw order needs to be decided at runtime*/
public class SetSortingLayer : MonoBehaviour {
    public string sortingLayerName;
    /**This array will store all sprite renderers from any children, so the script can be added to the parent object.*/
    private SpriteRenderer[] sprites;
    void Start () {
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName = sortingLayerName;
        sprites = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingLayerName = sortingLayerName;
            //This line calculates the number of the sort order for each object.
            sprites[i].sortingOrder = Mathf.RoundToInt(sprites[i].GetComponent<Transform>().transform.position.z * 100f) * -1;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
