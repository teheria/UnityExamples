using UnityEngine;
using System.Collections;

/********************************************************************
 * 
 * Script - QuadBezierCurve
 * 
 * Example of a Quadradic Bezier Curve
 * 
 * Use - attach to a GameObject that you want to move on the curve
 * 
 * A - first control point
 * B - second control point
 * C - third control point
 * rate - time to move from A to C or C to A
 * 
 * Author - Jason Roth
 * 
 *******************************************************************/

public class QuadBezierCurve : MonoBehaviour {
	
	public Transform		A, B, C;
	public float			rate;
	
	private Transform		_transform;
	
	void Awake()
	{
		_transform = transform;
	}
	
	void Update()
	{
		float t = Mathf.PingPong(Time.time / rate, 1.0f);
		_transform.position = GetCurve(t);
	}
	
	private Vector3 GetCurve(float t)
	{
		// result vector for curve
		Vector3 pt;
		float s = 1 - t;
		
		pt = (s * s * A.position) + (2 * (s * t * B.position)) + (t * t * C.position);
		
		return pt;
	}
}
