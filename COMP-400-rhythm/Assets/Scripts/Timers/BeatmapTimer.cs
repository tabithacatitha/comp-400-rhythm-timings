using System.Collections;
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

    void Awake()
    {
        jump = InputSystem.actions.FindAction("Jump");
        jump.performed += Judge;
    }

    void Start()
    {
        audioSource.Play();
        musicStartTime = Time.unscaledTime; // can use audiosource.time instead if you want
    }

    void FixedUpdate()
    {
        float time = GetMusicTime();

        // float timeSinceLastBeat = Time.unscaledTime - lastBeatTime;
        // SpawnNoteEvent.Invoke(i, timeSinceLastBeat - interval);
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
        return Time.unscaledTime - musicStartTime;
    }

    public float GetMusicTime()
    {
        return audioSource.time - audioDelay;
    }
}