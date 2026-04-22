using UnityEngine;

[System.Serializable]
public class DataValue
{
    float time;
    float x;
    float y;

    public DataValue(float x, float y)
    {
        this.x = x;
        this.y = y;

        time = Time.time;
    }

    public string ToCSVLine()
    {
        return string.Format("{0},{1},{2}", time, x, y);
    }
}