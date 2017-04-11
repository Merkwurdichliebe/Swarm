using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

	public GameObject cubePrefab;
	public GameObject attractorPrefab;
	public int cubeCount;
	public int speedMin = 1;				// Minimum object speed
	public int speedMax = 50;				// Maximum object speed
	public float minInterval = 0f;			// Shortest time before turn
	public float maxInterval = 5f;			// Longest time before turn
	public float maxDistToAttractor = 10;	// Further away objects start approaching the Attractor
	public int attractorVolume = 10;		// Variation for Attractor direction
	public float attractorThreshold = 100;	// Percentage of chance for object too far away to approach the Attractor

	private GameObject attractor;
	private GameObject cube;

	void Awake() {
		attractor = Instantiate (attractorPrefab, Vector3.zero, Quaternion.identity);
		attractor.name = "Attractor";
		for (int i=0; i < cubeCount; i++) {
			Vector3 pos = Utilities.randomVectorInRange (10);
			cube = Instantiate(cubePrefab, pos, Quaternion.identity);
			cube.name = "Cube " + i;
		}
	}
}
