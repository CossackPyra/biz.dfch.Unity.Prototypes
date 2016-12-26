using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {

    }

    public void Setup(Material material)
    {
        Liner liner = gameObject.AddComponent<Liner>() as Liner;

        int STEPS = 11;
        int STEPS2 = 9;

        GameObject[] ago = new GameObject[STEPS];
        for (int i = 0; i < STEPS; i++)
        {
            float angle1 = 2f * Mathf.PI * i / STEPS2;
            float radius1 = 0.4f + 0.05f * Mathf.Cos(angle1);
            float angle = 2f * Mathf.PI * i / STEPS2;
            float x = radius1 * Mathf.Cos(angle);
            float y = radius1 * Mathf.Sin(angle);

            GameObject go1 = new GameObject();
            go1.transform.parent = transform;
            go1.transform.localPosition = new Vector3(x, y, 0f);
            ago[i] = go1;

        }
        liner.points = ago;
        liner.material = material;
        liner.Setup();

        transform.name = "Cursor";
    }


    // Update is called once per frame
    void Update()
    {

    }
}
