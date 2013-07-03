using UnityEngine;
using System.Collections;

/********************************************************************
 * 
 * Script - FadeManager
 * 
 * This is a manager class that will fade in and out screen based on
 * the GUI alpha.
 * 
 * Use - to use this manager, attach it to an empty game object in the
 * scene. Call it through its singleton instance proptery.
 * 
 * fadeColor - the color you wish to fade to
 * 
 * Author - Jason Roth
 * 
 *******************************************************************/

public class FadeManager : Singleton<FadeManager> {
	
	public Color		fadeColor;
	
	private Texture2D	_texture;
	private Color		_alpha;
	private bool		_isFading = false;
	
	public bool IsFading {
		get { return _isFading; }
	}
	
	void Awake() {
		_texture = new Texture2D(1, 1);
		_texture.SetPixel(0, 0, fadeColor);
		_texture.Apply();
		_alpha = fadeColor;
	}
	
	void Start() {
		FadeIn(5.0f);
	}
	
	void OnGUI() {
		if (_isFading) {
			GUI.depth = -1000;
			GUI.color = _alpha;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);
		}
	}
	
	public void FadeIn(float rate) {
		_isFading = true;
		StartCoroutine(DoFadeIn(rate, 1.0f, 0.0f));
	}
	
	public void FadeIn(float rate, Color c) {
		_isFading = true;
		StartCoroutine(DoFadeIn(rate, 1.0f, 0.0f));
		_texture.SetPixel(0, 0, c);
		_texture.Apply();
	}
	
	public void FadeOut(float rate) {
		_isFading = true;
		StartCoroutine(DoFadeIn(rate, 0.0f, 1.0f));
	}
	
	public void FadeOut(float rate, Color c) {
		_isFading = true;
		StartCoroutine(DoFadeIn(rate, 0.0f, 1.0f));
		_texture.SetPixel(0, 0, c);
		_texture.Apply();
	}
	
	private IEnumerator DoFadeIn(float rate, float start, float end) {
		float t = 0.0f;
		
		while (t < 1.0f) {
			_alpha.a = Mathf.Lerp(start, end, t);
			t += Time.deltaTime / rate;
			yield return null;
		}
		
		_alpha.a = end;
		_isFading = false;
	}
}
