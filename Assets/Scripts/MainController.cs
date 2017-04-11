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

	private List<GameObject> cubes;
	private int requestedCubes;

	void Awake() {
		attractor = Instantiate (attractorPrefab, Vector3.zero, Quaternion.identity);
		attractor.name = "Attractor";

		cubes = new List<GameObject> ();
		SpawnCubes (cubeCount);
	}

	public void SliderCubesCount(float newValue) {
		requestedCubes = (int)newValue;
		if (requestedCubes != cubes.Count) {
			if (requestedCubes > cubes.Count) {
				SpawnCubes (requestedCubes - cubeCount);
			} else {
				RemoveCubes (requestedCubes);
			}
		}
	}

	void Update() {

	}

	void SpawnCubes(int newCubes) {
		int currentCount = cubes.Count;
		for (int i=currentCount; i < currentCount + newCubes; i++) {
			Vector3 pos = Utilities.randomVectorInRange (20);
			cubes.Add(Instantiate(cubePrefab, pos, Quaternion.identity));
			cubes[i].name = "Cube " + i;
		}
	}

	void RemoveCubes (int requestedCubes) {
		while (cubes.Count > requestedCubes) {
			Destroy (cubes [cubes.Count - 1]);
			cubes.RemoveAt (cubes.Count - 1);
		}
	}
}
