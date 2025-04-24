// MazePathfindingUnity.cs
// Full implementation for Percas Studio pathfinding test in Unity

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager_1 : MonoBehaviour {
    public GameObject cellPrefab;
    public int width = 10, height = 10;

    private GridCell[,] grid;
    private Vector2Int start, goal;
    private readonly Color wallColor = Color.gray;
    private readonly Color pathColor = Color.white;
    private readonly Color visitedColor = Color.yellow;
    private readonly Color startColor = Color.green;
    private readonly Color goalColor = Color.red;

    int[,] map = new int[,]
    {
        {0,0,1,0,0,0,0,0,0,0},
        {0,1,1,0,1,1,1,1,0,0},
        {0,0,0,0,0,0,0,1,0,0},
        {0,1,1,1,1,1,0,1,0,0},
        {0,0,0,0,0,1,0,1,0,0},
        {1,1,1,1,0,1,0,1,0,0},
        {0,0,0,1,0,0,0,1,0,0},
        {0,1,0,1,1,1,0,1,0,0},
        {0,1,0,0,0,0,0,0,0,0},
        {0,1,1,1,1,1,1,1,1,0},
    };

    void Start() {
        width = map.GetLength(1);
        height = map.GetLength(0);
        grid = new GridCell[width, height];
        GenerateGrid();
        StartCoroutine(FindPath());
    }

    void GenerateGrid() {
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                GameObject cell = Instantiate(cellPrefab, new Vector3(x, -y, 0), Quaternion.identity);
                GridCell cellScript = cell.GetComponent<GridCell>();
                grid[x, y] = cellScript;
                cellScript.Init(x, y);

                if (map[y, x] == 1) {
                    cellScript.SetType(CellType.Wall, wallColor);
                }
                else {
                    cellScript.SetType(CellType.Empty, pathColor);
                }
            }
        }
        start = new Vector2Int(0, 0);
        goal = new Vector2Int(9, 9);
        grid[start.x, start.y].SetType(CellType.Start, startColor);
        grid[goal.x, goal.y].SetType(CellType.Goal, goalColor);
    }

    IEnumerator FindPath() {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(start);
        cameFrom[start] = start;

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        while (queue.Count > 0) {
            Vector2Int current = queue.Dequeue();
            if (current != start && current != goal)
                grid[current.x, current.y].SetColor(visitedColor);

            if (current == goal)
                break;

            foreach (var dir in directions) {
                Vector2Int next = current + dir;
                if (InBounds(next) && map[next.y, next.x] == 0 && !cameFrom.ContainsKey(next)) {
                    queue.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
            yield return new WaitForSeconds(0.02f);
        }

        if (!cameFrom.ContainsKey(goal)) yield break;

        Vector2Int pathStep = goal;
        while (pathStep != start) {
            if (pathStep != goal)
                grid[pathStep.x, pathStep.y].SetColor(Color.cyan);
            pathStep = cameFrom[pathStep];
            yield return new WaitForSeconds(0.02f);
        }
    }

    bool InBounds(Vector2Int pos) {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }
}

public enum CellType { Empty, Wall, Start, Goal }

public class GridCell : MonoBehaviour {
    public int x, y;
    public SpriteRenderer sr;

    public void Init(int _x, int _y) {
        x = _x; y = _y;
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetType(CellType type, Color color) {
        sr.color = color;
    }

    public void SetColor(Color c) {
        sr.color = c;
    }
}
