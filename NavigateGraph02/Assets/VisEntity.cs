using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisEntity : MonoBehaviour
{
    public Text name1;

    public GameObject beams;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    Main m_main;
    BizEntity m_entity;

    public void Setup(Main main, BizEntity entity)
    {
        m_main = main;
        m_entity = entity;
        entity.Vis = this;

        name1.text = entity.BizName;
    }
}
