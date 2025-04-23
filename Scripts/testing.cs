namespace Godot;

using Godot;
using System;
using System.Collections.Generic;

public partial class testing : Node2D
{
	public override void _Ready()
	{
	}
	
	public override void _Draw()
	{
		float width = GetViewportRect().Size.X;
		float height = GetViewportRect().Size.Y;
		float xStart = 0;
		float xEnd = width;

		// Garis Naik (m = 2, c = 50) - Merah
		DrawLine(new Vector2(xStart, 2 * xStart + 50), new Vector2(xEnd, 2 * xEnd + 50), Colors.Red, 2.0f);

		// Garis Turun (m = -1.5, c = 300) - Biru
		DrawLine(new Vector2(xStart, -1.5f * xStart + 300), new Vector2(xEnd, -1.5f * xEnd + 300), Colors.Blue, 2.0f);

		// Garis Horizontal (y = 200) - Hijau
		DrawLine(new Vector2(xStart, 200), new Vector2(xEnd, 200), Colors.Green, 2.0f);

		// Garis Vertikal (x = 400) - Kuning
		DrawLine(new Vector2(400, 0), new Vector2(400, height), Colors.Yellow, 2.0f);
	}

	

	public override void _Process(double delta)
	{
		// Jika ingin memperbarui gambar secara dinamis, panggil QueueRedraw()
		QueueRedraw();
	}
}
