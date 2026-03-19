using UnityEngine;
using UnityEngine.UI;

class ControlPanel : MonoBehaviour
{
    bool lagging = false;
    public void GenerateLag()
    {
        for (int i = 0; i < 10e3; ++i) {
            Debug.Log("lag");
        }
    }

    public void ToggleFramerateStutter()
    {
        lagging = !lagging;
        Debug.Log(string.Format("toggled lag {0}", lagging));
    }

    void FixedUpdate()
    {
        if (lagging) System.Threading.Thread.Sleep(Random.Range(10,100));
    }
}