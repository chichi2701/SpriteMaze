using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour {
    public MapGenerator mapGenerator;
    public AStarPathfinder pathfinder;
    public GameObject npcPrefab;

    private GameObject npcInstance;
    NPCMover mover;
    List<Vector2Int> path;
    void Start() {
        SetupGame();
    }

    void SetupGame() {
        // 1. Lấy map và start/goal từ map generator
        mapGenerator.SetupMap();
        int[,] map = mapGenerator.Map;
        Vector2Int start = mapGenerator.StartPosition;
        Vector2Int goal = mapGenerator.GoalPosition;

        // 2. Tìm đường
        path = pathfinder.FindPath(map, start, goal);
        // pathfinder.HighlightPath(path, mapGenerator.transform);
        pathfinder.HighlightPath(path, mapGenerator.CellGrid, goal);

        // 3. Spawn NPC và gán đường đi
        npcInstance = Instantiate(npcPrefab, new Vector3(start.x, -start.y, -1), Quaternion.identity);
        mover = npcInstance.GetComponent<NPCMover>();
    }

    public void MoveNPC() {
        if (mover != null)
            mover.StartMoving(path);
    }

    public void ResetMap() {
        // 1. Lấy map và start/goal từ map generator
        mapGenerator.SetupMap();
        int[,] map = mapGenerator.Map;
        Vector2Int start = mapGenerator.StartPosition;
        Vector2Int goal = mapGenerator.GoalPosition;

        // 2. Tìm đường
        path = pathfinder.FindPath(map, start, goal);
        pathfinder.HighlightPath(path, mapGenerator.CellGrid, goal);

        npcInstance.transform.position = new Vector3(start.x, -start.y, -1);
    }
}
