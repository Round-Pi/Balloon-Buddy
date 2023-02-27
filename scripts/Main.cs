using Godot;
using System;

public class Main : Node {
    public const int tileSize = 64;
    public Player player;
    Balloon balloon;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        player = GetNode<Node2D>("level").GetNode<Player>("Player");
        balloon = GetNode<Node2D>("level").GetNode<Balloon>("BalloonArea2D");
        Position2D startPosition = GetNode<Position2D>("StartPosition");
        player.Start(startPosition.Position);
        balloon.Position = new Vector2(player.Position.x, player.Position.y - Balloon.ribbonLength);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        balloon.Position -= new Vector2(0, Balloon.howMuchHelium);
        balloon.BalloonFollows(player.Position);
    }
}
