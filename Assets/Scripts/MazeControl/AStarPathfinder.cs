using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinder : MonoBehaviour {
    [SerializeField] private MapGenerator mapGenerator;

    private int[,] map;
    private int width, height;
    private Vector2Int startPos, goalPos;

    public List<Vector2Int> FindPath(int[,] map, Vector2Int start, Vector2Int goal) {
        this.map = map;
        width = map.GetLength(1);
        height = map.GetLength(0);
        startPos = start;
        goalPos = goal;

        return FindPathInternal(start, goal);
    }

    private List<Vector2Int> FindPathInternal(Vector2Int start, Vector2Int goal) {
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, float> { [start] = 0 };
        var fScore = new Dictionary<Vector2Int, float> { [start] = Heuristic(start, goal) };
        var openSet = new List<Vector2Int> { start };

        while (openSet.Count > 0) {
            openSet.Sort((a, b) => fScore.GetValueOrDefault(a, float.MaxValue)
                                         .CompareTo(fScore.GetValueOrDefault(b, float.MaxValue)));

            Vector2Int current = openSet[0];
            openSet.RemoveAt(0);

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            foreach (Vector2Int dir in Directions()) {
                Vector2Int neighbor = current + dir;
                if (!IsInBounds(neighbor) || IsWall(neighbor)) continue;

                float tentativeG = gScore[current] + 1;
                if (tentativeG < gScore.GetValueOrDefault(neighbor, float.MaxValue)) {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<Vector2Int>(); // No path found
    }

    private float Heuristic(Vector2Int a, Vector2Int b) =>
        Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance

    private bool IsInBounds(Vector2Int pos) =>
        pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;

    private bool IsWall(Vector2Int pos) => map[pos.y, pos.x] == 1;

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current) {
        var path = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    public void HighlightPath(List<Vector2Int> path, Transform[,] cellGrid, Vector2Int goal) {
        foreach (var pos in path) {
            if (pos == goal) continue;

            Transform cell = cellGrid[pos.y, pos.x];
            if (cell != null) {
                var sr = cell.GetComponent<SpriteRenderer>();
                if (sr != null) {
                    sr.color = Color.yellow;
                }
            }
        }
    }
    private Vector2Int[] Directions() => new Vector2Int[] {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };
}
