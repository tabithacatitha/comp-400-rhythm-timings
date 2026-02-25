using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

class AudioTimer : MonoBehaviour, ITimer
{
    public event Judgement JudgementEvent;
    public event SpawnNote SpawnNoteEvent;
    float lastBeatTime;
    float interval = 2.0f;
    int i = 0;
    [SerializeField] AudioSource audioSource;
    public InputAction jump;
    [SerializeField] Graph graph;

    void Awake()
    {
        jump = InputSystem.actions.FindAction("Jump");
        jump.performed += Judge;
    }

    void Start()
    {
        audioSource.Play();
        lastBeatTime = Time.time;
    }

    void FixedUpdate()
    {
        float timeSinceLastBeat = Time.time - lastBeatTime;
        if (timeSinceLastBeat > interval)
        {
            // spawn note
            ++i;
            SpawnNoteEvent.Invoke(i);
            lastBeatTime = Time.time;
        }
    }

    void Judge(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            int id = JudgementEvent.Invoke();
        }
    }

    public float GetGameTime()
    {
        return Time.time;
    }

    public float GetMusicTime()
    {
        return audioSource.time;
    }
}