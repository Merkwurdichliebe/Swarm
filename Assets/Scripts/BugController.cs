using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugController : MonoBehaviour {

	public static int bugCount = 0;
	public static int countActive = 0;
	public static int countEncounters = 0;
	public static int countEncountersWithLight = 0;

	// From Main

	private float timeToNextTurn;			// Time before next turn
	private float timeAtTurn;				// Time at turn
	private float speedMult;				// Bug speed multiplier
	private float distToAttractor;			// Bug distance to Attractor object
	private float maxDist;					// Maximum distance from the Attractor

	private Vector3 dir;					// Random new direction for bug
	private Vector3 dirVar;					// Random variation vector added to Attractor object

	private GameObject attractor;
	private AttractorController attractorController;
	private Manager settings;
	private BoxCollider boxCollider;
	private Vector3 startSize;

	public enum Gender {Male, Female};
	public Gender gender;

	private Color colorMale = new Color(0.5f, 0.5f, 1.0f, 1.0f);
	private Color colorFemale = new Color(1.0f, 0.5f, 0.5f, 1.0f);

	private float birthTime;
	private float lifespan;

	void Awake() {
		settings = GameObject.Find ("Manager").GetComponent<Manager>();
		boxCollider = GetComponent<BoxCollider> ();
		startSize = boxCollider.size;
		gameObject.tag = "Bug";
		birthTime = Time.time;
		lifespan = settings.averageLifespan + Random.Range (-3f, 4f);
		bugCount++;
	}
		
	void Start () {
		// Needs to be in Start because Prefab is instantiated in Awake
		// Can this be done more nicely?
		// TODO
		attractorController = attractor.GetComponent<AttractorController>();

		if (gender == Gender.Male) {
			gameObject.GetComponent<Renderer> ().material.color = colorMale;
		}
		else {
			gameObject.GetComponent<Renderer> ().material.color = colorFemale;
		}
	}

	void OnEnable() {
		countActive++;
	}

	void OnDisable() {
		countActive--;
	}

	void OnDestroy() {
		bugCount--;
	}

	void Update () {
		// Check if object should turn
		if (Time.time >= (timeAtTurn + timeToNextTurn)) {
			timeToNextTurn = Random.Range (settings.minInterval, settings.maxInterval);
			speedMult = Random.Range (settings.speedMin, settings.speedMax);
			timeAtTurn = Time.time;

			if (attractorController.isOn) {
				maxDist = settings.maxDistToAttractor * settings.maxDistToAttractor;
				distToAttractor = (transform.position - attractor.transform.position).sqrMagnitude;

				// If the object is too far from the attractor, give it a chance to get closer to it
				if (distToAttractor > maxDist && Random.Range(0, 100) < settings.attractorThreshold) {
					dirVar = Utilities.randomVectorInRange (settings.attractorVolume);
					dir = attractor.transform.position - transform.position + dirVar;
				} else {
					dir = Utilities.randomVectorInRange(1);
				}

			} else {
				dir = Utilities.randomVectorInRange(1);
			}
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * 10);
		transform.Translate (Vector3.forward * speedMult * Time.deltaTime);
		boxCollider.size = startSize * settings.colliderScale;

		// Check lifespan

		if (Time.time - birthTime > lifespan) {
			settings.Death (gameObject);
		}
	}

	public void AddAttractor(GameObject att) {
		attractor = att;
	}

	void OnTriggerEnter(Collider other) {
		timeAtTurn = Time.time;
		timeToNextTurn = 0;
		settings.Encounter (gameObject, other.gameObject);
		if (other.gameObject.tag == "Bug") {
			if (other.gameObject.GetComponent<BugController>().gender != gender) {
				gameObject.GetComponent<Renderer> ().material.color = Color.white;
			}
		}
	}
}
