using Godot;
using System;

public class Balloon : KinematicBody2D {
    // [Signal] public void BalloonPhysics();
    // [Signal] public void _on_BalloonArea2D_ready();
    public const float ribbonLength = Main.tileSize * 2;
    public const float howMuchHelium = 500; // positive float plz

    private bool isParked = false;
    // public bool readyToPark = true;
    public Vector2 isParkedAt;
    private Vector2 velocity = new Vector2();
    Vector2 anchor;
    private Main main;
    private Player player;

    // public override void _Ready() {
    //     // main = GetParent().GetParent<Main>();
    //     main = GetParent<Player>().GetParent().GetParent<Main>();
    //     player = main.player;
    // }

    public override void _Process(float delta) {
        // Position.y -= howMuchHelium;
        // Position -= new Vector2(0, howMuchHelium);
        BalloonPhysics(delta);
    }
    public void Start(Main m) {
        main = m;
        player = main.player;
    }
    public void BalloonPhysics(float delta) {
        // Position -= new Vector2(0, howMuchHelium);
        // velocity = new Vector2(main.player.velocity.x, -howMuchHelium);
        Vector2 tempVelocity;
        if (isParked) tempVelocity = new Vector2(0, -howMuchHelium);
        else tempVelocity = new Vector2(-player.velocity.x, -howMuchHelium);
        // velocity.y += -howMuchHelium;
        velocity = MoveAndSlide(tempVelocity, new Vector2(0, 1));

        // if (isParked && readyToPark) {
        //     isParkedAt = main.player.Position;
        //     readyToPark = false;
        //     anchor = isParkedAt;
        // }
        // else if (!isParked) {
        //     anchor = main.player.Position;
        // }
        // else {
        //     anchor = isParkedAt;
        // }
        if (ShortcutTools.DistanceFloat(anchor, Position) > ribbonLength) {
            Position = ShortcutTools.Normalize(ribbonLength, Position);
        }
    }

    public void Park(Vector2 pos) {
        anchor = pos;
        isParked = true;
        // readyToPark = false;
    }
    public void PickUp() {
        isParked = false;
    }
    // Add new functions above^
}

