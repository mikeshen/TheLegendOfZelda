﻿using UnityEngine;
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

		// Room 1, 0 should actually be bats
		room = rooms[1, 0];
		room.setKeyRoom(26, 2);
		room.addSpawnObject(Prefab.SKELETON, 21, 6);
		room.addSpawnObject(Prefab.SKELETON, 21, 4);
		room.addSpawnObject(Prefab.SKELETON, 19, 3);

		// Room 2, 1
		room = rooms[2, 1];
		room.addSpawnObject(Prefab.SKELETON, 41, 17);
		room.addSpawnObject(Prefab.SKELETON, 38, 17);
		room.addSpawnObject(Prefab.SKELETON, 37, 15);

        // Room 3, 0
        room = rooms[3, 0];
        room.setKeyRoom(50, 6);
        room.addSpawnObject(Prefab.SKELETON, 53, 8);
		room.addSpawnObject(Prefab.SKELETON, 53, 2);
		room.addSpawnObject(Prefab.SKELETON, 56, 6);
		room.addSpawnObject(Prefab.SKELETON, 56, 4);
		room.addSpawnObject(Prefab.SKELETON, 60, 7);

		// Room 2, 2
		room = rooms[2, 2];
		room.setKeyRoom(39, 27);
		room.addSpawnObject(Prefab.SKELETON, 42, 26);
		room.addSpawnObject(Prefab.SKELETON, 40, 27);
		room.addSpawnObject(Prefab.SKELETON, 41, 28);
		room.addSpawnObject(Prefab.SKELETON, 42, 30);
		room.addSpawnObject(Prefab.SKELETON, 44, 29);

		// Room 3, 2 should actually be bats
		room = rooms[3, 2];
		room.addSpawnObject(Prefab.SKELETON, 53, 30);
		room.addSpawnObject(Prefab.SKELETON, 53, 24);
		room.addSpawnObject(Prefab.SKELETON, 58, 28);
		room.addSpawnObject(Prefab.SKELETON, 56, 28);
		room.addSpawnObject(Prefab.SKELETON, 58, 26);
		room.addSpawnObject(Prefab.SKELETON, 60, 29);
		room.addSpawnObject(Prefab.SKELETON, 60, 25);
		room.addSpawnObject(Prefab.SKELETON, 61, 27);

		// Room 1, 2 should actually be bats
		room = rooms[1, 2];
		room.addSpawnObject(Prefab.SKELETON, 26, 30);
		room.addSpawnObject(Prefab.SKELETON, 26, 24);
		room.addSpawnObject(Prefab.SKELETON, 21, 26);
		room.addSpawnObject(Prefab.SKELETON, 19, 25);
		room.addSpawnObject(Prefab.SKELETON, 18, 27);
		room.addSpawnObject(Prefab.SKELETON, 19, 29);

		// Room 1, 3
		room = rooms[1, 3];
		room.addSpawnObject(Prefab.BLOB, 21, 41);
		room.addSpawnObject(Prefab.BLOB, 19, 40);
		room.addSpawnObject(Prefab.BLOB, 28, 40);

		// Room 2, 3
		room = rooms[2, 3];
		room.addSpawnObject(Prefab.BLOB, 37, 41);
		room.addSpawnObject(Prefab.BLOB, 37, 35);
		room.addSpawnObject(Prefab.BLOB, 40, 39);
		room.addSpawnObject(Prefab.BLOB, 40, 37);
		room.addSpawnObject(Prefab.BLOB, 45, 38);

		// Room 2, 4 
		room = rooms[2, 4];
		room.setKeyRoom(40, 49);
		room.addSpawnObject(Prefab.SKELETON, 40, 49);
		room.addSpawnObject(Prefab.SKELETON, 37, 51);
		room.addSpawnObject(Prefab.SKELETON, 42, 47);

		// Room 2, 5
		room = rooms[2, 5];
		room.setKeyRoom(40, 63);
		room.addSpawnObject(Prefab.BOOMER, 37, 63);
		room.addSpawnObject(Prefab.BOOMER, 42, 59);
		room.addSpawnObject(Prefab.BOOMER, 39, 61);

		// Room 3, 3 also boomerang drop
		room = rooms[3, 3];
		room.addSpawnObject(Prefab.BOOMER, 53, 41);
		room.addSpawnObject(Prefab.BOOMER, 53, 35);
		room.addSpawnObject(Prefab.BOOMER, 61, 38);

		// Room 4, 4 also heart container drop
		room = rooms[4, 4];
		room.addSpawnObject(Prefab.DRAGON, 75, 49);
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
