using UnityEngine;

interface ITimer
{
    public event Judgement JudgementEvent;

    public event SpawnNote SpawnNoteEvent;
    public float GetGameTime();
    public float GetMusicTime();
}