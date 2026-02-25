using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Mono.Cecil.Cil;
using UnityEngine;

public delegate void SpawnNote(int id);
public delegate int Judgement();
public class GameVisualizer : MonoBehaviour
{
    [SerializeField] GameObject timerObject;
    ITimer timer;
    [SerializeField] float interval;
    [SerializeField] Graph graph;
    [SerializeField] GameObject notePrefab;
    Dictionary<int, (GameObject, INote)> notes = new(); // visible notes
    float noteY = -6.0f;

    void Awake()
    {
        timer = timerObject.GetComponent<ITimer>();
        graph.SetDataColors(Color.blue,
                            Color.red);
        timer.SpawnNoteEvent += new SpawnNote(OnSpawnNote);
        timer.JudgementEvent += new Judgement(OnJudgement);
    }

    void FixedUpdate()
    {
        foreach ((int id, (GameObject noteObject, INote note)) in notes.ToList())
        {
            if (note.MissedJudgement())
            {
                Destroy(noteObject);
                notes.Remove(id);
            }
        }
    }

    int OnJudgement()
    {
        if (notes.Count == 0) return -1;
        // pick the note with lowest id
        int id = notes.Min(x => x.Key);

        // TODO: if note shouldn't be judged yet, don't judge it.


        graph?.AddData(0, new Vector2(Time.time, notes[id].Item2.TimeUntilJudgement()));

        Destroy(notes[id].Item1);
        notes.Remove(id);
        return id;
    }

    void OnSpawnNote(int id)
    {
        noteY += 1.0f;
        if (noteY > 4.0f) noteY = -4.0f;
        GameObject noteObject = Instantiate(notePrefab, new Vector3(10.0f, noteY, 0), Quaternion.identity, transform);
        INote note = noteObject.GetComponent<INote>();
        note.SetExpectedJudgementTime(0.5f);
        notes[id] = (noteObject, note);
    }
}
