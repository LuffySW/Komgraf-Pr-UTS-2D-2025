using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class karya3 : Node2D
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




// Method to fill a polygon with color
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

// Method to fill a circle with a border
private void FillCircleWithBorder(float cx, float cy, float radius, Color fillColor, Color borderColor, float borderThickness)
{
	// Fill the circle
	FillCircle(cx, cy, radius, fillColor);
	
	// Draw the border
	DrawCircle(cx, cy, radius, borderColor, borderThickness);
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

// Additional methods as needed...

public override void _Draw()
{
	// Background pattern with batik motif - positioned as a frame
	// Geser dan perkecil sedikit area frame batik untuk memberikan ruang lebih
	DrawTruntumBatikMotif(new Vector2(850, 350), 30, 60, 5, 5, _lineColor);
	
	// Posisikan objek dengan lebih terstruktur:
	
	// 1. Wayang kulit di bagian kiri bawah (geser lebih ke kiri)
	DrawWayangKulit(new Vector2(150, 350), 0.9f);
	
	// 2. Kendang di bagian kanan (geser lebih ke kanan dan sesuaikan tinggi)
	DrawTiltedKendang(480, 250);
	
	// 3. Keris di bagian tengah atas (angkat lebih tinggi dan pindah ke tengah)
	DrawKeris(new Vector2(380, 130), 1.3f, 60);
}
public override void _Process(double delta)
{
	// Update the drum animation
	_drumAnimationTimer += (float)delta;
	
	// Check if it's time to hit the drum
	if (_drumAnimationTimer >= _drumHitInterval)
	{
		_drumIsHit = true;
		_drumDeformation = 1.0f;
		_drumAnimationTimer = 0f;
		
		// Randomize hit position slightly for visual interest
		float angle = Mathf.DegToRad((float)Godot.GD.RandRange(-30.0, 30.0));
		_hitPosition = _headLeftCenter + new Vector2(
			_headLeftRadius * 0.6f * Mathf.Cos(angle),
			_headLeftRadius * 0.6f * Mathf.Sin(angle)
		);  
		
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
	
	// Update Truntum batik animation
	_truntumAnimationTimer += (float)delta;
	
	// Update keris animation
	_kerisAnimationTimer += (float)delta;
	_kerisShimmerTimer += (float)delta;
	
	// Check if it's time for the keris to shimmer
	if (_kerisShimmerTimer >= _kerisShimmerInterval)
	{
		_kerisIsShimmering = true;
		_kerisShimmerTimer = 0f;
	}
	
	// Update shimmer effect duration
	if (_kerisIsShimmering)
	{
		if (_kerisShimmerTimer >= _kerisShimmerDuration)
		{
			_kerisIsShimmering = false;
		}
	}

	// Update wayang animation
	_wayangSwayTimer += (float)delta;
	_wayangArmAnimTimer += (float)delta;
	_wayangBreathTimer += (float)delta;
	
	// Always update the animation each frame
	QueueRedraw();
}
private void DrawSmallCircle(Vector2 center, float radius, Color color)
	{
		int segments = 16;
		float angleStep = 2 * Mathf.Pi / segments;
		
		for (int i = 0; i < segments; i++)
		{
			float a1 = i * angleStep;
			float a2 = (i + 1) * angleStep;
			
			Vector2 p1 = center + new Vector2(radius * Mathf.Cos(a1), radius * Mathf.Sin(a1));
			Vector2 p2 = center + new Vector2(radius * Mathf.Cos(a2), radius * Mathf.Sin(a2));
			
			DrawLine(p1, p2, color, 1.0f);
		}
	}
private void DrawWayangKulitBody(Vector2 position, float scale)
{
	// Definisi titik untuk siluet tubuh yang elegan khas wayang kulit
	Vector2[] bodyOutlinePoints = {
		position + new Vector2(-40, 50) * scale,     // Left hip (narrower)
		position + new Vector2(-45, 0) * scale,      // Left waist (more defined)
		position + new Vector2(-35, -70) * scale,    // Left shoulder (adjusted)
		position + new Vector2(0, -100) * scale,     // Neck (shorter)
		position + new Vector2(35, -70) * scale,     // Right shoulder
		position + new Vector2(45, 0) * scale,       // Right waist
		position + new Vector2(40, 50) * scale,      // Right hip
		position + new Vector2(30, 120) * scale,     // Right leg (longer)
		position + new Vector2(0, 135) * scale,      // Bottom center
		position + new Vector2(-30, 120) * scale,    // Left leg
	};
	
	// Fill tubuh dengan warna dasar yang lebih tua
	FillPolygon(bodyOutlinePoints, _wayangBodyColor);
	
	// Tambahkan shading dan detil pada tubuh
	Vector2[] upperBodyShade = {
		position + new Vector2(-35, -70) * scale,    // Left shoulder
		position + new Vector2(0, -100) * scale,     // Neck
		position + new Vector2(35, -70) * scale,     // Right shoulder
		position + new Vector2(25, -40) * scale,     // Upper right chest
		position + new Vector2(0, -30) * scale,      // Center chest
		position + new Vector2(-25, -40) * scale,    // Upper left chest
	};
	
	// Berikan bayangan yang lebih gelap pada bagian dada
	Color upperBodyShadeColor = new Color(_wayangBodyColor.R * 0.9f, _wayangBodyColor.G * 0.9f, _wayangBodyColor.B * 0.9f);
	FillPolygon(upperBodyShade, upperBodyShadeColor);
	
	// Gambar outline dengan garis yang lebih halus
	for (int i = 0; i < bodyOutlinePoints.Length - 1; i++)
	{
		DrawLine(bodyOutlinePoints[i], bodyOutlinePoints[i+1], _wayangDetailColor, 1.5f);
	}
	// Tutup bentuk
	DrawLine(bodyOutlinePoints[bodyOutlinePoints.Length-1], bodyOutlinePoints[0], _wayangDetailColor, 1.5f);
	
	// Tambahkan pola dekoratif di bagian dada
	DrawChestPatterns(position, scale);
}

private void DrawChestPatterns(Vector2 position, float scale)
{
	// Gambar pola dekoratif pada dada - motif ukiran khas wayang kulit
	Vector2 centerChest = position + new Vector2(0, -50) * scale;
	
	// Pola melengkung pada dada (seperti kalung)
	Vector2 neckCenter = position + new Vector2(0, -85) * scale;
	float necklaceRadius = 15 * scale;
	
	// Kalung/hiasan leher
	DrawSmallArc(neckCenter, necklaceRadius, 160, 380, _wayangPatternColor, 1.2f);
	DrawSmallArc(neckCenter, necklaceRadius - 2 * scale, 160, 380, _wayangPatternColor, 0.8f);
	
	// Hiasan pada kalung
	for (int i = 0; i < 5; i++) {
		float angle = Mathf.DegToRad(180 + i * 45);
		Vector2 jewelPos = neckCenter + new Vector2(
			Mathf.Cos(angle) * necklaceRadius,
			Mathf.Sin(angle) * necklaceRadius
		);
		DrawSmallCircle(jewelPos, 2 * scale, _wayangJewelColor);
	}
	
	// Pola pada bagian dada
	for (int i = -1; i <= 1; i++) {
		Vector2 patternCenter = centerChest + new Vector2(i * 15 * scale, 0);
		DrawCircularPattern(patternCenter, 8 * scale, _wayangPatternColor);
	}
}

private void DrawWayangKulitHeadAndCrown(Vector2 position, float scale)
{
	// Posisikan kepala di atas tubuh - lebih kecil dibanding tubuh untuk proporsi yang tepat
	Vector2 headPos = position + new Vector2(0, -120) * scale;
	
	// Definisi bentuk wajah oval memanjang khas wayang kulit
	Vector2[] facePoints = {
		headPos + new Vector2(-20, -30) * scale,  // Crown top left
		headPos + new Vector2(0, -35) * scale,    // Crown top center
		headPos + new Vector2(20, -30) * scale,   // Crown top right
		headPos + new Vector2(23, -10) * scale,   // Crown right
		headPos + new Vector2(20, 10) * scale,    // Face right
		headPos + new Vector2(12, 25) * scale,    // Jaw right
		headPos + new Vector2(0, 30) * scale,     // Chin
		headPos + new Vector2(-12, 25) * scale,   // Jaw left
		headPos + new Vector2(-20, 10) * scale,   // Face left
		headPos + new Vector2(-23, -10) * scale,  // Crown left
		headPos + new Vector2(-20, -30) * scale   // Back to start
	};
	
	// Fill kepala dengan warna dasar
	FillPolygon(facePoints, _wayangHeadColor);
	
	// Gambar garis wajah
	for (int i = 0; i < facePoints.Length - 1; i++)
	{
		DrawLine(facePoints[i], facePoints[i+1], _wayangDetailColor, 1.5f);
	}
	
	// Gambar detail mahkota - desain sumping tradisional
	DrawWayangKulitCrown(headPos, scale);
	
	// Gambar fitur wajah - gaya wayang kulit tradisional
	DrawWayangKulitFace(headPos, scale);
}

private void DrawWayangKulitCrown(Vector2 position, float scale)
{
	// Gambar mahkota/sumping yang rumit khas wayang kulit
	Vector2 crownTop = position + new Vector2(0, -40) * scale;
	
	// Area dasar mahkota
	Vector2[] crownBase = {
		position + new Vector2(-20, -30) * scale,
		position + new Vector2(0, -35) * scale,
		position + new Vector2(20, -30) * scale,
		position + new Vector2(23, -10) * scale,
		position + new Vector2(-23, -10) * scale
	};
	
	// Fill area mahkota dengan warna emas
	FillPolygon(crownBase, _wayangCrownColor);
	
	// Spike mahkota utama - lebih ornamental dan banyak
	int spikeCount = 9; // Meningkatkan jumlah spike untuk headdress yang lebih rumit
	float crownWidth = 45 * scale;
	float spikeSpacing = crownWidth / (spikeCount - 1);
	
	// Gambar struktur mahkota utama dengan ketinggian bervariasi untuk siluet yang lebih menarik
	for (int i = 0; i < spikeCount; i++)
	{
		Vector2 spikeBase = position + new Vector2(-crownWidth/2 + i * spikeSpacing, -30) * scale;
		// Buat pola lebih menarik dengan variasi ketinggian
		float heightVariation = (i % 3 == 0) ? 12 : ((i % 2 == 0) ? 8 : 5);
		Vector2 spikeTop = spikeBase + new Vector2(0, -heightVariation) * scale;
		
		DrawLine(spikeBase, spikeTop, _wayangDetailColor, 1.0f);
		
		// Tambahkan hiasan keemasan pada setiap spike
		if (i % 2 == 0) {
			DrawSmallCircle(spikeBase + new Vector2(0, -heightVariation * 0.7f) * scale, 1.5f * scale, _wayangGoldAccentColor);
		}
	}
	
	// Kurva dekoratif mahkota yang lebih rumit - lengkungan kecil antara spike
	for (int i = 0; i < spikeCount - 1; i++)
	{
		Vector2 curveStart = position + new Vector2(-crownWidth/2 + i * spikeSpacing, -30) * scale;
		Vector2 curveEnd = position + new Vector2(-crownWidth/2 + (i+1) * spikeSpacing, -30) * scale;
		Vector2 curveControl = new Vector2(
			(curveStart.X + curveEnd.X) / 2,
			curveStart.Y - 4 * scale
		);
		
		List<Vector2> crownCurve = _primitif.QuadraticBezier(curveStart, curveControl, curveEnd);
		foreach (var point in crownCurve)
		{
			DrawRect(new Rect2(point, new Vector2(1.0f, 1.0f)), _wayangDetailColor);
		}
	}
	
	// Permata tengah pada mahkota
	DrawSmallCircle(position + new Vector2(0, -30) * scale, 3 * scale, _wayangJewelColor);
	DrawSmallCircle(position + new Vector2(0, -30) * scale, 1.5f * scale, _wayangGoldAccentColor);
	
	// Ornamen samping (sumping) - bagian samping dekoratif khas mahkota wayang kulit
	DrawSumpingOrnament(position + new Vector2(-23, -15) * scale, scale, true);
	DrawSumpingOrnament(position + new Vector2(23, -15) * scale, scale, false);
}

private void DrawSumpingOrnament(Vector2 position, float scale, bool isLeft)
{
	// Pengali arah ornamen samping (mirror untuk sisi kanan)
	float dir = isLeft ? -1 : 1;
	
	// Fill area sumping dengan warna
	Vector2[] sumpingArea = {
		position,
		position + new Vector2(dir * 10, 5) * scale,
		position + new Vector2(dir * 8, -8) * scale
	};
	FillPolygon(sumpingArea, _wayangCrownColor);
	
	// Gambar elemen dekoratif melengkung
	Vector2 start = position;
	Vector2 end = position + new Vector2(dir * 10, 5) * scale;
	Vector2 control = position + new Vector2(dir * 5, -5) * scale;
	
	List<Vector2> curve = _primitif.QuadraticBezier(start, control, end);
	foreach (var point in curve)
	{
		DrawRect(new Rect2(point, new Vector2(1.0f, 1.0f)), _wayangDetailColor);
	}
	
	// Tambahkan lingkaran dekoratif kecil khas desain sumping
	DrawSmallCircle(position + new Vector2(dir * 4, 0) * scale, 2 * scale, _wayangJewelColor);
	DrawSmallCircle(position + new Vector2(dir * 4, 0) * scale, 1 * scale, _wayangGoldAccentColor);
	DrawSmallCircle(end, 1 * scale, _wayangDetailColor);
}

private void DrawWayangKulitKain(Vector2 position, float scale)
{
	// Gambar pola kain tradisional mulai dari bagian bawah tubuh
	Vector2 sarungTop = position + new Vector2(0, 20) * scale;
	float sarungWidth = 85 * scale;  // Lebih sempit untuk menyesuaikan siluet tubuh
	float sarungHeight = 115 * scale; // Lebih pendek untuk menyesuaikan tinggi tubuh
	
	// Definisikan bentuk dasar dari kain
	Vector2[] kainShape = {
		position + new Vector2(-42, 20) * scale,
		position + new Vector2(42, 20) * scale,
		position + new Vector2(30, 120) * scale,
		position + new Vector2(0, 135) * scale,
		position + new Vector2(-30, 120) * scale
	};
	
	// Fill bentuk kain dengan warna dasar
	FillPolygon(kainShape, _wayangKainColor);
	
	// Gambar pola lipatan segitiga khas dodot wayang kulit
	int foldCount = 5; // Dikurangi untuk proporsi lebih baik
	float foldWidth = sarungWidth / foldCount;
	
	for (int i = 0; i < foldCount; i++)
	{
		float x1 = sarungTop.X - sarungWidth/2 + i * foldWidth;
		float x2 = x1 + foldWidth/2;
		float x3 = x1 + foldWidth;
		
		// Lipatan atas
		Vector2 top = new Vector2(x2, sarungTop.Y);
		Vector2 left = new Vector2(x1, sarungTop.Y + 25 * scale);
		Vector2 right = new Vector2(x3, sarungTop.Y + 25 * scale);
		
		// Fill area lipatan dengan warna yang sedikit berbeda
		Vector2[] foldArea = { top, left, right };
		Color foldColor = (i % 2 == 0) ? 
			new Color(_wayangKainColor.R * 1.1f, _wayangKainColor.G * 1.1f, _wayangKainColor.B * 1.1f) : 
			new Color(_wayangKainColor.R * 0.9f, _wayangKainColor.G * 0.9f, _wayangKainColor.B * 0.9f);
		FillPolygon(foldArea, foldColor);
		
		// Garis outline
		DrawLine(top, left, _wayangDetailColor, 1.0f);
		DrawLine(left, right, _wayangDetailColor, 1.0f);
		DrawLine(right, top, _wayangDetailColor, 1.0f);
		
		// Tambahkan pola batik
		if (i % 2 == 0) {
			for (float y = sarungTop.Y + 30 * scale; y < sarungTop.Y + sarungHeight; y += 15 * scale)
			{
				DrawLine(
					new Vector2(x1 + 3 * scale, y), 
					new Vector2(x3 - 3 * scale, y), 
					_wayangPatternColor, 0.8f
				);
			}
		} else {
			for (float y = sarungTop.Y + 35 * scale; y < sarungTop.Y + sarungHeight; y += 15 * scale)
			{
				Vector2 center = new Vector2(x2, y);
				DrawSmallCircle(center, 3 * scale, _wayangPatternColor);
				DrawSmallCircle(center, 1.5f * scale, _wayangDetailColor);
			}
		}
	}
	
	// Tambahkan hiasan pada bagian bawah kain
	for (int i = -4; i <= 4; i++) {
		Vector2 bottomOrnament = new Vector2(
			sarungTop.X + i * 10 * scale,
			sarungTop.Y + sarungHeight - 15 * scale
		);
		
		if (i % 2 == 0) {
			DrawSmallCircle(bottomOrnament, 2 * scale, _wayangGoldAccentColor);
		} else {
			DrawSmallCircle(bottomOrnament, 2 * scale, _wayangJewelColor);
		}
	}
}

private void DrawWayangKulitHandleRod(Vector2 position, float scale)
{
	// Buat batang pegangan bambu/tanduk khas wayang kulit
	float rodThickness = 2.5f * scale;
	
	// Batang vertikal utama melewati tengah tubuh
	Vector2 rodTop = position + new Vector2(0, -110) * scale;
	Vector2 rodBottom = position + new Vector2(0, 150) * scale;
	DrawLine(rodTop, rodBottom, _wayangRodColor, rodThickness); 
	
	// Gradasi warna pada batang untuk memberikan dimensi
	for (float t = 0; t <= 1; t += 0.2f) {
		Vector2 pos1 = MyLerp(rodTop, rodBottom, t);
		Vector2 pos2 = MyLerp(rodTop, rodBottom, t + 0.05f);	
		Color gradColor = new Color(
			_wayangRodColor.R * (1 - (t * 0.3f)),
			_wayangRodColor.G * (1 - (t * 0.3f)),
			_wayangRodColor.B * (1 - (t * 0.3f))
		);
		DrawLine(pos1, pos2, gradColor, rodThickness);
	}
	
	// Gambar titik ikat dekoratif dimana batang melekat pada wayang
	DrawBindingPoint(position + new Vector2(0, -70) * scale, scale, _wayangRodColor);
	DrawBindingPoint(position + new Vector2(0, 20) * scale, scale, _wayangRodColor);
	DrawBindingPoint(position + new Vector2(0, 90) * scale, scale, _wayangRodColor);
}

private void DrawBindingPoint(Vector2 position, float scale, Color color)
{
	// Gambar benang/tali yang mengikat batang ke tubuh wayang
	
	// Benang horizontal
	for (float y = -3; y <= 3; y += 1.5f)
	{
		Vector2 left = position + new Vector2(-5, y) * scale;
		Vector2 right = position + new Vector2(5, y) * scale;
		DrawLine(left, right, _wayangThreadColor, 1.0f);
	}
	
	// Benang vertikal penguat
	Vector2 top = position + new Vector2(0, -4) * scale;
	Vector2 bottom = position + new Vector2(0, 4) * scale;
	DrawLine(top, bottom, _wayangThreadColor, 0.8f);
	
	// Tambahkan aksen warna berbeda di tengah
	DrawSmallCircle(position, 1 * scale, _wayangDetailColor);
}

private void DrawWayangKulitAnimatedArms(Vector2 position, float scale, float leftArmAngle, float rightArmAngle)
{
	// Posisi bahu
	Vector2 leftShoulderPos = position + new Vector2(-35, -70) * scale;
	Vector2 rightShoulderPos = position + new Vector2(35, -70) * scale;
	
	// Konversi sudut ke radian untuk perhitungan
	float leftRadians = Mathf.DegToRad(leftArmAngle);
	float rightRadians = Mathf.DegToRad(rightArmAngle);
	
	// Lengan kiri dengan sudut animasi
	float leftElbowLength = 30 * scale;
	float leftHandLength = 40 * scale;
	
	Vector2 leftElbowPos = leftShoulderPos + new Vector2(
		leftElbowLength * Mathf.Cos(leftRadians),
		leftElbowLength * Mathf.Sin(leftRadians)
	);
	
	Vector2 leftHandPos = leftElbowPos + new Vector2(
		leftHandLength * Mathf.Cos(leftRadians * 1.1f), // Sedikit lebih bengkok
		leftHandLength * Mathf.Sin(leftRadians * 1.1f)
	);
	
	// Lengan kanan dengan sudut animasi
	float rightElbowLength = 30 * scale;
	float rightHandLength = 40 * scale;
	
	Vector2 rightElbowPos = rightShoulderPos + new Vector2(
		rightElbowLength * Mathf.Cos(rightRadians),
		rightElbowLength * Mathf.Sin(rightRadians)
	);
	
	Vector2 rightHandPos = rightElbowPos + new Vector2(
		rightHandLength * Mathf.Cos(rightRadians * 1.1f), // Sedikit lebih bengkok
		rightHandLength * Mathf.Sin(rightRadians * 1.1f)
	);
	
	// Fill area lengan dengan warna dasar
	// Lengan kiri
	Vector2[] leftArmShape = {
		leftShoulderPos,
		leftShoulderPos + new Vector2(-10, 10) * scale,
		leftElbowPos + new Vector2(-8, 8) * scale,
		leftElbowPos,
	};
	FillPolygon(leftArmShape, _wayangBodyColor);
	
	Vector2[] leftForearmShape = {
		leftElbowPos,
		leftElbowPos + new Vector2(-8, 8) * scale,
		leftHandPos + new Vector2(-5, 3) * scale,
		leftHandPos
	};
	FillPolygon(leftForearmShape, _wayangBodyColor);
	
	// Lengan kanan
	Vector2[] rightArmShape = {
		rightShoulderPos,
		rightShoulderPos + new Vector2(10, 10) * scale,
		rightElbowPos + new Vector2(8, 8) * scale,
		rightElbowPos,
	};
	FillPolygon(rightArmShape, _wayangBodyColor);
	
	Vector2[] rightForearmShape = {
		rightElbowPos,
		rightElbowPos + new Vector2(8, 8) * scale,
		rightHandPos + new Vector2(5, 3) * scale,
		rightHandPos
	};
	FillPolygon(rightForearmShape, _wayangBodyColor);
	
	// Gambar lengan kiri dengan kurva bezier
	List<Vector2> leftUpperArm = _primitif.QuadraticBezier(
		leftShoulderPos,
		leftShoulderPos + new Vector2(-15, 20) * scale,
		leftElbowPos
	);
	
	List<Vector2> leftLowerArm = _primitif.QuadraticBezier(
		leftElbowPos,
		leftElbowPos + new Vector2(-15, 15) * scale,
		leftHandPos
	);
	
	foreach (var point in leftUpperArm.Concat(leftLowerArm))
	{
		DrawRect(new Rect2(point, new Vector2(1.5f, 1.5f)), _wayangDetailColor);
	}
	
	// Gambar lengan kanan dengan kurva bezier
	List<Vector2> rightUpperArm = _primitif.QuadraticBezier(
		rightShoulderPos,
		rightShoulderPos + new Vector2(15, 20) * scale,
		rightElbowPos
	);
	
	List<Vector2> rightLowerArm = _primitif.QuadraticBezier(
		rightElbowPos,
		rightElbowPos + new Vector2(15, 15) * scale,
		rightHandPos
	);
	
	foreach (var point in rightUpperArm.Concat(rightLowerArm))
	{
		DrawRect(new Rect2(point, new Vector2(1.5f, 1.5f)), _wayangDetailColor);
	}
	
	// Gambar bentuk tangan
	DrawWayangKulitHand(leftHandPos, leftArmAngle, scale);
	DrawWayangKulitHand(rightHandPos, rightArmAngle, scale);
	
	// Gambar batang kontrol untuk memanipulasi tangan - animasikan juga
	float leftRodAngle = leftArmAngle - 120;
	float rightRodAngle = rightArmAngle - 60;
	DrawControlRod(leftHandPos, leftRodAngle, scale);
	DrawControlRod(rightHandPos, rightRodAngle, scale);
}

private void DrawWayangKulitHand(Vector2 position, float angleOffset, float scale)
{
	// Fill area telapak tangan
	DrawSmallCircle(position, 2 * scale, _wayangBodyColor);
	
	// Gambar tangan stilisasi dengan jari lebih panjang dan elegan khas wayang kulit
	float angle = Mathf.DegToRad(angleOffset);
	float fingerLength = 12 * scale;
	
	Color fingerColor = new Color(_wayangBodyColor.R * 0.9f, _wayangBodyColor.G * 0.9f, _wayangBodyColor.B * 0.9f);
	
	// Wayang kulit tradisional memiliki jari yang sangat halus dan panjang
	for (int i = 0; i < 5; i++)
	{
		float fingerAngle = angle + Mathf.DegToRad(-20 + i * 10); // Sebaran jari lebih dekat
		float thisFingerLength = fingerLength * (i % 3 == 1 ? 1.2f : 1.0f); // Jari tengah lebih panjang
		Vector2 fingerEnd = position + new Vector2(
			Mathf.Cos(fingerAngle) * thisFingerLength,
			Mathf.Sin(fingerAngle) * thisFingerLength
		);
		
		// Gambar shading dasar jari
		Vector2 fingerMid = position + new Vector2(
			Mathf.Cos(fingerAngle) * thisFingerLength * 0.5f,
			Mathf.Sin(fingerAngle) * thisFingerLength * 0.5f
		);
		
		Vector2[] fingerShape = {
			position,
			position + new Vector2(
				Mathf.Cos(fingerAngle + 0.1f) * 3 * scale,
				Mathf.Sin(fingerAngle + 0.1f) * 3 * scale
			),
			fingerEnd + new Vector2(
				Mathf.Cos(fingerAngle + 0.05f) * scale,
				Mathf.Sin(fingerAngle + 0.05f) * scale
			),
			fingerEnd,
			fingerEnd + new Vector2(
				Mathf.Cos(fingerAngle - 0.05f) * scale,
				Mathf.Sin(fingerAngle - 0.05f) * scale
			),
			position + new Vector2(
				Mathf.Cos(fingerAngle - 0.1f) * 3 * scale,
				Mathf.Sin(fingerAngle - 0.1f) * 3 * scale
			)
		};
		
		FillPolygon(fingerShape, fingerColor);
		
		DrawLine(position, fingerEnd, _wayangDetailColor, 1.0f);
		
		// Tambahkan detail pada ujung jari
		DrawSmallCircle(fingerEnd, 0.8f * scale, _wayangDetailColor);
	}
}

private void DrawWayangKulitFace(Vector2 position, float scale)
{
	// Gambar mata tajam, miring khas karakter wayang kulit halus
	DrawWayangKulitEye(position + new Vector2(-9, -5) * scale, scale, false);
	DrawWayangKulitEye(position + new Vector2(9, -5) * scale, scale, true);
	
	// Gambar hidung - lebih menonjol dan stilisasi
	Vector2 noseTop = position + new Vector2(0, 0) * scale;
	Vector2 noseBottom = position + new Vector2(0, 10) * scale;
	Vector2 noseRight = position + new Vector2(4, 8) * scale;
	
	// Fill hidung dengan warna sedikit lebih gelap
	Vector2[] noseShape = { noseTop, noseBottom, noseRight };
	Color noseColor = new Color(_wayangHeadColor.R * 0.9f, _wayangHeadColor.G * 0.9f, _wayangHeadColor.B * 0.9f);
	FillPolygon(noseShape, noseColor);
	
	DrawLine(noseTop, noseBottom, _wayangDetailColor, 1.2f);
	DrawLine(noseBottom, noseRight, _wayangDetailColor, 1.2f);
	
	// Gambar mulut - senyuman stilisasi khas
	Vector2 mouthLeft = position + new Vector2(-8, 18) * scale;
	Vector2 mouthRight = position + new Vector2(8, 18) * scale;
	Vector2 mouthCenter = position + new Vector2(0, 20) * scale;
	
	// Fill area mulut dengan warna sedikit lebih gelap
	List<Vector2> mouthCurve = _primitif.QuadraticBezier(mouthLeft, mouthCenter, mouthRight);
	List<Vector2> mouthBottomCurve = _primitif.QuadraticBezier(
		mouthRight, 
		new Vector2(0, 19f) * scale + position, 
		mouthLeft
	);
	
	// Gabungkan kurva untuk membuat area yang bisa diisi
	List<Vector2> mouthArea = new List<Vector2>();
	mouthArea.AddRange(mouthCurve);
	mouthArea.AddRange(mouthBottomCurve);
	
	// Fill area mulut
	Color mouthColor = new Color(0.7f, 0.3f, 0.25f); // Warna kemerahan untuk bibir
	FillPolygon(mouthArea.ToArray(), mouthColor);
	
	// Outline mulut
	foreach (var point in mouthCurve)
	{
		DrawRect(new Rect2(point, new Vector2(1.0f, 1.0f)), _wayangDetailColor);
	}
	
	// Tambahkan detail wajah lainnya - alis, tanda di dahi, dll
	
	// Alis
	Vector2 leftEyebrowStart = position + new Vector2(-15, -12) * scale;
	Vector2 leftEyebrowEnd = position + new Vector2(-3, -13) * scale;
	Vector2 leftEyebrowControl = position + new Vector2(-9, -16) * scale;
	
	Vector2 rightEyebrowStart = position + new Vector2(3, -13) * scale;
	Vector2 rightEyebrowEnd = position + new Vector2(15, -12) * scale;
	Vector2 rightEyebrowControl = position + new Vector2(9, -16) * scale;
	
	List<Vector2> leftEyebrow = _primitif.QuadraticBezier(leftEyebrowStart, leftEyebrowControl, leftEyebrowEnd);
	List<Vector2> rightEyebrow = _primitif.QuadraticBezier(rightEyebrowStart, rightEyebrowControl, rightEyebrowEnd);
	
	foreach (var point in leftEyebrow.Concat(rightEyebrow))
	{
		DrawRect(new Rect2(point, new Vector2(1.5f, 1.5f)), _wayangDetailColor);
	}
	
	// Tanda di dahi (seperti tilaka/urna) - khas untuk beberapa karakter wayang
	DrawSmallCircle(position + new Vector2(0, -15) * scale, 1.5f * scale, _wayangDetailColor);
	DrawSmallCircle(position + new Vector2(0, -15) * scale, 0.8f * scale, _wayangJewelColor);
}

private void DrawWayangKulitEye(Vector2 position, float scale, bool isRight)
{
	// Gambar mata bentuk almond khas wayang kulit - lebih tajam/miring
	float eyeWidth = 8 * scale;
	float eyeHeight = 5 * scale;
	
	Vector2 eyeLeft = position + new Vector2(-eyeWidth/2, 0);
	Vector2 eyeTop = position + new Vector2(0, -eyeHeight/2);
	Vector2 eyeRight = position + new Vector2(eyeWidth/2, 0);
	Vector2 eyeBottom = position + new Vector2(0, eyeHeight/2);
	
	// Definisikan bentuk mata untuk diisi warna
	List<Vector2> topCurve = _primitif.QuadraticBezier(
		eyeLeft, 
		position + new Vector2(0, -eyeHeight * 0.7f), // Kurva atas lebih runcing
		eyeRight
	);
	
	List<Vector2> bottomCurve = _primitif.QuadraticBezier(
		eyeRight, 
		position + new Vector2(0, eyeHeight * 0.4f), // Kurva bawah lebih datar
		eyeLeft
	);
	
	// Gabungkan kurva untuk membuat area yang bisa diisi
	List<Vector2> eyeArea = new List<Vector2>();
	eyeArea.AddRange(topCurve);
	eyeArea.AddRange(bottomCurve);
	
	// Fill mata dengan warna putih
	Color eyeWhiteColor = new Color(0.95f, 0.95f, 0.9f);
	FillPolygon(eyeArea.ToArray(), eyeWhiteColor);
	
	// Gambar outline mata
	foreach (var point in topCurve.Concat(bottomCurve))
	{
		DrawRect(new Rect2(point, new Vector2(1.0f, 1.0f)), _wayangDetailColor);
	}
	
	// Pupil - sedikit digeser
	Vector2 pupilPos = position + new Vector2((isRight ? 1.5f : -1.5f) * scale, 0);
	DrawSmallCircle(pupilPos, 1.5f * scale, _wayangDetailColor);
	
	// Highlight pada pupil untuk memberikan efek mengkilap
	DrawSmallCircle(pupilPos + new Vector2(0.5f, -0.5f) * scale, 0.5f * scale, eyeWhiteColor);
}

private void DrawWayangKulitDetails(Vector2 position, float scale)
{
	// Gambar pola tradisional yang rumit pada tubuh - lebih halus
	
	// Pola dada (kawung atau motif batik serupa)
	Vector2 chestCenter = position + new Vector2(0, -50) * scale;
	DrawCircularPattern(chestCenter, 20 * scale, _wayangPatternColor);
	
	// Hiasan bahu (lebih kecil)
	DrawCircularPattern(position + new Vector2(-30, -60) * scale, 10 * scale, _wayangPatternColor);
	DrawCircularPattern(position + new Vector2(30, -60) * scale, 10 * scale, _wayangPatternColor);
	
	// Detail ikat pinggang/sabuk (lebih sempit)
	Vector2 beltStart = position + new Vector2(-42, 20) * scale;
	Vector2 beltEnd = position + new Vector2(42, 20) * scale;
	
	// Fill area sabuk dengan warna khusus
	Vector2[] beltArea = {
		position + new Vector2(-42, 17) * scale,
		position + new Vector2(42, 17) * scale,
		position + new Vector2(42, 23) * scale,
		position + new Vector2(-42, 23) * scale
	};
	Color beltColor = new Color(0.8f, 0.7f, 0.2f); // Warna keemasan untuk sabuk
	FillPolygon(beltArea, beltColor);
	
	DrawLine(beltStart, beltEnd, _wayangDetailColor, 2.0f);
	
	// Tambahkan gesper ikat pinggang
	Vector2 buckleCenter = position + new Vector2(0, 20) * scale;
	DrawSmallCircle(buckleCenter, 5 * scale, _wayangGoldAccentColor);
	DrawSmallCircle(buckleCenter, 3 * scale, _wayangJewelColor);
	
	// Tambahkan pola kecil tradisional di dada - seperti tumpal atau elemen batik lainnya
	for (int i = -2; i <= 2; i++) {
		for (int j = -2; j <= 2; j++) {
			if (i*i + j*j <= 4) { // Buat pola melingkar
				Vector2 patternPos = position + new Vector2(i * 12, j * 12 - 20) * scale;
				DrawSmallCircle(patternPos, 1.5f * scale, _wayangDetailColor);
				
				// Tambah variasi warna pada pola
				if ((i+j) % 2 == 0) {
					DrawSmallCircle(patternPos, 0.8f * scale, _wayangGoldAccentColor);
				}
			}
		}
	}
	
	// Tambahkan detail ukiran pada bahu
	Vector2 leftShoulderCenter = position + new Vector2(-30, -60) * scale;
	Vector2 rightShoulderCenter = position + new Vector2(30, -60) * scale;
	
	// Pattern pada bahu kiri
	for (int i = -1; i <= 1; i++) {
		DrawSmallCircle(leftShoulderCenter + new Vector2(i * 5, 0) * scale, 2 * scale, _wayangPatternColor);
		DrawSmallCircle(leftShoulderCenter + new Vector2(i * 5, 0) * scale, 1 * scale, _wayangGoldAccentColor);
	}
	
	// Pattern pada bahu kanan
	for (int i = -1; i <= 1; i++) {
		DrawSmallCircle(rightShoulderCenter + new Vector2(i * 5, 0) * scale, 2 * scale, _wayangPatternColor);
		DrawSmallCircle(rightShoulderCenter + new Vector2(i * 5, 0) * scale, 1 * scale, _wayangGoldAccentColor);
	}
	
	// Tambahkan aksen emas pada bagian lain dari tubuh
	DrawGoldAccents(position, scale);
}

private void DrawGoldAccents(Vector2 position, float scale)
{
	// Tambahkan detail ukiran emas di beberapa bagian tubuh
	
	// Gelang di lengan dekat siku
	Vector2 leftArmBand = position + new Vector2(-45, -30) * scale;
	Vector2 rightArmBand = position + new Vector2(45, -30) * scale;
	
	DrawArmBand(leftArmBand, scale, -1);
	DrawArmBand(rightArmBand, scale, 1);
	
	// Aksen melengkung di dada
	Vector2 chestTop = position + new Vector2(0, -70) * scale;
	float chestWidth = 50 * scale;
	
	Vector2 leftChest = chestTop + new Vector2(-chestWidth/2, 0);
	Vector2 rightChest = chestTop + new Vector2(chestWidth/2, 0);
	Vector2 chestControl = chestTop + new Vector2(0, 20 * scale);
	
	List<Vector2> chestCurve = _primitif.QuadraticBezier(leftChest, chestControl, rightChest);
	
	foreach (var point in chestCurve)
	{
		DrawRect(new Rect2(point, new Vector2(1.5f, 1.5f)), _wayangGoldAccentColor);
	}
	
	// Tambahkan aksen perhiasan di sepanjang kurva dada
	for (int i = 0; i < chestCurve.Count; i += chestCurve.Count / 5) {
		if (i < chestCurve.Count) {
			DrawSmallCircle(chestCurve[i], 2 * scale, _wayangJewelColor);
		}
	}
	
	// Kalungat/kalung tradisional
	Vector2 neckCenter = position + new Vector2(0, -95) * scale;
	float necklaceWidth = 30 * scale;
	
	for (int i = -2; i <= 2; i++) {
		Vector2 jewelPos = neckCenter + new Vector2(i * necklaceWidth/2, Math.Abs(i) * 5 * scale);
		DrawSmallCircle(jewelPos, 2 * scale, _wayangGoldAccentColor);
		
		if (i % 2 == 0) {
			DrawSmallCircle(jewelPos, 1 * scale, _wayangJewelColor);
		}
	}
}

private void DrawArmBand(Vector2 position, float scale, float direction)
{
	// Gelang lengan atas tradisional
	float bandWidth = 10 * scale;
	float bandHeight = 5 * scale;
	
	// Fill area gelang
	Vector2[] bandShape = {
		position + new Vector2(-bandWidth/2, -bandHeight/2),
		position + new Vector2(bandWidth/2, -bandHeight/2),
		position + new Vector2(bandWidth/2, bandHeight/2),
		position + new Vector2(-bandWidth/2, bandHeight/2)
	};
	FillPolygon(bandShape, _wayangGoldAccentColor);
	
	// Border gelang
	DrawLine(bandShape[0], bandShape[1], _wayangDetailColor, 1.0f);
	DrawLine(bandShape[1], bandShape[2], _wayangDetailColor, 1.0f);
	DrawLine(bandShape[2], bandShape[3], _wayangDetailColor, 1.0f);
	DrawLine(bandShape[3], bandShape[0], _wayangDetailColor, 1.0f);
	
	// Detail dekoratif pada gelang
	int detailCount = 3;
	for (int i = 0; i < detailCount; i++) {
		float x = position.X - bandWidth/2 + (i+1) * bandWidth/(detailCount+1);
		DrawSmallCircle(new Vector2(x, position.Y), 1 * scale, _wayangJewelColor);
	}
}

private void DrawCircularPattern(Vector2 center, float radius, Color color)
{
	// Fill area pattern dengan warna dasar
	DrawFilledCircle(center, radius, new Color(_wayangBodyColor.R * 0.9f, _wayangBodyColor.G * 0.9f, _wayangBodyColor.B * 0.9f));
	
	// Gambar lingkaran luar
	DrawSmallCircle(center, radius, color);
	
	// Gambar lingkaran dalam
	DrawSmallCircle(center, radius * 0.7f, color);
	
	// Gambar sinar dekoratif
	int rayCount = 8;
	for (int i = 0; i < rayCount; i++)
	{
		float angle = i * Mathf.Pi * 2 / rayCount;
		Vector2 inner = center + new Vector2(
			radius * 0.7f * Mathf.Cos(angle),
			radius * 0.7f * Mathf.Sin(angle)
		);
		
		Vector2 outer = center + new Vector2(
			radius * Mathf.Cos(angle),
			radius * Mathf.Sin(angle)
		);
		
		DrawLine(inner, outer, color, 1.0f);
	}
	
	// Tambahkan aksen emas di tengah
	DrawSmallCircle(center, radius * 0.3f, _wayangGoldAccentColor);
}

private void DrawFilledCircle(Vector2 center, float radius, Color color)
{
	// Gambar lingkaran dengan fill (implementasi sederhana)
	int segments = 24;
	float angleStep = 2 * Mathf.Pi / segments;
	
	// Buat array titik untuk polygon
	Vector2[] points = new Vector2[segments];
	for (int i = 0; i < segments; i++)
	{
		float angle = i * angleStep;
		points[i] = center + new Vector2(
			radius * Mathf.Cos(angle),
			radius * Mathf.Sin(angle)
		);
	}
	
	// Fill polygon untuk efek lingkaran terisi
	FillPolygon(points, color);
}

private void DrawSmallArc(Vector2 center, float radius, float startAngleDeg, float endAngleDeg, Color color, float thickness)
{
	// Konversi derajat ke radian
	float startAngle = Mathf.DegToRad(startAngleDeg);
	float endAngle = Mathf.DegToRad(endAngleDeg);
	
	int segments = 24;
	float angleRange = endAngle - startAngle;
	float angleStep = angleRange / segments;
	
	for (int i = 0; i < segments; i++)
	{
		float a1 = startAngle + i * angleStep;
		float a2 = startAngle + (i + 1) * angleStep;
		
		Vector2 p1 = center + new Vector2(radius * Mathf.Cos(a1), radius * Mathf.Sin(a1));
		Vector2 p2 = center + new Vector2(radius * Mathf.Cos(a2), radius * Mathf.Sin(a2));
		
		DrawLine(p1, p2, color, thickness);
	}
}
// Add this method to your karya3 class
private Vector2 MyLerp(Vector2 start, Vector2 end, float weight)
{
	return new Vector2(
		start.X + (end.X - start.X) * weight,
		start.Y + (end.Y - start.Y) * weight
	);
}
private void DrawControlRod(Vector2 handPosition, float angleOffset, float scale)
{
	// Gambar batang kontrol tradisional untuk menggerakkan tangan wayang
	float rodAngle = (float)Mathf.DegToRad(angleOffset);
	float rodLength = 60 * scale;
	float rodThickness = 1.5f * scale;
	
	// Arah batang dari tangan
	Vector2 rodEnd = handPosition + new Vector2(
		Mathf.Cos(rodAngle) * rodLength,
		Mathf.Sin(rodAngle) * rodLength
	);
	
	// Gambar gradasi warna pada batang
	for (float t = 0; t <= 1; t += 0.2f) {
		Vector2 pos1 = MyLerp(handPosition, rodEnd, t);
		Vector2 pos2 = MyLerp(handPosition, rodEnd, t + 0.15f);		
		
		// Warna yang semakin gelap ke ujung batang
		Color gradColor = new Color(
			_wayangRodColor.R * (1 - (t * 0.3f)),
			_wayangRodColor.G * (1 - (t * 0.3f)),
			_wayangRodColor.B * (1 - (t * 0.3f))
		);
		
		DrawLine(pos1, pos2, gradColor, rodThickness);
	}
	
	// Gambar pegangan di ujung batang kontrol
	DrawSmallCircle(rodEnd, 2 * scale, _wayangRodColor);
	// Tambahkan detail pada pegangan
	DrawSmallCircle(rodEnd, 1 * scale, _wayangThreadColor);
}

// Metode ini akan dipanggil untuk mengupdate timer animasi
private void UpdateWayangAnimation(float delta)
{
	// Update timer untuk animasi gerakan
	_wayangSwayTimer += delta;
	_wayangBreathTimer += delta;
	_wayangArmAnimTimer += delta;
	
	// Buat pergerakan tidak terlalu cepat
	if (_wayangSwayTimer > 10000) _wayangSwayTimer = 0;
	if (_wayangBreathTimer > 10000) _wayangBreathTimer = 0;
	if (_wayangArmAnimTimer > 10000) _wayangArmAnimTimer = 0;
}

private Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angleRadians)
{
	// Translate point to origin (relative to pivot)
	float translatedX = point.X - pivot.X;
	float translatedY = point.Y - pivot.Y;
	
	// Rotate the point around origin
	float rotatedX = translatedX * Mathf.Cos(angleRadians) - translatedY * Mathf.Sin(angleRadians);
	float rotatedY = translatedX * Mathf.Sin(angleRadians) + translatedY * Mathf.Cos(angleRadians);
	
	// Translate back to the original position
	return new Vector2(
		rotatedX + pivot.X,
		rotatedY + pivot.Y
	);
}

private void DrawWayangKulit(Vector2 position, float scale)
{
	// Hitung efek animasi
	float sway = Mathf.Sin(_wayangSwayTimer * Mathf.Pi * _wayangSwayRate) * _wayangSwayAmount;
	float swayRadians = Mathf.DegToRad(sway);
	
	// Efek pernapasan lembut
	float breathEffect = Mathf.Sin(_wayangBreathTimer * Mathf.Pi * _wayangBreathRate) * 0.05f + 1.0f;
	
	// Hitung sudut lengan dengan animasi
	float leftArmAngle = _wayangLeftArmAngle + Mathf.Sin(_wayangArmAnimTimer * 0.8f) * 15f;
	float rightArmAngle = _wayangRightArmAngle + Mathf.Sin(_wayangArmAnimTimer * 0.6f + Mathf.Pi * 0.5f) * 10f;
	
	// Buat matriks transformasi untuk seluruh wayang (disederhanakan hanya untuk rotasi di sini)
	Vector2 rotateOrigin = position + new Vector2(0, 50) * scale; // Titik pivot dekat pusat
	
	// Gambar komponen dalam urutan wayang kulit yang benar (belakang ke depan)
	DrawWayangKulitHandleRod(position, scale);  // Batang tengah tetap diam
	
	// Terapkan rotasi dan scaling halus pada bagian tubuh
	DrawWayangKulitBody(RotatePoint(position, rotateOrigin, swayRadians), scale * breathEffect);
	DrawWayangKulitKain(RotatePoint(position, rotateOrigin, swayRadians), scale);
	
	// Gambar lengan dengan sudut animasi
	DrawWayangKulitAnimatedArms(RotatePoint(position, rotateOrigin, swayRadians), scale, leftArmAngle, rightArmAngle);
	
	// Kepala dan mahkota dengan rotasi
	DrawWayangKulitHeadAndCrown(RotatePoint(position, rotateOrigin, swayRadians), scale);
	DrawWayangKulitDetails(RotatePoint(position, rotateOrigin, swayRadians), scale);
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

	private void DrawLine(Vector2 start, Vector2 end, Color color, float thickness)
	{
		var points = _primitif.LineDDA(start.X, start.Y, end.X, end.Y);
		foreach (var p in points)
		{
			DrawRect(new Rect2(p, new Vector2(thickness, thickness)), color);
		}
	}

	private void DrawTruntumBatikMotif(Vector2 center, float width, float height, int rows, int columns, Color color)
	{
		// Frame position and size
		float startX = 650;
		float startY = 170;
		float totalWidth = 350;
		float totalHeight = 450;

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
				DrawCircle(dotPos.X + 1, dotPos.Y + 1, animatedSize, color * new Color(1,1,1,0.3f * dotPulse), 0.8f);
				DrawCircle(dotPos.X, dotPos.Y, animatedSize, color, 0.8f);
			}
		}
	}
}
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
	DrawCircle(center.X, center.Y, size * 0.2f * centerPulse, color, 1.2f);

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
	DrawCircle(center.X, center.Y, size * 0.1f * (2.0f - centerPulse), color, 0.8f);
}
private void DrawTruntumFlower(Vector2 center, float size, Color color)
{
	// Simplified flower with 6 petals for hexagonal alignment
	int petals = 6;
	float angleStep = Mathf.Pi * 2 / petals;

	// Center dot
	DrawCircle(center.X, center.Y, size * 0.2f, color, 1.2f);

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
	DrawCircle(center.X, center.Y, size * 0.1f, color, 0.8f);
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

private void DrawKeris(Vector2 basePosition, float scale, float rotationDegrees)
{
	// Calculate floating movement
	float floatOffset = Mathf.Sin(_kerisAnimationTimer * Mathf.Pi * _kerisFloatRate) * _kerisFloatAmplitude;
	
	// Calculate subtle rotation oscillation
	float rotationOffset = Mathf.Sin(_kerisAnimationTimer * Mathf.Pi * 0.4f) * _kerisRotationAmount;
	
	// Apply the floating movement and rotation to the base position
	Vector2 animatedPosition = basePosition + new Vector2(0, floatOffset);
	float animatedRotation = rotationDegrees + rotationOffset;
	
	// Shift the entire keris slightly to the right to avoid the wayang
	Vector2 origin = animatedPosition + new Vector2(40, 0);
	float rotation = Mathf.DegToRad(animatedRotation);
	
	// Change offset direction to position sheath above the keris instead of to the side
	float sheathOffset = 25f * scale;
	
	// Calculate offset vector opposite to keris direction (pointing upward)
	Vector2 sheathOffsetVector = new Vector2(
		sheathOffset * Mathf.Cos(rotation - Mathf.Pi),
		sheathOffset * Mathf.Sin(rotation - Mathf.Pi)
	);
	
	// Add a horizontal offset to position the sheath slightly to the right
	Vector2 sheathHorizontalOffset = new Vector2(
		10f * scale * Mathf.Cos(rotation + Mathf.Pi/2),
		10f * scale * Mathf.Sin(rotation + Mathf.Pi/2)
	);
	
	// Position sheath with both offsets applied
	Vector2 sheathPosition = origin + sheathOffsetVector + sheathHorizontalOffset;
	
	// Offset for handle remains the same
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



}
