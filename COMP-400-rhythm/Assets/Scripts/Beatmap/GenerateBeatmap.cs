using UnityEngine;

class GenerateBeatmap : MonoBehaviour
{
    [SerializeField] Beatmap beatmap;
    void Awake()
    {
        beatmap.beats.Clear();
        for (float i = 0; i < 200; i += 0.5f)
        {
            beatmap.beats.Add(new Beat(i + 1f, i));
        }
    }
}