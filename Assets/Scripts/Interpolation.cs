using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Interpolation
{
    public Vector3 start;
    public Vector3 end;
    public float duration;
    public float time;
    public bool check;

    public Interpolation(Vector3 start, Vector3 end, float duration, float time, bool check)
    {
        this.start = start;
        this.end = end;
        this.duration = duration;
        this.time = time;
        this.check = check;
    }
}
