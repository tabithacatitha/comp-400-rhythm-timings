using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

class SynchronizedAudioTimer : MonoBehaviour, ITimer
{
    // audio is synchronized such that it jumps if it desyncs further than tolerance.
    public event Judgement JudgementEvent;
    public event SpawnNote SpawnNoteEvent;
    float lastBeatTime;
    float interval = 0.5f;
    int i = 0;
    [SerializeField] AudioSource audioSource;
    public InputAction jump;
    [SerializeField] Graph graph;
    float interpolatedMusicTime;
    float audioDesyncTolerance = 0.1f;
    float musicStartTime;

    void Awake()
    {
        jump = InputSystem.actions.FindAction("Jump");
        jump.performed += Judge;
    }

    void Start()
    {
        audioSource.Play();
        lastBeatTime = Time.unscaledTime;
        musicStartTime = Time.unscaledTime;
    }

    void FixedUpdate()
    {
        float timeSinceLastBeat = Time.unscaledTime - lastBeatTime;
        while (timeSinceLastBeat > interval)
        {
            while (timeSinceLastBeat > 2 * interval)
            {
                // ignore one note
                timeSinceLastBeat -= interval;
                lastBeatTime += interval;
                SkipNote();
            }
            // spawn note
            ++i;
            // SpawnNoteEvent should consider spawning offsets.
            SpawnNoteEvent.Invoke(i, timeSinceLastBeat - interval); // timeSinceLastBeat - interval guaranteed to be positive
            graph?.AddData(0, new Vector2(GetGameTime(), GetGameTime() - GetMusicTime()));
            lastBeatTime += interval;
            timeSinceLastBeat = Time.unscaledTime - lastBeatTime;
        }

        // update music time
        float playbackDuration = Time.unscaledTime - musicStartTime;
        if (audioSource.time - playbackDuration > audioDesyncTolerance)
        {
            // just synchronize it for now.
            Debug.Log("Audio Sync Corrected");
            audioSource.time = GetGameTime() - musicStartTime;
        }
    }

    void SkipNote()
    {
        
    }

    void Judge(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log(ctx.time);
            int id = JudgementEvent.Invoke();
        }
    }

    public float GetGameTime()
    {
        return Time.unscaledTime;
    }

    public float GetMusicTime()
    {
        return audioSource.time;
    }
}