using Godot;
using System;

public class Main : Node {
    public const int tileSize = 64;
    public Player player;
    public Balloon balloon;
    [Export] public ParkSpot[] parkSpots;
    public ParkSpot parkSpot;
    [Export] public Area2D liftLock;
    public Node2D level;
    // public BoxCam2D boxCam2D;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        level = GetNode<Node2D>("level");
        player = level.GetNode<Player>("Player");
        // balloon = level.GetNode<Balloon>("Balloon");
        balloon = player.GetNode<Balloon>("Balloon");
        parkSpot = level.GetNode<ParkSpot>("Park Spot");
        liftLock = level.GetNode<Area2D>("Lift Lock");
        // boxCam2D = player.GetNode<BoxCam2D>("Camera2D");
        // Position2D startPosition = GetNode<Position2D>("StartPosition");

        player.Start(this);
        balloon.Start(this);
        // balloon.Position = new Vector2(player.Position.x, player.Position.y - Balloon.ribbonLength);

    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(float delta) {}
}
