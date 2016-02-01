using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragonSpriteAnimation : State {
	SpriteRenderer renderer;
	Sprite[] animation;
	int animation_length;
	float animation_start_time;
	int fps;

	public DragonSpriteAnimation(SpriteRenderer renderer, Sprite[] animation, int fps) {
		this.renderer = renderer;
		this.animation = animation;
		this.animation_length = animation.Length;
		this.fps = fps;

		if (this.animation_length <= 0)
			Debug.LogError("Empty animation submitted to state machine!");
	}

	public override void OnStart() {
		animation_start_time = Time.time;
	}

	public override void OnUpdate(float time_delta_fraction) {
		if (this.animation_length <= 0) {
			Debug.LogError("Empty animation submitted to state machine!");
			return;
		}

		// Modulus is necessary so we don't overshoot the length of the animation.
		int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation_length);
		renderer.sprite = animation[current_frame_index];
	}
}

public class StateDragonMovement : State {
	DragonControl ec;
	Vector3 targetLoc;
	Vector3 velocity;
    public float fireballSpeed = 6;
    float timer = 0.0f;
    float timeShoot = 3.5f;

	public StateDragonMovement(DragonControl ec) {
		this.ec = ec;
		targetLoc = ec.transform.position;
	}

	public override void OnUpdate(float time_delta_fraction) {
		Vector3 currentPosition = ec.transform.position;

		if (Vector3.Distance(targetLoc, currentPosition) <= 0.1f) {
			float fixedX = Mathf.Round(ec.transform.position.x);
			float fixedY = Mathf.Round(ec.transform.position.y);
			Vector3 fixedPos = new Vector3(fixedX, fixedY, 0);
			ec.transform.position = fixedPos;
			velocity = nextDragonDirection(ec.transform.position);
			targetLoc = ec.transform.position + velocity;
			ec.currentDirection = Utilities.findDirection(velocity);


            float roomCenterX = PlayerControl.instance.roomOffsetX * 16 + 7.5f;
            float roomCenterY = PlayerControl.instance.roomOffsetY * 11 + 5;
            Vector3 roomCenter = new Vector3(roomCenterX, roomCenterY, 0);

            float rightX = roomCenter.x + 5;
            float leftX = roomCenter.x + 1.5f;

            if (leftX >= currentPosition.x) {
				targetLoc = ec.transform.position + Vector3.right;
				velocity = Vector3.right;
            }
            else if (rightX <= currentPosition.x) {
				targetLoc = ec.transform.position + Vector3.left;
				velocity = Vector3.left;
            }
		}

        timer += Time.deltaTime;
        if (timer > timeShoot) {
            shootFireball();
            timer = 0.0f;
        }

        ec.GetComponent<Rigidbody>().velocity = velocity * ec.walkingVelocity;
	}

	public Vector3 nextDragonDirection(Vector3 position) {
        List<Vector3> allowedDirections = new List<Vector3>();
		addAllowedDirections(allowedDirections, position);
		int range = allowedDirections.Count;
		int random = Random.Range(0, range);
		return allowedDirections[random];
	}

	public void addAllowedDirections(List<Vector3> allowedDirections, Vector3 position) {
		int x = ec.currentDirection == Direction.EAST ? 1 : -1;

		if (Utilities.checkWalkable((int)position.x + 1, (int)position.y))
			allowedDirections.Add(Vector3.right);
		if (Utilities.checkWalkable((int)position.x - 1, (int)position.y))
			allowedDirections.Add(Vector3.left);
		if (Utilities.checkWalkable((int)position.x + x, (int)position.y))
			allowedDirections.Add(new Vector3(x, 0, 0));
	}

    public void shootFireball() {
        PlayerControl pc = PlayerControl.instance;
        GameObject fireballInstance = MonoBehaviour.Instantiate(ec.fireballPrefab, ec.transform.position, Quaternion.identity) as GameObject;
        Vector3 shootDirection = (pc.transform.position - ec.transform.position).normalized; 
        fireballInstance.GetComponent<Rigidbody>().velocity = shootDirection * fireballSpeed;

        // Doing math to calculate the tangent
        float yDist = Mathf.Tan(0.25f) * Mathf.Abs(pc.transform.position.x - ec.transform.position.x);
        shootDirection = (new Vector3(pc.transform.position.x, pc.transform.position.y - yDist, 0) - ec.transform.position).normalized;


        fireballInstance = MonoBehaviour.Instantiate(ec.fireballPrefab, new Vector3(ec.transform.position.x, ec.transform.position.y - 1, 0), Quaternion.identity) as GameObject;
        fireballInstance.GetComponent<Rigidbody>().velocity = shootDirection * fireballSpeed;

        shootDirection = (new Vector3(pc.transform.position.x, pc.transform.position.y + yDist, 0) - ec.transform.position).normalized;
        fireballInstance = MonoBehaviour.Instantiate(ec.fireballPrefab, new Vector3(ec.transform.position.x, ec.transform.position.y + 1, 0), Quaternion.identity) as GameObject;
        fireballInstance.GetComponent<Rigidbody>().velocity = shootDirection * fireballSpeed;
    }

}
