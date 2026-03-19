using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Graph : MonoBehaviour
{
    static readonly int maxDataPoints = 10;
    [SerializeField] GameObject lineRendererPrefab;
    List<LineRenderer> lineRenderers = new();
    List<List<Vector2>> data = new(); // key is Vector2.x
    List<Color> colors = new();
    (Vector2, Vector2) viewRegion;
    Vector2 minVisibleValues = Vector2.zero;

    public void SetDataColors(List<Color> colors)
    {
        // if too many
        while (lineRenderers.Count() > colors.Count())
        {
            LineRenderer renderer = lineRenderers.Last();
            lineRenderers.Remove(renderer);
            Destroy(renderer.gameObject);
        }

        // if too few
        while (lineRenderers.Count() < colors.Count())
        {
            GameObject instance = Instantiate(lineRendererPrefab, transform);
            lineRenderers.Add(instance.GetComponent<LineRenderer>());
        }
    
        data.Clear();
        for (int i = 0; i < colors.Count(); ++i)
        {
            data.Add(new List<Vector2>());
            lineRenderers[i].colorGradient.SetColorKeys(new GradientColorKey[] {new GradientColorKey(colors[i], 0.0f)});

        }

        this.colors = colors;
    }

    public void SetDataColors(params Color[] colors)
    {
        SetDataColors(new List<Color>(colors));
    }

    public void AddData(int index, Vector2 datapoint)
    {
        // adds the data to the index's list.
        // Debug.Log(string.Format("Datapoint added on line {0} at {1}", index, datapoint));
        data[index].Add(datapoint);
        if (data[index].Count > maxDataPoints)
        {
            data[index].RemoveAt(0);
        }
        lineRenderers[index].positionCount += 1;
        lineRenderers[index].SetPositions(data[index].Select(x => (Vector3) x).ToArray());
        lineRenderers[index].positionCount = data[index].Count();
        Redraw();
    }

    void Redraw()
    {
        foreach(LineRenderer lineRenderer in lineRenderers)
        {
            if (lineRenderer.positionCount > 0)
            {
                Vector3 lastPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
                if (lastPosition.magnitude > 0)
                {
                    for (int i = 0; i < lineRenderer.positionCount; ++i)
                    {
                        lineRenderer.SetPosition(i, lineRenderer.GetPosition(i) - lastPosition);
                    }
                }
            }
        }
    }

    // void SetViewMin()
    // {
    //     // fixed width for now.
    //     Vector2 viewRange = new(20.0f, 20.0f); // seconds
    //     float maxX = 0.0f;
    //     float maxY = 0.0f;
    //     foreach (SortedList<float, Vector2> datatype in data)
    //     {
    //         if (datatype.Count > 0)
    //         {
    //             Vector2 value = datatype.Last().Value;

    //             if (value.x > maxX) maxX = value.x;
    //             if (value.y > maxY) maxY = value.y;
    //         }
    //     }

    //     // has maxX and maxY
    //     minVisibleValues = new Vector2(maxX, maxY) - viewRange;
    // }

    // void SetViewRegion(Vector2 min, Vector2 max)
    // {
    //     // view region on the screen
    //     viewRegion = (min, max);
    // }


}
