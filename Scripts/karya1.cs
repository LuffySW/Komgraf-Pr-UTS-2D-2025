using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class karya1 : Node2D
{
	private primitif _primitif = new primitif();
	private Color _hitam = Colors.Black;

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

	private void DrawWayangKulit(Vector2 position, float scale)
	{
		// Draw components in proper wayang kulit order (back to front)
		DrawWayangKulitHandleRod(position, scale);  // Draw central handle rod
		DrawWayangKulitBody(position, scale);       // Body outline
		DrawWayangKulitKain(position, scale);       // Cloth/costume pattern
		DrawWayangKulitArms(position, scale);       // Arms with control rods
		DrawWayangKulitHeadAndCrown(position, scale);  // Face and headdress
		DrawWayangKulitDetails(position, scale);    // Ornamental details
	}

	private void DrawWayangKulitBody(Vector2 position, float scale)
	{
		// Slimmer, more elegant body silhouette typical of wayang kulit
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
			position + new Vector2(-40, 50) * scale      // Close the shape
		};
		
		// Draw the outline with thinner, more delicate lines
		for (int i = 0; i < bodyOutlinePoints.Length - 1; i++)
		{
			DrawLine(bodyOutlinePoints[i], bodyOutlinePoints[i+1], _lineColor, 1.5f);
		}
	}

	private void DrawWayangKulitHandleRod(Vector2 position, float scale)
	{
		// Create central bamboo/horn handle rod typical in wayang kulit
		Color rodColor = new Color(0.7f, 0.55f, 0.4f); // Bamboo/horn color
		float rodThickness = 2.5f * scale;
		
		// Main vertical rod going through body center
		Vector2 rodTop = position + new Vector2(0, -110) * scale;
		Vector2 rodBottom = position + new Vector2(0, 150) * scale;
		DrawLine(rodTop, rodBottom, rodColor, rodThickness);
		
		// Draw decorative binding points where rod attaches to puppet
		DrawBindingPoint(position + new Vector2(0, -70) * scale, scale, rodColor);
		DrawBindingPoint(position + new Vector2(0, 20) * scale, scale, rodColor);
		DrawBindingPoint(position + new Vector2(0, 90) * scale, scale, rodColor);
	}

	private void DrawBindingPoint(Vector2 position, float scale, Color color)
	{
		// Draw wrapped thread/string binding the rod to puppet body
		Color threadColor = new Color(0.8f, 0.75f, 0.6f);
		
		// Horizontal wrapping threads
		for (float y = -3; y <= 3; y += 1.5f)
		{
			Vector2 left = position + new Vector2(-5, y) * scale;
			Vector2 right = position + new Vector2(5, y) * scale;
			DrawLine(left, right, threadColor, 1.0f);
		}
		
		// Vertical securing thread
		Vector2 top = position + new Vector2(0, -4) * scale;
		Vector2 bottom = position + new Vector2(0, 4) * scale;
		DrawLine(top, bottom, threadColor, 0.8f);
	}

	private void DrawWayangKulitArms(Vector2 position, float scale)
	{
		// Shoulder positions
		Vector2 leftShoulderPos = position + new Vector2(-35, -70) * scale;
		Vector2 rightShoulderPos = position + new Vector2(35, -70) * scale;
		
		// Left arm - with elbow joint and thinner design
		Vector2 leftElbowPos = leftShoulderPos + new Vector2(-25, 40) * scale;
		Vector2 leftHandPos = leftElbowPos + new Vector2(-30, 30) * scale;
		
		// Draw left arm with bezier curves for more organic shape
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
			DrawRect(new Rect2(point, new Vector2(1.5f, 1.5f)), _lineColor);
		}
		
		// Right arm - with elbow joint
		Vector2 rightElbowPos = rightShoulderPos + new Vector2(25, 40) * scale;
		Vector2 rightHandPos = rightElbowPos + new Vector2(30, 30) * scale;
		
		// Draw right arm with bezier curves
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
			DrawRect(new Rect2(point, new Vector2(1.5f, 1.5f)), _lineColor);
		}
		
		// Draw hand shapes and control rods
		DrawWayangKulitHand(leftHandPos, -30, scale);
		DrawWayangKulitHand(rightHandPos, 30, scale);
		
		// Draw the control rods for manipulating hands
		DrawControlRod(leftHandPos, -150, scale);
		DrawControlRod(rightHandPos, -30, scale);
	}

	private void DrawControlRod(Vector2 handPosition, float angleOffset, float scale)
	{
		// Draw traditional control rod used to move puppet arms
		Color rodColor = new Color(0.7f, 0.55f, 0.4f); // Bamboo/horn color
		float rodAngle = Mathf.DegToRad(angleOffset);
		float rodLength = 60 * scale;
		float rodThickness = 1.5f * scale;
		
		// Rod direction from hand
		Vector2 rodEnd = handPosition + new Vector2(
			Mathf.Cos(rodAngle) * rodLength,
			Mathf.Sin(rodAngle) * rodLength
		);
		
		DrawLine(handPosition, rodEnd, rodColor, rodThickness);
		
		// Draw handle at the end of control rod
		DrawSmallCircle(rodEnd, 2 * scale, rodColor);
	}

	private void DrawWayangKulitHand(Vector2 position, float angleOffset, float scale)
	{
		// Draw a stylized hand with longer, more elegant fingers typical of wayang kulit
		float angle = Mathf.DegToRad(angleOffset);
		float fingerLength = 12 * scale;
		
		// Traditional wayang kulit has very fine, long fingers
		for (int i = 0; i < 5; i++)
		{
			float fingerAngle = angle + Mathf.DegToRad(-20 + i * 10); // Closer finger spread
			Vector2 fingerEnd = position + new Vector2(
				Mathf.Cos(fingerAngle) * fingerLength * (i % 3 == 1 ? 1.2f : 1.0f), // Middle finger longer
				Mathf.Sin(fingerAngle) * fingerLength * (i % 3 == 1 ? 1.2f : 1.0f)
			);
			
			DrawLine(position, fingerEnd, _lineColor, 1.0f);
		}
	}

	private void DrawWayangKulitHeadAndCrown(Vector2 position, float scale)
	{
		// Position the head on top of the body - smaller compared to body for proper proportion
		Vector2 headPos = position + new Vector2(0, -120) * scale;
		
		// Draw the face shape with elongated oval shape typical of wayang kulit
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
		
		// Draw the face outline
		for (int i = 0; i < facePoints.Length - 1; i++)
		{
			DrawLine(facePoints[i], facePoints[i+1], _lineColor, 1.5f);
		}
		
		// Draw elaborate crown details - traditional sumping design
		DrawWayangKulitCrown(headPos, scale);
		
		// Draw facial features - traditional wayang kulit style
		DrawWayangKulitFace(headPos, scale);
	}

	private void DrawWayangKulitCrown(Vector2 position, float scale)
	{
		// Draw the elaborate crown/headdress (sumping) typical of wayang kulit
		Vector2 crownTop = position + new Vector2(0, -40) * scale;
		
		// Main crown spikes - more ornate and numerous
		int spikeCount = 9; // Increased spike count for more intricate headdress
		float crownWidth = 45 * scale;
		float spikeSpacing = crownWidth / (spikeCount - 1);
		
		// Draw main crown structure with varying heights for more interesting silhouette
		for (int i = 0; i < spikeCount; i++)
		{
			Vector2 spikeBase = position + new Vector2(-crownWidth/2 + i * spikeSpacing, -30) * scale;
			// Create more interesting pattern with varied heights
			float heightVariation = (i % 3 == 0) ? 12 : ((i % 2 == 0) ? 8 : 5);
			Vector2 spikeTop = spikeBase + new Vector2(0, -heightVariation) * scale;
			
			DrawLine(spikeBase, spikeTop, _lineColor, 1.0f);
		}
		
		// More intricate crown decorative curves - small arcs between spikes
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
				DrawRect(new Rect2(point, new Vector2(1.0f, 1.0f)), _lineColor);
			}
		}
		
		// Draw central jewel in crown
		DrawSmallCircle(position + new Vector2(0, -30) * scale, 3 * scale, _lineColor);
		
		// Side ornaments (sumping) - decorative side pieces typical of wayang kulit crowns
		DrawSumpingOrnament(position + new Vector2(-23, -15) * scale, scale, true);
		DrawSumpingOrnament(position + new Vector2(23, -15) * scale, scale, false);
	}

	private void DrawSumpingOrnament(Vector2 position, float scale, bool isLeft)
	{
		// Side ornament direction multiplier (mirror for right side)
		float dir = isLeft ? -1 : 1;
		
		// Draw the curved decorative elements
		Vector2 start = position;
		Vector2 end = position + new Vector2(dir * 10, 5) * scale;
		Vector2 control = position + new Vector2(dir * 5, -5) * scale;
		
		List<Vector2> curve = _primitif.QuadraticBezier(start, control, end);
		foreach (var point in curve)
		{
			DrawRect(new Rect2(point, new Vector2(1.0f, 1.0f)), _lineColor);
		}
		
		// Add small decorative circles typical of sumping design
		DrawSmallCircle(position + new Vector2(dir * 4, 0) * scale, 2 * scale, _lineColor);
		DrawSmallCircle(end, 1 * scale, _lineColor);
	}

	private void DrawWayangKulitKain(Vector2 position, float scale)
	{
		// Draw the traditional cloth pattern starting lower on the body
		Vector2 sarungTop = position + new Vector2(0, 20) * scale;
		float sarungWidth = 85 * scale;  // Narrower to match body silhouette
		float sarungHeight = 115 * scale; // Shorter to match body height
		
		// Draw the triangular fold patterns typical of wayang kulit kain dodot
		int foldCount = 5; // Reduced for better proportion
		float foldWidth = sarungWidth / foldCount;
		
		for (int i = 0; i < foldCount; i++)
		{
			float x1 = sarungTop.X - sarungWidth/2 + i * foldWidth;
			float x2 = x1 + foldWidth/2;
			float x3 = x1 + foldWidth;
			
			// Upper fold
			Vector2 top = new Vector2(x2, sarungTop.Y);
			Vector2 left = new Vector2(x1, sarungTop.Y + 25 * scale);
			Vector2 right = new Vector2(x3, sarungTop.Y + 25 * scale);
			
			DrawLine(top, left, _lineColor, 1.0f);
			DrawLine(left, right, _lineColor, 1.0f);
			DrawLine(right, top, _lineColor, 1.0f);
			
			// Add detailed batik-like pattern
			if (i % 2 == 0) {
				for (float y = sarungTop.Y + 30 * scale; y < sarungTop.Y + sarungHeight; y += 15 * scale)
				{
					DrawLine(
						new Vector2(x1 + 3 * scale, y), 
						new Vector2(x3 - 3 * scale, y), 
						_lineColor, 0.8f
					);
				}
			} else {
				for (float y = sarungTop.Y + 35 * scale; y < sarungTop.Y + sarungHeight; y += 15 * scale)
				{
					Vector2 center = new Vector2(x2, y);
					DrawSmallCircle(center, 3 * scale, _lineColor);
				}
			}
		}
	}

	private void DrawWayangKulitDetails(Vector2 position, float scale)
	{
		// Draw intricate traditional patterns on the body - more refined
		
		// Chest pattern (kawung or similar batik motif)
		Vector2 chestCenter = position + new Vector2(0, -50) * scale;
		DrawCircularPattern(chestCenter, 20 * scale, _lineColor);
		
		// Shoulder ornaments (smaller)
		DrawCircularPattern(position + new Vector2(-30, -60) * scale, 10 * scale, _lineColor);
		DrawCircularPattern(position + new Vector2(30, -60) * scale, 10 * scale, _lineColor);
		
		// Belt/sash detail (narrower)
		Vector2 beltStart = position + new Vector2(-42, 20) * scale;
		Vector2 beltEnd = position + new Vector2(42, 20) * scale;
		DrawLine(beltStart, beltEnd, _lineColor, 2.0f);
		
		// Add decorative belt buckle
		Vector2 buckleCenter = position + new Vector2(0, 20) * scale;
		DrawSmallCircle(buckleCenter, 5 * scale, _lineColor);
		DrawSmallCircle(buckleCenter, 3 * scale, _lineColor);
		
		// Add traditional small pattern across chest - like tumpal or other batik element
		for (int i = -2; i <= 2; i++) {
			for (int j = -2; j <= 2; j++) {
				if (i*i + j*j <= 4) { // Create a circular pattern
					Vector2 patternPos = position + new Vector2(i * 12, j * 12 - 20) * scale;
					DrawSmallCircle(patternPos, 1.5f * scale, _lineColor);
				}
			}
		}
	}

	private void DrawWayangKulitFace(Vector2 position, float scale)
	{
		// Draw sharp, angled eyes typical of refined wayang kulit characters
		DrawWayangKulitEye(position + new Vector2(-9, -5) * scale, scale, false);
		DrawWayangKulitEye(position + new Vector2(9, -5) * scale, scale, true);
		
		// Draw nose - more prominent and stylized
		Vector2 noseTop = position + new Vector2(0, 0) * scale;
		Vector2 noseBottom = position + new Vector2(0, 10) * scale;
		Vector2 noseRight = position + new Vector2(4, 8) * scale;
		
		DrawLine(noseTop, noseBottom, _lineColor, 1.2f);
		DrawLine(noseBottom, noseRight, _lineColor, 1.2f);
		
		// Draw mouth - typical stylized smile
		Vector2 mouthLeft = position + new Vector2(-8, 18) * scale;
		Vector2 mouthRight = position + new Vector2(8, 18) * scale;
		Vector2 mouthCenter = position + new Vector2(0, 20) * scale;
		
		List<Vector2> mouthCurve = _primitif.QuadraticBezier(mouthLeft, mouthCenter, mouthRight);
		foreach (var point in mouthCurve)
		{
			DrawRect(new Rect2(point, new Vector2(1.0f, 1.0f)), _lineColor);
		}
	}

	private void DrawWayangKulitEye(Vector2 position, float scale, bool isRight)
	{
		// Draw almond-shaped eye typical of wayang kulit - more angled/sharp
		float eyeWidth = 8 * scale;
		float eyeHeight = 5 * scale;
		
		Vector2 eyeLeft = position + new Vector2(-eyeWidth/2, 0);
		Vector2 eyeTop = position + new Vector2(0, -eyeHeight/2);
		Vector2 eyeRight = position + new Vector2(eyeWidth/2, 0);
		Vector2 eyeBottom = position + new Vector2(0, eyeHeight/2);
		
		// Draw eye outline with bezier curves - sharper curves for traditional look
		List<Vector2> topCurve = _primitif.QuadraticBezier(
			eyeLeft, 
			position + new Vector2(0, -eyeHeight * 0.7f), // More pointed top curve
			eyeRight
		);
		
		List<Vector2> bottomCurve = _primitif.QuadraticBezier(
			eyeRight, 
			position + new Vector2(0, eyeHeight * 0.4f), // Flatter bottom curve
			eyeLeft
		);
		
		foreach (var point in topCurve.Concat(bottomCurve))
		{
			DrawRect(new Rect2(point, new Vector2(1.0f, 1.0f)), _lineColor);
		}
		
		// Draw pupil - slightly offset
		DrawSmallCircle(position + new Vector2((isRight ? 1.5f : -1.5f) * scale, 0), 1.5f * scale, _lineColor);
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

	private void DrawCircularPattern(Vector2 center, float radius, Color color)
	{
		// Draw outer circle
		DrawSmallCircle(center, radius, color);
		
		// Draw inner circle
		DrawSmallCircle(center, radius * 0.7f, color);
		
		// Draw decorative rays
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
	}

	private void DrawTiltedKendang(float centerX, float centerY)
	{
		// Increase rotation angle for more curve (from -30 to -40 degrees)
		float angle = -40f * Mathf.Pi / 180;
		float cos = Mathf.Cos(angle);
		float sin = Mathf.Sin(angle);
		
		// Kendang dimensions
		float length = 270; // Slightly longer to compensate for increased angle
		float bigEndRadius = 80;  // Keep main drum head size
		float smallEndRadius = 50; // Reduced from 65 to 50 for smaller back circle
		
		// Calculate drum end centers after rotation
		Vector2 bigEnd = new Vector2(
			centerX - length/2 * cos,
			centerY - length/2 * sin
		);
		
		Vector2 smallEnd = new Vector2(
			centerX + length/2 * cos,
			centerY + length/2 * sin
		);
		
		// Store end centers and radiuses for the rope pattern
		_headLeftCenter = bigEnd;
		_headRightCenter = smallEnd;
		_headLeftRadius = bigEndRadius;
		_headRightRadius = smallEndRadius;
		
		// Draw only outlines of the drum heads (no fill)
		DrawDrumHead(bigEnd, bigEndRadius, _lineColor); 
		DrawDrumEnd(smallEnd, smallEndRadius, _lineColor);
		
		// Draw the outline of the drum - increasing thickness to 2
		DrawDrumOutline(bigEnd, smallEnd, bigEndRadius, smallEndRadius, _lineColor);
		
		// Draw the string pattern that doesn't penetrate the drum heads
		DrawStringPattern(bigEnd, smallEnd, bigEndRadius, smallEndRadius, _lineColor);
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
	
	List<Vector2> flowerCenters = new List<Vector2>();

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
			flowerCenters.Add(center);
			DrawTruntumFlower(center, flowerSize, color);
		}
	}
	
	// Draw hexagonal connections
	DrawHexagonalConnections(flowerCenters, color, flowerSpacing);
	
	// Add dots in hexagonal centers
	DrawHexagonalDots(flowerCenters, startX, startY, width, height, flowerSpacing, rowSpacing, color);
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
				// Draw dot with subtle shadow
				DrawCircle(dotPos.X + 1, dotPos.Y + 1, dotSize, color * new Color(1,1,1,0.3f), 0.8f);
				DrawCircle(dotPos.X, dotPos.Y, dotSize, color, 0.8f);
			}
		}
	}
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
		DrawTruntumFlower(new Vector2(x, y), cornerSize, color);
		DrawTruntumFlower(new Vector2(x + width, y), cornerSize, color);
		DrawTruntumFlower(new Vector2(x, y + height), cornerSize, color);
		DrawTruntumFlower(new Vector2(x + width, y + height), cornerSize, color);

		// Add decorative stars along the border at regular intervals
		int borderStarCount = 10;
		float borderStarSpacing = width / borderStarCount;
		
		for (int i = 1; i < borderStarCount; i++)
		{
			// Top border stars
			DrawTruntumFlower(new Vector2(x + i * borderStarSpacing, y), cornerSize * 0.7f, color);
			
			// Bottom border stars
			DrawTruntumFlower(new Vector2(x + i * borderStarSpacing, y + height), cornerSize * 0.7f, color);
		}
		
		// Side border stars
		int sideBorderStarCount = (int)(height / borderStarSpacing);
		for (int i = 1; i < sideBorderStarCount; i++)
		{
			// Left side stars
			DrawTruntumFlower(new Vector2(x, y + i * borderStarSpacing), cornerSize * 0.7f, color);
			
			// Right side stars
			DrawTruntumFlower(new Vector2(x + width, y + i * borderStarSpacing), cornerSize * 0.7f, color);
		}
	}

private void DrawKeris(Vector2 basePosition, float scale, float rotationDegrees)
{
	// Shift the entire keris slightly to the right to avoid the wayang
	Vector2 origin = basePosition + new Vector2(40, 0);
	float rotation = Mathf.DegToRad(rotationDegrees);
	
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
	DrawBlade(origin, scale, rotation);
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
	
	// Titik-titik yang dibuat lebih tipis dan mengikuti alur keris (luk)
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
		new Vector2(-7, 20)       // Tutup kurva
	};

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
}
