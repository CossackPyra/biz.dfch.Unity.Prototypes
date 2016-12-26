using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisLayer : MonoBehaviour
{

    public Text name1;

    public GameObject entities;
    public GameObject body;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    Main m_main;
    BizLayer m_layer;

    public void Setup(Main main, BizLayer layer)
    {
        m_main = main;
        m_layer = layer;
        layer.Vis = this;

        name1.text = layer.BizName;
    }

    public void SetupEntity(VisEntity entity)
    {
        entity.transform.parent = entities.transform;
        RepositionEntities();
    }
    public void RepositionEntities()
    {
		int count = entities.transform.childCount;

		float radius = (count - 1f) /3f;
		for(int i = 0; i < count; i++){
			float x = radius * Mathf.Sin(2f*Mathf.PI * i / count);
			float y = radius * Mathf.Cos(2f*Mathf.PI * i / count);
			entities.transform.GetChild(i).localPosition = new Vector3(x, y, -0.3f);
		}
		body.transform.localScale = new Vector3(count, count, count);
    }
}
