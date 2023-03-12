using Godot;
using System;

// namespace BalloonBuddy {

public class Player : KinematicBody2D {
    const float walkSpeed = 600; // positive
    const float runSpeed = 1200; // positive
    const float maxSpeedBoost = 2500;
    const int maxSpeedBoostTime = 8; // in frames, can be shorten by letting go of shift key
    const int speedBoostCoolDown = 4; // a nth of speedBoost is removed per frame, also used for counting
    const float initialJumpSpeed = 500; // positive
    const int swingCounterMax = 2;
    const float slowFall = 1000;
    const float normalFall = 1200;

    float speedBoost = 0;
    int speedBoostTimeCounter = 0;
    int speedBoostCoolDownCounter = 0;
    float jumpSpeed = 0;
    int counter = 0;
    bool counting = false;
    public bool haveBalloon = true;
    public float fall;
    public Vector2 velocity;
    private Main main;
    private Balloon balloon;
    private bool facingRight = true;
    bool canPark = false;
    Area2D parkHere;
    bool swingStart = false;
    int swingCounter = 0;
    // private bool boostLeft = false;
    // private bool boostRight = false;

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
        Testing();
        if (Input.IsActionJustPressed("secondary_action") && canPark) {
            if (haveBalloon) {
                haveBalloon = false;
                // check if near balloon parking space, park it at that location
                balloon.Park(Position);
                RemoveChild(balloon);
                // balloon.Owner = main.level;
                parkHere.AddChild(balloon);
            }
            else {
                haveBalloon = true;
                balloon.PickUp();
                parkHere.RemoveChild(balloon);
                AddChild(balloon);
            }
        }

        // velocity.x = 0;
        velocity.x = speedBoost;
        if (Input.IsActionJustPressed("hold_to_run") && speedBoost == 0 && AreAllBoostCountersZero()) {
            SpeedBoostStart();
        }
        // else if (speedBoost != 0 && Input.IsActionPressed("hold_to_run"))
        else if (speedBoostTimeCounter > 0 && speedBoost != 0) {
            // velocity.x = speedBoost;
            speedBoostTimeCounter--;
        }
        if (speedBoostTimeCounter <= 0 && speedBoostCoolDownCounter > 0) {
            // if (speedBoost >= -1 && speedBoost <= 1) 
            if (speedBoost >= -(maxSpeedBoost / speedBoostCoolDown) && speedBoost <= (maxSpeedBoost / speedBoostCoolDown)) {
                speedBoost = 0;
                SetAllBoostCountersToZero();
            }
            if (speedBoost > 0) {
                // speedBoost--;
                speedBoost -= maxSpeedBoost / speedBoostCoolDown;
            }
            else if (speedBoost < 0) {
                // speedBoost++;
                speedBoost += maxSpeedBoost / speedBoostCoolDown;
            }
            speedBoostCoolDownCounter--;
        }
        if (AreAllBoostCountersZero()) {
            speedBoost = 0;
        }

        if (speedBoostTimeCounter > 0 && Input.IsActionJustReleased("hold_to_run")) {
            speedBoostTimeCounter = 0;
            // speedBoost = 0;
            // speedBoostCoolDownCounter = speedBoostCoolDown;
        }

        if (Input.IsActionPressed("left")) { // to the left!
            if (Input.IsActionPressed("hold_to_run") && !Input.IsActionJustPressed("hold_to_run") && IsOnFloor()) {
                velocity.x = -runSpeed;
            }
            else { velocity.x = -walkSpeed; }
            animatedSprite.FlipH = true;
            facingRight = false;
            // if (boostRight) { // use to cancel boosts
            //     speedBoost = 0;
            //     boostLeft = false;
            //     boostRight = false;
            // }
        }
        else if (Input.IsActionPressed("right")) { // to the Right!
            if (Input.IsActionPressed("hold_to_run") && !Input.IsActionJustPressed("hold_to_run") && IsOnFloor()) {
                velocity.x = runSpeed;
            }
            else { velocity.x = walkSpeed; }
            animatedSprite.FlipH = false;
            facingRight = true;
            // if (boostLeft) { // use to cancel boosts
            //     speedBoost = 0;
            //     boostRight = false;
            //     boostLeft = false;
            // }
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
        if (haveBalloon) {
            if (balloon.isStuck && !balloon.isParked) {
                // YouPhysics(delta);
                if (balloon.Position.y < Position.y
                && ShortcutTools.DistanceFloat(Position, balloon.Position) >= Balloon.ribbonLength
                ) {
                    // Position = balloon.Position + ShortcutTools.Normalize(Balloon.ribbonLength, Position);
                    if (
                        // (swingCounter <= swingCounterMax) &&
                        !swingStart &&
                        ((Input.IsActionPressed("left") && Position.x < balloon.Position.x)
                        || (Input.IsActionPressed("right") && Position.x > balloon.Position.x)
                        || (Input.IsActionPressed("hold_to_run") &&
                            ((!facingRight && Position.x > balloon.Position.x)
                            || (facingRight && Position.x < balloon.Position.x))))
                    ) {
                        // velocity.y -= Math.Abs(velocity.x) / 6; // converts x velocity to y velocity
                        velocity -= balloon.Position;
                        swingStart = true;
                    }
                    else if (
                    // swingStart &&
                    (swingCounter >= swingCounterMax || (Input.IsActionJustReleased("left") || Input.IsActionJustReleased("right") || Input.IsActionJustReleased("hold_to_run")))) {
                        swingStart = false;
                        swingCounter = 0;
                    }
                    else if (swingStart) swingCounter++;
                    // else {
                    //     velocity.y = 0;
                    //     // if (velocity.y <= -6)
                    //     //     velocity.y += 6;
                    // }
                }
            }
        }

        velocity = MoveAndSlide(velocity, new Vector2(0, -1));
    }
    // public void YouPhysics(float delta) { // TODO:
    //     if (ShortcutTools.DistanceFloat(balloon.Position, Position) > Balloon.ribbonLength) {
    //         Position = ShortcutTools.Normalize(Balloon.ribbonLength, Position);
    //     }
    // }

    public void SpeedBoostStart() { // only runs on the first frame of boost
        if (facingRight) {
            speedBoost = maxSpeedBoost;
            // boostRight = true;
        }
        else { // facing left
            speedBoost = -maxSpeedBoost;
            // boostLeft = true;
        }
        // set all counters to max:
        speedBoostTimeCounter = maxSpeedBoostTime;
        speedBoostCoolDownCounter = speedBoostCoolDown;
    }

    private void SetAllBoostCountersToZero() {
        speedBoostTimeCounter = 0;
        speedBoostCoolDownCounter = 0;
    }
    private bool AreAllBoostCountersZero() {
        if (speedBoostTimeCounter < 0 && speedBoostCoolDownCounter < 0) {
            SetAllBoostCountersToZero();
        }
        return speedBoostTimeCounter <= 0 && speedBoostCoolDownCounter <= 0;
    }
    private void Testing() {
        if (Input.IsActionPressed("down")) { // for testing
            balloon.isStuck = true;
        }
        else if (Input.IsActionPressed("up")) {
            balloon.isStuck = false;
        }
    }
    public void _on_Park_Spot_body_entered(Node ps) {
        parkHere = main.level.GetNode<Area2D>("Park Spot"); // TODO: Change later
        canPark = true;
        GD.Print("Parking!");
    }
    public void _on_Park_Spot_body_exited(Node ps) {
        canPark = false;
    }
    // Add new functions above^
}
// }
