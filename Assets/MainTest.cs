using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTest : MonoBehaviour {

	public GameObject prefab;

	void Start () {
		ObjectPool pool = new ObjectPool (prefab, 5, "Prefab");
		pool.Pool ();
	}
}
