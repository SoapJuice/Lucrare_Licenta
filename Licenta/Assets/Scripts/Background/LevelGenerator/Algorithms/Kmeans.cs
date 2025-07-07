using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KMeans
{
    public Vector3[] Centroids { get; private set; }
    public void Fit(List<Vector3> data, int k = 3, int maxIterations = 10)
    {
        Centroids = data
            .OrderBy(_ => Random.value)
            .Take(k)
            .Select(p => new Vector3(p.x, p.y, p.z))
            .ToArray();

        var assignments = new int[data.Count];

        for (int iter = 0; iter < maxIterations; iter++)
        {
            bool changed = false;
            for (int i = 0; i < data.Count; i++)
            {
                float bestDist = float.MaxValue;
                int bestIndex = 0;
                for (int c = 0; c < k; c++)
                {
                    float d = Vector3.SqrMagnitude(data[i] - Centroids[c]);
                    if (d < bestDist)
                    {
                        bestDist = d;
                        bestIndex = c;
                    }
                }

                if (assignments[i] != bestIndex)
                {
                    assignments[i] = bestIndex;
                    changed = true;
                }
            }

            if (!changed) break;

            for (int c = 0; c < k; c++)
            {
                var members = data
                    .Where((pt, idx) => assignments[idx] == c)
                    .ToList();

                if (members.Count > 0)
                    Centroids[c] = new Vector3(
                        members.Average(pt => pt.x),
                        members.Average(pt => pt.y),
                        members.Average(pt => pt.z)
                    );
            }
        }

    }
    public int Predict(Vector3 point)
    {
        if (Centroids == null || Centroids.Length == 0)
            throw new System.InvalidOperationException("Must Fit() before Predict()");

        float bestDist = float.MaxValue;
        int bestIndex = 0;
        for (int c = 0; c < Centroids.Length; c++)
        {
            float d = Vector3.SqrMagnitude(point - Centroids[c]);
            if (d < bestDist)
            {
                bestDist = d;
                bestIndex = c;
            }
        }
        return bestIndex;
    }
    public string GetClusterLabel(int index)
    {
        switch (index)
        {
            case 0: return "Fast";
            case 1: return "Aggressive";
            case 2: return "Safe";
            default: return "Unknown";
        }
    }

}
