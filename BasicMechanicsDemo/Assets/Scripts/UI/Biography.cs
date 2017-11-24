#define testing

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Biography : MonoBehaviour 
{
	#if testing
	string[] testText = {
		"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque non ex massa. Aenean bibendum dapibus est, sit amet tempor quam rutrum sit amet. Morbi sodales mollis vulputate. In suscipit ligula dolor, fringilla bibendum justo laoreet et. In commodo dolor sit amet lorem blandit posuere. Phasellus egestas viverra sapien, eget aliquet lectus lobortis eget. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae.\n\n",
		"Donec id leo sit amet tortor aliquet condimentum. Integer a pellentesque ante, nec faucibus mauris. Sed et turpis in neque sagittis pulvinar eget vel orci. Aenean libero ipsum, malesuada nec aliquet et, tempus ut nunc. Vivamus mattis fringilla odio, vel pharetra tortor condimentum ac. Praesent aliquet et urna ac viverra. Nam elit risus, bibendum vitae mauris at, facilisis viverra eros. Nunc mattis nisl mauris. Curabitur tincidunt molestie augue, ut aliquam ex imperdiet non.\n\n",
		"Aliquam a magna sed mauris hendrerit pellentesque. Fusce aliquet ligula quis felis accumsan hendrerit. Suspendisse et luctus enim, sit amet dapibus orci. Aenean vitae lacus scelerisque, fermentum lorem nec, mattis metus. Integer in molestie dui.\n\n",
		"Aenean dictum nisi eu quam interdum, ut elementum erat finibus. Vestibulum sed justo erat. Vivamus molestie diam purus, quis convallis mi condimentum eget. Sed nec pulvinar risus. Quisque eleifend metus nulla, et fringilla mi tincidunt quis. Nam dapibus ligula vulputate odio aliquam tincidunt. Duis blandit magna metus. Nullam ultricies pulvinar varius. Fusce vel turpis vitae nulla gravida condimentum eget eget tellus.\n\n",
		"Fusce maximus eros ut augue volutpat rutrum. Nam vitae vestibulum tellus. Morbi interdum neque et risus fermentum viverra. Quisque vitae venenatis metus, non dictum magna. Cras pellentesque porttitor nibh quis cursus. Sed commodo turpis ac nunc commodo, vel rhoncus nisl rhoncus. Maecenas auctor ut libero quis luctus. Nulla eu augue sed ex pulvinar porttitor vel vitae enim.\n\n"
	};
	int currentIndex = 0;
	#endif

	string currentBio;


	Text bioText;
	RectTransform rectTrans;
	// Use this for initialization
	void Start () 
	{
		bioText = gameObject.GetComponentInChildren<Text> ();
		rectTrans = gameObject.GetComponent<RectTransform> ();
		currentBio = "";
	}
	
	// Update is called once per frame
	void Update () 
	{
		#if testing
		if(Input.GetKeyDown(KeyCode.F1) && currentIndex < testText.Length)
		{
			appendBioText(testText[currentIndex]);
			currentIndex++;
		}
		#endif
	}

	public void appendBioText(string text)
	{
		currentBio += text;
		bioText.text = currentBio;
		changeRectTransHeight (currentBio.Length);
	}

	void changeRectTransHeight(int characterCount)
	{
		if (characterCount/2 > rectTrans.sizeDelta.y) 
		{
			rectTrans.sizeDelta = new Vector2 (rectTrans.sizeDelta.x, rectTrans.sizeDelta.y + characterCount/3);
		}
	}
}
