/* 
 * The Bug Prefab needs to use a Material with the Rendering Mode set to Fade
 * in order to make the fade-out effect work (in the Update method switch)
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BugGender {Male, Female};
public enum Status {Born, Adult, Dying};

public class BugController : MonoBehaviour {

	// Static counting variables
	public static int BugCount = 0;
	public static int CountActive = 0;
	public static int CountEncounters = 0;
	public static int CountEncountersWithLight = 0;
	public static int CountDeaths = 0;

	// Bug parameters
	private float timeToNextTurn;				// Time before next turn
	private float timeAtTurn;					// Time at turn
	private float speedMult;					// Bug speed multiplier
	private float birthTime;					// Time when enabled
	private float deathTime;					// Time at the end of lifespan, used for death animation
	private float lifespan;					// Time before death
	private Vector3 dir;						// Direction bug is heading
	private Vector3 colliderStartSize;		// Size of the collider at creation, for scaling later

	// Object references
	private AttractorController attractor;
	private Manager manager;
	private BoxCollider boxCollider;

	public BugGender gender;
	public Status status;

	// Renderer
	private Material material;
	private Color colorMale = new Color(0.5f, 0.5f, 1.0f, 1.0f);
	private Color colorFemale = new Color(1.0f, 0.5f, 0.5f, 1.0f);

	void Awake() 
	{
		manager = GameObject.Find ("Manager").GetComponent<Manager>();
		boxCollider = GetComponent<BoxCollider> ();
		material = gameObject.GetComponent<Renderer> ().material;
		colliderStartSize = boxCollider.size;
		gameObject.tag = "Bug";
		lifespan = Random.Range (manager.averageLifespan / 2f, manager.averageLifespan * 1.5f);
		status = Status.Adult;
		BugCount++;
	}
		
	void Start () 
	{
		if (gender == BugGender.Male) 
		{
			gameObject.GetComponent<Renderer> ().material.color = colorMale;
		}
		else 
		{
			gameObject.GetComponent<Renderer> ().material.color = colorFemale;
		}
	}

	void OnEnable() 
	{
		birthTime = Time.time;
		CountActive++;
	}

	void OnDisable() 
	{
		CountActive--;
	}

	void OnDestroy() 
	{
		BugCount--;
	}

	void Update () 
	{
		switch (status)
		{
			case Status.Adult:
				if (Time.time - birthTime > lifespan) 
				{
					status = Status.Dying;
					deathTime = Time.time;
				}
				Move ();
				break;
			case Status.Dying:
				// Do the death animation for 3.0 seconds, then die
				if (Time.time > deathTime + 3.0)
				{
					Die ();
				}
				else
				{
					DoDeathAnimation ();
				}
				break;	
		}
	}

	private void DoDeathAnimation()
	{
		// Slow down the bug to 20 percent of speed over time and fade it out
		speedMult = Mathf.Lerp (speedMult, 0, 3 * Time.deltaTime);
		material.color = new Color(material.color.r, material.color.g, material.color.b, Mathf.Lerp(material.color.a, 0, 2f * Time.deltaTime));
		Move ();		
	}
		
	private void Move()
	{
		// Check if object should turn; don't turn if Dying
		if (Time.time >= (timeAtTurn + timeToNextTurn) && status != Status.Dying) 
		{
			timeToNextTurn = Random.Range (manager.turnMin, manager.turnMax);
			speedMult = Random.Range (manager.speedMin, manager.speedMax);
			timeAtTurn = Time.time;

			if (attractor.isOn) 
			{
				// If the object is too far from the attractor, give it a chance to get closer to it
				if ((transform.position - attractor.gameObject.transform.position).sqrMagnitude > (manager.distanceMax * manager.distanceMax) 
					&& Random.Range (0, 100) < manager.attractorThreshold) 
				{
					dir = attractor.gameObject.transform.position - transform.position + Utilities.randomVectorInRange (manager.attractorVolume);
				} 
				else 
				{
					dir = Utilities.randomVectorInRange(1);
				}
			} 
			else 
			{
				dir = Utilities.randomVectorInRange(1);
			}
		}

		// Set new rotation and move the bug
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (dir), Time.deltaTime * 10);
		transform.Translate (Vector3.forward * speedMult * Time.deltaTime);

		// TODO This should go into a method when paranoia slider is moved, not needed in every frame
		boxCollider.size = colliderStartSize * manager.colliderScale;
	}

	// Add an attractor that the bug will attract to
	// We only need the AttractorController script
	public void AddAttractor(GameObject att)
	{
		attractor = att.GetComponent<AttractorController>();
	}

	void OnTriggerEnter(Collider other)
	{
		timeAtTurn = Time.time;
		timeToNextTurn = 0;
		manager.Encounter (gameObject, other.gameObject);
		if (other.gameObject.tag == "Bug" && status == Status.Adult)
		{
			if (other.gameObject.GetComponent<BugController>().gender != gender)
			{
				material.color = Color.white;
			}
		}
	}

	void Die()
	{
		CountDeaths++;
		manager.Death (gameObject);
	}
}
