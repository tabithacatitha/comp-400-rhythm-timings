using Unity.VisualScripting;
using UnityEngine;

class LerpNote : MonoBehaviour, INote
{
    static readonly float judgementPosition = -6.0f;
    float expectedJudgementTime;
    float startTime;
    public void SetExpectedJudgementTime(float time)
    {
        this.expectedJudgementTime = time;
    }

    void Awake()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float timeNow = Time.time - startTime;
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
        return this.expectedJudgementTime - (Time.time - startTime);
    }
}