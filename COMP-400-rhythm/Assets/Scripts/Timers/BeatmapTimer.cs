using UnityEngine;
using UnityEngine.InputSystem;

class BeatmapTimer : MonoBehaviour, ITimer
{
    // audio is synchronized such that it jumps if it desyncs further than tolerance.
    public event Judgement JudgementEvent;
    public event SpawnNote SpawnNoteEvent;
    [SerializeField] Beatmap beatmap;
    int i = 0;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float audioDelay;
    public InputAction jump;
    [SerializeField] Graph graph;
    float musicStartTime;
    float prevMusicTime = 0.0f;
    // float audioDesyncTolerance = 0.1f;

    void Awake()
    {
        jump = InputSystem.actions.FindAction("Jump");
        jump.performed += Judge;
    }

    void Start()
    {
        audioSource.Play();
        musicStartTime = Time.unscaledTime;
    }

    void Update()
    {
        DataRecorder.Instance.AddData("Updates", 1, Time.unscaledTime);
    }

    void FixedUpdate()
    {
        DataRecorder.Instance.AddData("Updates", 0, Time.unscaledTime);
        if (prevMusicTime != audioSource.time)
        {
            prevMusicTime = audioSource.time;
            DataRecorder.Instance.AddData("Updates", 2, Time.unscaledTime);
        }
        if (i < beatmap.beats.Count)
        {
            float time = GetGameTime() - audioDelay;
            Beat beat = beatmap.beats[i];
            while (time > beat.spawnTime)
            {
                SpawnNoteEvent.Invoke(i, time - beat.spawnTime);
                // Debug.Log(string.Format("{0}, {1}, {2}", beat.spawnTime, time, GetMusicTime()));
                ++i;
                beat = beatmap.beats[i];
            }
        }

        // update music time
        // float playbackDuration = Time.unscaledTime - musicStartTime;
        // if (Mathf.Abs(audioSource.time - playbackDuration) > audioDesyncTolerance)
        // {
        //     // just synchronize it for now.
        //     Debug.Log("Audio Sync Corrected");
        //     audioSource.time = Time.unscaledTime - musicStartTime - audioDelay;
        // }

        // update music time
        // float playbackDuration = Time.unscaledTime - musicStartTime;
        // if (Mathf.Abs(audioSource.timeSamples / AudioSettings.outputSampleRate - playbackDuration) > audioDesyncTolerance)
        // {
        //     // just synchronize it for now.
        //     Debug.Log("Audio Sync Corrected");
        //     audioSource.timeSamples = (int) ((Time.unscaledTime - musicStartTime - audioDelay) * AudioSettings.outputSampleRate);
        // }
    }

    void Judge(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            graph.AddData(0, new Vector2(Time.unscaledTime, (float) ctx.startTime));
            Debug.Log(Time.unscaledTime - ctx.startTime);
            // Debug.Log(GetMusicTime());
            int id = JudgementEvent.Invoke();
        }
    }

    public float GetGameTime()
    {
        return Time.unscaledTime - musicStartTime;
    }

    public float GetMusicTime()
    {
        return audioSource.time - audioDelay;
    }
}