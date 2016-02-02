using UnityEngine;

// State Machines are responsible for processing states, notifying them when they're about to begin or conclude, etc.
public class StateMachine {
	private State _current_state;

	public void ChangeState(State new_state) {
		if (_current_state != null)
			_current_state.OnFinish();

		_current_state = new_state;
		// States sometimes need to reset their machine.
		// This reference makes that possible.
		_current_state.state_machine = this;
		_current_state.OnStart();
	}

	public void Reset() {
		if (_current_state != null)
			_current_state.OnFinish();
		_current_state = null;
	}

	public void Update() {
		if (_current_state != null) {
			float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
			_current_state.OnUpdate(time_delta_fraction);
		}
	}

	public bool IsFinished() {
		return _current_state == null;
	}
}

// A State is merely a bundle of behavior listening to specific events, such as...
// OnUpdate -- Fired every frame of the game.
// OnStart -- Fired once when the state is transitioned to.
// OnFinish -- Fired as the state concludes.
// State Constructors often store data that will be used during the execution of the State.
public class State {
	// A reference to the State Machine processing the state.
	public StateMachine state_machine;

	public virtual void OnStart() {}
	public virtual void OnUpdate(float time_delta_fraction) {} // time_delta_fraction is a float near 1.0 indicating how much more / less time this frame took than expected.
	public virtual void OnFinish() {}

	// States may call ConcludeState on themselves to end their processing.
	public void ConcludeState() { state_machine.Reset(); }
}

// A State that takes a renderer and a sprite, and implements idling behavior.
// The state is capable of transitioning to a walking state upon key press.
public class StateIdleWithSprite : State {
    PlayerControl pc;
	SpriteRenderer renderer;
	Sprite sprite;

	public StateIdleWithSprite(PlayerControl pc, SpriteRenderer renderer, Sprite sprite) {
		this.pc = pc;
		this.renderer = renderer;
		this.sprite = sprite;
	}

	public override void OnStart() {
		renderer.sprite = sprite;
	}

	public override void OnUpdate(float time_delta_fraction) {

		// Transition to walking animations on key press.
		if (Input.GetKeyDown(KeyCode.DownArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.linkRunDown, 6, KeyCode.DownArrow));
		else if (Input.GetKeyDown(KeyCode.UpArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.linkRunUp, 6, KeyCode.UpArrow));
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.linkRunRight, 6, KeyCode.RightArrow));
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.linkRunLeft, 6, KeyCode.LeftArrow));
	}
}

// A State for playing an animation until a particular key is released.
// Good for animations such as walking.
public class StatePlayAnimationForHeldKey : State {
	PlayerControl pc;
	SpriteRenderer renderer;
	KeyCode key;
	Sprite[] animation;
	int animation_length;
	float animation_progression;
	float animation_start_time;
	int fps;

	public StatePlayAnimationForHeldKey(PlayerControl pc, SpriteRenderer renderer, Sprite[] animation, int fps, KeyCode key) {
		this.pc = pc;
		this.renderer = renderer;
		this.key = key;
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

		// If another key is pressed, we need to transition to a different walking animation.
		if (Input.GetKeyDown(KeyCode.DownArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.linkRunDown, 6, KeyCode.DownArrow));
		else if (Input.GetKeyDown(KeyCode.UpArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.linkRunUp, 6, KeyCode.UpArrow));
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.linkRunRight, 6, KeyCode.RightArrow));
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.linkRunLeft, 6, KeyCode.LeftArrow));

		// If we detect the specified key has been released, return to the idle state.
		else if (!Input.GetKey(key))
			state_machine.ChangeState(new StateIdleWithSprite(pc, renderer, animation[1]));
	}
}

public class StateLinkNormalMovement : State {
	PlayerControl pc;

	public StateLinkNormalMovement(PlayerControl pc) {
		this.pc = pc;
	}

	public override void OnUpdate(float time_delta_fraction) {

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		// Mod 1 gets decimal, to be used for Grid-based movement
		Vector3 location = PlayerControl.instance.transform.position;
		float horizontalOffset = location.x % 1;
		float verticalOffset = location.y % 1;

        if (horizontalInput != 0) {
            pc.secondaryDirection = (int)verticalInput;
            verticalInput = 0;
        }

		if (horizontalInput > 0)
			pc.currentDirection = Direction.EAST;
		else if (horizontalInput < 0)
			pc.currentDirection = Direction.WEST;
		else if (verticalInput > 0)
			pc.currentDirection = Direction.NORTH;
		else if (verticalInput < 0)
			pc.currentDirection = Direction.SOUTH;
		
		if (horizontalInput != 0) {
			if (verticalOffset != 0 && verticalOffset != 0.5f) {
				horizontalInput = 0;
				location.y = gridPosition(verticalOffset, location.y, ref verticalInput);
				pc.transform.position = location;
			}
		} else if (verticalInput != 0) {
			if (horizontalOffset != 0 && horizontalOffset != 0.5f) {
				verticalInput = 0;
				location.x = gridPosition(horizontalOffset, location.x, ref horizontalInput);
				pc.transform.position = location;
			}
		}

		if (Input.GetKeyDown(KeyCode.I)) {
			pc.isInvincible = true;
			pc.cooldown = 9000;
			pc.keyCount = 9000;
			pc.rupeeCount = 9000;
			SpriteRenderer sr = pc.GetComponent<SpriteRenderer>();
			sr.color = Color.red;
		}

        pc.GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput, verticalInput, 0) * pc.walkingVelocity * time_delta_fraction;

		if (Input.GetKeyDown(KeyCode.A))
			state_machine.ChangeState(new StateLinkAttack(pc, pc.weapons[0], 15));
		else if (Input.GetKeyDown(KeyCode.S))
            if (pc.hasBoomerang && pc.bowOrBoomerang)
                state_machine.ChangeState(new StateLinkBoomerangThrow(pc, pc.weapons[2], 15));
            else if (pc.hasBow && !pc.bowOrBoomerang)
			    state_machine.ChangeState(new StateLinkBowShoot(pc, pc.weapons[1], 15));

	}

	float gridPosition(float offset, float location, ref float input) {
		float margin = 0.85f;
		if (offset <= 0.25f) {
			input = -1;
			if (offset < margin) {
				input = 0;
				location = Mathf.Floor(location);
			}
		}
		else if (offset >= 0.75f) {
			input = 1;
			if (Mathf.Abs(offset - 0.75f) < margin) {
				input = 0;
				location = Mathf.Ceil(location);
			}
		}
		else if (offset < 0.5f) {
			input = 1;
			if (Mathf.Abs(offset - 0.5f) < margin) {
				input = 0;
				location = Mathf.Floor(location) + 0.5f;
			}
		}
		else {
			input = -1;
			if (Mathf.Abs(offset - 0.5f) < margin) {
				input = 0;
				location = Mathf.Floor(location) + 0.5f;
			}
		}
		return location;
	}
}

public class StateLinkKnockBack: State {
    PlayerControl pc;

    public StateLinkKnockBack(PlayerControl pc) {
        this.pc = pc;
    }

    public override void OnStart() {
        pc.knockback = false;
        pc.GetComponent<Rigidbody>().AddForce(pc.knockbackDir * 9999);
        ConcludeState();
    }
}

public class StateLinkAttack : State {
	PlayerControl pc;
	GameObject weaponPrefab;
	GameObject weaponInstance;
	float cooldown = 0f;
	Vector3 directionOffset = Vector3.zero;

	public StateLinkAttack(PlayerControl pc, GameObject weaponPrefab, int cooldown) {
		this.pc = pc;
		this.weaponPrefab = weaponPrefab;
		this.cooldown = cooldown;
	}

    public override void OnStart() {
        pc.currentState = EntityState.ATTACKING;

        // No movement is allowed when link swings
        pc.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Spawn the weapon object
        weaponInstance = MonoBehaviour.Instantiate(weaponPrefab, pc.transform.position, Quaternion.identity) as GameObject;

        // Detect the offset position and new angle of the weapon
        // This is based on the direction that Link is facing
        Vector3 directionEulerangle = Vector3.zero;

        if (pc.currentDirection == Direction.NORTH) {
            directionOffset = Vector3.up;
            directionEulerangle = new Vector3(0, 0, 90);
        }
        else if (pc.currentDirection == Direction.EAST) {
            directionOffset = Vector3.right;
            directionEulerangle = new Vector3(30, 0, 0);
        }
        else if (pc.currentDirection == Direction.SOUTH) {
            directionOffset = Vector3.down;
            directionEulerangle = new Vector3(0, 0, 270);
        }
        else if (pc.currentDirection == Direction.WEST) {
            directionOffset = Vector3.left;
            directionEulerangle = new Vector3(0, 0, 180);
        }

        // Move and rotate weapon
        // weaponInstance.transform.position += directionOffset;
        Quaternion newWeaponRotation = new Quaternion();
        newWeaponRotation.eulerAngles = directionEulerangle;
        weaponInstance.transform.rotation = newWeaponRotation;
		if (pc.totalHealth == pc.currentHealth && !pc.swordThrown && pc.magicSword) {
            GameObject magicSword = MonoBehaviour.Instantiate(weaponPrefab, pc.transform.position, Quaternion.identity) as GameObject;
            magicSword.transform.rotation = newWeaponRotation;
            pc.swordThrown = true;
            magicSword.GetComponent<Rigidbody>().velocity = directionOffset * 12;
            magicSword.AddComponent<SwordThrow>();
        }
        directionOffset += weaponInstance.transform.position;
    }

	public override void OnUpdate(float time_delta_fraction) {
        weaponInstance.transform.position = Vector3.Slerp(weaponInstance.transform.position, directionOffset, 0.7f);

		cooldown -= time_delta_fraction;
		if (cooldown <= 0)
			ConcludeState();
	}

	public override void OnFinish() {
		pc.currentState = EntityState.NORMAL;
        MonoBehaviour.Destroy(weaponInstance);
	}
}

public class StateLinkBowShoot : State {
	PlayerControl pc;
	GameObject weaponPrefab;
	float cooldown = 0;

	public StateLinkBowShoot(PlayerControl pc, GameObject weaponPrefab, int cooldown) {
		this.pc = pc;
		this.weaponPrefab = weaponPrefab;
		this.cooldown = cooldown;
	}

    public override void OnStart() {
        if (pc.rupeeCount <= 0 || pc.arrowShot)
            ConcludeState();
        else {
            pc.rupeeCount--;
            pc.arrowShot = true;
            pc.currentState = EntityState.ATTACKING;

            // No movement is allowed when link swings
            pc.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // Spawn the weapon object
            GameObject weaponInstance = MonoBehaviour.Instantiate(weaponPrefab, pc.transform.position, Quaternion.identity) as GameObject;

            // Detect the offset position and new angle of the weapon
            // This is based on the direction that Link is facing
            Vector3 directionEulerangle = Vector3.zero;
            Vector3 directionOffset = Vector3.zero;

            if (pc.currentDirection == Direction.NORTH) {
                directionOffset = Vector3.up;
                directionEulerangle = new Vector3(0, 0, 90);
            }
            else if (pc.currentDirection == Direction.EAST) {
                directionOffset = Vector3.right;
                directionEulerangle = new Vector3(0, 0, 0);
            }
            else if (pc.currentDirection == Direction.SOUTH) {
                directionOffset = Vector3.down;
                directionEulerangle = new Vector3(0, 0, 270);
            }
            else if (pc.currentDirection == Direction.WEST) {
                directionOffset = Vector3.left;
                directionEulerangle = new Vector3(0, 0, 180);
            }

            // Move and rotate weapon
            Quaternion newWeaponRotation = new Quaternion();
            newWeaponRotation.eulerAngles = directionEulerangle;
            weaponInstance.transform.rotation = newWeaponRotation;
            weaponInstance.GetComponent<Rigidbody>().velocity = directionOffset * 12;
            weaponInstance.AddComponent<BowShoot>();
        }
    }

	public override void OnUpdate(float time_delta_fraction) {
		cooldown -= time_delta_fraction;
		if (cooldown <= 0)
			ConcludeState();
	}

	public override void OnFinish() {
		pc.currentState = EntityState.NORMAL;
	}
}

public class StateLinkBoomerangThrow : State {
    PlayerControl pc;
    GameObject weaponPrefab;
    float cooldown = 0;

    public StateLinkBoomerangThrow(PlayerControl pc, GameObject weaponPrefab, int cooldown) {
        this.pc = pc;
        this.weaponPrefab = weaponPrefab;
        this.cooldown = cooldown;
    }

    public override void OnStart() {
        if (pc.boomerangThrown)
            ConcludeState();
        else {
            pc.boomerangThrown = true;
            pc.currentState = EntityState.ATTACKING;

            // No movement is allowed when link swings
            pc.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // Spawn the weapon object
            GameObject weaponInstance = MonoBehaviour.Instantiate(weaponPrefab, pc.transform.position, Quaternion.identity) as GameObject;
            weaponInstance.AddComponent<BoomerangThrow>();
        }
    }

    public override void OnUpdate(float time_delta_fraction) {
        cooldown -= time_delta_fraction;
        if (cooldown <= 0)
            ConcludeState();
    }

    public override void OnFinish() {
        pc.currentState = EntityState.NORMAL;
    }
}
// Additional recommended states:
// StateDeath
// StateVictory
