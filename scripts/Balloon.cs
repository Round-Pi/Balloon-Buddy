using Godot;
using System;

public class Balloon : RigidBody2D {
    // [Signal] public void BalloonPhysics();
    // [Signal] public void _on_BalloonArea2D_ready();
    public const float ribbonLength = Main.tileSize * 3;
    public const float howMuchHelium = 500; // positive float plz

    public bool isParked = false;
    public bool isJustParked = false;
    public Vector2 isParkedAt;
    private Vector2 velocity = new Vector2();
    public Main main;

    public override void _Ready() {
        main = GetParent().GetParent<Main>();
    }

    public override void _Process(float delta) {
        // Position.y -= howMuchHelium;
        // Position -= new Vector2(0, howMuchHelium);
        BalloonPhysics(delta);
    }

    public void Park() {

    }
    public void BalloonPhysics(float delta) {
        // Position -= new Vector2(0, howMuchHelium);
        velocity = new Vector2(main.player.velocity.x - 5, -howMuchHelium);
        // velocity.y += -howMuchHelium;
        // velocity = MoveAndSlide(velocity, new Vector2(0, -1));

        Vector2 anchor;
        if (isParked && !isJustParked) {
            isParkedAt = main.player.Position;
            isJustParked = true;
            anchor = isParkedAt;
        }
        else if (!isParked) {
            anchor = main.player.Position;
        }
        else {
            anchor = isParkedAt;
        }
        if (isParked && ShortcutTools.DistanceFloat(anchor, Position) > ribbonLength) {
            Vector2 v = Position - anchor;
            Position = anchor + ribbonLength * v.Normalized();
        }
        else if (!isParked && ShortcutTools.DistanceFloat(anchor, Position) > ribbonLength) {
            Vector2 v = Position - main.player.Position;
            Position = anchor + ribbonLength * v.Normalized();
            // if (main.player.Position.x != Position.x) {
            //     velocity += Position - main.player.Position;
            // }
        }
    }
    // Add new functions above^
}

