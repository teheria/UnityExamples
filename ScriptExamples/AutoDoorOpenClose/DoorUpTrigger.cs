using UnityEngine;
using System.Collections;

/********************************************************************
 * 
 * Script - DoorUpTrigger
 * 
 * This will make a door open and close when the player gets close.
 * This should be used for doors that move up and down like in a
 * space ship for example.
 * 
 * Use - to use this script, attach it to a empty box collider marked
 * as a trigger. Drag in the door and the target position transforms.
 * TargetPosition should be an empty GameObject, and should not be a
 * child of the door. The doors original position and targetPosition
 * will do a swap so that the next time the coroutine is called it
 * will either open or close it correctly.
 * 
 * door - the door to open and close.
 * targetPosition - the final position the door moves to
 * rate - how fast the door will open and close
 * 
 * Author - Jason Roth
 * 
 *******************************************************************/

public class DoorUpTrigger : MonoBehaviour {
	
	public Transform	door;
	public Transform 	targetPosition;
	public float		rate;
	
	private bool _isRunning = false;
	
	void Awake() {
		if (collider) {
			collider.isTrigger = true;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			StartCoroutine(OpenCloseDoor());
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			StartCoroutine(OpenCloseDoor());
		}
	}
	
	private IEnumerator OpenCloseDoor() {
		
		while (_isRunning) {
			yield return null;
		}
		
		_isRunning = true;
		
		// cache current position
		Vector3 temp = door.position;
		
		// requires that you use the Transform extension class found in
		// the method extensions folder.
		yield return StartCoroutine(door.EasyLerpPosition(
			targetPosition.position, rate));
		
		// set target to old position
		targetPosition.position = temp;
		
		_isRunning = false;
	}
}
