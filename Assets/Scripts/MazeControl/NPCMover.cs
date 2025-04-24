using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMover : MonoBehaviour {
    public float moveSpeed = 2f;

    public void StartMoving(List<Vector2Int> path) {
        StartCoroutine(MoveAlongPath(path));
    }

    IEnumerator MoveAlongPath(List<Vector2Int> path) {
        foreach (var pos in path) {
            Vector3 target = new Vector3(pos.x, -pos.y, -1);
            while (Vector3.Distance(transform.position, target) > 0.01f) {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
