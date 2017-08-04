using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	[Header("References")]
	public GameObject BugPrefab;
	public GameObject AttractorPrefab;

	[Header("Variables")]
	public int maxBugs = 100;
	public int startBugs = 50;
	public float speedMin = 1f;				// Minimum bug speed
	public float speedMax = 50f;				// Maximum bug speed
	public float turnMin = 0f;				// Shortest time before turn
	public float turnMax = 5f;				// Longest time before turn
	public float distanceMax = 10;			// Further away objects start approaching the Attractor
	public float attractorVolume = 10;		// Variation for Attractor direction
	public float attractorThreshold = 100;	// Percentage of chance for object too far away to approach the Attractor
	public float colliderScale = 1;			// Used to scale the bug collider for paranoia level
	public float averageLifespan = 10;		// Average lifespan in seconds

	private Slider sliderBugsCount;

	// Bugs pool

	private List<GameObject> bugsPool;
	private int requestedBugs;

	// TODO Add Exceptions when no prefabs are assigned in Editor

	void Awake() 
	{
		Assert.IsNotNull (BugPrefab, "[Manager Awake] BugPrefab is NULL");
		Assert.IsNotNull (AttractorPrefab, "[Manager Awake] AttractorPrefab is NULL");

		// Create the Attractor
		GameObject attractor = Instantiate (AttractorPrefab, Vector3.zero, Quaternion.identity);
		attractor.name = "Attractor";
		attractor.tag = "Attractor";

		// Create the Bugs empty parent object
		GameObject bugsRoot = new GameObject ();
		bugsRoot.name = "Bugs";

		// Create the Bugs object pool
		bugsPool = new List<GameObject> ();
		for (int i = 0; i < maxBugs; i++)
		{
			// TODO make OBJ & BC reference the same thing, it's confusing here
			GameObject obj = Instantiate (BugPrefab, Vector3.zero, Quaternion.identity);
			BugController bc = obj.GetComponent<BugController> ();
			// obj.name = "Bug " + i + " (" + bc.gender + ")";
			obj.name = "Bug " + i;
			obj.transform.parent = bugsRoot.transform;
			bc.AddAttractor(attractor);
			obj.SetActive (false);
			bugsPool.Add(obj);
		}

		// Setup the population slider min, max and default
		sliderBugsCount = GameObject.Find ("SliderBugsCount").GetComponent<Slider> ();
		sliderBugsCount.maxValue = (float)maxBugs;
		sliderBugsCount.minValue = 0f;
		sliderBugsCount.value = startBugs;
		requestedBugs = (int)sliderBugsCount.value;
		Debug.Log ("-- END MAIN AWAKE --");
		Debug.Log ("Requested bugs = " + requestedBugs);
	}

	void Start() 
	{
		// Spawn initial bugs from the pool with random Genders
		for (int i = 0; i < startBugs; i++)
		{
			GetNewBugFromPool((BugGender)Random.Range(0, 2));	
		}
		Debug.Log ("-- END MAIN START --");
	}


	// Get the requested bug count from the UI Slider
	public void SliderBugsCount(float newValue) 
	{
		requestedBugs = (int)newValue;
	}


	void Update() 
	{
		// End if all bugs are dead
		if (BugController.CountActive == 0) 
		{
			Debug.Log ("All bugs have died. Terminating.");
			Debug.Break ();
		}

		if (Input.GetKeyDown(KeyCode.F)) {
			GetNewBugFromPool (BugGender.Female);
		}

		if (Input.GetKeyDown(KeyCode.M)) {
			GetNewBugFromPool (BugGender.Male);
		}
	}
		
	// Get a positive or negative number and adjust the number of active bugs in the pool
	private void GetNewBugFromPool(BugGender g) 
	{
		for (int i = 0; i < bugsPool.Count; i++)
		{
			if (!bugsPool[i].activeSelf)
			{
				bugsPool [i].SetActive (true);
				bugsPool [i].GetComponent<BugController>().Gender = g;
				Debug.Log (string.Format("FROM MANAGER: Activated bug {0}", i));
				return;
			}
		}
		Debug.Log ("Pool is at maximum");
	}

	public void Encounter(GameObject obj1, GameObject obj2) 
	{
		if (obj2.name == "Sphere")
		{
			BugController.CountEncountersWithLight++;
		} 
		else 
		{
			BugController.CountEncounters++;
		}
	}

	public void Death(GameObject obj) 
	{
		obj.SetActive (false);
		Debug.Log (obj.name + " has died. Death count is now " + BugController.CountDeaths);
		// obj.name = obj.name + " (Dead) ";
	}
}
