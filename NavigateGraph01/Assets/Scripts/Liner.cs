using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Liner : MonoBehaviour
{


	public Material material;

	public GameObject[] points;

	List<GameObject> lines = new List<GameObject> ();
	LineRenderer lr1;
	// Use this for initialization
	void Start ()
	{


		GenerateLine ();

		GameObject go1 = new GameObject ();
		lr1 = go1.AddComponent<LineRenderer> ();
		lr1.sharedMaterial = material;
	}
	
	// Update is called once per frame
	void Update ()
	{


		int n1 = points.Length;
		int n = n1 - 1;

		float[] coef = new float[n1];
		for (int k = 0; k < n1; k++) {
			coef [k] = GetBinCoeff (n, k);
		}

		int NUM = 20;

		Vector3[] v1 = new Vector3[NUM + 1];

		for (int i = 0; i <= NUM; i++) {
			float f1 = 1f * i / NUM;
			float f2 = 1f - f1;


			Vector3 p1 = new Vector3 ();
			for (int k = 0; k < n1; k++) {
				float f3 = 1;
				for (int j = 0; j < k; j++) {
					f3 *= f2;
				}
				for (int j = k + 1; j < n1; j++) {
					f3 *= f1;
				}

				p1 += points [k].transform.position * (coef [k] * f3);
			}
			v1 [i] = p1;
		}

		lr1.SetVertexCount (NUM + 1);
		lr1.SetPositions (v1);
		lr1.SetWidth(1f, 1f);


		AnimationCurve curve = new AnimationCurve();
			curve.AddKey(0.0f, 0.0f);
			curve.AddKey(0.1f, 1.0f);
			curve.AddKey(0.2f, 1.0f);
			curve.AddKey(0.8f, 1.0f);
			curve.AddKey(0.9f, 1.0f);
			curve.AddKey(1.0f, 0.0f);

		lr1.widthCurve = curve;

		lr1.widthMultiplier = 0.1f;
	}

	void 		GenerateLine ()
	{




	}


	// http://stackoverflow.com/questions/12983731/algorithm-for-calculating-binomial-coefficient
	public static long GetBinCoeff (long N, long K)
	{
		// This function gets the total number of unique combinations based upon N and K.
		// N is the total number of items.
		// K is the size of the group.
		// Total number of unique combinations = N! / ( K! (N - K)! ).
		// This function is less efficient, but is more likely to not overflow when N and K are large.
		// Taken from:  http://blog.plover.com/math/choose.html
		//
		long r = 1;
		long d;
		if (K > N)
			return 0;
		for (d = 1; d <= K; d++) {
			r *= N--;
			r /= d;
		}
		return r;
	}
}
