/*
* A script to be attached to anything in the scenery that could obstruct the player's movement (i.e. a tree, a boulder).
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Whatever has this script MUST have a collider
[RequireComponent(typeof(Collider))]
public class Obstructable : MonoBehaviour {


}
