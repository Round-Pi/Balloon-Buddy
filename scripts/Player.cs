using Godot;
using System;

// namespace BalloonBuddy {

public class Player : KinematicBody2D {
    const float walkSpeed = 600; // positive
    const float runSpeed = 1200; // positive
    const float airSpeedPenalty = -600; // TODO: Add airSpeedPenalty
    const float initialJumpSpeed = 500; // positive
    const float slowFall = 1000;
    const float normalFall = 1200;

    float jumpSpeed = 0;
    int counter = 0;
    bool counting = false;
    public bool haveBalloon = true;
    public float fall;
    public Vector2 velocity;
    private Main main;
    private Balloon balloon;

    public void Start(Main m) {
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
        main = m;
    }
    public void Start(Main m, Vector2 pos) {
        Position = pos;
        Start(m);
    }

    public override void _Ready() {
        fall = normalFall;
        GD.Print("Hi");
        // main = GetParent().GetParent<Main>();
        balloon = GetNode<Balloon>("Balloon");
    }
    public override void _Process(float delta) {
        // if (haveBalloon) hand.Position = Position;
        if (counting) counter++;
        else counter = 0;
    }
    public override void _PhysicsProcess(float delta) {
        AnimatedSprite animatedSprite = GetNode<AnimatedSprite>("PlayerSprite");
        velocity.x = 0;
        if (Input.IsActionJustPressed("secondary_action")) {
            if (haveBalloon) {
                haveBalloon = false;
                // check if near balloon parking space, park it at that location
                balloon.Park(Position);
                RemoveChild(balloon);
                // balloon.Owner = main.level;
                main.level.GetNode<RigidBody2D>("Block").AddChild(balloon);
            }
            else {
                haveBalloon = true;
                balloon.PickUp();
                main.level.GetNode<RigidBody2D>("Block").RemoveChild(balloon);
                AddChild(balloon);
            }
        }

        if (Input.IsActionPressed("left")) { // to the left!
            if (Input.IsActionPressed("hold_to_run")) {
                velocity.x = -runSpeed;
            }
            else { velocity.x = -walkSpeed; }
            animatedSprite.FlipH = true;
        }
        else if (Input.IsActionPressed("right")) { // to the Right!
            if (Input.IsActionPressed("hold_to_run")) {
                velocity.x = runSpeed;
            }
            else { velocity.x = walkSpeed; }
            animatedSprite.FlipH = false;
        }

        // Falling, gravity physics stuff
        if (!IsOnFloor()) {
            if (jumpSpeed > 0) {
                jumpSpeed = 0;
                // jumpSpeed -= initialJumpSpeed / 3;
            }
        }
        // Player jumps	
        if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
            jumpSpeed = initialJumpSpeed;
            // play jump sound
            counting = true;
        }

        if (Input.IsActionPressed("jump") && !IsOnFloor()) {
            fall = slowFall;
        }
        else {
            fall = normalFall;
        }
        if (jumpSpeed < 0) jumpSpeed = 0;
        velocity.y += fall * delta - jumpSpeed;
        velocity = MoveAndSlide(velocity, new Vector2(0, -1));
    }

    // Add new functions above^
}
// }
