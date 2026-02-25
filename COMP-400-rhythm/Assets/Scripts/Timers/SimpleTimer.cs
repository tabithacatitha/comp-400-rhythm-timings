using System.Collections;
using UnityEngine;

class SimpleTimer : MonoBehaviour, ITimer
{
    public event Judgement JudgementEvent;
    public event SpawnNote SpawnNoteEvent;

    public void Start()
    {
        StartCoroutine(NoteSpawner());
    }

    IEnumerator NoteSpawner()
    {
        int i = 0;
        while (true)
        {
            SpawnNoteEvent.Invoke(i);
            yield return new WaitForSeconds(0.5f);
            JudgementEvent.Invoke();
            ++i;
        }
    }

    public float GetGameTime()
    {
        return Time.time;
    }

    public float GetMusicTime()
    {
        return Time.time;
    }
}