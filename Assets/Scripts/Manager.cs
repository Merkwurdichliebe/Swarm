using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	[Header("References")]
	public GameObject bugPrefab;
	public GameObject attractorPrefab;

	[Header("Variables")]
	public int maxNumberOfBugs;
	public int startingNumberOfBugs;

	public float speedMin = 1f;				// Minimum bug speed
	public float speedMax = 50f;			// Maximum bug speed
	public float minInterval = 0f;			// Shortest time before turn
	public float maxInterval = 5f;			// Longest time before turn
	public float maxDistToAttractor = 10;	// Further away objects start approaching the Attractor
	public float attractorVolume = 10;		// Variation for Attractor direction
	public float attractorThreshold = 100;	// Percentage of chance for object too far away to approach the Attractor
	public float colliderScale = 1;
	public float averageLifespan = 10;		// Average lifespan in seconds

	private GameObject attractor;

	private int countDeath;

	private Slider sliderBugsCount;

	// Bugs pool

	private List<GameObject> bugsPool;
	private int requestedBugs;

	void Awake() {
		attractor = Instantiate (attractorPrefab, Vector3.zero, Quaternion.identity);
		attractor.name = "Attractor";
		bugsPool = new List<GameObject> ();

		// Create the object pool
		for (int i = 0; i < maxNumberOfBugs; i++) {
			Vector3 pos = Utilities.randomVectorInRange (50);
			bugsPool.Add(Instantiate(bugPrefab, pos, Quaternion.identity));
			bugsPool [i].GetComponent<BugController>().gender = (BugController.Gender)Random.Range(0, 2);
			bugsPool [i].name = "Bug " + i + " (" + bugsPool [i].GetComponent<BugController>().gender + ")";
			bugsPool [i].transform.parent = gameObject.transform;
			bugsPool [i].GetComponent<BugController> ().AddAttractor(attractor);
			bugsPool [i].SetActive (false);
		}

		// Setup the population slider min, max and default
		sliderBugsCount = GameObject.Find ("SliderBugsCount").GetComponent<Slider> ();
		sliderBugsCount.maxValue = (float)maxNumberOfBugs;
		sliderBugsCount.minValue = 0f;
		sliderBugsCount.value = startingNumberOfBugs;
		requestedBugs = (int)sliderBugsCount.value;
		Debug.Log ("-- END MAIN AWAKE --");
		Debug.Log ("Requested bugs = " + requestedBugs);
	}

	void Start() {

		// Spawn initial bugs from the pool
		AddRemoveBugs(requestedBugs);
		Debug.Log ("-- END MAIN START --");
	}


	// Get the requested bug count from the UI Slider

	public void SliderBugsCount(float newValue) {
		requestedBugs = (int)newValue;
	}


	void Update() {
//		if (requestedBugs != BugController.countActive) {
//			Debug.Log ("Requested (" + requestedBugs + ") differs from active (" + BugController.countActive + "), adjusting...");
//			AddRemoveBugs (requestedBugs - BugController.countActive);
//		}
		if (BugController.countActive == 0) {
			Debug.Log ("All bugs have died. Terminating.");
			Debug.Break ();
		}
	}


	// Get a positive or negative number and adjust the number of active bugs in the pool

	void AddRemoveBugs(int quantity) {
		int targetNumberOfBugs = BugController.countActive + quantity;
		while(BugController.countActive != targetNumberOfBugs) {
			if (BugController.countActive < targetNumberOfBugs) {
				bugsPool [BugController.countActive].SetActive (true);
			}
			else {
				bugsPool [BugController.countActive].SetActive (false);
			}
			Debug.Log ("Added/Removed, Target is " + targetNumberOfBugs + ", Now active is " + BugController.countActive);
		}
		Debug.Log ("AddRemove finished. Active bugs : " + BugController.countActive);
	}

	public void Encounter(GameObject obj1, GameObject obj2) {
		if (obj2.name == "Sphere") {
			BugController.countEncountersWithLight++;
		} else {
			BugController.countEncounters++;
		}
	}

	public void Death(GameObject obj) {
		countDeath++;
		obj.SetActive (false);
		Debug.Log (obj.name + " has died. Death count is now " + countDeath);
		obj.name = obj.name + " (Dead) ";
	}
}
