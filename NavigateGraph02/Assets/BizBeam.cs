using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BizBeam
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
    public int Start { get; set; }
    public int End { get; set; }
    public VisBeam Vis { get; set; }
}
