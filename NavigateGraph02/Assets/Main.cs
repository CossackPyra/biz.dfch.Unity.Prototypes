using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    public GameObject PrefabLayer;
    public GameObject PrefabEntiry;

    public Material beamMaterial;
    public Material greenBeamMaterial;
    int m_id;
    public Dictionary<int, BizLayer> layers = new Dictionary<int, BizLayer>();
    public Dictionary<int, BizEntity> entities = new Dictionary<int, BizEntity>();

    public Dictionary<int, BizBeam> beams = new Dictionary<int, BizBeam>();

    int m_selectedEntity;

    public float cameraDistance = 15f;

    // Use this for initialization
    void Start()
    {
        int layer1 = CreateLayer("1");
        int root = CreateEntity(layer1, "r00t");

        m_selectedEntity = root;

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



        int layer3_1 = CreateLayer("3_1");
        int vm3_1_1 = CreateEntity(layer3_1, "vm3_1_1");
        int vm3_1_2 = CreateEntity(layer3_1, "vm3_1_2");

        CreateBeam(tenantA, vm3_1_1);
        CreateBeam(tenantA, vm3_1_2);


        int layer3_2 = CreateLayer("3_2");

        int vm3_2_1 = CreateEntity(layer3_2, "VM1");
        int vm3_2_2 = CreateEntity(layer3_2, "VM2");
        int light = CreateEntity(layer3_2, "Light");

        CreateBeam(tenantB, vm3_2_1);
        CreateBeam(tenantB, vm3_2_2);
        CreateBeam(tenantB, light);




        int layer4_1 = CreateLayer("4_1");
        int license4_1_1 = CreateEntity(layer4_1, "license");
        CreateBeam(vm3_1_1, license4_1_1);


        int layer4_2 = CreateLayer("4_2");
        int license4_2_1 = CreateEntity(layer4_2, "license");
        CreateBeam(vm3_2_1, license4_2_1);

        int layer4_3 = CreateLayer("4_3");
        int cat4_3_1 = CreateEntity(layer4_3, "Cat");
        CreateBeam(light, cat4_3_1);



        int layer5 = CreateLayer("5");
        int management5_1 = CreateEntity(layer5, "Management");

        CreateBeam(license4_1_1, management5_1, 1);
        CreateBeam(license4_2_1, management5_1, 1);
        CreateBeam(cat4_3_1, management5_1, 1);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("hit");
                BizEntity entity = GetEntity(hitInfo.transform.gameObject);

                if (entity != null)
                {
                    // 
                    Debug.Log("has Entity");
                    if (IsConnected(entity.BizId))
                    {
                        Debug.Log("Connected");
                        m_selectedEntity = entity.BizId;
                    }
                }


            }
            else
            {
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            cameraDistance -= 1f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cameraDistance += 1f;
        }

        if (cameraDistance < 2f)
        {
            cameraDistance = 2f;
        }

        if (cameraDistance > 20f)
        {
            cameraDistance = 20f;
        }

        {
            // Camera
            BizEntity entity = entities[m_selectedEntity];
            Camera.main.transform.position = entity.Vis.transform.position + new Vector3(0f, 0f, -cameraDistance);
        }
    }

    bool IsConnected(int id)
    {
        foreach (KeyValuePair<int, BizBeam> kvp in beams)
        {
            BizBeam beam = kvp.Value;
            // Debug.Log(string.Format("beam {0} {1} {2} {3}", beam.Start, beam.End, m_selectedEntity, id));
            if ((beam.Start == m_selectedEntity && beam.End == id)
                || (beam.Start == id && beam.End == m_selectedEntity))
            {
                return true;
            }
        }
        return false;
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

    public int CreateBeam(int startId, int endId, int type = 0)
    {
        int _id = GenerateId();

        BizBeam beam = new BizBeam();
        beam.BizId = _id;
        beam.BizName = name;
        beam.Start = startId;
        beam.End = endId;
        beam.BizType = type;

        beams.Add(_id, beam);



        BizEntity startEntity = entities[startId];
        BizEntity endEntity = entities[endId];


        GameObject go1 = new GameObject();
        Liner liner = go1.AddComponent<Liner>() as Liner;

        liner.points = new GameObject[2]{
            startEntity.Vis.gameObject,
            endEntity.Vis.gameObject};

        if (type == 0)
        {
            liner.material = beamMaterial;

        }
        else if (type == 1)
        {
            liner.material = greenBeamMaterial;

        }


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

    BizEntity GetEntity(GameObject go1)
    {
        if (go1 == null)
        {
            return null;
        }
        // Debug.Log("GetEntity " + go1.transform.name);
        VisEntity entity = go1.GetComponent<VisEntity>();
        if (entity == null)
        {
            // Debug.Log("GetEntity no VisEntity");
            Transform parentT = go1.transform.parent;
            if (parentT == null)
            {
                // Debug.Log("GetEntity reached ROOT");
                return null;
            }
            return GetEntity(parentT.gameObject);
        }
        else
        {
            // Debug.Log("GetEntity HAS! VisEntity");
            return entity.Biz;
        }
    }
}
