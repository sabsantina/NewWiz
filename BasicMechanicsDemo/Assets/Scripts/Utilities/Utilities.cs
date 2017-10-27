using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**Class to be used for different utilities.*/
public class Utilities {
    /**This function takes in a percentage and returns true if a random float is smaller than the percentage.*/
	public static bool ProbabilityCheck(float percentage)
    {
        float randomFloat = Random.Range(0.0f, 100.0f);
        if (randomFloat <= percentage)
            return true;
        return false;
    }
}
