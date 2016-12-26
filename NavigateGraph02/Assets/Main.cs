using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    public GameObject PrefabLayer;
    public GameObject PrefabEntiry;

    public Material beamMaterial;
    int m_id;
    public Dictionary<int, BizLayer> layers = new Dictionary<int, BizLayer>();
    public Dictionary<int, BizEntity> entities = new Dictionary<int, BizEntity>();

    public Dictionary<int, BizBeam> beams = new Dictionary<int, BizBeam>();


    // Use this for initialization
    void Start()
    {
        int layer1 = CreateLayer("1");
        int root = CreateEntity(layer1, "r00t");

        int layer2 = CreateLayer("2");
        int tenantA = CreateEntity(layer2, "Tenant A");
        int tenantB = CreateEntity(layer2, "Tenant B");
        // CreateEntity(layer2, "xxx");
        // CreateEntity(layer2, "xxx");
        // CreateEntity(layer2, "xxx");
        // CreateEntity(layer2, "xxx");
        // CreateEntity(layer2, "xxx");
        // CreateEntity(layer2, "xxx");

        CreateBeam(root, tenantA);
        CreateBeam(root, tenantB);


        {
            int layer3_1 = CreateLayer("3_1");
            int vm1 = CreateEntity(layer3_1, "VM1");
            int vm2 = CreateEntity(layer3_1, "VM2");

            CreateBeam(tenantA, vm1);
            CreateBeam(tenantA, vm2);

        }

        {
            int layer3_2 = CreateLayer("3_2");

            int vm1 = CreateEntity(layer3_2, "VM1");
            int vm2 = CreateEntity(layer3_2, "VM2");
            int light = CreateEntity(layer3_2, "Light");

            CreateBeam(tenantB, vm1);
            CreateBeam(tenantB, vm2);
            CreateBeam(tenantB, light);

        }

    }

    // Update is called once per frame
    void Update()
    {

    }


    public int GenerateId()
    {
        m_id = m_id + 1;
        return m_id;
    }

    public int CreateLayer(string name)
    {
        int _id = GenerateId();

        BizLayer layer = new BizLayer();
        layer.BizId = _id;
        layer.BizName = name;
        layers.Add(_id, layer);

        GameObject go1 = GameObject.Instantiate(PrefabLayer) as GameObject;

        go1.transform.name = string.Format("Layer {0}", _id);
        go1.transform.parent = this.transform;
        // go1.transform.localPosition = GetPosForId(_id);
        go1.transform.localPosition = GetPosForName(name);

        VisLayer vLayer = go1.GetComponent<VisLayer>() as VisLayer;

        vLayer.Setup(this, layer);

        return _id;
    }

    public int CreateEntity(int layerId, string name)
    {
        int _id = GenerateId();

        BizLayer layer = layers[layerId];

        BizEntity entity = new BizEntity();
        entity.BizId = _id;
        entity.BizName = name;

        layer.entities.Add(_id, entity);
        entities.Add(_id, entity);


        GameObject go1 = GameObject.Instantiate(PrefabEntiry) as GameObject;

        go1.transform.name = string.Format("Entity {0}", _id);
        // go1.transform.parent = layer.Vis.transform;

        VisEntity vEntity = go1.GetComponent<VisEntity>() as VisEntity;

        vEntity.Setup(this, entity);
        layer.Vis.SetupEntity(entity.Vis);

        return _id;
    }

    public int CreateBeam(int startId, int endId)
    {
        int _id = GenerateId();

        BizBeam beam = new BizBeam();
        beam.BizId = _id;
        beam.BizName = name;
        beam.Start = startId;
        beam.End = endId;

        beams.Add(_id, beam);



        BizEntity startEntity = entities[startId];
        BizEntity endEntity = entities[endId];


        GameObject go1 = new GameObject();
        Liner liner = go1.AddComponent<Liner>() as Liner;

        liner.points = new GameObject[2]{
            startEntity.Vis.gameObject,
            endEntity.Vis.gameObject};

        liner.material = beamMaterial;

        liner.Setup();


        return _id;
    }

    public Vector3 GetPosForName(string name)
    {
        int steps = 0;
        int[] arr1 = null;
        try
        {
            string[] arr = name.Split('_');
            arr1 = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr1[i] = int.Parse(arr[i]);
                steps++;
            }
        }
        catch (Exception ex)
        {

        }
        if (steps == 1)
        {
            steps = 2;
            arr1 = new int[2] { arr1[0], 1 };
        }
        if (steps >= 2)
        {
            float radius = 2f * arr1[0];
            float angle = 0.7f * Mathf.PI * (arr1[1] - 1f) / arr1[0];
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            return new Vector3(-10f + x, -5f + y, 0f);
        }
        else
        {
            return new Vector3(0f, -10f, 0f);
        }
    }

    public Vector3 GetPosForId(int id)
    {
        Vector3 v1 = new Vector3(-10f, -5f, 0f) + 3f * Vector3.right * id + Vector3.up * (1f - Mathf.Sin(id * 11f)) * id;
        return v1;
    }
}
