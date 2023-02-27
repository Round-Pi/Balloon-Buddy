using Godot;
using System;

public class Balloon : Area2D {
    // [Signal]
    // public void BalloonFollows();
    // [Signal]
    // public void _on_BalloonArea2D_ready();
    public const float ribbonLength = Main.tileSize * 2;
    public const float howMuchHelium = 10; // positive float plz
    // Player player;
    // public Main main;

    // public override void _Ready() {
    //     // player = GetNode<Player>("Player");
    //     main = GetParent().GetParent().GetNode<Main>("Main");
    // }
    // Add your update logic here
    // public override void _Process(float delta) {
    //     // Position.y -= howMuchHelium;
    //     Position -= new Vector2(0, howMuchHelium);
    //     BalloonFollows(main.player.Position);
    // }

    public void BalloonFollows(Vector2 playerPosition) {
        if (ShortcutTools.DistanceFloat(playerPosition, Position) > ribbonLength) {
            Vector2 v = Position - playerPosition;
            Position = playerPosition + ribbonLength * v.Normalized();
        }
    }
    // Add new functions above^
}

