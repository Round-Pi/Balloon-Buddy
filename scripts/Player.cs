using Godot;
using System;

// namespace BalloonBuddy {

// public class Player : Area2D
public class Player : RigidBody2D {
	// [Signal]
	// public delegate void Hit();
	const float walkSpeed = 600; // positive
	const float runSpeed = 1200; // positive
	const float initialJumpSpeed = 10; // positive
									   //	const float slowFall = 0.3f;
	const float normalFall = 2;

	const float lowestY = 532;
	//	public float startY = lowestY; // the Y position where jump starts    
	float jumpSpeed = 0;
	public bool jumping = false;
	public float fall;
	public Vector2 position;
	//	Timer timer;
	int counter = 0;
	bool counting = false;
	// private Node2D node;
	// Balloon balloon;

	// public void BalloonFollows(object obj) {
	//     if (balloon == null) balloon = GetNode<Balloon>("BalloonArea2D");
	//     if (ShortcutTools.DistanceFloat(Position, balloon.Position) > Balloon.ribbonLength) {
	//         Vector2 v = balloon.Position - Position;
	//         balloon.Position = Position + Balloon.ribbonLength * v.Normalized();
	//     }
	// }

	public void Start(Vector2 pos) {
		position = pos;
		// node = GetNode<Node2D>("PlayerArea2D");
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	// public Player(Game myGame, Game1 g1) : base(myGame) {}

	//	public void OnPlayerBodyEntered(PhysicsBody2D body) {
	//		if(body.name =="Terrain Layer")
	//		else if (body.name=="Enemy") {
	//			Hide(); // Player disappears after being hit.
	//			EmitSignal(nameof(Hit));
	//			// Must be deferred as we can't change physics properties on a physics callback.
	//			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
	//		}
	//
	//	}
	public override void _Ready() {
		position = Position;
		fall = normalFall;
		//		timer = new Timer();
		GD.Print("Hi");

	}
	public override void _Process(float delta) {
		// KeyboardState keyState = Keyboard.GetState(); // check keyboard state
		AnimatedSprite animatedSprite = GetNode<AnimatedSprite>("PlayerSprite");
		position = Position;
		if (Input.IsActionPressed("left")) { // to the left!
			if (Input.IsActionPressed("hold_to_run")) {
				position.x -= runSpeed * delta;
			}
			else { position.x -= walkSpeed * delta; }
			animatedSprite.FlipH = true;
		}
		else if (Input.IsActionPressed("right")) { // to the Right!
			if (Input.IsActionPressed("hold_to_run")) {
				position.x += runSpeed * delta;
			}
			else { position.x += walkSpeed * delta; }
			animatedSprite.FlipH = false;
		}


		if (counter >= 30 || !Input.IsActionPressed("jump")) { // and falling, gravity physics stuff
			counting = false;
			position.y += jumpSpeed; // gravity is an increase in Y
			if (counter > 0) counter--;

			//			jumping = false;
			//			if (Input.IsActionPressed("jump") || jumpSpeed > 0) { jumpSpeed += fall/4; } // Fall slower when spacebar down
			//			// else if (Input.IsActionReleased("jump")) { jumpSpeed += normalFall; } // Fall faster when spacebar up
			//			else if (!Input.IsActionPressed("jump")) { jumpSpeed += fall; } // Fall faster when spacebar up
			//			if (position.y >= startY) {
			//				position.y = startY; // like a clamp?
			//				jumping = false; // aka, go to the else statement below.
			//			}
		}
		else {
			if (Input.IsActionPressed("jump") && !counting) { // Player jumps			
															  //				timer.Stop();
															  //				jumping = true; // aka, go to the if statement above.
				jumpSpeed = -12; // initial speed of jump
								 // play jump sound
								 //				timer.Start(0.2f);
				counting = true;
				//				startY = lowestY; // startY resets
			}
		}
		if (counting) counter++;
		Position = new Vector2(position.x, position.y + fall);
		// base._Process(delta);
	}

	public void OnCollisionEnter() {
		//		if (node.GetCollisionLayerBit(1)) 
		fall = 0;
		GD.Print("Aaaaaaaahhhh");
	}


	public void OnCollisionExit() {
		fall = normalFall;
		GD.Print("ok");
	}

	// Add new functions above^
}
// }


// private void _on_BalloonArea2D_ready()
// {
// 	// Replace with function body.
// }
