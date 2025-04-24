using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [Range(0f, 1f)][SerializeField] float wallRate = 0.2f;

    private int[,] map;
    private Vector2Int startPos, goalPos;
    private List<Vector2Int> guaranteedPath = new List<Vector2Int>();
    private Transform[,] cellGrid;


    public int[,] Map => map;
    public Vector2Int StartPosition => startPos;
    public Vector2Int GoalPosition => goalPos;

    public Transform[,] CellGrid => cellGrid;

    //public int[,] GetMap() => map;
    //public Vector2Int GetStart() => startPos;
    //public Vector2Int GetGoal() => goalPos;

    private Color GetColor(int type) {
        switch (type) {
            case 0: return Color.white;  // Empty
            case 1: return Color.gray;   // Wall
            case 2: return Color.green;  // Start
            case 3: return Color.red;    // Goal
            default: return Color.black;
        }
    }

    //void Start() {
    //    SetupMap();
    //}

    public void SetupMap() {
        GenerateMapWithGuaranteedPath();
        DrawMap();
    }

    void GenerateMapWithGuaranteedPath() {
        map = new int[height, width];
        cellGrid = new Transform[height, width];

        // Step 1: Chọn start ngẫu nhiên
        startPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        guaranteedPath.Clear();
        guaranteedPath.Add(startPos);

        // Step 2: Tạo đường đi đảm bảo
        Vector2Int current = startPos;
        Vector2Int lastDir = Vector2Int.zero;
        int maxSteps = Mathf.Max(width, height) + 5;
        // Step 2: Generate guaranteed path
        for (int i = 0; i < maxSteps; i++) {
            List<Vector2Int> neighbors = new List<Vector2Int>();
            if (current.x > 0) neighbors.Add(current + Vector2Int.left);
            if (current.x < width - 1) neighbors.Add(current + Vector2Int.right);
            if (current.y > 0) neighbors.Add(current + Vector2Int.down);
            if (current.y < height - 1) neighbors.Add(current + Vector2Int.up);

            neighbors.RemoveAll(p => guaranteedPath.Contains(p));

            // Shuffle để tránh hướng đi quá thẳng
            neighbors = ShuffleList(neighbors);

            if (neighbors.Count == 0)
                break;

            current = neighbors[0];
            guaranteedPath.Add(current);
        }

        // Step 3: Set start và goal
        startPos = guaranteedPath[0];
        // Chọn goal sao cho cách start đủ xa
        int minDistance = 3; // có thể chỉnh
        goalPos = startPos; //nếu kết quả cuối cùng là goalPos = startPos thì 

        int maxDist = -1;
        // Tìm tất cả các điểm cách xa hơn minDistance
        foreach (var pos in guaranteedPath) {
            if (pos == startPos) continue; // bỏ qua điểm start

            int dist = ManhattanDistance(startPos, pos);

            if (dist >= minDistance && dist > maxDist) {
                maxDist = dist;
                goalPos = pos;
            }
        }

        // Nếu không tìm được điểm nào xa hơn minDistance, chọn điểm xa nhất (dù nhỏ hơn minDistance)
        if (goalPos == startPos && guaranteedPath.Count > 1) {
            foreach (var pos in guaranteedPath) {
                if (pos == startPos) continue;
                int dist = ManhattanDistance(startPos, pos);
                if (dist > maxDist) {
                    maxDist = dist;
                    goalPos = pos;
                }
            }
        }

        // Step 4: Đánh dấu map
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                map[y, x] = 1; // default = wall

        foreach (var pos in guaranteedPath)
            map[pos.y, pos.x] = 0; // empty path

        map[startPos.y, startPos.x] = 2;
        map[goalPos.y, goalPos.x] = 3;

        // Step 5: Random thêm tường cho ô không thuộc đường đi
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++) {
                Vector2Int pos = new Vector2Int(x, y);
                if (!guaranteedPath.Contains(pos) && map[y, x] != 2 && map[y, x] != 3)
                    map[y, x] = Random.value < wallRate ? 1 : 0;
            }
    }
    int ManhattanDistance(Vector2Int a, Vector2Int b) {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    List<T> ShuffleList<T>(List<T> list) {
        for (int i = 0; i < list.Count; i++) {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
        return list;
    }

    void DrawMap() {
        foreach (Transform child in transform)
            Destroy(child.gameObject); // reset trước khi vẽ

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                GameObject cell = Instantiate(cellPrefab, new Vector3(x, -y, 0), Quaternion.identity, transform);
                SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                cell.gameObject.SetActive(true);
                sr.color = GetColor(map[y, x]);
                cell.name = $"Cell_{x}_{y}";
                cellGrid[y, x] = cell.transform;
            }
        }
    }


}
