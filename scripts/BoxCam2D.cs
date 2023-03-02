using Godot;
using System;


public class BoxCam2D : Camera2D {
    [Export] NodePath player = "";
    public Vector2 playerPos = new Vector2(0, 0);
    public Vector2 box = new Vector2(0, 0);
    private Vector2 designRes;
    [Signal] delegate void OutOfTheBox();
    Main main;

    public void Start(Main m) {
        main = m;
    }
    public override void _Ready() {
        designRes = new Vector2(
            (int)ProjectSettings.GetSetting("display/window/size/width"),
            (int)ProjectSettings.GetSetting("display/window/size/height")
        );
        if (player == "") {
            GD.Print("BoxCam2D: Assign Player Node in BoxCam2D using Inspector.");
            this.QueueFree();
            return;
        }
        else if (!this.Current) {
            GD.Print("BoxCam2D: Set 'Current' Enabled or it won't work.'");

        }
        SetBoxPos();
        if (!IsConnected("OutOfTheBox", this, "SetBoxPos")) {
            GD.Print("Box_Cam: Failed To Connect Signal");
        }
    }

    public override void _Process(float _delta) {
        playerPos = main.player.Position;
        Vector2 currentBox = new Vector2(
            Mathf.Floor(playerPos.x / (designRes.x) * Zoom.x),
            Mathf.Floor(playerPos.y / (designRes.y) * Zoom.y)
        );
        if (currentBox.x != box.x || currentBox.y != box.y) {
            EmitSignal("out_of_the_box");
            box = currentBox;
        }
    }

    public void SetBoxPos() {
        float boxPosX = (
            (designRes.x * Zoom.x) * Mathf.Floor(playerPos.x / (designRes.x) * Zoom.x)
        );
        float boxPosY = (
            (designRes.y * Zoom.y) * Mathf.Floor(playerPos.y / (designRes.y) * Zoom.y)
        );
        // this.Position.x = (((designRes.x) / 2) * Zoom.x) + boxPosX;
        // this.Position.y = (((designRes.y) / 2) * Zoom.y) + boxPosY;
        Position = new Vector2(
            (((designRes.x) / 2) * Zoom.x) + boxPosX,
            (((designRes.y) / 2) * Zoom.y) + boxPosY
        );
    }

}
