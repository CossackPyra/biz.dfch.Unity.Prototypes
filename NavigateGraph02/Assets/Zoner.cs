using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoner : MonoBehaviour {

	public float radius = 1f;
	public int num = 5;
	public Material material;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < num; i++){
			GameObject go1 = new GameObject();
			go1.transform.parent = transform;
			go1.transform.localPosition = Vector3.zero;
			Liner liner = go1.AddComponent<Liner>() as Liner;

			float radius1 = radius * (i + 1);


			int STEPS=15;
			GameObject[] ago = new GameObject[STEPS];
			for(int j = 0; j < STEPS; j++){
				float angle = 0.9f * Mathf.PI * (j - 3) / STEPS;
				float x = radius1 * Mathf.Cos(angle);
				float y = radius1 * Mathf.Sin(angle);
				ago[j] = new GameObject();
				ago[j].transform.parent = go1.transform;
				ago[j].transform.localPosition = new Vector3(x, y, 0f);
			}

			liner.points = ago;
			liner.material = material;
			liner.Setup();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
