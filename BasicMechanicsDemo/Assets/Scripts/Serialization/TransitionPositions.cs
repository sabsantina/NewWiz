using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransitionPositions {
	

	/**The position the player should be in in the Overworld scene if they go from the demo to the Overworld.*/
	public static Vector3 Transition_Demo_To_Overworld = (new Vector3(-28.83f, 0.55f, 1.19f));
	/**The position the player should be in in the Demo scene if they go from the Overworld to the demo.*/
	public static Vector3 Transition_Overworld_To_Demo = (new Vector3 (21.0f, 0.55f, -5.750586f));
    /**The position the player should be in in the Overworld scene if they go from the Left of Soirhbeach to the Overworld.*/
    public static Vector3 Transition_Soirhbeach_Left_To_Overworld = (new Vector3(46.0f, 0.55f, -19.0f));
    /**The position the player should be in in the Overworld scene if they go from the Right of Soirhbeach to the Overworld.*/
    public static Vector3 Transition_Soirhbeach_Right_To_Overworld = (new Vector3(75.0f, 0.55f, -19.0f));
    /**The position the player should be in in the Overworld scene if they go from the Right of Soirhbeach to the Overworld.*/
    public static Vector3 Transition_Overworld_To_Left_Soirhbeach = (new Vector3(-73.0f, 0.55f, -2.0f));

}
