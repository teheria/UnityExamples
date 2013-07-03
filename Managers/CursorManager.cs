using UnityEngine;
using System.Collections;

/********************************************************************
 * 
 * Script - CursorManager
 * 
 * This is a manager class that manages the screen cursor.
 * 
 * Use - To use this manager, attach it to an empty game object in the
 * scene. Call it through its singleton instance proptery.
 * 
 * Note - if you'd like to change the key bind, simply change the key
 * code value in update. The key bind will only work in editor mode.
 * 
 * Author - Jason Roth
 * 
 *******************************************************************/

public class CursorManager : Singleton<CursorManager> {
	
	void Start() {
		Screen.lockCursor = true;
	}
	
#if UNITY_EDITOR
	void Update() {
		
		if (Input.GetKeyDown(KeyCode.R)) {
			Screen.lockCursor = !Screen.lockCursor;
		}
	}
#endif
	
	public void LockCursor() {
		Screen.lockCursor = true;
	}
	
	public void UnlockCursor() {
		Screen.lockCursor = false;
	}
}
