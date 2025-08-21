using UnityEngine;

public class MyTimer
{
    private float duration;
    private float endTime;

    public MyTimer(float duration_sec)
    {
        duration = duration_sec;
        endTime = Time.time + duration_sec;
    }

    public bool TimerHasEnded()
    {
        return Time.time > endTime;
    }

    public void Reset(float dur)
    {
        endTime = Time.time + dur;
    }
}
