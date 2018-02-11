using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool {

	private List<GameObject> objectPool;	

	public ObjectPool(GameObject obj, int count, string name)
	{
		objectPool = new List<GameObject>();
		GameObject root = new GameObject ();
		root.name = String.Format("{0} Object Pool", name);
		for (int i = 0; i < count; i++)
		{
			GameObject go = UnityEngine.Object.Instantiate (obj, Vector3.zero, Quaternion.identity);
			go.name = name + " " + i;
			go.transform.parent = root.transform;
			go.SetActive (false);
			objectPool.Add (go);
		}
	}

	public GameObject Pool()
	{
		foreach (GameObject obj in objectPool)
		{
			if (!obj.activeSelf)
			{
				obj.SetActive (true);
				return obj;
			}
		}
		Debug.Log ("Poll is at maximum");
		return null;
	}
}
