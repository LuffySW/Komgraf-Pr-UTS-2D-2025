namespace Godot
{
	using Godot;
	using System;
	using System.Collections.Generic;

	[GlobalClass]
	public partial class primitif : RefCounted
	{
		// Hello world method for testing
		public void Helloworld()
		{
			GD.Print("hello world");
		}

		// DDA line drawing algorithm
		public List<Vector2> LineDDA(float x1, float y1, float x2, float y2)
		{
			List<Vector2> points = new List<Vector2>();

			// Calculate the differences
			float dx = x2 - x1;
			float dy = y2 - y1;

			// Determine the number of steps required for the line
			int steps = Mathf.Max(Mathf.Abs((int)dx), Mathf.Abs((int)dy));
			
			// Handle the case where steps is 0 (same point)
			if (steps == 0)
			{
				points.Add(new Vector2((int)x1, (int)y1));
				return points;
			}

			// Calculate the increments for x and y
			float Xinc = dx / steps;
			float Yinc = dy / steps;

			// Start drawing the line
			float x = x1;
			float y = y1;

			// Add points along the line
			for (int i = 0; i <= steps; i++)
			{
				// Explicitly cast to int to ensure proper conversion
				int roundedX = (int)Mathf.Round(x);
				int roundedY = (int)Mathf.Round(y);
				
				points.Add(new Vector2(roundedX, roundedY));
				
				x += Xinc;
				y += Yinc;
			}

			return points;
		}

		// Bresenham's line drawing algorithm
		public List<Vector2> LineBresenham(float xa, float ya, float xb, float yb)
		{
			List<Vector2> res = new List<Vector2>();
			int x1 = (int)xa;
			int y1 = (int)ya;
			int x2 = (int)xb;
			int y2 = (int)yb;

			int dx = Math.Abs(x2 - x1);
			int dy = Math.Abs(y2 - y1);
			int sx = (x1 < x2) ? 1 : -1;
			int sy = (y1 < y2) ? 1 : -1;
			int err = dx - dy;

			while (true)
			{
				res.Add(new Vector2(x1, y1));
				if (x1 == x2 && y1 == y2) break;
				int e2 = 2 * err;
				if (e2 > -dy) { err -= dy; x1 += sx; }
				if (e2 < dx) { err += dx; y1 += sy; }
			}
			return res;
		}

		// Convert to Cartesian coordinates (screen space)
		public static float[] ConvertToKartesian(float xa, float ya, float xb, float yb, float screenWidth, float screenHeight)
		{
			float axis = screenWidth / 2.0f;
			float ordinal = screenHeight / 2.0f;

			xa = axis + xa;
			xb = axis + xb;
			ya = ordinal - ya;
			yb = ordinal - yb;

			return new float[] { xa, ya, xb, yb };
		}

		// Convert to pixel coordinates
		public static float[] ConvertToPixel(float xa, float ya, float xb, float yb, float screenWidth, float screenHeight)
		{
			// Adjust for screen size and flipping the Y-axis for 2D screen space
			xa = xa + screenWidth / 2.0f;
			xb = xb + screenWidth / 2.0f;
			ya = screenHeight / 2.0f - ya;
			yb = screenHeight / 2.0f - yb;

			return new float[] { xa, ya, xb, yb };
		}

		// Convert from pixel coordinates to world coordinates (opposite of ConvertToPixel)
		public static float[] ConvertToWorld(float x, float y, float screenWidth, float screenHeight)
		{
			// Convert screen coordinates back to world coordinates
			float axis = screenWidth / 2.0f;
			float ordinal = screenHeight / 2.0f;

			x = x - axis;
			y = ordinal - y;

			return new float[] { x, y };
		}

		// Generate points for a parametric curve
		public List<Vector2> ParametricCurve(
			Func<float, float> xFunction, 
			Func<float, float> yFunction,
			float tStart, 
			float tEnd, 
			int segments)
		{
			List<Vector2> points = new List<Vector2>();
			
			float tStep = (tEnd - tStart) / segments;
			
			for (int i = 0; i <= segments; i++)
			{
				float t = tStart + i * tStep;
				float x = xFunction(t);
				float y = yFunction(t);
				
				points.Add(new Vector2(x, y));
			}
			
			return points;
		}

		// Special case for S-curve generation
		public List<Vector2> SCurve(Vector2 start, float size, int segments)
		{
			return ParametricCurve(
				t => start.X + size * 0.5f * (2 * t - 1),
				t => start.Y + size * 0.5f * Mathf.Sin(2 * Mathf.Pi * t),
				0.0f,
				1.0f,
				segments
			);
		}
		public void DrawPixel(Node2D canvas, float x, float y, Color color, float thickness = 1.0f)
		{
			// Use the built-in Godot method as we don't have direct pixel buffer access
			canvas.DrawRect(new Rect2(x, y, thickness, thickness), color);
		}
		public List<Vector2> QuadraticBezier(Vector2 p0, Vector2 p1, Vector2 p2, int segments = 20)
		{
			List<Vector2> points = new List<Vector2>();
			for (int i = 0; i <= segments; i++)
			{
				float t = i / (float)segments;
				float u = 1 - t;
				Vector2 point = u * u * p0 + 2 * u * t * p1 + t * t * p2;
				points.Add(point);
			}
			return points;
		}

		public List<Vector2> CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int segments = 30)
		{
			List<Vector2> points = new List<Vector2>();
			for (int i = 0; i <= segments; i++)
			{
				float t = i / (float)segments;
				float u = 1 - t;
				Vector2 point = 
					u * u * u * p0 + 
					3 * u * u * t * p1 + 
					3 * u * t * t * p2 + 
					t * t * t * p3;
				points.Add(point);
			}
			return points;
		}

		// Implement DrawRect using line primitives
		public List<Vector2> DrawRectangle(float x, float y, float width, float height)
		{
			List<Vector2> points = new List<Vector2>();
			
			// Draw horizontal lines to fill the rectangle
			for (int dy = 0; dy < height; dy++)
			{
				List<Vector2> line = LineDDA(x, y + dy, x + width - 1, y + dy);
				points.AddRange(line);
			}
			
			return points;
		}

		// Alternative approach - just the outline of a rectangle
		public List<Vector2> DrawRectOutline(float x, float y, float width, float height)
		{
			List<Vector2> points = new List<Vector2>();
			
			// Top edge
			points.AddRange(LineDDA(x, y, x + width - 1, y));
			// Right edge
			points.AddRange(LineDDA(x + width - 1, y, x + width - 1, y + height - 1));
			// Bottom edge
			points.AddRange(LineDDA(x + width - 1, y + height - 1, x, y + height - 1));
			// Left edge
			points.AddRange(LineDDA(x, y + height - 1, x, y));
			
			return points;
		}
		public List<Vector2> Ellipse(float centerX, float centerY, float width, float height)
{
	List<Vector2> points = new List<Vector2>();
	
	// Define how many segments to use for the ellipse
	// More segments = smoother curve
	int segments = 72;
	
	// Loop through the segments and calculate points
	for (int i = 0; i <= segments; i++)
	{
		// Parameter t goes from 0 to 2π
		float t = i * Mathf.Pi * 2 / segments;
		
		// Ellipse parametric equations
		float x = centerX + (width / 2) * Mathf.Cos(t);
		float y = centerY + (height / 2) * Mathf.Sin(t);
		
		// Round to nearest integer to avoid sub-pixel rendering issues
		int xRounded = (int)Mathf.Round(x);
		int yRounded = (int)Mathf.Round(y);
		
		points.Add(new Vector2(xRounded, yRounded));
	}
	
	return points;
}
	}
}
