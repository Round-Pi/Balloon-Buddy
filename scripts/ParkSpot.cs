using Godot;
using System;

public class ParkSpot : Area2D {
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] bool isALift = false;
    public Area2D liftLock;
    public bool lifting;
    bool lockedIn = false;
    Main main;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        if (isALift) lifting = false;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        if (isALift && lifting && !lockedIn) {
            // got up
            Position += new Vector2(0, -2);
        }
    }
    // public void _on_Lift_Lock_body_entered(Node l) 
    public void _on_Lift_Lock_area_entered(Area2D l) {
        StaticBody2D temp = l.GetNode<StaticBody2D>("StaticBody2D");
        GD.Print("Locking block in...", l.Name, " ", temp.Name);
        if (temp != null) {
            // liftLock = main.level.GetNode<Area2D>("Lift Lock");
            // liftLock = temp;
            lockedIn = true;
            // Position = new Vector2(main.liftLock.Position.x, main.liftLock.Position.y - (Main.tileSize / 2));

        }
    }
}
