using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BizLayer
{
    public int BizId
    {
        get;
        set;
    }
    public string BizName
    {
        get;
        set;
    }

    public Dictionary<int, BizEntity> entities = new Dictionary<int, BizEntity>();

    public VisLayer Vis
    {
        get; set;
    }
}
