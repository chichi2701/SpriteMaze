using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {
    public GameObject cellPrefab;
    public int width = 10;
    public int height = 10;
    [Range(0f, 1f)] public float wallRate = 0.2f;

    private int[,] map;
    private Vector2Int startPos, goalPos;
    private List<Vector2Int> guaranteedPath = new List<Vector2Int>();

    private Color GetColor(int type) {
        switch (type) {
            case 0: return Color.white;  // Empty
            case 1: return Color.gray;   // Wall
            case 2: return Color.green;  // Start
            case 3: return Color.red;    // Goal
            default: return Color.black;
        }
    }

    void Start() {
        GenerateMapWithGuaranteedPath();
        DrawMap();
    }

    void GenerateMapWithGuaranteedPath() {
        map = new int[height, width];

        // Step 1: Chọn start ngẫu nhiên
        startPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        guaranteedPath.Clear();
        guaranteedPath.Add(startPos);

        // Step 2: Tạo đường đi đảm bảo
        Vector2Int current = startPos;
        int maxSteps = Mathf.Max(width, height) + 5;
        for (int i = 0; i < maxSteps; i++) {
            List<Vector2Int> neighbors = new List<Vector2Int>();
            if (current.x > 0) neighbors.Add(current + Vector2Int.left);
            if (current.x < width - 1) neighbors.Add(current + Vector2Int.right);
            if (current.y > 0) neighbors.Add(current + Vector2Int.down);
            if (current.y < height - 1) neighbors.Add(current + Vector2Int.up);

            // Lọc ra các ô chưa đi qua
            neighbors.RemoveAll(p => guaranteedPath.Contains(p));

            if (neighbors.Count == 0)
                break; // không thể mở rộng nữa

            current = neighbors[Random.Range(0, neighbors.Count)];
            guaranteedPath.Add(current);
        }

        // Step 3: Set start và goal
        startPos = guaranteedPath[0];
        goalPos = guaranteedPath[guaranteedPath.Count - 1];

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
            }
        }
    }

    public int[,] GetMap() => map;
    public Vector2Int GetStart() => startPos;
    public Vector2Int GetGoal() => goalPos;
}
