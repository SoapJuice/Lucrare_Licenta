using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public static bool IsMapCompletable(int[,] map)
    {
        int rows = map.GetLength(0), cols = map.GetLength(1);
        Vector2Int start = new Vector2Int(5, 1), goal = new Vector2Int(5, cols - 2);
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        bool[,] visited = new bool[rows, cols];
        queue.Enqueue(start);
        visited[start.y, start.x] = true;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == goal) return true;

            for (int i = 0; i < 4; i++)
            {
                int nx = current.x + dx[i], ny = current.y + dy[i];
                if (nx < 0 || ny < 0 || nx >= cols || ny >= rows) continue;
                if (visited[ny, nx]) continue;
                if (map[ny, nx] == 9) continue; // wall

                visited[ny, nx] = true;
                queue.Enqueue(new Vector2Int(nx, ny));
            }
        }

        return false;
    }
}
