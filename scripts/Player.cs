using Godot;
using System;

// namespace BalloonBuddy {

public class Player : RigidBody2D {
    // [Signal]
    // public delegate void Hit();
    [Export] bool disableFalling = false;
    const float walkSpeed = 600; // positive
    const float runSpeed = 1200; // positive
    const float initialJumpSpeed = 20; // positive
    const float slowFall = 9;
    const float normalFall = 15;

    const float lowestY = 600;
    //	public float startY = lowestY; // the Y position where jump starts    
    float jumpSpeed = 0;
    public bool jumping = false;
    public float fall;
    public Vector2 position;
    int counter = 0;
    bool counting = false;
    public bool haveBalloon = true;

    Main main;

    public void Start() {
        // node = GetNode<Node2D>("PlayerArea2D");
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }
    public void Start(Vector2 pos) {
        position = pos;
        Start();
    }

    public override void _Ready() {
        position = Position;
        if (!disableFalling)
            fall = normalFall;
        GD.Print("Hi");
        main = GetParent().GetParent<Main>();
    }
    public override void _Process(float delta) {
        AnimatedSprite animatedSprite = GetNode<AnimatedSprite>("PlayerSprite");
        position = Position;
        if (haveBalloon && Input.IsActionJustPressed("secondary_action")) {
            haveBalloon = false;
            // check if near balloon parking space, park it at that location
            main.balloon.isParked = true;
        }
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
        // Falling, gravity physics stuff
        if (counter >= 30) {
            counting = false;
            counter = 0;
        }
        if (jumping && (counting || Input.IsActionPressed("jump"))) {
            // if (!Input.IsActionPressed("jump")) 

            if (jumpSpeed <= 0) {
                position.y += jumpSpeed; // gravity is an increase in Y
                jumpSpeed++;
            }
            else {
                jumpSpeed = 0;
                jumping = false;
            }

            // jumping = false;
            // if (Input.IsActionPressed("jump") || jumpSpeed > 0) { jumpSpeed += fall / 4; } // Fall slower when spacebar down
            //                                                                                // else if (Input.IsActionReleased("jump")) { jumpSpeed += normalFall; } // Fall faster when spacebar up
            // else if (!Input.IsActionPressed("jump")) { jumpSpeed += fall; } // Fall faster when spacebar up
            // if (position.y >= startY) {
            //     position.y = startY; // like a clamp?
            //     jumping = false; // aka, go to the else statement below.
            // }
        }

        // Player jumps	
        if (Input.IsActionJustPressed("jump") && !counting && !jumping) {
            // jumping = true; // aka, go to the if statement above.
            jumping = true;
            jumpSpeed = -initialJumpSpeed;
            // play jump sound
            position.y += jumpSpeed;
            counting = true;
            // startY = lowestY; // startY resets
        }
        if (!disableFalling) {
            if (Input.IsActionPressed("jump")) {
                fall = slowFall;
            }
            else {
                fall = normalFall;
            }
        }
        if (counting) counter++;
        if (disableFalling) {
            Position = new Vector2(position.x, position.y + jumpSpeed);
        }
        else {
            if (Position.y <= lowestY) Position = new Vector2(position.x, position.y + fall + jumpSpeed);
            else Position = new Vector2(position.x, position.y);
        }

    }

    public void OnCollisionEnter() {
        // if (node.GetCollisionLayerBit(1))
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
