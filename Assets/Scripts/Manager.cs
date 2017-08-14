using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	[Header("References")]
	public GameObject BugPrefab;
	public GameObject AttractorPrefab;

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
		for (int i = 0; i < Setup.maxBugs; i++)
		{
			// TODO make OBJ & BC reference the same thing, it's confusing here
			GameObject obj = Instantiate (BugPrefab, Vector3.zero, Quaternion.identity);
			Bug bc = obj.GetComponent<Bug> ();
			// obj.name = "Bug " + i + " (" + bc.gender + ")";
			obj.name = "Bug " + i;
			obj.transform.parent = bugsRoot.transform;
			bc.AddAttractor(attractor);
			obj.SetActive (false);
			bugsPool.Add(obj);
		}

		// Setup the population slider min, max and default
		sliderBugsCount = GameObject.Find ("SliderBugsCount").GetComponent<Slider> ();
		sliderBugsCount.maxValue = (float)Setup.maxBugs;
		sliderBugsCount.minValue = 0f;
		sliderBugsCount.value = Setup.startBugs;
		requestedBugs = (int)sliderBugsCount.value;
		Debug.Log ("-- END MAIN AWAKE --");
		Debug.Log ("Requested bugs = " + requestedBugs);
	}

	void OnBugDeath(Bug obj)
	{
		Debug.Log (obj.name + " DEATH SUBSCRIBED");
		Debug.Log (obj.name + " has died. Death count is now " + Bug.CountDeaths);

		// End if all bugs are dead
		if (Bug.CountActive == 0) 
		{
			Debug.Log ("All bugs have died. Terminating.");
			Debug.Break ();
		}
	}

	void OnBugEncounter(Bug bug)
	{
		Debug.Log (bug.transform.name + " collided with " + bug.lastCollider);
	}

	void Start() 
	{
		// Spawn initial bugs from the pool with random Genders
		for (int i = 0; i < Setup.startBugs; i++)
		{
			GetNewBugFromPool((BugGender)Random.Range(0, 2));	
		}

		Bug.BugDeath += OnBugDeath;
		Bug.BugEncounter += OnBugEncounter;

		Debug.Log ("-- END MAIN START --");
	}


	// Get the requested bug count from the UI Slider
	public void SliderBugsCount(float newValue) 
	{
		requestedBugs = (int)newValue;
	}


	void Update() 
	{
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
				bugsPool [i].GetComponent<Bug> ().Initialize (g);
				return;
			}
		}
		Debug.Log ("Pool is at maximum");
	}

	public void Encounter(GameObject obj1, GameObject obj2) 
	{
		if (obj2.name == "Sphere")
		{
			Bug.CountEncountersWithLight++;
		} 
		else 
		{
			Bug.CountEncounters++;
		}
	}

//	public void Death(GameObject obj) 
//	{
//		Debug.Log (obj.name + " has died. Death count is now " + Bug.CountDeaths);
//
//		// End if all bugs are dead
//		if (Bug.CountActive == 0) 
//		{
//			Debug.Log ("All bugs have died. Terminating.");
//			Debug.Break ();
//		}
//	}
}
