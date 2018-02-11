using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour {

	[Header("Setup Options")]
	public static int maxBugs = 1000;
	public static int startBugs = 1000;
	public static float speedMin = 1f;				// Minimum bug speed
	public static float speedMax = 50f;				// Maximum bug speed
	public static float turnMin = 0f;				// Shortest time before turn
	public static float turnMax = 5f;				// Longest time before turn
	public static float distanceMax = 10;			// Further away objects start approaching the Attractor
	public static float attractorVolume = 10;		// Variation for Attractor direction
	public static float attractorThreshold = 100;	// Percentage of chance for object too far away to approach the Attractor
	public static float colliderScale = 1;			// Used to scale the bug collider for paranoia level
	public static float averageLifespan = 30;		// Average lifespan in seconds
}
