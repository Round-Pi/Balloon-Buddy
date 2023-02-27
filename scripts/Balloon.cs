using Godot;
using System;

public class Balloon : Area2D {
    // [Signal] public void BalloonPhysics();
    // [Signal] public void _on_BalloonArea2D_ready();
    public const float ribbonLength = Main.tileSize * 2;
    public const float howMuchHelium = 10; // positive float plz

    public bool isParked = false;
    public bool isJustParked = false;
    public Vector2 isParkedAt;
    public Main main;

    public override void _Ready() {
        // player = GetNode<Player>("Player");
        main = GetParent().GetParent<Main>();
    }

    public override void _Process(float delta) {
        // Position.y -= howMuchHelium;
        // Position -= new Vector2(0, howMuchHelium);
        BalloonPhysics();
    }

    public void Park() {

    }
    public void BalloonPhysics() {
        Position -= new Vector2(0, howMuchHelium);
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
        if (ShortcutTools.DistanceFloat(anchor, Position) > ribbonLength) {
            Vector2 v = Position - anchor;
            Position = anchor + ribbonLength * v.Normalized();
        }
    }
    // Add new functions above^
}

