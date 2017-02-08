using UnityEngine;
using System.Collections;

/// <summary>
/// 計算式を格納するクラス
/// </summary>
public static class _Math{

	/// <summary>
	/// ベジェ曲線の公式を用いて座標を計算する
	/// </summary>
	/// <returns>The curve.</returns>
	/// <param name="start">始点となる座標</param>
	/// <param name="point1">Point1.</param>
	/// <param name="point2">Point2.</param>
	/// <param name="end">End.</param>
	/// <param name="t">T.</param>
	public static float BezierCurve(float start,float point1,float point2,float end,float t)
	{
		// (x4-3*(x3+x2)-x1)*t^3 + 3*(x3-2*x2+x1)*t^2 + 3*(x2-x1)*t + x1
		float tmp =0f;
		//ベジェ曲線の式を用いて、座標を計算する
		tmp = (end - 3 * (point2 + point1) - start) * Mathf.Pow (t, 3);
		tmp += (3 * (point2 - 2 * point1 + start) * Mathf.Pow (t, 2));
		tmp += (3 * (point1 - start) * t + start);

		return tmp;
	}


}
