using UnityEngine;
using System.Collections;

/********************************************************************
 * 
 * Extension Methods for the Unity Transform class.
 * 
 * Author - Jason Roth
 * 
 *******************************************************************/
public static class TransformExtensions {
	
	/*
	 * Interplotes the position of a transform.
	 *
	 * target - final position the transform will move to
	 * rate - how fast it interplotes to the target position.
	 * 
	 */
	public static IEnumerator EasyLerpPosition(this Transform tr, Vector3 target, float rate) {
		
		float t = 0.0f;
		
		Vector3 temp = tr.position;
		
		while (t < 1.0f) {
			tr.position = Vector3.Lerp(temp, target, t);
			t += Time.deltaTime / rate;
			yield return null;
		}
		
		tr.position = target;
	}
}
