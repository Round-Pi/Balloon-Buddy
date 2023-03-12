using Godot;
using System;

public class Balloon : KinematicBody2D {
    // [Signal] public void BalloonPhysics();
    // [Signal] public void _on_BalloonArea2D_ready();
    public const float ribbonLength = Main.tileSize * 2.5f;
    public const float howMuchHelium = 500; // positive float plz

    public bool isParked = false;
    // public bool readyToPark = true;
    // public Vector2 isParkedAt;
    // public ParkSpot isParkedAtParkSpot;
    private Vector2 velocity = new Vector2();
    Vector2 anchor;
    public bool isStuck = false; // TODO: How can it detect getting stuck?
    private Main main;
    private Player player;

    // public override void _Ready() {}

    public override void _Process(float delta) {
        BalloonPhysics(delta);
    }
    public void Start(Main m) {
        main = m;
        player = main.player;
    }
    public void BalloonPhysics(float delta) {
        if (!isStuck) {
            Vector2 tempVelocity;
            if (isParked) tempVelocity = new Vector2(0, -howMuchHelium);
            else tempVelocity = new Vector2(-player.velocity.x, -howMuchHelium);
            velocity = MoveAndSlide(tempVelocity, new Vector2(0, -1));

            // if (!isStuck) {
            if (ShortcutTools.DistanceFloat(anchor, Position) > ribbonLength) {
                Position = ShortcutTools.Normalize(ribbonLength, Position);
            }
        }
        else if (isStuck && GetParentOrNull<Player>() != null) {
            player.RemoveChild(this);
            main.level.AddChild(this);
            Position = player.Position + Position;
        }
    }

    public void Park(Vector2 pos) {
        anchor = pos;
        isParked = true;
        main.parkSpot.lifting = true;
    }
    public void PickUp() {
        isParked = false;
    }

    // public void _on_Hook_Magnet_body_entered(Node hm) {
    //     isStuck = true;
    // }
    // public void _on_Hook_Magnet_body_exited(Node hm) {
    //     isStuck = false;
    // }

    // Add new functions above^
}

