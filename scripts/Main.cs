using Godot;
using System;

public class Main : Node {
    public const int tileSize = 64;
    public Player player;
    public Balloon balloon;
    public Node2D level;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        level = GetNode<Node2D>("level");
        player = level.GetNode<Player>("Player");
        // balloon = GetNode<Node2D>("level").GetNode<Balloon>("Balloon");
        balloon = player.GetNode<Balloon>("Balloon");
        Position2D startPosition = GetNode<Position2D>("StartPosition");
        player.Start(this);
        balloon.Start(this);
        balloon.Position = new Vector2(player.Position.x, player.Position.y - Balloon.ribbonLength);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(float delta) {}
}
