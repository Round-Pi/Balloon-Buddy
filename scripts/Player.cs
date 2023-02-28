using Godot;
using System;

// namespace BalloonBuddy {

public class Player : KinematicBody2D {
    // [Signal]
    // public delegate void Hit();
    // [Export] bool disableFalling = false;
    const float walkSpeed = 600; // positive
    const float runSpeed = 1200; // positive
    const float airSpeedPenalty = -600;
    const float initialJumpSpeed = 500; // positive
    const float slowFall = 1000;
    const float normalFall = 1200;
    // const int airTime = 2;
    const float lowestY = 600;
    //	public float startY = lowestY; // the Y position where jump starts    
    float jumpSpeed = 0;
    // public bool jumping = false;
    public float fall;
    // public Vector2 position;
    public Vector2 velocity;
    int counter = 0;
    bool counting = false;
    public bool haveBalloon = true;
    // private Bone2D hand;

    Main main;

    public void Start(Main m) {
        // node = GetNode<Node2D>("PlayerArea2D");
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
        main = m;
        // hand = main.balloon.GetNode<Skeleton2D>("Ribbon")
        //     .GetNode<Bone2D>("Segment5").GetNode<Bone2D>("Segment4")
        //     .GetNode<Bone2D>("Segment3").GetNode<Bone2D>("Segment2")
        //     .GetNode<Bone2D>("Segment1");
    }
    public void Start(Main m, Vector2 pos) {
        Position = pos;
        Start(m);
    }

    public override void _Ready() {
        fall = normalFall;
        GD.Print("Hi");
        // main = this.GetParent().GetParent<Main>();
    }

    // public void GetInput() {
    //     velocity.x = 0;
    //     bool right = Input.IsActionPressed("right");
    //     bool left = Input.IsActionPressed("left");
    //     bool jump = Input.IsActionPressed("jump");
    //     bool holdToRun = Input.IsActionPressed("hold_to_run");

    //     if (jump && IsOnFloor()) {
    //         jumping = true;
    //         velocity.y = -initialJumpSpeed;
    //     }
    //     if (holdToRun) {
    //         if (right)
    //             velocity.x += walkSpeed;
    //         if (left)
    //             velocity.x -= walkSpeed;
    //     }
    //     else {
    //         if (right)
    //             velocity.x += runSpeed;
    //         if (left)
    //             velocity.x -= runSpeed;
    //     }

    //     if (haveBalloon && Input.IsActionJustPressed("secondary_action")) {
    //         haveBalloon = false;
    //         // check if near balloon parking space, park it at that location
    //         main.balloon.isParked = true;
    //     }
    // }
    public override void _Process(float delta) {
        // if (haveBalloon) hand.Position = Position;
        if (counting) counter++;
        else counter = 0;
    }
    public override void _PhysicsProcess(float delta) {
        AnimatedSprite animatedSprite = GetNode<AnimatedSprite>("PlayerSprite");
        velocity.x = 0;
        if (haveBalloon && Input.IsActionJustPressed("secondary_action")) {
            haveBalloon = false;
            // check if near balloon parking space, park it at that location
            main.balloon.isParked = true;
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
        // if (counter >= airTime && counting) {
        //     counting = false;
        //     counter = 0;
        // }
        // if (!IsOnFloor()) {
        // if (!Input.IsActionPressed("jump")) 

        // if (jumpSpeed <= 0) {
        //     velocity.y = jumpSpeed; // gravity is an increase in Y
        //     jumpSpeed++; // lower jumpSpeed over time
        // }
        // else {
        // jumpSpeed = 0;

        // jumping = false;
        // }
        // }
        if (!IsOnFloor()) {
            if (jumpSpeed > 0) {
                jumpSpeed = 0;
                // jumpSpeed -= initialJumpSpeed / 3;
            }
        }
        // Player jumps	
        if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
            // jumping = true;
            jumpSpeed = initialJumpSpeed;
            // velocity.y = jumpSpeed;
            // velocity.y = -initialJumpSpeed;
            // play jump sound
            counting = true;
            // startY = lowestY; // startY resets
        }

        if (Input.IsActionPressed("jump") && !IsOnFloor() //&& !Input.IsActionJustPressed("jump")
        ) {
            fall = slowFall;
        }
        else {
            fall = normalFall;
        }
        // if (disableFalling) {
        // 	Position = new Vector2(position.x, position.y + jumpSpeed);
        // }
        // else {
        // 	if (Position.y <= lowestY) Position = new Vector2(position.x, position.y + fall + jumpSpeed);
        // 	else Position = new Vector2(position.x, position.y);
        // }
        // GetInput();
        if (jumpSpeed < 0) jumpSpeed = 0;
        velocity.y += fall * delta - jumpSpeed;
        // if (jumping && IsOnFloor())
        //     jumping = false;
        velocity = MoveAndSlide(velocity, new Vector2(0, -1));
    }

    // Add new functions above^
}
// }
