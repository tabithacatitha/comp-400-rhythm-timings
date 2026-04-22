using System.Collections;
using UnityEngine;

class ControlPanel : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField inputField;
    bool lagging = false;
    public void GenerateLag()
    {
        for (int i = 0; i < 10e3; ++i) {
            Debug.Log("lag");
        }
    }

    void Start()
    {
        // StartCoroutine(SaveData());
    }

    private IEnumerator SaveData()
    {
        while (true) {
            yield return new WaitForSecondsRealtime(20);
            Debug.Log("data saved");
            DataRecorder.Instance.Save();
        }
    }

    public void ToggleFramerateStutter()
    {
        lagging = !lagging;
        Debug.Log(string.Format("toggled lag {0}", lagging));
    }

    public void WriteToCSV()
    {
        DataRecorder.Instance.OutputToCSV(inputField.text);
    }

    void FixedUpdate()
    {
        if (lagging) System.Threading.Thread.Sleep(Random.Range(10,100));
    }
}