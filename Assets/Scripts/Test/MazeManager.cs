using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour {
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private AStarPathfinder pathfinder;
    [SerializeField] private GameObject npcPrefab;

    private GameObject npcInstance;
    private NPCMover mover;
    private List<Vector2Int> path;

    void Start() {
        SetupGame();
    }

    void SetupGame() {
        mapGenerator.SetupMap();

        int[,] map = mapGenerator.Map;
        Vector2Int start = mapGenerator.StartPosition;
        Vector2Int goal = mapGenerator.GoalPosition;

        path = pathfinder.FindPath(map, start, goal);
        pathfinder.HighlightPath(path, mapGenerator.CellGrid, goal);

        npcInstance = Instantiate(npcPrefab, new Vector3(start.x, -start.y, -1), Quaternion.identity);
        mover = npcInstance.GetComponent<NPCMover>();
    }

    // GUI FUNCTIONS //
    public void MoveNPC() {
        if (mover != null) {
            mover.StartMoving(path);
        }
    }

    public void ResetMap() {
        mapGenerator.SetupMap();

        int[,] map = mapGenerator.Map;
        Vector2Int start = mapGenerator.StartPosition;
        Vector2Int goal = mapGenerator.GoalPosition;

        path = pathfinder.FindPath(map, start, goal);
        pathfinder.HighlightPath(path, mapGenerator.CellGrid, goal);

        // Move NPC back to new start position
        npcInstance.transform.position = new Vector3(start.x, -start.y, -1);
    }
}
