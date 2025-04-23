using UnityEngine;
namespace Test {

    public class MapGenerator : MonoBehaviour {
        public GameObject cellPrefab;
        public Vector2 cellSize = new Vector2(1, 1);

        // Bản đồ 2D: 0 = Trống, 1 = Tường, 2 = Start, 3 = Goal
        public int[,] map = new int[,]
        {
        {0,0,0,0,0},
        {1,1,0,1,0},
        {2,0,0,1,0},
        {0,1,0,0,0},
        {0,1,1,1,3},
        };

        private Color GetColor(int type) {
            switch (type) {
                case 0: return Color.white; // Trống
                case 1: return Color.gray;  // Tường
                case 2: return Color.green; // Start
                case 3: return Color.red;   // Goal
                default: return Color.black;
            }
        }

        void Start() {
            GenerateMap();
        }

        public void GenerateMap() {
            int width = map.GetLength(1);
            int height = map.GetLength(0);

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    GameObject cell = Instantiate(cellPrefab, new Vector3(x * 0.25f * cellSize.x, -y * 0.25f * cellSize.y, 0), Quaternion.identity, transform);
                    SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                    cell.gameObject.SetActive(true);
                    sr.color = GetColor(map[y, x]);
                    cell.name = $"Cell_{x}_{y}";
                }
            }
        }
    }

}