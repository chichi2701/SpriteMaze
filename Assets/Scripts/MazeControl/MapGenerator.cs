using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [Range(0f, 1f)][SerializeField] private float wallRate = 0.2f;

    private int[,] map;
    private Vector2Int startPos, goalPos;
    private List<Vector2Int> guaranteedPath = new List<Vector2Int>();
    private Transform[,] cellGrid;

    public int[,] Map => map;
    public Vector2Int StartPosition => startPos;
    public Vector2Int GoalPosition => goalPos;
    public Transform[,] CellGrid => cellGrid;

    public void SetupMap() {
        GenerateMapWithGuaranteedPath();
        DrawMap();
    }

    private void GenerateMapWithGuaranteedPath() {
        map = new int[height, width];
        cellGrid = new Transform[height, width];

        startPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        guaranteedPath.Clear();
        guaranteedPath.Add(startPos);

        Vector2Int current = startPos;
        int maxSteps = Mathf.Max(width, height) + 5;

        for (int i = 0; i < maxSteps; i++) {
            var neighbors = new List<Vector2Int>();

            if (current.x > 0) neighbors.Add(current + Vector2Int.left);
            if (current.x < width - 1) neighbors.Add(current + Vector2Int.right);
            if (current.y > 0) neighbors.Add(current + Vector2Int.down);
            if (current.y < height - 1) neighbors.Add(current + Vector2Int.up);

            neighbors.RemoveAll(p => guaranteedPath.Contains(p));
            neighbors = ShuffleList(neighbors);

            if (neighbors.Count == 0) break;

            current = neighbors[0];
            guaranteedPath.Add(current);
        }

        startPos = guaranteedPath[0];
        goalPos = SelectGoalPosition();

        FillMapWithWalls();
        MarkGuaranteedPath();
        RandomizeNonPathCells();
    }

    private Vector2Int SelectGoalPosition() {
        int minDistance = 3;
        Vector2Int bestGoal = startPos;
        int maxDist = -1;

        foreach (var pos in guaranteedPath) {
            if (pos == startPos) continue;

            int dist = ManhattanDistance(startPos, pos);
            if (dist >= minDistance && dist > maxDist) {
                maxDist = dist;
                bestGoal = pos;
            }
        }

        if (bestGoal == startPos && guaranteedPath.Count > 1) {
            foreach (var pos in guaranteedPath) {
                if (pos == startPos) continue;
                int dist = ManhattanDistance(startPos, pos);
                if (dist > maxDist) {
                    maxDist = dist;
                    bestGoal = pos;
                }
            }
        }

        return bestGoal;
    }

    private void FillMapWithWalls() {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                map[y, x] = 1; // Default to wall
    }

    private void MarkGuaranteedPath() {
        foreach (var pos in guaranteedPath)
            map[pos.y, pos.x] = 0;

        map[startPos.y, startPos.x] = 2; // Start
        map[goalPos.y, goalPos.x] = 3;   // Goal
    }

    private void RandomizeNonPathCells() {
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                var pos = new Vector2Int(x, y);
                if (!guaranteedPath.Contains(pos) && map[y, x] != 2 && map[y, x] != 3) {
                    map[y, x] = Random.value < wallRate ? 1 : 0;
                }
            }
        }
    }

    private void DrawMap() {
        foreach (Transform child in transform)
            Destroy(child.gameObject); // Clear old map

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                var cell = Instantiate(cellPrefab, new Vector3(x, -y, 0), Quaternion.identity, transform);
                var sr = cell.GetComponent<SpriteRenderer>();
                sr.color = GetColor(map[y, x]);
                cell.name = $"Cell_{x}_{y}";
                cellGrid[y, x] = cell.transform;
            }
        }
    }

    private Color GetColor(int type) {
        return type switch {
            0 => Color.white,
            1 => Color.gray,
            2 => Color.green,
            3 => Color.red,
            _ => Color.black,
        };
    }

    private int ManhattanDistance(Vector2Int a, Vector2Int b) {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<T> ShuffleList<T>(List<T> list) {
        for (int i = 0; i < list.Count; i++) {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
        return list;
    }
}
