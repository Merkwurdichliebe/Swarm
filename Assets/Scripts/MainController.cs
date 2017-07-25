using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

	public GameObject bugPrefab;
	public GameObject attractorPrefab;

	public int numberOfBugs;

	public float speedMin = 1f;				// Minimum bug speed
	public float speedMax = 50f;			// Maximum bug speed
	public float minInterval = 0f;			// Shortest time before turn
	public float maxInterval = 5f;			// Longest time before turn
	public float maxDistToAttractor = 10;	// Further away objects start approaching the Attractor
	public float attractorVolume = 10;		// Variation for Attractor direction
	public float attractorThreshold = 100;	// Percentage of chance for object too far away to approach the Attractor
	public float colliderScale = 1;

	public int encounterCount;
	public int encounterWithLightCount;

	public GameObject attractor;
	private int SliderBugsCountGet;		// The initial value of the bugs Count Slider

	// Bugs pool

	private List<GameObject> bugsPool;
	private int requestedBugs;
	private int activeBugs;

	void Awake() {
		attractor = Instantiate (attractorPrefab, Vector3.zero, Quaternion.identity);
		attractor.name = "Attractor";
		bugsPool = new List<GameObject> ();
		encounterCount = 0;
		encounterWithLightCount = 0;

		// Create the object pool
		for (int i = 0; i < numberOfBugs + 1; i++) {
			Vector3 pos = Utilities.randomVectorInRange (50);
			bugsPool.Add(Instantiate(bugPrefab, pos, Quaternion.identity));
			bugsPool [i].name = "Bug " + i;
			bugsPool [i].transform.parent = gameObject.transform;
			bugsPool [i].SetActive (false);
		}
			
		// Get the default number of bugs from the Editor before the Slider is touched
		requestedBugs = (int)GameObject.Find ("SliderBugsCount").GetComponent<Slider>().value;
	}

	void Start() {

		// Spawn initial bugs from the pool
		AddRemoveBugs(requestedBugs);
	}


	// Get the requested bug count from the UI Slider

	public void SliderBugsCount(float newValue) {
		requestedBugs = (int)newValue;
	}


	void Update() {
		if (requestedBugs != activeBugs) {
			AddRemoveBugs (requestedBugs - activeBugs);
		}
	}


	// Get a positive or negative number and adjust the number of active bugs in the pool

	void AddRemoveBugs(int quantity) {
		int targetNumberOfBugs = activeBugs + quantity;
		while(activeBugs != targetNumberOfBugs) {
			if (activeBugs < targetNumberOfBugs) {
				bugsPool [activeBugs].SetActive (true);
				activeBugs++;	
			}
			else {
				activeBugs--;
				bugsPool [activeBugs].SetActive (false);
			}
		}
	}

	public void AddEncounter(GameObject obj1, GameObject obj2) {
		// print(obj1.name + " & " + obj2.name + " have met.");
		if (obj2.name == "Sphere") {
			encounterWithLightCount++;
		} else {
			encounterCount++;
		}
	}
}
