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
    public SoundPlayerType LoopSound;
    public SoundPlayerType ShotSound;

    public Interpolation(Vector3 start, Vector3 end, float duration, float time, bool check, SoundPlayerType loopSound = SoundPlayerType.BALL_ROLL, SoundPlayerType shotSound = SoundPlayerType.BALL_ROLL)
    {
        this.start = start;
        this.end = end;
        this.duration = duration;
        this.time = time;
        this.check = check;
        this.LoopSound = loopSound;
        this.ShotSound = shotSound;
    }
}
