using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class karya4 : Node2D
{
	private primitif _primitif = new primitif();
	private Color _hitam = Colors.Black;
	private float _drumAnimationTimer = 0f;
	private float _drumHitDuration = 0.3f; // Duration of the hit animation
	private float _drumHitInterval = 1.5f; // Time between drum hits
	private bool _drumIsHit = false;
	private float _drumDeformation = 0f; // How much the drumhead deforms when hit
	private Vector2 _hitPosition; // Where on the drum to show the hit
	// Truntum batik animation variables
	private float _truntumAnimationTimer = 0f;
	private float _truntumPulseRate = 2.5f; // Complete cycle every 2.5 seconds
	private float _truntumRotationRate = 8.0f; // Slower rotation
	private Dictionary<Vector2, float> _flowerPhases = new Dictionary<Vector2, float>();
	private List<Vector2> _flowerCenters = new List<Vector2>();

	// Keris animation variables
	private float _kerisAnimationTimer = 0f;
	private float _kerisFloatAmplitude = 5.0f; // How far it moves up and down
	private float _kerisFloatRate = 0.7f; // How fast it floats
	private float _kerisShimmerTimer = 0f;
	private float _kerisShimmerInterval = 3.0f; // Time between shimmers
	private bool _kerisIsShimmering = false;
	private float _kerisShimmerDuration = 0.5f; // How long the shimmer lasts
	private float _kerisRotationAmount = 2.0f; // Small rotation in degrees

	// Wayang animation variables
	private float _wayangSwayTimer = 0f;
	private float _wayangSwayRate = 0.6f; // How fast it sways
	private float _wayangSwayAmount = 3.0f; // How much it tilts in degrees
	private float _wayangArmAnimTimer = 0f;
	private float _wayangLeftArmAngle = -30f; // Base angle for left arm
	private float _wayangRightArmAngle = 30f; // Base angle for right arm
	private float _wayangBreathTimer = 0f;
	private float _wayangBreathRate = 0.4f; // Breathing cycle rate
	// whiteline color
	private Color _lineColor = new Color(1.0f, 1.0f, 1.0f); // White color for lines
	private Color _ungu = new Color(0.4f, 0.15f, 0.35f); // Rich purple color
	private Color _kuning = new Color(0.95f, 0.85f, 0.55f); // Warm beige for drum skin
	private Color _ropeColor = new Color(0.3f, 0.15f, 0.05f); // Brown for ropes
	
	// Neon colors for the sketch-like appearance
	private Color _neonGreen = new Color(0.0f, 1.0f, 0.5f);  // Bright green like in sketch
	private Color _neonRed = new Color(1.0f, 0.3f, 0.3f);    // Bright red like in sketch

	private Color _kerisColor = new Color(0.7f, 0.6f, 0.5f); // Warna kuningan

	private Vector2 _headLeftCenter, _headRightCenter;
	private float _headLeftRadius, _headRightRadius;
	// Warna baru untuk baju Lurik
	private Color _lurikBase = new Color(0.15f, 0.15f, 0.4f); // Biru tua tradisional
	private Color _lurikStripe1 = new Color(0.95f, 0.95f, 0.9f); // Krem untuk garis
	private Color _lurikStripe2 = new Color(0.8f, 0.3f, 0.2f);  // Merah tanah untuk aksen
	
	// Tambahkan definisi warna untuk bagian-bagian wayang kulit
	private Color _wayangBodyColor = new Color(0.8f, 0.65f, 0.4f); // Warna kulit yang lebih tua
	private Color _wayangKainColor = new Color(0.7f, 0.3f, 0.1f); // Warna merah tua untuk kain
	private Color _wayangHeadColor = new Color(0.85f, 0.7f, 0.45f); // Warna kulit kepala sedikit lebih terang
	private Color _wayangCrownColor = new Color(0.85f, 0.75f, 0.2f); // Warna emas untuk mahkota
	private Color _wayangDetailColor = new Color(0.3f, 0.1f, 0.05f); // Warna coklat gelap untuk detail
	private Color _wayangPatternColor = new Color(0.2f, 0.1f, 0.05f); // Warna hitam kecokelatan untuk pola
	private Color _wayangRodColor = new Color(0.6f, 0.45f, 0.3f); // Warna bambu/tanduk yang lebih gelap
	private Color _wayangJewelColor = new Color(0.5f, 0.1f, 0.1f); // Warna merah tua untuk hiasan batu
	private Color _wayangThreadColor = new Color(0.7f, 0.65f, 0.5f); // Warna benang pengikat
	private Color _wayangGoldAccentColor = new Color(0.9f, 0.8f, 0.2f); // Warna aksen emas

	// Colors for keris
	private Color _bladeColor = new Color(0.8f, 0.75f, 0.7f); // Silver-like blade
	private Color _hiltColor = new Color(0.6f, 0.45f, 0.3f); // Wood color for hilt
	private Color _sheathColor = new Color(0.5f, 0.35f, 0.25f); // Darker wood for sheath
	private Color _selutColor = new Color(0.9f, 0.8f, 0.2f); // Gold for selut

	// Colors for kendang
	private Color _drumBodyColor = new Color(0.7f, 0.6f, 0.5f); // Wood color for drum body
	private Color _drumHeadColor = new Color(0.95f, 0.9f, 0.8f); // Lighter color for drum head
	// Updated kendang colors for more realism
	private Color _drumWoodColor = new Color(0.65f, 0.45f, 0.3f); // Darker wood color for drum body
	private Color _drumHeadNaturalColor = new Color(0.95f, 0.92f, 0.85f); // Realistic goat skin color
	private Color _drumBindingColor = new Color(0.3f, 0.25f, 0.2f); // Dark leather/rope color
	private Color _drumRingColor = new Color(0.85f, 0.8f, 0.7f); // Ring color for drum head edges

	// Add traditional batik color scheme
	private Color _batikBackground = new Color(0.1f, 0.05f, 0.15f); // Dark purple-black background
	private Color _batikPattern = new Color(0.95f, 0.92f, 0.85f); // Cream/white pattern color
	private Color _batikAccent = new Color(0.7f, 0.5f, 0.3f); // Subtle brown accent for details

	// Add these variables to the class
	private bool _isBatikClicked = false;
	private float _animationTimer = 0f;
	private float _pulseRate = 1.5f;
	private Color _batikColor = new Color(0.95f, 0.92f, 0.85f); // Default cream color
	private Color _highlightColor = new Color(0.9f, 0.7f, 0.3f); // Gold highlight color
	private Vector2 _batikPosition = new Vector2(650, 170);
	private Vector2 _batikSize = new Vector2(350, 450);
	private float _batikBaseRotation = 0f;
	private bool _rotationEnabled = false;

	private bool _manualDrumTrigger = false;
private bool _isAnimatingDrum = false;
private float _lastDrumAnimationTime = 0f;

// Tambahkan di bagian atas class karya4 (bersama variabel lainnya)
private bool _showKeris = false;        // Mengontrol apakah keris ditampilkan atau tidak
private bool _manualKerisShimmer = false; // Pemicu animasi shimmer manual
private Vector2 _kerisPosition = new Vector2(200, 350); // Posisi default keris
private float _kerisScale = 1.0f;       // Skala default keris
private float _kerisRotation = -45.0f;   // Rotasi default keris (dalam derajat)

// Tambahkan di bagian atas class karya4 (bersama variabel lainnya)
private Vector2 _kendangPosition = new Vector2(400, 350); // Posisi default kendang
private float _kendangScale = 1.0f;      // Skala default kendang

	private void FillPolygon(Vector2[] points, Color color)
	{
		if (points.Length < 3) return; // Need at least a triangle to fill

		// Find the bounding box
		float minX = points[0].X;
		float maxX = points[0].X;
		float minY = points[0].Y;
		float maxY = points[0].Y;

		for (int i = 1; i < points.Length; i++)
		{
			if (points[i].X < minX) minX = points[i].X;
			if (points[i].X > maxX) maxX = points[i].X;
			if (points[i].Y < minY) minY = points[i].Y;
			if (points[i].Y > maxY) maxY = points[i].Y;
		}

		// Convert bounds to integers to ensure we cover all pixels
		int startX = (int)Math.Floor(minX);
		int endX = (int)Math.Ceiling(maxX);
		int startY = (int)Math.Floor(minY);
		int endY = (int)Math.Ceiling(maxY);

		// Scan line algorithm with horizontal scan lines
		for (int y = startY; y <= endY; y++)
		{
			List<float> intersections = new List<float>();

			// Find intersections with all edges
			for (int i = 0; i < points.Length; i++)
			{
				int j = (i + 1) % points.Length;
				
				float y1 = points[i].Y;
				float y2 = points[j].Y;
				
				// Skip horizontal edges and edges not crossing current scanline
				if ((y1 == y2) || 
					(y1 < y && y2 < y) || 
					(y1 > y && y2 > y))
					continue;
				
				// Calculate x-coordinate where the edge intersects the scanline
				float x = points[i].X + (y - y1) * (points[j].X - points[i].X) / (y2 - y1);
				intersections.Add(x);
			}

			// Sort intersections by x-coordinate
			intersections.Sort();

			// Fill between pairs of intersections
			for (int i = 0; i < intersections.Count - 1; i += 2)
			{
				// Ensure we have a valid pair
				if (i + 1 >= intersections.Count) break;

				// Convert to integers and expand slightly to avoid gaps
				int startFillX = (int)Math.Floor(intersections[i]);
				int endFillX = (int)Math.Ceiling(intersections[i + 1]);

				// Fill the span
				for (int x = startFillX; x <= endFillX; x++)
				{
					DrawRect(new Rect2(x, y, 1, 1), color);
				}
			}
		}
	}

	// Method to fill a circle with color
	private void FillCircle(float cx, float cy, float radius, Color color)
	{
		float rSquared = radius * radius;
		
		// Fixed loop - x and y coordinates were swapped before
		for (float y = cy - radius; y <= cy + radius; y++)
		{
			for (float x = cx - radius; x <= cx + radius; x++)
			{
				float dx = x - cx;
				float dy = y - cy;
				float distanceSquared = dx * dx + dy * dy;
				
				if (distanceSquared <= rSquared)
				{
					DrawRect(new Rect2(x, y, 1, 1), color);
				}
			}
		}
	}

	// Fix FillCircleWithBorder method - remove extra lineWidth parameter
	private void FillCircleWithBorder(float cx, float cy, float radius, Color fillColor, Color borderColor)
	{
		// Fill the circle
		FillCircle(cx, cy, radius, fillColor);
		
		// Draw the border - using Vector2 for position
		DrawCircle(new Vector2(cx, cy), radius, borderColor);
	}

	// Method to fill an area between curved paths
	private void FillBetweenCurves(List<Vector2> curve1, List<Vector2> curve2, Color color)
	{
		if (curve1.Count == 0 || curve2.Count == 0) return;
		
		Vector2[] polygonPoints = new Vector2[curve1.Count + curve2.Count];
		
		// Add first curve
		for (int i = 0; i < curve1.Count; i++)
		{
			polygonPoints[i] = curve1[i];
		}
		
		// Add second curve in reverse
		for (int i = 0; i < curve2.Count; i++)
		{
			polygonPoints[curve1.Count + i] = curve2[curve2.Count - 1 - i];
		}
		
		// Fill the polygon
		FillPolygon(polygonPoints, color);
	}

	// Called when the node enters the scene tree for the first time
	public override void _Ready()
{
	// Initialize the Truntum batik pattern
	_flowerCenters = new List<Vector2>();
	_flowerPhases = new Dictionary<Vector2, float>();
	
	// Reposisi objek untuk layout yang lebih baik
	_batikPosition = new Vector2(700, 100);  // Geser batik ke kanan dan atas
	_kerisPosition = new Vector2(200, 250);  // Ubah posisi default keris
	_kendangPosition = new Vector2(400, 350); // Definisikan posisi kendang
	
	
	// Only setup the initial phases here, don't try to draw
	InitializePhases();
	
	// Print message to confirm initialization
	GD.Print("Batik Truntum initialized");
}
	
	// Initialize phases for animation
	private void InitializePhases()
	{
		// Setup initial flower positions without drawing
		float startX = _batikPosition.X;
		float startY = _batikPosition.Y;
		float width = _batikSize.X;
		float height = _batikSize.Y;
		float flowerSpacing = 48f;
		float rowSpacing = 42f;
		
		// Calculate positions and phases
		int rows = (int)(height / rowSpacing);
		int cols = (int)(width / flowerSpacing);

		for (int row = 0; row < rows; row++)
		{
			float xOffset = (row % 2 == 0) ? 0 : flowerSpacing / 2;
			
			for (int col = 0; col < cols; col++)
			{
				float x = startX + 30 + col * flowerSpacing + xOffset;
				float y = startY + 30 + row * rowSpacing;
				
				if (x > startX + width - 30 || y > startY + height - 30)
					continue;
				
				Vector2 center = new Vector2(x, y);
				_flowerCenters.Add(center);
				
				// Initialize phase for this flower
				_flowerPhases[center] = (float)GD.RandRange(0f, Mathf.Pi * 2);
			}
		}
		
		// Add corner and border positions
		AddFrameFlowerPositions();
	}
	
	

	// Process for updating animations
	public override void _Process(double delta)
{
	_animationTimer += (float)delta;
	_truntumAnimationTimer += (float)delta;
	
	// Update animation parameters based on click state
	if (_isBatikClicked)
	{
		_truntumPulseRate = 3.5f;
		_truntumRotationRate = _rotationEnabled ? 12.0f : 0.0f;
	}
	else
	{
		_truntumPulseRate = 2.5f;
		_truntumRotationRate = 8.0f;
	}
	
	// Only handle manually triggered drum animations
	if (_isAnimatingDrum)
	{
		// We're manually controlling the animation
		_lastDrumAnimationTime += (float)delta;
		
		// If we're done with the full animation cycle (hit + recovery)
		if (_lastDrumAnimationTime > _drumHitDuration + 0.2f)
		{
			_isAnimatingDrum = false;
			_lastDrumAnimationTime = 0f;
		}
	}
	
	// Check if manually triggered
	if (_manualDrumTrigger)
	{
		_drumIsHit = true;
		_drumDeformation = 1.0f;
		
		// Randomize hit position slightly for visual interest
		float angle = Mathf.DegToRad((float)Godot.GD.RandRange(-30.0, 30.0));
		_hitPosition = _headLeftCenter + new Vector2(
			_headLeftRadius * 0.6f * Mathf.Cos(angle),
			_headLeftRadius * 0.6f * Mathf.Sin(angle)
		);  
		
		// Reset manual trigger flag
		_manualDrumTrigger = false;
		
		// Force a redraw to show the hit
		QueueRedraw();
	}
	
	// Animate the drum hit recovery
	if (_drumIsHit)
	{
		_drumDeformation -= (float)(delta / _drumHitDuration);
		
		if (_drumDeformation <= 0)
		{
			_drumDeformation = 0;
			_drumIsHit = false;
		}
		
		// Update visual as long as the animation is active
		QueueRedraw();
	}

	// Update keris animation
	_kerisAnimationTimer += (float)delta;
	
	// Update shimmer effect duration jika sedang berjalan
	if (_kerisIsShimmering)
	{
		// Tetap track durasi shimmer
		_kerisShimmerTimer += (float)delta;
		
		if (_kerisShimmerTimer >= _kerisShimmerDuration)
		{
			_kerisIsShimmering = false;
			_kerisShimmerTimer = 0f;
		}
	}

	if (_showKeris && _kerisFloatAmplitude > 5.0f)
	{
		// Kembalikan secara perlahan ke nilai normal
		_kerisFloatAmplitude -= (float)(delta * 0.5f);
		_kerisFloatRate -= (float)(delta * 0.1f);
		
		// Batasi ke nilai minimum
		if (_kerisFloatAmplitude < 5.0f) _kerisFloatAmplitude = 5.0f;
		if (_kerisFloatRate < 0.7f) _kerisFloatRate = 0.7f;
	}
	
	// Reset manual shimmer trigger setelah selesai
	if (_manualKerisShimmer && _kerisIsShimmering == false)
	{
		_manualKerisShimmer = false;
	}

	
	// Request redraw after updating animation parameters
	QueueRedraw();
}

	// Handle user input for interactive batik
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && 
			mouseEvent.ButtonIndex == MouseButton.Left && 
			mouseEvent.Pressed)
		{
			// Check if click is inside the batik area
			Rect2 batikRect = new Rect2(_batikPosition, _batikSize);
			if (batikRect.HasPoint(mouseEvent.Position))
			{
				_isBatikClicked = !_isBatikClicked; // Toggle click state
				GD.Print("Batik Truntum interaction toggled!");
				
				// Toggle rotation
				_rotationEnabled = !_rotationEnabled;
				
				// Cycle through color schemes when clicked
				if (_batikColor == _batikPattern)
				{
					_batikColor = new Color(0.9f, 0.7f, 0.3f); // Gold
					_batikBackground = new Color(0.2f, 0.1f, 0.15f); // Darker background
				}
				else if (_batikColor.R > 0.8f && _batikColor.G > 0.6f)
				{
					_batikColor = new Color(0.7f, 0.3f, 0.3f); // Reddish
					_batikBackground = new Color(0.15f, 0.05f, 0.05f); // Dark red background
				}
				else
				{
					_batikColor = _batikPattern; // Back to original
					_batikBackground = new Color(0.1f, 0.05f, 0.15f); // Original background
				}
				
				// Trigger a redraw
				QueueRedraw();
			}
		}
		// Add new keyboard input detection
	if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
	{
		// Check if the 'K' key was pressed
		if (keyEvent.Keycode == Key.K)
		{
			// Trigger the drum hit manually
			_manualDrumTrigger = true;
			_isAnimatingDrum = true;
			GD.Print("Kendang animation triggered by keyboard!");
			
			// Force a redraw
			QueueRedraw();
		}
		if (keyEvent.Keycode == Key.C)
{
	// Toggle tampilan keris
	_showKeris = !_showKeris;
	 float moveAmount = 10.0f; // Jumlah pergerakan per tombol ditekan
	
	// Jika menampilkan keris, juga memicu animasi shimmer
	if (_showKeris)
	{
		_manualKerisShimmer = true;
		_kerisIsShimmering = true;
		_kerisShimmerTimer = 0f;
		
		// Reset animasi timer untuk memberikan efek animasi dari awal
		_kerisAnimationTimer = 0f;
		
		// Meningkatkan intensitas animasi saat dimulai
		_kerisFloatAmplitude = 8.0f; // Amplitudo lebih besar untuk awal animasi
		_kerisFloatRate = 1.2f;      // Rate lebih cepat untuk awal animasi

		 if (keyEvent.Keycode == Key.Up)
			_kerisPosition.Y -= moveAmount;
		else if (keyEvent.Keycode == Key.Down)
			_kerisPosition.Y += moveAmount;
		else if (keyEvent.Keycode == Key.Left)
			_kerisPosition.X -= moveAmount;
		else if (keyEvent.Keycode == Key.Right)
			_kerisPosition.X += moveAmount;

			if (keyEvent.Keycode == Key.W)
		_kendangPosition.Y -= moveAmount;
	else if (keyEvent.Keycode == Key.S && !keyEvent.Echo) // Hanya jika bukan tombol 'S' yang sudah dideteksi
		_kendangPosition.Y += moveAmount;
	else if (keyEvent.Keycode == Key.A)
		_kendangPosition.X -= moveAmount;
	else if (keyEvent.Keycode == Key.D)
		_kendangPosition.X += moveAmount;
		
		GD.Print("Keris animation triggered by keyboard!");
	}
	 

	// Tombol R untuk mereset posisi semua objek
	if (keyEvent.Keycode == Key.R)
	{
		_batikPosition = new Vector2(700, 100);
		_kerisPosition = new Vector2(200, 250);
		_kendangPosition = new Vector2(400, 350);
	}
	
	// Force redraw
	QueueRedraw();
}
	}
	// Tambahkan di _Input() setelah bagian deteksi tombol 'K'
// Deteksi tombol 'S' untuk mengaktifkan keris
// Modifikasi kode di bagian _Input() untuk tombol 'S'

	}
	

	// THIS IS THE KEY METHOD - all drawing must happen here or in methods called from here
	public override void _Draw()
{
	// Draw the batik pattern (repositioned further to the right)
	DrawTruntumBatikMotif(
		_batikPosition,
		_batikSize.X,
		_batikSize.Y,
		8,
		6,
		_batikColor
	);

	// Draw the kendang with new position
	DrawTiltedKendang(_kendangPosition.X, _kendangPosition.Y);

	// Draw keris jika diaktifkan
	if (_showKeris)
	{
		DrawKeris(_kerisPosition, _kerisScale, _kerisRotation);
	}
}

	private void AddFrameFlowerPositions()
	{
		float x = _batikPosition.X;
		float y = _batikPosition.Y;
		float width = _batikSize.X;
		float height = _batikSize.Y;
		
		// Corner positions
		Vector2 topLeft = new Vector2(x, y);
		Vector2 topRight = new Vector2(x + width, y);
		Vector2 bottomLeft = new Vector2(x, y + height);
		Vector2 bottomRight = new Vector2(x + width, y + height);
		
		_flowerPhases[topLeft] = 0;
		_flowerPhases[topRight] = Mathf.Pi * 0.5f;
		_flowerPhases[bottomLeft] = Mathf.Pi;
		_flowerPhases[bottomRight] = Mathf.Pi * 1.5f;
		
		// Border positions
		int borderStarCount = 10;
		float borderStarSpacing = width / borderStarCount;
		
		for (int i = 1; i < borderStarCount; i++)
		{
			Vector2 topPos = new Vector2(x + i * borderStarSpacing, y);
			Vector2 bottomPos = new Vector2(x + i * borderStarSpacing, y + height);
			
			_flowerPhases[topPos] = i * 0.7f;
			_flowerPhases[bottomPos] = i * 0.7f + Mathf.Pi;
		}
		
		int sideBorderStarCount = (int)(height / borderStarSpacing);
		for (int i = 1; i < sideBorderStarCount; i++)
		{
			Vector2 leftPos = new Vector2(x, y + i * borderStarSpacing);
			Vector2 rightPos = new Vector2(x + width, y + i * borderStarSpacing);
			
			_flowerPhases[leftPos] = i * 0.7f + Mathf.Pi * 0.5f;
			_flowerPhases[rightPos] = i * 0.7f + Mathf.Pi * 1.5f;
		}
	}
	private void DrawTruntumBatikMotif(Vector2 center, float width, float height, int rows, int columns, Color color)
{
	// Gunakan posisi yang diteruskan, bukan hardcoded
	float startX = center.X;
	float startY = center.Y;
	float totalWidth = width;
	float totalHeight = height;

	// Draw the improved Truntum pattern
	DrawTruntumElements(startX, startY, totalWidth, totalHeight, color);
	
	// Draw decorated frame that matches the pattern
	DrawTruntumFrame(startX, startY, totalWidth, totalHeight, color);
}

	private void DrawTruntumElements(float startX, float startY, float width, float height, Color color)
	{
		// Parameters for hexagonal grid
		float flowerSpacing = 48f;
		float rowSpacing = 42f; // √3/2 * spacing for perfect hexagon
		float flowerSize = 12f;
		
		// Fill background with traditional dark batik color first
		Vector2[] backgroundPoints = {
			new Vector2(startX, startY),
			new Vector2(startX + width, startY),
			new Vector2(startX + width, startY + height),
			new Vector2(startX, startY + height)
		};
		FillPolygon(backgroundPoints, _batikBackground);
		
		// Clear the previous flower centers
		_flowerCenters.Clear();

		// Draw hexagonal grid of flowers
		int rows = (int)(height / rowSpacing);
		int cols = (int)(width / flowerSpacing);

		for (int row = 0; row < rows; row++)
		{
			float xOffset = (row % 2 == 0) ? 0 : flowerSpacing / 2;
			
			for (int col = 0; col < cols; col++)
			{
				float x = startX + 30 + col * flowerSpacing + xOffset;
				float y = startY + 30 + row * rowSpacing;
				
				if (x > startX + width - 30 || y > startY + height - 30)
					continue;
				
				Vector2 center = new Vector2(x, y);
				_flowerCenters.Add(center);
				
				// Initialize phase for this flower if it doesn't exist
				if (!_flowerPhases.ContainsKey(center))
				{
					_flowerPhases[center] = (float)Godot.GD.RandRange(0f, Mathf.Pi * 2);
				}
				
				// Draw animated flower with traditional batik pattern color
				DrawAnimatedTruntumFlower(center, flowerSize, _batikPattern, _flowerPhases[center]);
			}
		}
		
		// Draw hexagonal connections with batik color
		DrawHexagonalConnections(_flowerCenters, _batikPattern, flowerSpacing);
		
		// Add dots in hexagonal centers with batik color
		DrawHexagonalDots(_flowerCenters, startX, startY, width, height, flowerSpacing, rowSpacing, _batikPattern);
	}

	private void DrawHexagonalConnections(List<Vector2> centers, Color color, float spacing)
	{
		float connectionThickness = 0.5f; // Diubah dari 0.3f ke 0.5f
		float maxConnectionDistance = spacing * 0.6f;
		
		// Warna dengan opacity lebih tinggi untuk kontras lebih baik
		Color lineColor = color * new Color(1,1,1,0.8f);

		foreach (Vector2 center in centers)
		{
			List<Vector2> neighbors = centers.Where(c => 
				c != center && 
				(c - center).Length() < maxConnectionDistance
			).ToList();

			foreach (Vector2 neighbor in neighbors)
			{
				List<Vector2> line = _primitif.LineDDA(center.X, center.Y, neighbor.X, neighbor.Y);
				
				// Tambahkan efek garis double untuk ketebalan bertahap
				foreach (Vector2 p in line)
				{
					// Garis utama
					DrawRect(new Rect2(p, new Vector2(connectionThickness, connectionThickness)), lineColor);
					
					// Efek anti-alias pinggiran
					DrawRect(new Rect2(p + new Vector2(0.3f, 0.3f), new Vector2(0.2f, 0.2f)), lineColor * 0.5f);
				}
			}
		}
	}

	// Fix DrawHexagonalDots method (around line 363)
	private void DrawHexagonalDots(List<Vector2> flowerCenters, float startX, float startY, 
								 float width, float height, float flowerSpacing, float rowSpacing, Color color)
	{
		float dotSize = 1.2f;
		float minDistanceFromFlower = flowerSpacing * 0.4f;

		for (int row = 0; row < (int)(height/rowSpacing); row++)
		{
			float xOffset = (row % 2 == 0) ? flowerSpacing * 0.5f : flowerSpacing * 0.25f;
			
			for (float x = startX + xOffset; x < startX + width; x += flowerSpacing)
			{
				float y = startY + 30 + row * rowSpacing + rowSpacing/2;
				
				Vector2 dotPos = new Vector2(x, y);
				
				// Validate position
				bool valid = true;
				foreach (Vector2 flower in flowerCenters)
				{
					if ((dotPos - flower).Length() < minDistanceFromFlower)
					{
						valid = false;
						break;
					}
				}
				
				if (valid && dotPos.X < startX + width - 30 && dotPos.Y < startY + height - 30)
				{
					// Phase based on position for varied animation
					float dotPhase = (x * 0.1f + y * 0.13f) % (Mathf.Pi * 2);
					
					// Animate dot size
					float dotPulse = Mathf.Sin(_truntumAnimationTimer * 2.0f + dotPhase) * 0.3f + 1.0f;
					float animatedSize = dotSize * dotPulse;
					
					// Draw dot with subtle shadow and animation
					DrawCircle(new Vector2(dotPos.X + 1, dotPos.Y + 1), animatedSize, color * new Color(1,1,1,0.3f * dotPulse));
					DrawCircle(dotPos, animatedSize, color);
				}
			}
		}
	}

	// Fix DrawAnimatedTruntumFlower method (around line 397)
	private void DrawAnimatedTruntumFlower(Vector2 center, float size, Color color, float phaseOffset)
	{
		// Calculate animation factors
		float pulse = Mathf.Sin(_truntumAnimationTimer * Mathf.Pi * _truntumPulseRate + phaseOffset) * 0.2f + 1.0f;
		float rotation = _truntumAnimationTimer * _truntumRotationRate + phaseOffset;
		
		// Adjust size with pulse
		float animatedSize = size * pulse;
		
		// Simplified flower with 6 petals for hexagonal alignment
		int petals = 6;
		float angleStep = Mathf.Pi * 2 / petals;

		// Center dot with subtle pulsing
		float centerPulse = Mathf.Sin(_truntumAnimationTimer * Mathf.Pi * _truntumPulseRate * 1.5f + phaseOffset) * 0.15f + 1.0f;
		DrawCircle(center, size * 0.2f * centerPulse, color);

		// Petals with rotation
		for (int i = 0; i < petals; i++)
		{
			float angle = i * angleStep + rotation;
			Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			
			Vector2 start = center + dir * animatedSize * 0.4f;
			Vector2 end = center + dir * animatedSize * 0.8f;
			
			List<Vector2> petal = _primitif.LineDDA(start.X, start.Y, end.X, end.Y);
			foreach (Vector2 p in petal)
			{
				DrawRect(new Rect2(p, new Vector2(1.5f, 1.5f)), color);
			}
		}

		// Inner circle with inverse pulsing
		DrawCircle(center, size * 0.1f * (2.0f - centerPulse), color);
	}

	// Fix DrawTruntumFlower method (around line 426)
	private void DrawTruntumFlower(Vector2 center, float size, Color color)
	{
		// Simplified flower with 6 petals for hexagonal alignment
		int petals = 6;
		float angleStep = Mathf.Pi * 2 / petals;

		// Center dot
		DrawCircle(center, size * 0.2f, color);

		// Petals
		for (int i = 0; i < petals; i++)
		{
			float angle = i * angleStep;
			Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			
			Vector2 start = center + dir * size * 0.4f;
			Vector2 end = center + dir * size * 0.8f;
			
			List<Vector2> petal = _primitif.LineDDA(start.X, start.Y, end.X, end.Y);
			foreach (Vector2 p in petal)
			{
				DrawRect(new Rect2(p, new Vector2(1.5f, 1.5f)), color);
			}
		}

		// Inner circle
		DrawCircle(center, size * 0.1f, color);
	}

	private void DrawTruntumFrame(float x, float y, float width, float height, Color color)
	{
		float thickness = 2.0f;
		
		// Draw the main frame lines
		List<Vector2> topLine = _primitif.LineDDA(x, y, x + width, y);
		List<Vector2> bottomLine = _primitif.LineDDA(x, y + height, x + width, y + height);
		List<Vector2> leftLine = _primitif.LineDDA(x, y, x, y + height);
		List<Vector2> rightLine = _primitif.LineDDA(x + width, y, x + width, y + height);
		
		foreach (var p in topLine.Concat(bottomLine).Concat(leftLine).Concat(rightLine))
		{
			DrawRect(new Rect2(p, new Vector2(thickness, thickness)), color);
		}

		// Add Truntum stars at the corners to match the pattern
		float cornerSize = 10f;
		float cornerPhase = _truntumAnimationTimer * 1.2f;
		
		// Initialize phases for corner flowers
		Vector2 topLeft = new Vector2(x, y);
		Vector2 topRight = new Vector2(x + width, y);
		Vector2 bottomLeft = new Vector2(x, y + height);
		Vector2 bottomRight = new Vector2(x + width, y + height);
		
		if (!_flowerPhases.ContainsKey(topLeft)) _flowerPhases[topLeft] = 0;
		if (!_flowerPhases.ContainsKey(topRight)) _flowerPhases[topRight] = Mathf.Pi * 0.5f;
		if (!_flowerPhases.ContainsKey(bottomLeft)) _flowerPhases[bottomLeft] = Mathf.Pi;
		if (!_flowerPhases.ContainsKey(bottomRight)) _flowerPhases[bottomRight] = Mathf.Pi * 1.5f;
		
		// Draw animated corner flowers
		DrawAnimatedTruntumFlower(topLeft, cornerSize, color, _flowerPhases[topLeft]);
		DrawAnimatedTruntumFlower(topRight, cornerSize, color, _flowerPhases[topRight]);
		DrawAnimatedTruntumFlower(bottomLeft, cornerSize, color, _flowerPhases[bottomLeft]);
		DrawAnimatedTruntumFlower(bottomRight, cornerSize, color, _flowerPhases[bottomRight]);

		// Add decorative stars along the border at regular intervals
		int borderStarCount = 10;
		float borderStarSpacing = width / borderStarCount;
		
		for (int i = 1; i < borderStarCount; i++)
		{
			// Create positions for the border flowers
			Vector2 topPos = new Vector2(x + i * borderStarSpacing, y);
			Vector2 bottomPos = new Vector2(x + i * borderStarSpacing, y + height);
			
			// Initialize phases if needed
			if (!_flowerPhases.ContainsKey(topPos)) _flowerPhases[topPos] = i * 0.7f;
			if (!_flowerPhases.ContainsKey(bottomPos)) _flowerPhases[bottomPos] = i * 0.7f + Mathf.Pi;
			
			// Draw animated border flowers
			DrawAnimatedTruntumFlower(topPos, cornerSize * 0.7f, color, _flowerPhases[topPos]);
			DrawAnimatedTruntumFlower(bottomPos, cornerSize * 0.7f, color, _flowerPhases[bottomPos]);
		}
		
		// Side border stars
		int sideBorderStarCount = (int)(height / borderStarSpacing);
		for (int i = 1; i < sideBorderStarCount; i++)
		{
			// Create positions for the side border flowers
			Vector2 leftPos = new Vector2(x, y + i * borderStarSpacing);
			Vector2 rightPos = new Vector2(x + width, y + i * borderStarSpacing);
			
			// Initialize phases if needed
			if (!_flowerPhases.ContainsKey(leftPos)) _flowerPhases[leftPos] = i * 0.7f + Mathf.Pi * 0.5f;
			if (!_flowerPhases.ContainsKey(rightPos)) _flowerPhases[rightPos] = i * 0.7f + Mathf.Pi * 1.5f;
			
			// Draw animated side border flowers
			DrawAnimatedTruntumFlower(leftPos, cornerSize * 0.7f, color, _flowerPhases[leftPos]);
			DrawAnimatedTruntumFlower(rightPos, cornerSize * 0.7f, color, _flowerPhases[rightPos]);
		}
	}

	private void DrawTiltedKendang(float centerX, float centerY)
{
	// Use less extreme rotation angle for better visual balance
	float angle = -35f * Mathf.Pi / 180; // Changed from -40 to -35 degrees
	float cos = Mathf.Cos(angle);
	float sin = Mathf.Sin(angle);
	
	// Adjusted dimensions for better proportions
	float length = 260; // Slightly shorter for better aspect ratio
	float bigEndRadius = 80;
	float smallEndRadius = 55; // Increased from 50 to 55 for more balanced look
	
	// Calculate drum end centers with more precise positioning
	Vector2 bigEnd = new Vector2(
		centerX - length/2 * cos,
		centerY - length/2 * sin
	);
	
	Vector2 smallEnd = new Vector2(
		centerX + length/2 * cos,
		centerY + length/2 * sin
	);
	
	// Apply a slight movement to the drum when hit
	if (_drumIsHit)
	{
		// Reduced hitOffset for more subtle, realistic movement
		float hitOffset = 2.5f * _drumDeformation;
		bigEnd += new Vector2(cos, sin) * hitOffset;
		smallEnd += new Vector2(cos, sin) * hitOffset;
	}
	
	// Store end centers and radiuses for the rope pattern and hit detection
	_headLeftCenter = bigEnd;
	_headRightCenter = smallEnd;
	_headLeftRadius = bigEndRadius;
	_headRightRadius = smallEndRadius;
	
	// Calculate drum orientation angle
	float angleMain = Mathf.Atan2(smallEnd.Y - bigEnd.Y, smallEnd.X - bigEnd.X);

	// IMPROVED: Create drum body with more points for smoother rendering
	List<Vector2> upperPoints = new List<Vector2>();
	List<Vector2> lowerPoints = new List<Vector2>();
	
	// Use more segments for smoother circles
	int segments = 24; // Increased from previous value
	float angleStep = Mathf.Pi / segments;
	
	// Generate upper half of big circle (left end)
	for (int i = 0; i <= segments; i++)
	{
		float a = angleMain - Mathf.Pi/2 + i * angleStep;
		upperPoints.Add(new Vector2(
			bigEnd.X + bigEndRadius * Mathf.Cos(a),
			bigEnd.Y + bigEndRadius * Mathf.Sin(a)
		));
	}
	
	// Generate upper half of small circle (right end)
	for (int i = segments; i >= 0; i--)
	{
		float a = angleMain - Mathf.Pi/2 + i * angleStep;
		upperPoints.Add(new Vector2(
			smallEnd.X + smallEndRadius * Mathf.Cos(a),
			smallEnd.Y + smallEndRadius * Mathf.Sin(a)
		));
	}
	
	// Generate lower half of small circle (right end)
	for (int i = 0; i <= segments; i++)
	{
		float a = angleMain + Mathf.Pi/2 + i * angleStep;
		lowerPoints.Add(new Vector2(
			smallEnd.X + smallEndRadius * Mathf.Cos(a),
			smallEnd.Y + smallEndRadius * Mathf.Sin(a)
		));
	}
	
	// Generate lower half of big circle (left end)
	for (int i = segments; i >= 0; i--)
	{
		float a = angleMain + Mathf.Pi/2 + i * angleStep;
		lowerPoints.Add(new Vector2(
			bigEnd.X + bigEndRadius * Mathf.Cos(a),
			bigEnd.Y + bigEndRadius * Mathf.Sin(a)
		));
	}
	
	// Combine all points to form complete drum body polygon
	Vector2[] drumBodyPoints = new Vector2[upperPoints.Count + lowerPoints.Count];
	upperPoints.CopyTo(drumBodyPoints, 0);
	lowerPoints.CopyTo(drumBodyPoints, upperPoints.Count);
	
	// Fill the drum body with wood color
	FillPolygon(drumBodyPoints, _drumWoodColor);
	
	// Add wood grain texture to the drum body
	DrawWoodGrainTexture(drumBodyPoints, _drumWoodColor);
	
	// Fill both circles at ends with the natural skin color
	FillCircle(bigEnd.X, bigEnd.Y, bigEndRadius, _drumHeadNaturalColor);
	FillCircle(smallEnd.X, smallEnd.Y, smallEndRadius, _drumHeadNaturalColor);
	
	// Draw the outlines with improved consistency 
	DrawDrumOutline(bigEnd, smallEnd, bigEndRadius, smallEndRadius, _lineColor);
	
	// Draw the string/binding pattern
	DrawStringPattern(bigEnd, smallEnd, bigEndRadius, smallEndRadius, _drumBindingColor);
	
	// Draw detailed drum heads with animation effects
	DrawDetailedDrumHead(smallEnd, smallEndRadius, _lineColor);
	DrawAnimatedDetailedDrumHead(bigEnd, bigEndRadius, _lineColor);
	
	// Draw the hand/stick hitting the drum if currently in hit animation
	if (_drumIsHit && _drumDeformation > 0.2f)
	{
		DrawDrumStick(_hitPosition, angle, _drumDeformation);
	}
}

private void DrawAnimatedDetailedDrumHead(Vector2 center, float radius, Color color)
{
	// Base skin color slightly affected by deformation (tension)
	Color skinColor = _drumHeadNaturalColor;
	if (_drumIsHit) {
		// Slightly darken the skin color when hit to simulate tension
		skinColor = _drumHeadNaturalColor * new Color(0.98f, 0.98f, 0.98f);
	}
	
	// Fill the drumhead with skin color
	FillCircle(center.X, center.Y, radius, skinColor);
	
	// Draw the main circle outline
	DrawCircle(center.X, center.Y, radius, color, 1.5f);
	
	// Draw the rim edge (slightly darker)
	DrawCircle(center.X, center.Y, radius * 0.95f, _drumRingColor, 1.0f);
	
	// Draw subtle texture on the drum head
	int textureDots = 300;
	float maxTextureRadius = radius * 0.9f;
	
	for (int i = 0; i < textureDots; i++)
	{
		// Random angle and distance from center
		float angle = (float)Godot.GD.RandRange(0, Mathf.Pi * 2);
		float dist = (float)Godot.GD.RandRange(0, maxTextureRadius);
		
		if (_drumIsHit) {
			// Make texture dots follow deformation pattern
			Vector2 hitDir = (_hitPosition - center).Normalized();
			float distFromHit = (center + new Vector2(dist * Mathf.Cos(angle), dist * Mathf.Sin(angle)) - _hitPosition).Length();
			
			// Adjust distance based on proximity to hit location
			float adjustmentFactor = Mathf.Max(0, 1.0f - (distFromHit / (radius * 2)) * _drumDeformation);
			dist -= adjustmentFactor * 5.0f; // Create a subtle depression effect
		}
		
		// Position
		float x = center.X + dist * Mathf.Cos(angle);
		float y = center.Y + dist * Mathf.Sin(angle);
		
		// Create subtle texture
		float opacity = (float)Godot.GD.RandRange(0.05f, 0.15f);
		Color textureColor = new Color(0.3f, 0.3f, 0.3f, opacity);
		DrawRect(new Rect2(x, y, 1.0f, 1.0f), textureColor);
	}
	
	// If the drum is being hit, show deformation effect
	if (_drumIsHit)
	{
		// Direction from center to hit position
		Vector2 hitDir = (_hitPosition - center).Normalized();
		
		// Draw main impact point - darker spot where hit occurs
		float impactRadius = radius * 0.1f * _drumDeformation;
		Color impactColor = new Color(0.3f, 0.3f, 0.3f, 0.2f * _drumDeformation);
		FillCircle(_hitPosition.X, _hitPosition.Y, impactRadius, impactColor);
		
		// Draw several ripple waves with decreasing opacity
		for (int i = 0; i < 4; i++)
		{
			float waveRadius = radius * (0.3f + i * 0.15f) * _drumDeformation;
			float rippleOpacity = _drumDeformation * (1.0f - (float)i / 4);
			
			// Draw an incomplete circle for the ripple (more realistic)
			float startAngle = hitDir.Angle() - Mathf.Pi * 0.8f;
			float endAngle = hitDir.Angle() + Mathf.Pi * 0.8f;
			
			DrawPartialCircle(
				_hitPosition.X, _hitPosition.Y,
				waveRadius,
				startAngle, endAngle,
				color * new Color(1, 1, 1, rippleOpacity * 0.6f),
				0.8f
			);
		}
	}
	
	// Draw inner ring pattern - slightly distorted during hit
	float distortion = _drumIsHit ? (1.0f + _drumDeformation * 0.15f) : 1.0f;
	
	// Rings appear to move slightly toward the hit point
	Vector2 ringOffset = Vector2.Zero;
	if (_drumIsHit) {
		Vector2 hitDir = (_hitPosition - center).Normalized();
		ringOffset = hitDir * radius * 0.05f * _drumDeformation;
	}
	
	// Draw the rings with slight distortion
	DrawCircle(center.X + ringOffset.X * 0.7f, center.Y + ringOffset.Y * 0.7f, 
			  radius * 0.7f * distortion, _lineColor, 0.8f);
	DrawCircle(center.X + ringOffset.X * 0.5f, center.Y + ringOffset.Y * 0.5f, 
			  radius * 0.5f * distortion, _lineColor, 0.8f);
	DrawCircle(center.X + ringOffset.X * 0.3f, center.Y + ringOffset.Y * 0.3f, 
			  radius * 0.3f * distortion, _lineColor, 0.8f);
	DrawCircle(center.X + ringOffset.X * 0.1f, center.Y + ringOffset.Y * 0.1f, 
			  radius * 0.1f * distortion, _lineColor, 0.8f);
}

private void DrawDrumStick(Vector2 hitPosition, float drumAngle, float animationProgress)
{
	// Calculate stick angle (corrected to hit from the proper direction)
	// Change from 0.6π to -0.4π to reverse the direction
	float stickAngle = drumAngle - Mathf.Pi * 0.4f;
	
	// Stick length varies based on animation progress (moving in and out)
	float stickLength = 80f * (1f - animationProgress * 0.3f);
	
	// Calculate stick end positions
	Vector2 stickEnd = hitPosition + new Vector2(
		stickLength * Mathf.Cos(stickAngle),
		stickLength * Mathf.Sin(stickAngle)
	);
	
	// Draw the stick
	Color stickColor = new Color(0.8f, 0.7f, 0.5f); // Light wood color
	
	// Stick shaft
	List<Vector2> stickPoints = _primitif.LineDDA(
		hitPosition.X, hitPosition.Y,
		stickEnd.X, stickEnd.Y
	);
	
	foreach (var point in stickPoints)
	{
		// Draw a thicker line to represent the stick shaft
		DrawRect(new Rect2(point.X - 1f, point.Y - 1f, 2.0f, 2.0f), stickColor);
	}
	
	// Stick handle/end
	DrawCircle(stickEnd.X, stickEnd.Y, 5.0f, stickColor, 1.0f);
	
	// Fingertips at the stick end
	Color skinColor = new Color(0.9f, 0.75f, 0.65f);
	for (int i = 0; i < 3; i++)
	{
		// Calculate finger positions around the stick end
		float fingerAngle = stickAngle + Mathf.Pi + (i - 1) * 0.2f;
		Vector2 fingerPos = stickEnd + new Vector2(
			7.0f * Mathf.Cos(fingerAngle),
			7.0f * Mathf.Sin(fingerAngle)
		);
		
		// Draw the fingertips
		DrawCircle(fingerPos.X, fingerPos.Y, 3.0f, skinColor, 0.8f);
	}
}
	private void DrawDrumOutline(Vector2 bigEnd, Vector2 smallEnd, float bigRadius, float smallRadius, Color color)
	{
		// Calculate drum orientation angle
		float angleMain = Mathf.Atan2(smallEnd.Y - bigEnd.Y, smallEnd.X - bigEnd.X);

		// Draw connecting tangent lines in white using DDA
		Vector2 bigTop = new Vector2(
			bigEnd.X + bigRadius * Mathf.Cos(angleMain - Mathf.Pi / 2),
			bigEnd.Y + bigRadius * Mathf.Sin(angleMain - Mathf.Pi / 2)
		);
		
		Vector2 bigBottom = new Vector2(
			bigEnd.X + bigRadius * Mathf.Cos(angleMain + Mathf.Pi / 2),
			bigEnd.Y + bigRadius * Mathf.Sin(angleMain + Mathf.Pi / 2)
		);
		
		Vector2 smallTop = new Vector2(
			smallEnd.X + smallRadius * Mathf.Cos(angleMain - Mathf.Pi / 2),
			smallEnd.Y + smallRadius * Mathf.Sin(angleMain - Mathf.Pi / 2)
		);
		
		Vector2 smallBottom = new Vector2(
			smallEnd.X + smallRadius * Mathf.Cos(angleMain + Mathf.Pi / 2),
			smallEnd.Y + smallRadius * Mathf.Sin(angleMain + Mathf.Pi / 2)
		);
		
		// Use DDA to get points for top and bottom outlines
		List<Vector2> pointsTop = _primitif.LineDDA(bigTop.X, bigTop.Y, smallTop.X, smallTop.Y);
		List<Vector2> pointsBottom = _primitif.LineDDA(bigBottom.X, bigBottom.Y, smallBottom.X, smallBottom.Y);

		// Draw the top and bottom outlines using the points from DDA with consistent thickness
		foreach (var point in pointsTop)
		{
			DrawRect(new Rect2(point, new Vector2(2.0f, 2.0f)), color); // Use 2.0f for consistency
		}

		foreach (var point in pointsBottom)
		{
			DrawRect(new Rect2(point, new Vector2(2.0f, 2.0f)), color); // Use 2.0f for consistency
		}

		// Draw the outline of the circles with consistent thickness
		DrawPartialCircle(bigEnd.X, bigEnd.Y, bigRadius, angleMain - Mathf.Pi / 2, angleMain + Mathf.Pi / 2, color, 2.0f);
		DrawPartialCircle(smallEnd.X, smallEnd.Y, smallRadius, angleMain - Mathf.Pi / 2, angleMain + Mathf.Pi / 2, color, 2.0f);
	}
	
	private void DrawStringPattern(Vector2 bigEnd, Vector2 smallEnd, float bigRadius, float smallRadius, Color color)
	{
		// Calculate angle and direction
		float angle = Mathf.Atan2(smallEnd.Y - bigEnd.Y, smallEnd.X - bigEnd.X);
		
		// Increase string count from 4 to 5 for more ropes
		int stringCount = 5;
		float spacing = (bigRadius + smallRadius) / 2.0f / (stringCount + 1);
		
		// Make all ropes consistently thick
		float ropeThickness = 2.0f;
		
		for (int i = 1; i <= stringCount; i++)
		{
			// Calculate offset perpendicular to drum axis with slight variation
			float offsetMultiplier = 1.7f;
			if (i % 2 == 0) offsetMultiplier = 1.75f;  // Slight variation for even ropes
			
			float offset = -bigRadius + i * spacing * offsetMultiplier;
			
			// Calculate string direction (perpendicular to drum axis)
			float stringPerpAngle = angle + Mathf.Pi / 2;
			Vector2 stringPerp = new Vector2(Mathf.Cos(stringPerpAngle), Mathf.Sin(stringPerpAngle));
			Vector2 stringDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			
			// Calculate string path with slight vertical variation
			float verticalShift = (i % 2 == 0) ? 5.0f : -1.0f;  // Alternate slight shifts
			Vector2 stringCenter = new Vector2(
				(bigEnd.X + smallEnd.X) / 2 + offset * stringPerp.X,
				(bigEnd.Y + smallEnd.Y) / 2 + offset * stringPerp.Y + verticalShift
			);
			
			// Calculate intersections precisely to ensure proper connections
			Vector2 leftPoint = IntersectLineWithCircle(stringCenter, -stringDir, bigEnd, bigRadius);
			Vector2 rightPoint = IntersectLineWithCircle(stringCenter, stringDir, smallEnd, smallRadius);
			
			// Only draw if we have valid intersection points
			if (leftPoint != Vector2.Zero && rightPoint != Vector2.Zero)
			{
				// Draw the string with consistent thickness using our primitive methods
				List<Vector2> ropePoints = _primitif.LineDDA(leftPoint.X, leftPoint.Y, rightPoint.X, rightPoint.Y);
				foreach (var point in ropePoints)
				{
					DrawRect(new Rect2(point, new Vector2(ropeThickness, ropeThickness)), color);
				}
			}
		}
	}

	private Vector2 IntersectLineWithCircle(Vector2 linePoint, Vector2 lineDir, Vector2 circleCenter, float radius)
	{
		// Calculate vector from line point to circle center
		Vector2 toCircle = circleCenter - linePoint;
		
		// Project this vector onto the line direction
		float projection = toCircle.Dot(lineDir);
		
		// Calculate the closest point on the line to the circle center
		Vector2 closestPoint = linePoint + lineDir * projection;
		
		// Calculate distance from closest point to circle center
		float distance = (closestPoint - circleCenter).Length();
		
		// If the line doesn't intersect the circle, return zero vector
		if (distance > radius)
			return Vector2.Zero;
		
		// Calculate distance from closest point to intersection point
		float distToIntersection = Mathf.Sqrt(radius * radius - distance * distance);
		
		// Calculate the intersection point
		Vector2 intersection = closestPoint - lineDir * distToIntersection;
		
		return intersection;
	}

	private void DrawDrumHead(Vector2 center, float radius, Color color)
	{
		// Only draw the outline of the drum head (no fill) - thicker line (1.5)
		DrawCircle(center.X, center.Y, radius, color, 1.5f);
		
		// Draw fewer concentric circles for texture on the drum head
		DrawCircle(center.X, center.Y, radius * 0.5f, color, 1.5f);
	}

	private void DrawDrumEnd(Vector2 center, float radius, Color color)
	{
		// Only draw the outline of the drum end (no fill) - thicker line (1.5)
		DrawCircle(center.X, center.Y, radius, color, 1.5f);
	}

	private void DrawCircle(float cx, float cy, float radius, Color color, float thickness)
	{
		int segments = 48;
		// Match the angleStep with segments count (was using 36 instead of 48)
		float angleStep = 2 * Mathf.Pi / segments;
		
		for (int i = 0; i < segments; i++)
		{
			float a1 = i * angleStep;
			float a2 = (i + 1) * angleStep;
			float x1 = cx + radius * Mathf.Cos(a1);
			float y1 = cy + radius * Mathf.Sin(a1);
			float x2 = cx + radius * Mathf.Cos(a2);
			float y2 = cy + radius * Mathf.Sin(a2);
			
			var points = _primitif.LineDDA(x1, y1, x2, y2);
			foreach (var p in points)
			{
				DrawRect(new Rect2(p, new Vector2(thickness, thickness)), color);
			}
		}
	}

	private void DrawPartialCircle(float cx, float cy, float radius, float startAngle, float endAngle, Color color, float thickness)
	{
		int segments = 48;
		float angleRange = endAngle - startAngle;
		float angleStep = angleRange / segments;
		
		for (int i = 0; i < segments; i++)
		{
			float a1 = startAngle + i * angleStep;
			float a2 = startAngle + (i + 1) * angleStep;
			float x1 = cx + radius * Mathf.Cos(a1);
			float y1 = cy + radius * Mathf.Sin(a1);
			float x2 = cx + radius * Mathf.Cos(a2);
			float y2 = cy + radius * Mathf.Sin(a2);
			
			var points = _primitif.LineDDA(x1, y1, x2, y2);
			foreach (var p in points)
			{
				DrawRect(new Rect2(p, new Vector2(thickness, thickness)), color);
			}
		}
	}

	private void DrawWoodGrainTexture(Vector2[] bodyPoints, Color baseColor)
{
	// Create wood grain lines that follow the body contour
	// Determine the bounding box of the body
	float minX = float.MaxValue, maxX = float.MinValue;
	float minY = float.MaxValue, maxY = float.MinValue;
	
	foreach (Vector2 point in bodyPoints)
	{
		minX = Mathf.Min(minX, point.X);
		maxX = Mathf.Max(maxX, point.X);
		minY = Mathf.Min(minY, point.Y);
		maxY = Mathf.Max(maxY, point.Y);
	}
	
	// Calculate center and main axis of the drum
	Vector2 center = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
	
	// Draw curved grain lines that follow the drum contour
	int grainLines = 12;
	float spacing = (maxY - minY) / (grainLines + 1);
	
	// Slightly darker color for grain lines
	Color grainColor = baseColor * new Color(0.85f, 0.85f, 0.85f);
	Color highlightColor = baseColor * new Color(1.15f, 1.15f, 1.15f);
	
	// Draw horizontal grain lines with slight variations
	for (int i = 1; i <= grainLines; i++)
	{
		float y = minY + i * spacing;
		
		// Find left and right bounds at this height
		float leftX = float.MaxValue, rightX = float.MinValue;
		bool foundPoint = false;
		
		// Find points on the outline at this approximate height
		foreach (Vector2[] segment in FindOutlineSegments(bodyPoints, y, 5.0f))
		{
			if (segment.Length == 2)
			{
				leftX = Mathf.Min(leftX, segment[0].X);
				rightX = Mathf.Max(rightX, segment[1].X);
				foundPoint = true;
			}
		}
		
		if (foundPoint)
		{
			// Randomize grain line properties for natural appearance
			float thickness = Mathf.Max(0.5f, (float)Godot.GD.RandRange(0.5, 1.5));
			float waviness = (float)Godot.GD.RandRange(0, 3.0);
			int segments = (int)((rightX - leftX) / 10);
			
			// Choose which color to use (random variation between dark and light grain)
			Color lineColor = Godot.GD.Randf() > 0.7f ? highlightColor : grainColor;
			
			// Generate a wavy line for natural grain
			List<Vector2> points = new List<Vector2>();
			for (int j = 0; j <= segments; j++)
			{
				float x = leftX + (rightX - leftX) * j / segments;
				float yOffset = Mathf.Sin(j * 0.3f + i * 1.5f) * waviness;
				points.Add(new Vector2(x, y + yOffset));
			}
			
			// Draw the grain line
			for (int j = 0; j < points.Count - 1; j++)
			{
				List<Vector2> linePoints = _primitif.LineDDA(
					points[j].X, points[j].Y, 
					points[j+1].X, points[j+1].Y
				);
				
				foreach (Vector2 point in linePoints)
				{
					DrawRect(new Rect2(point, new Vector2(thickness, thickness)), lineColor);
				}
			}
		}
	}
}

private void DrawDetailedDrumHead(Vector2 center, float radius, Color color)
{
	// Draw main outline
	DrawCircle(center.X, center.Y, radius, _lineColor, 1.5f);
	
	// Draw the rim edge (slightly darker)
	DrawCircle(center.X, center.Y, radius * 0.95f, _drumRingColor, 1.0f);
	
	// Draw subtle texture on the drum head to simulate goat skin
	int textureDots = 300;
	float maxTextureRadius = radius * 0.9f;
	
	for (int i = 0; i < textureDots; i++)
	{
		// Random angle and distance from center
		float angle = (float)Godot.GD.RandRange(0, Mathf.Pi * 2);
		float dist = (float)Godot.GD.RandRange(0, maxTextureRadius);
		
		// Position
		float x = center.X + dist * Mathf.Cos(angle);
		float y = center.Y + dist * Mathf.Sin(angle);
		
		// Create subtle texture with tiny dots of varying opacity
		float opacity = (float)Godot.GD.RandRange(0.05, 0.15);
		Color textureColor = new Color(0.3f, 0.3f, 0.3f, opacity);
		DrawRect(new Rect2(x, y, 1.0f, 1.0f), textureColor);
	}
	
	// Draw inner ring pattern common in kendang
	DrawCircle(center.X, center.Y, radius * 0.7f, _lineColor, 0.8f);
	DrawCircle(center.X, center.Y, radius * 0.5f, _lineColor, 0.8f);
	DrawCircle(center.X, center.Y, radius * 0.3f, _lineColor, 0.8f);
	DrawCircle(center.X, center.Y, radius * 0.1f, _lineColor, 0.8f);
}

private Vector2[][] FindOutlineSegments(Vector2[] outline, float y, float tolerance)
{
	List<Vector2[]> segments = new List<Vector2[]>();
	List<Vector2> currentSegment = new List<Vector2>();
	
	for (int i = 0; i < outline.Length; i++)
	{
		Vector2 current = outline[i];
		Vector2 next = outline[(i + 1) % outline.Length];
		
		// Check if line segment crosses y level within tolerance
		if ((current.Y <= y + tolerance && next.Y >= y - tolerance) ||
			(next.Y <= y + tolerance && current.Y >= y - tolerance))
		{
			// Find approximate intersection point
			float t = (y - current.Y) / (next.Y - current.Y);
			if (float.IsNaN(t) || float.IsInfinity(t)) t = 0;
			t = Mathf.Clamp(t, 0, 1);
			
			Vector2 intersection = new Vector2(
				current.X + t * (next.X - current.X),
				current.Y + t * (next.Y - current.Y)
			);
			
			// Add to current segment
			currentSegment.Add(intersection);
			
			// If we have at least two points, add as a segment
			if (currentSegment.Count >= 2)
			{
				segments.Add(currentSegment.ToArray());
				currentSegment.Clear();
			}
		}
	}
	
	return segments.ToArray();
}

private void DrawKeris(Vector2 basePosition, float scale, float rotationDegrees)
{
	// Calculate floating movement
	float floatOffset = Mathf.Sin(_kerisAnimationTimer * Mathf.Pi * _kerisFloatRate) * _kerisFloatAmplitude;
	
	// Calculate subtle rotation oscillation
	float rotationOffset = Mathf.Sin(_kerisAnimationTimer * Mathf.Pi * 0.4f) * _kerisRotationAmount;
	
	// Apply the floating movement and rotation to the base position
	Vector2 animatedPosition = basePosition + new Vector2(0, floatOffset);
	float animatedRotation = rotationDegrees + rotationOffset;
	
	// Shift the entire keris untuk posisi yang lebih baik
	Vector2 origin = animatedPosition; // Tidak perlu ditambah offset horizontal
	float rotation = Mathf.DegToRad(animatedRotation);
	
	// Gunakan offset yang lebih baik untuk sarung (sheath)
	float sheathOffset = 30f * scale;
	Vector2 sheathOffsetVector = new Vector2(
		sheathOffset * Mathf.Cos(rotation - Mathf.Pi * 0.9f),
		sheathOffset * Mathf.Sin(rotation - Mathf.Pi * 0.9f)
	);
	
	// Position sheath with improved offset
	Vector2 sheathPosition = origin + sheathOffsetVector;
	
	// Offset for handle dengan posisi yang lebih sesuai
	float hiltOffset = 10f * scale; 
	Vector2 hiltPosition = origin + new Vector2(0, hiltOffset).Rotated(rotation);
	
	// Draw components in the right order (sheath behind, then blade, then hilt)
	DrawSheath(sheathPosition, scale, rotation);
	DrawAnimatedBlade(origin, scale, rotation);
	DrawHilt(hiltPosition, scale, rotation);
}

private void DrawBlade(Vector2 origin, float scale, float rotation)
{
	// Detail blade dengan 13 titik untuk luk yang lebih halus
	Vector2[] bladePoints = new Vector2[]
	{
		new Vector2(0, 0),       // Pangkal
		new Vector2(-2, -15),    // Luk 1
		new Vector2(3, -30),     // Luk 2
		new Vector2(-4, -45),    // Luk 3
		new Vector2(4, -60),     // Luk 4
		new Vector2(-3, -75),    // Luk 5
		new Vector2(3, -90),     // Luk 6
		new Vector2(-2, -105),   // Luk 7
		new Vector2(1, -120),    // Luk 8
		new Vector2(-1, -135),   // Luk 9
		new Vector2(2, -150),    // Luk 10
		new Vector2(0, -165),    // Ujung
	};

	// Gambar blade dengan kurva Bezier
	Color bladeColor = new Color(0.75f, 0.7f, 0.65f);
	
	for(int i = 0; i < bladePoints.Length - 3; i += 3)
	{
		Vector2 p0 = TransformPoint(bladePoints[i], origin, scale, rotation);
		Vector2 p1 = TransformPoint(bladePoints[i+1], origin, scale, rotation);
		Vector2 p2 = TransformPoint(bladePoints[i+2], origin, scale, rotation);
		
		List<Vector2> curve = _primitif.QuadraticBezier(p0, p1, p2);
		foreach(Vector2 p in curve)
		{
			DrawRect(new Rect2(p, new Vector2(1.8f, 1.8f)), _lineColor);
		}
	}

	// Gambar sogokan (alur tengah blade)
	DrawBladeGroove(bladePoints, origin, scale, rotation);
	
	// Gambar ganja (base)
	DrawCircle(origin.X, origin.Y, 6 * scale, _lineColor, 2.0f);
}

private void DrawAnimatedBlade(Vector2 origin, float scale, float rotation)
{
	// Create blade points for filling
	Vector2[] bladeOutlinePoints = new Vector2[]
	{
		new Vector2(0, 0),
		new Vector2(-2, -15),
		new Vector2(3, -30),
		new Vector2(-4, -45),
		new Vector2(4, -60),
		new Vector2(-3, -75),
		new Vector2(3, -90),
		new Vector2(-2, -105),
		new Vector2(1, -120),
		new Vector2(-1, -135),
		new Vector2(2, -150),
		new Vector2(0, -165),
		new Vector2(-2, -150),
		new Vector2(1, -135),
		new Vector2(-1, -120),
		new Vector2(2, -105),
		new Vector2(-3, -90),
		new Vector2(3, -75),
		new Vector2(-4, -60),
		new Vector2(4, -45),
		new Vector2(-3, -30),
		new Vector2(2, -15),
		new Vector2(0, 0),
	};

	// Transform the blade points
	Vector2[] transformedBladePoints = new Vector2[bladeOutlinePoints.Length];
	for(int i = 0; i < bladeOutlinePoints.Length; i++)
	{
		transformedBladePoints[i] = TransformPoint(bladeOutlinePoints[i], origin, scale, rotation);
	}

	// Fill the blade with color
	FillPolygon(transformedBladePoints, _bladeColor);

	// Draw the blade outline
	Vector2[] bladePoints = new Vector2[]
	{
		new Vector2(0, 0),       // Pangkal
		new Vector2(-2, -15),    // Luk 1
		new Vector2(3, -30),     // Luk 2
		new Vector2(-4, -45),    // Luk 3
		new Vector2(4, -60),     // Luk 4
		new Vector2(-3, -75),    // Luk 5
		new Vector2(3, -90),     // Luk 6
		new Vector2(-2, -105),   // Luk 7
		new Vector2(1, -120),    // Luk 8
		new Vector2(-1, -135),   // Luk 9
		new Vector2(2, -150),    // Luk 10
		new Vector2(0, -165),    // Ujung
	};
	
	// Gambar blade dengan kurva Bezier
	for(int i = 0; i < bladePoints.Length - 3; i += 3)
	{
		Vector2 p0 = TransformPoint(bladePoints[i], origin, scale, rotation);
		Vector2 p1 = TransformPoint(bladePoints[i+1], origin, scale, rotation);
		Vector2 p2 = TransformPoint(bladePoints[i+2], origin, scale, rotation);
		
		List<Vector2> curve = _primitif.QuadraticBezier(p0, p1, p2);
		foreach(Vector2 p in curve)
		{
			DrawRect(new Rect2(p, new Vector2(1.8f, 1.8f)), _lineColor);
		}
	}

	// Gambar sogokan (alur tengah blade)
	DrawBladeGroove(bladePoints, origin, scale, rotation);
	
	// Gambar ganja (base)
	FillCircle(origin.X, origin.Y, 6 * scale, _selutColor);
	DrawCircle(origin.X, origin.Y, 6 * scale, _lineColor, 2.0f);
	
	// Add shimmer effect if active
	if (_kerisIsShimmering)
	{
		// Detail points for shimmer location reference
		Vector2[] shimmerPoints = new Vector2[]
		{
			new Vector2(0, 0),
			new Vector2(0, -30),
			new Vector2(0, -60),
			new Vector2(0, -90),
			new Vector2(0, -120),
			new Vector2(0, -150)
		};
		
		// Calculate shimmer progress (0 to 1)
		float shimmerProgress = _kerisShimmerTimer / _kerisShimmerDuration;
		
		// Draw shimmer lines that move from tip to base
		for (int i = 0; i < shimmerPoints.Length - 1; i++)
		{
			// Only draw if in the current shimmer zone
			float shimmerZone = 1.0f - shimmerProgress;
			float pointProgress = (float)i / (shimmerPoints.Length - 1);
			
			if (Math.Abs(pointProgress - shimmerZone) < 0.25f)
			{
				// Calculate opacity based on distance from shimmer center
				float opacity = 1.0f - Math.Abs(pointProgress - shimmerZone) * 4.0f;
				
				// Draw horizontal shimmer line at this point
				Vector2 center = TransformPoint(shimmerPoints[i], origin, scale, rotation);
				Vector2 left = center + new Vector2(-4, 0).Rotated(rotation) * scale;
				Vector2 right = center + new Vector2(4, 0).Rotated(rotation) * scale;
				
				// Shimmer color - gold/white
				Color shimmerColor = new Color(1.0f, 0.95f, 0.7f, opacity);
				DrawLine(left, right, shimmerColor, 1.5f);
			}
		}
	}
}
private void DrawBladeGroove(Vector2[] points, Vector2 origin, float scale, float rotation)
{
	Color grooveColor = new Color(0.65f, 0.6f, 0.55f);
	
	for(int i = 1; i < points.Length - 1; i++)
	{
		Vector2 current = TransformPoint(points[i], origin, scale, rotation);
		Vector2 next = TransformPoint(points[i+1], origin, scale, rotation);
		
		Vector2 midPoint = (current + next) / 2;
		List<Vector2> grooveLine = _primitif.LineDDA(current.X, current.Y, midPoint.X, midPoint.Y);
		
		foreach(Vector2 p in grooveLine)
		{
			DrawRect(new Rect2(p, new Vector2(0.8f, 0.8f)), grooveColor);
		}
	}
}

private void DrawHilt(Vector2 origin, float scale, float rotation)
{
	// Warna kayu dengan gradasi
	Color woodDark = new Color(0.5f, 0.35f, 0.2f);
	Color woodLight = new Color(0.7f, 0.55f, 0.4f);
	
	Vector2 hiltBase = origin + new Vector2(0, 10 * scale).Rotated(rotation);
	Vector2 hiltTop = origin + new Vector2(0, 70 * scale).Rotated(rotation);
	
	// Badan pegangan
	List<Vector2> hiltMain = _primitif.LineDDA(hiltBase.X, hiltBase.Y, hiltTop.X, hiltTop.Y);
	foreach(Vector2 p in hiltMain)
	{
		DrawRect(new Rect2(p, new Vector2(5 * scale, 5 * scale)), _lineColor);
	}

	// Ukiran detail (model hulu keris Jawa)
	float[] pattern = { 0.2f, 0.4f, 0.6f, 0.8f };
	foreach(float t in pattern)
	{
		Vector2 pos = hiltBase.Lerp(hiltTop, t);
		
		// Pola lingkaran konsentris
		DrawCircle(pos.X, pos.Y, 4 * scale, woodLight, 1.2f);
		DrawCircle(pos.X, pos.Y, 2 * scale, _lineColor, 1.0f);
		
		// Pola garis diagonal
		for(int i = -1; i <= 1; i++)
		{
			Vector2 start = pos + new Vector2(-3 * scale, i * 3 * scale).Rotated(rotation);
			Vector2 end = pos + new Vector2(3 * scale, i * 3 * scale).Rotated(rotation);
			List<Vector2> line = _primitif.LineDDA(start.X, start.Y, end.X, end.Y);
			
			foreach(Vector2 p in line)
			{
				DrawRect(new Rect2(p, new Vector2(0.8f, 0.8f)), _lineColor);
			}
		}
	}

	// Selut (cincin logam)
	Color selutColor = new Color(0.9f, 0.8f, 0.3f);
	DrawCircle(origin.X, origin.Y, 8 * scale, selutColor, 2.5f);
	DrawCircle(origin.X, origin.Y, 6 * scale, selutColor * 0.8f, 1.8f);
}

private void DrawSheath(Vector2 origin, float scale, float rotation)
{
	// Warna warangka
	Color sheathColor = new Color(0.4f, 0.3f, 0.2f);
	Color metalColor = new Color(0.8f, 0.75f, 0.6f);
	
	// Titik-titik untuk polygon sarung
	Vector2[] sheathPoints = new Vector2[]
	{
		new Vector2(-7, 20),      // Pangkal kiri
		new Vector2(-10, 45),     // Luk kiri 1
		new Vector2(-12, 65),     // Luk kiri 2
		new Vector2(-10, 85),     // Luk kiri 3
		new Vector2(-9, 105),     // Luk kiri 4
		new Vector2(-7, 125),     // Luk kiri 5
		new Vector2(-4, 140),     // Ujung kiri
		new Vector2(0, 145),      // Ujung tengah
		new Vector2(4, 140),      // Ujung kanan
		new Vector2(7, 125),      // Luk kanan 5
		new Vector2(9, 105),      // Luk kanan 4
		new Vector2(10, 85),      // Luk kanan 3
		new Vector2(12, 65),      // Luk kanan 2
		new Vector2(10, 45),      // Luk kanan 1
		new Vector2(7, 20),       // Pangkal kanan
	};

	// Transform sheath points
	Vector2[] transformedSheathPoints = new Vector2[sheathPoints.Length];
	for(int i = 0; i < sheathPoints.Length; i++)
	{
		transformedSheathPoints[i] = TransformPoint(sheathPoints[i], origin, scale, rotation);
	}
	
	// Fill the sheath with color
	FillPolygon(transformedSheathPoints, _sheathColor);

	// Gambar badan sarung dengan kurva Bezier untuk transisi yang lebih halus
	for(int i = 0; i < sheathPoints.Length - 1; i++)
	{
		Vector2 p1 = TransformPoint(sheathPoints[i], origin, scale, rotation);
		Vector2 p2 = TransformPoint(sheathPoints[i+1], origin, scale, rotation);
		
		List<Vector2> line = _primitif.LineDDA(p1.X, p1.Y, p2.X, p2.Y);
		foreach(Vector2 p in line)
		{
			DrawRect(new Rect2(p, new Vector2(2.5f * scale, 2.5f * scale)), _lineColor);
		}
	}
	
	// Close the shape
	Vector2 first = TransformPoint(sheathPoints[0], origin, scale, rotation);
	Vector2 last = TransformPoint(sheathPoints[sheathPoints.Length-1], origin, scale, rotation);
	List<Vector2> closingLine = _primitif.LineDDA(last.X, last.Y, first.X, first.Y);
	foreach(Vector2 p in closingLine)
	{
		DrawRect(new Rect2(p, new Vector2(2.5f * scale, 2.5f * scale)), _lineColor);
	}

	// Pendok (logam penghias sarung) - sedikit digeser ke bawah
	Vector2 pendokStart = TransformPoint(new Vector2(-9, 50), origin, scale, rotation);
	Vector2 pendokEnd = TransformPoint(new Vector2(9, 50), origin, scale, rotation);
	
	List<Vector2> pendokLine = _primitif.LineDDA(pendokStart.X, pendokStart.Y, pendokEnd.X, pendokEnd.Y);
	foreach(Vector2 p in pendokLine)
	{
		DrawRect(new Rect2(p, new Vector2(2 * scale, 2 * scale)), metalColor);
	}
	
	// Pola ukiran pada pendok
	for(int i = 0; i < 5; i++)
	{
		Vector2 pos = pendokStart.Lerp(pendokEnd, (i + 1)/6.0f);
		DrawCircle(pos.X, pos.Y, 2 * scale, metalColor * 0.9f, 1.0f);
	}
	
	// Tambahkan detail garis halus di warangka (sarung)
	DrawSheathDetail(origin, scale, rotation);
}

private void DrawSheathDetail(Vector2 origin, float scale, float rotation)
{
	// Garis-garis halus mengikuti kontur sarung
	Color detailColor = _lineColor * 0.85f;
	
	// Garis kontur tengah
	Vector2 topCenter = TransformPoint(new Vector2(0, 30), origin, scale, rotation);
	Vector2 bottomCenter = TransformPoint(new Vector2(0, 140), origin, scale, rotation);
	
	List<Vector2> centerLine = _primitif.LineDDA(topCenter.X, topCenter.Y, bottomCenter.X, bottomCenter.Y);
	foreach(Vector2 p in centerLine)
	{
		DrawRect(new Rect2(p, new Vector2(0.8f * scale, 0.8f * scale)), detailColor);
	}
	
	// Garis-garis melintang untuk detail ukiran
	float[] positions = { 70, 90, 110 };
	foreach(float pos in positions)
	{
		Vector2 leftPoint = TransformPoint(new Vector2(-8, pos), origin, scale, rotation);
		Vector2 rightPoint = TransformPoint(new Vector2(8, pos), origin, scale, rotation);
		
		List<Vector2> line = _primitif.LineDDA(leftPoint.X, leftPoint.Y, rightPoint.X, rightPoint.Y);
		foreach(Vector2 p in line)
		{
			DrawRect(new Rect2(p, new Vector2(1.0f * scale, 1.0f * scale)), detailColor);
		}
	}
}

private Vector2 TransformPoint(Vector2 point, Vector2 origin, float scale, float rotation)
{
	Vector2 scaled = point * scale;
	return origin + new Vector2(
		scaled.X * Mathf.Cos(rotation) - scaled.Y * Mathf.Sin(rotation),
		scaled.X * Mathf.Sin(rotation) + scaled.Y * Mathf.Cos(rotation)
	);
}
private void DrawRectangle(Vector2 position, float width, float height, Color color, float thickness)
{
	// Garis kiri
	DrawLine(position, new Vector2(position.X, position.Y + height), color, thickness);
	// Garis kanan
	DrawLine(new Vector2(position.X + width, position.Y), new Vector2(position.X + width, position.Y + height), color, thickness);
	// Garis atas
	DrawLine(position, new Vector2(position.X + width, position.Y), color, thickness);
	// Garis bawah
	DrawLine(new Vector2(position.X, position.Y + height), new Vector2(position.X + width, position.Y + height), color, thickness);
}

// Tambahkan metode ini untuk menghitung bounding box keris
private Rect2 GetKerisBoundingBox()
{
	// Calculate floating movement
	float floatOffset = Mathf.Sin(_kerisAnimationTimer * Mathf.Pi * _kerisFloatRate) * _kerisFloatAmplitude;
	
	// Apply animation to position
	Vector2 animatedPosition = _kerisPosition + new Vector2(0, floatOffset);
	
	// Approximate dimensions - adjust based on actual size
	float width = 60 * _kerisScale;
	float height = 240 * _kerisScale;
	
	return new Rect2(animatedPosition.X - width/2, animatedPosition.Y - height/2, width, height);
}

// Tambahkan metode ini untuk menghitung bounding box kendang
private Rect2 GetKendangBoundingBox()
{
	// Approximate dimensions
	float width = 320;
	float height = 180;
	
	return new Rect2(_kendangPosition.X - width/2, _kendangPosition.Y - height/2, width, height);
}

// Tambahkan metode ini untuk menghitung bounding box batik
private Rect2 GetBatikBoundingBox()
{
	return new Rect2(_batikPosition, _batikSize);
}
}
