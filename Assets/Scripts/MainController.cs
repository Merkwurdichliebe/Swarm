using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

	public GameObject cubePrefab;
	public GameObject attractorPrefab;
	public int cubeCount;

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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
