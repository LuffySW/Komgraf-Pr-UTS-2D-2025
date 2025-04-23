namespace Godot
{
	using Godot;
	using System;
	using System.Collections.Generic;

	[GlobalClass]
	public partial class bentukdasar : Node2D, IDisposable
	{
		private primitif _primitif;

		public override void _Ready()
		{
			// Pastikan _primitif sudah diassign jika belum
			if (_primitif == null)
			{
				_primitif = new primitif(); // Assign primitif jika belum ada
			}
		}

		// Fungsi untuk menggambar garis putus-putus (dashed)
		public List<Vector2> DrawDashedLine(Vector2 from, Vector2 to, float dashLen = 10f, float gapLen = 5f)
		{
			List<Vector2> points = new List<Vector2>();
			Vector2 dir = (to - from).Normalized();
			float length = from.DistanceTo(to);
			float t = 0;

			while (t < length)
			{
				Vector2 start = from + dir * t;
				t += dashLen;
				Vector2 end = from + dir * Mathf.Min(t, length);
				points.AddRange(_primitif.LineBresenham((int)start.X, (int)start.Y, (int)end.X, (int)end.Y));
				t += gapLen;
			}

			return points;
		}

		// Fungsi untuk menggambar garis titik-titik (dotted)
		public List<Vector2> DrawDottedLine(Vector2 from, Vector2 to, float spacing = 8f)
		{
			List<Vector2> points = new List<Vector2>();
			Vector2 dir = (to - from).Normalized();
			float length = from.DistanceTo(to);
			float t = 0;

			while (t < length)
			{
				Vector2 pos = from + dir * t;
				points.Add(pos);
				t += spacing;
			}

			return points;
		}

		// Fungsi untuk menggambar garis dash-dot
		public List<Vector2> DrawDashDotLine(Vector2 from, Vector2 to, float dashLen = 10f, float gapLen = 5f, float dotSpacing = 15f)
		{
			List<Vector2> points = new List<Vector2>();
			Vector2 dir = (to - from).Normalized();
			float length = from.DistanceTo(to);
			float t = 0;
			bool drawDash = true;

			while (t < length)
			{
				if (drawDash)
				{
					float end = Mathf.Min(t + dashLen, length);
					var startPoint = from + dir * t;
					var endPoint   = from + dir * end;
					points.AddRange(_primitif.LineBresenham((int)startPoint.X, (int)startPoint.Y, (int)endPoint.X, (int)endPoint.Y));
					t += dashLen;
					drawDash = false;
				}
				else
				{
					points.Add(from + dir * t);
					t += dotSpacing;
					drawDash = true;
				}
			}

			return points;
		}

		// Implementasi IDisposable - gunakan 'new' untuk menyembunyikan metode GodotObject.Dispose
		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); // Untuk mencegah finalizer dipanggil lagi
		}

		protected new virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				GD.Print($"_primitif is null in Dispose(): {_primitif == null}");
				_primitif?.Dispose(); // Dispose _primitif
				_primitif = null; // Set ke null (opsional)
				GD.Print($"_primitif is null after dispose: {_primitif == null}");
			}
			// bebaskan unmanaged resources di sini jika ada.
		}
	}
}
