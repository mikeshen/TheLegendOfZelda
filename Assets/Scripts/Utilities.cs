using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utilities : MonoBehaviour {

    public static Vector3 nextDirection(EnemyControl ec, Vector3 position) {
        List<Vector3> allowedDirections = new List<Vector3>();
        addAllowedDirections(ec, allowedDirections, position);
        int range = allowedDirections.Count;
        int random = Random.Range(0, range);
        return allowedDirections[random];
    }

    public static void addAllowedDirections(EnemyControl ec, List<Vector3> allowedDirections, Vector3 position) {
        int x = 0, y = 0;
        if (ec.currentDirection == Direction.EAST || ec.currentDirection == Direction.WEST)
            x = ec.currentDirection == Direction.EAST ? 1 : -1;
        else
            y = ec.currentDirection == Direction.NORTH ? 1 : -1;

        if (checkWalkable((int)position.x + 1, (int)position.y))
            allowedDirections.Add(Vector3.right);
        if (checkWalkable((int)position.x - 1, (int)position.y))
            allowedDirections.Add(Vector3.left);
        if (checkWalkable((int)position.x, (int)position.y + 1))
            allowedDirections.Add(Vector3.up);
        if (checkWalkable((int)position.x, (int)position.y - 1))
            allowedDirections.Add(Vector3.down);
        if (checkWalkable((int)position.x + x, (int)position.y + y))
            allowedDirections.Add(new Vector3(x, y, 0));
    }

    public static bool checkWalkable(int x, int y) {
        int tileType = ShowMapOnCamera.MAP[x, y];
        // Door Sprites
        if (tileType == 26 || tileType == 27 || tileType == 48 || tileType == 51 ||
            tileType == 80 || tileType == 81 || tileType == 92 || tileType == 93 ||
            tileType == 100 || tileType == 101 || tileType == 106)
            return false;
        return ShowMapOnCamera.S.collisionS[tileType] == '_';
    }

    public static Direction findDirection(Vector3 vec) {
        if (vec == Vector3.left)
            return Direction.WEST;
        else if (vec == Vector3.right)
            return Direction.EAST;
        else if (vec == Vector3.up)
            return Direction.NORTH;
        else
            return Direction.SOUTH;
    }

    // Use this for initialization
    void Start() {}

	// Update is called once per frame
	void Update() {}
}
