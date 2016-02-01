using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Prefab {
    BLOB,
    BOOMER,
    DRAGON,
    SKELETON,
    WALLMASTER,
    // BATS
    HEART,
    KEY,
    RUPEE
    // RUPEE5
};

public class SpawnObject {
    public Prefab prefab;
    public bool isAlive;
    public float x;
    public float y;

    public SpawnObject(Prefab prefab, bool isAlive, float x, float y) {
        this.prefab = prefab;
        this.isAlive = isAlive;
        this.x = x;
        this.y = y;
    }
};

public class Room {
    public List<SpawnObject> enemies = new List<SpawnObject>();
    public bool isKeyRoom = false;
    public int keyX = 0;
    public int keyY = 0;

    public void addSpawnObject(Prefab p, float x, float y) {
        enemies.Add(new SpawnObject(p, true, x, y));
    }
    public void setEnemyToDead(int i) {
        enemies[i].isAlive = true;
    }
    public void setKeyRoom( int keyX, int keyY) {
        isKeyRoom = true;
        this.keyX = keyX;
        this.keyY = keyY;
    }

    public bool isClear() {
        for (int i = 0; i < enemies.Count; i++)
            if (enemies[i].isAlive)
                return false;
        return true;
    }

    public void emptyFunc() {}
};

public class GameState : MonoBehaviour {
    public static GameState instance;
    public GameObject[] prefabs;
    public Room[,] rooms = new Room[6, 6];

    // Use this for initialization
    void Start() {
        instance = this;
        for (int i = 0; i < 6; i++) 
            for (int j = 0; j < 6; j++)
                rooms[i, j] = new Room();

        Room room;
        // Room 3, 0
        room = rooms[3, 0];
        room.setKeyRoom(50, 6);
        room.addSpawnObject(Prefab.SKELETON, 50, 8);

    }

	// Update is called once per frame
	void Update() {}

    public void markEnemyDestroyed(int i, int x, int y) {
        Room room = rooms[x, y];
        room.enemies[i].isAlive = false;
        if (room.isKeyRoom && room.isClear())
            Instantiate(prefabs[(int)Prefab.KEY], new Vector3(room.keyX, room.keyY, 0), Quaternion.identity);
    }

    public void spawnRoom(int x, int y) {
        Room room = rooms[x, y];
        
        for (int i = 0; i < room.enemies.Count; i ++) {
            SpawnObject s = room.enemies[i];
            if (s.isAlive)
                Instantiate(prefabs[(int)s.prefab], new Vector3(s.x, s.y, 0), Quaternion.identity);
        }

    }

    public static void destroyOnScreen() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Rupee");
        for (int i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);
        gameObjects = GameObject.FindGameObjectsWithTag("Rupee5");
        for (int i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);
        gameObjects = GameObject.FindGameObjectsWithTag("Heart");
        for (int i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);
        gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);
        gameObjects = GameObject.FindGameObjectsWithTag("Weapon");
        for (int i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);
        gameObjects = GameObject.FindGameObjectsWithTag("EnemyWeapon");
        for (int i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);
    }
}
