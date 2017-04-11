using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {

	// Returns a random Vector3
	public static Vector3 randomVectorInRange(float range) {
		return new Vector3 (Random.Range (-range, range), Random.Range (-range, range), Random.Range (-range, range));
	}

	// Maps a value from 0 to 1 to another arbitrary range
	public static float map01 (float value, float newMin, float newMax) {
		return value * (newMax - newMin) + newMin;
	}
	
	// Maps a value from one arbitrary range to another arbitrary range
	public static float map( float value, float leftMin, float leftMax, float rightMin, float rightMax )  
	{
		return rightMin + ( value - leftMin ) * ( rightMax - rightMin ) / ( leftMax - leftMin );
	}
}
