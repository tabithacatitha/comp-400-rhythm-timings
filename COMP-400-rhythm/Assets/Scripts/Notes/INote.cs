using UnityEngine;

interface INote
{
    void SetExpectedJudgementTime(float time); // relative time to spawning
    bool MissedJudgement(); // if missed judgement, delete.
    public float TimeUntilJudgement();
}