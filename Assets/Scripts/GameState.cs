using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Prefab {
    BLOB,
    BOOMER,
    DRAGON,
    SKELETON,
    WALLMASTER,
    HEART,
    KEY,
    RUPEE,
    RUPEE5,
    BOOMERANG,
    HEARTUP,
	BATS
};

public class SpawnObject {
    public Prefab prefab;
    public bool isAlive;
    public float x;
    public float y;
    public int index;

    public SpawnObject(Prefab prefab, bool isAlive, float x, float y, int index) {
        this.prefab = prefab;
        this.isAlive = isAlive;
        this.x = x;
        this.y = y;
        this.index = index;
    }
};

public class Room {
    public List<SpawnObject> enemies = new List<SpawnObject>();
    public bool isKeyRoom = false;
    public int keyX = 0;
    public int keyY = 0;

    public void addSpawnObject(Prefab p, float x, float y) {
        enemies.Add(new SpawnObject(p, true, x, y, enemies.Count));
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
    void Awake() {
        instance = this;

		if (Application.loadedLevelName != "CustomDungeon") {
        for (int i = 0; i < 6; i++)

            for (int j = 0; j < 6; j++)
                rooms[i, j] = new Room();

        Room room;

		// Room 1, 0 
		room = rooms[1, 0];
		room.setKeyRoom(26, 2);
		room.addSpawnObject(Prefab.BATS, 21, 6);
		room.addSpawnObject(Prefab.BATS, 21, 4);
		room.addSpawnObject(Prefab.BATS, 19, 3);

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

		// Room 3, 2
		room = rooms[3, 2];
		room.addSpawnObject(Prefab.BATS, 53, 30);
		room.addSpawnObject(Prefab.BATS, 53, 24);
		room.addSpawnObject(Prefab.BATS, 58, 28);
		room.addSpawnObject(Prefab.BATS, 56, 28);
		room.addSpawnObject(Prefab.BATS, 58, 26);
		room.addSpawnObject(Prefab.BATS, 60, 29);
		room.addSpawnObject(Prefab.BATS, 60, 25);
		room.addSpawnObject(Prefab.BATS, 61, 27);

		// Room 1, 2 
		room = rooms[1, 2];
		room.addSpawnObject(Prefab.BATS, 26, 30);
		room.addSpawnObject(Prefab.BATS, 26, 24);
		room.addSpawnObject(Prefab.BATS, 21, 26);
		room.addSpawnObject(Prefab.BATS, 19, 25);
		room.addSpawnObject(Prefab.BATS, 18, 27);
		room.addSpawnObject(Prefab.BATS, 19, 29);

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

		// Room 5, 0
		room = rooms[5, 0];
		room.addSpawnObject(Prefab.BATS, 87, 6);
		room.addSpawnObject(Prefab.BATS, 83, 3);
		room.addSpawnObject(Prefab.BATS, 90, 8);
		room.addSpawnObject(Prefab.BATS, 90, 6);
		}

		// Spawns for custom level
		else {
			for (int i = 0; i < 3; i++)

				for (int j = 0; j < 3; j++)
					rooms[i, j] = new Room();

			Room room;

			// Room (1, 2)
			room = rooms[1, 2];
			room.addSpawnObject(Prefab.BLOB, 21, 26);

			// Room (1, 1)
			room = rooms[1, 1];
			room.addSpawnObject(Prefab.BLOB, 23, 15);
			room.addSpawnObject(Prefab.BLOB, 19, 17);
			room.addSpawnObject(Prefab.BLOB, 22, 12);
			room.addSpawnObject(Prefab.BLOB, 28, 13);

			// Room (1, 0)
			room = rooms[1, 0];
			room.setKeyRoom(27, 4);
			room.addSpawnObject(Prefab.BOOMER, 23, 3);

			// Room (0, 0)
			room = rooms[0, 0];
			room.setKeyRoom(11, 4);
			room.addSpawnObject(Prefab.BATS, 11, 2);
			room.addSpawnObject(Prefab.BATS, 4, 2);
			room.addSpawnObject(Prefab.BLOB, 5, 6);
			room.addSpawnObject(Prefab.BLOB, 10, 6);
			room.addSpawnObject(Prefab.BOOMER, 12, 6);
			room.addSpawnObject(Prefab.BOOMER, 3, 2);
		}
	}

	// Update is called once per frame
	void Update() {}

    public void enemyTakeDamage(GameObject enemy) {
            EnemyControl ec = enemy.GetComponent<EnemyControl>();
            ec.currentHealth--;
            int x = PlayerControl.instance.roomOffsetX;
            int y = PlayerControl.instance.roomOffsetY;
            if (ec.currentHealth <= 0) {
                if (x == 4 && y == 3)
                    DropItem(enemy);
				else if (Application.loadedLevelName == "CustomDungeon" && x == 0 && y == 1)
					DropItem(enemy);
                else
                    markEnemyDestroyed(enemy, ec.index, x, y);
                Destroy(enemy);
            }
            else {
                ec.damageCooldown = 1;
                SpriteRenderer sr = ec.GetComponent<SpriteRenderer>();
                sr.color = Color.red;
            }

    }

    public void markEnemyDestroyed(GameObject enemy, int i, int x, int y) {
        Room room = rooms[x, y];
        room.enemies[i].isAlive = false;
        if (room.isKeyRoom && room.isClear())
            Instantiate(prefabs[(int)Prefab.KEY], new Vector3(room.keyX, room.keyY, 0), Quaternion.identity);
        else if (x == 3 && y == 3 && room.isClear())
            Instantiate(prefabs[(int)Prefab.BOOMERANG], new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0), Quaternion.identity);
        else if (x == 4 && y == 4 && room.isClear())
            Instantiate(prefabs[(int)Prefab.HEARTUP], new Vector3(75, 49, 0), Quaternion.identity);
		else if (Application.loadedLevelName == "CustomDungeon" && x == 1 && y == 2) {
			PlayerControl.instance.weapons[0] = PlayerControl.instance.weapons[3];
			Instantiate(PlayerControl.instance.weapons[4], new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0), Quaternion.identity);
		}
        else
            DropItem(enemy);
    }

    public void spawnRoom(int x, int y) {
		Room room = rooms[x, y];
		for (int i = 0; i < room.enemies.Count; i ++) {
			SpawnObject s = room.enemies[i];
			if (s.isAlive) {
				GameObject g = MonoBehaviour.Instantiate(prefabs[(int)s.prefab], new Vector3(s.x, s.y, 0), Quaternion.identity) as GameObject;
				g.GetComponent<EnemyControl>().index = s.index;
			}
		}
    }

    void DropItem(GameObject enemy) {
        float chance = Random.Range(0f, 1f);
        if (chance < 0.8)
            return;
        chance = Random.Range(0f, 1f);
        if (PlayerControl.instance.currentHealth != PlayerControl.instance.totalHealth) {
            if (chance < 0.25)
                Instantiate(prefabs[(int)Prefab.HEART], new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0), Quaternion.identity);
            else if (chance < 0.5)
                Instantiate(prefabs[(int)Prefab.RUPEE5], new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0), Quaternion.identity);
            else
                Instantiate(prefabs[(int)Prefab.RUPEE], new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0), Quaternion.identity);
        }
        else {
            if (chance < 0.33)
                Instantiate(prefabs[(int)Prefab.RUPEE5], new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0), Quaternion.identity);
            else
                Instantiate(prefabs[(int)Prefab.RUPEE], new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0), Quaternion.identity);
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

        PlayerControl pc = PlayerControl.instance;
        pc.arrowShot = false;
        pc.swordThrown = false;
        pc.boomerangThrown = false;
    }

}
