using Unity.VisualScripting;
using UnityEngine;

class LerpNote : MonoBehaviour, INote
{
    static readonly float judgementPosition = -6.0f;
    float expectedJudgementTime;
    float startTime;
    float delay;
    public void SetExpectedJudgementTime(float time, float delay)
    {
        this.expectedJudgementTime = time;
        this.delay = delay;
    }

    void Awake()
    {
        startTime = Time.unscaledTime - delay;
    }

    void Update()
    {
        float timeNow = Time.unscaledTime - startTime;
        Vector3 position = transform.position;
        // lerp with overflow
        position.x = Mathf.LerpUnclamped(9.0f, judgementPosition, timeNow / expectedJudgementTime);
        transform.position = position;
    }

    public bool MissedJudgement()
    {
        return transform.position.x < -9.0f;
    }

    public float TimeUntilJudgement()
    {
        return this.expectedJudgementTime - (Time.unscaledTime - startTime);
    }
}