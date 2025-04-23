// Attach this to the same GameObject as MapGenerator or fetch references manually
using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinder : MonoBehaviour {
    public MapGenerator mapGenerator;

    private int[,] map;
    private int width, height;

    public void Start() {
        if(map == null) {
            Debug.Log("Don't have map to find");
            return;
        }
        map = mapGenerator.GetMap();
        width = map.GetLength(1);
        height = map.GetLength(0);

        List<Vector2Int> path = FindPath(mapGenerator.GetStart(), mapGenerator.GetGoal());
        HighlightPath(path);
    }

    List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal) {
        Dictionary<Vector2Int, Vector2Int> cameFrom = new();
        Dictionary<Vector2Int, float> gScore = new() { [start] = 0 };
        Dictionary<Vector2Int, float> fScore = new() { [start] = Heuristic(start, goal) };

        List<Vector2Int> openSet = new() { start };

        while (openSet.Count > 0) {
            openSet.Sort((a, b) => fScore.GetValueOrDefault(a, float.MaxValue)
                                   .CompareTo(fScore.GetValueOrDefault(b, float.MaxValue)));
            Vector2Int current = openSet[0];

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.RemoveAt(0);

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

        return new List<Vector2Int>(); // no path found
    }

    float Heuristic(Vector2Int a, Vector2Int b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

    bool IsInBounds(Vector2Int pos) => pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    bool IsWall(Vector2Int pos) => map[pos.y, pos.x] == 1;

    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current) {
        List<Vector2Int> path = new() { current };
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    void HighlightPath(List<Vector2Int> path) {
        foreach (var pos in path) {
            Transform cell = mapGenerator.transform.Find($"Cell_{pos.x}_{pos.y}");
            if (cell != null) {
                var sr = cell.GetComponent<SpriteRenderer>();
                if (sr.color == Color.white) sr.color = Color.yellow; // highlight only path cells
            }
        }
    }

    Vector2Int[] Directions() => new Vector2Int[] {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };
}