using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse and Joystick Look")]
public class SmoothMouseAndJoystickLook : MonoBehaviour
{
	public enum RotationAxes { JoyXAndY = 3, JoyX = 4, JoyY = 5 }
    public RotationAxes axes = RotationAxes.JoyXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F; 

    float xInput = 0F;
    float yInput = 0F;
    float xInputOld = 0F;
    float yInputOld = 0F;
    float averageXInput=0F;
    float averageYInput=0F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;
	private Transform _transform;
	private bool _isStartDone = false;
	
	public float XInput
	{
		set { xInput = value; }
	}
	
	public float YInput
	{
		set { yInput = value; }
	}
	
	public float RotationX
	{
		get { return rotationY; }
		set { rotationX = value; }
	}
	
	public float RotationY
	{
		get { return rotationX; }
		set { rotationY = value; }
	}
	
	void Awake()
	{
		_transform = transform;
		
		if (ES2.Exists(LensValues.SavePath + "?tag=mouseSen"))
		{
			float temp = ES2.Load<float>(LensValues.SavePath + "?tag=mouseSen");
			sensitivityX = sensitivityY = temp;
			return;
		}
		
		sensitivityX = sensitivityY = LensValues.MouseSensitivity;
	}

    void FixedUpdate()
    {
		if (!_isStartDone) return;
		
	   float Xon = Mathf.Abs (Input.GetAxis ("JoyX"));
       float Yon = Mathf.Abs (Input.GetAxis ("JoyY"));
		
        if (axes == RotationAxes.JoyXAndY)
        {
            // Pass inputs to inputOlds first...
            xInputOld=xInput;
            yInputOld=yInput;        
						
			if (Xon >= 0.02 && Yon >= 0.02)
			{
	            // Read the Joy input axis
	            xInput = sensitivityX * Mathf.Pow(Input.GetAxis("JoyX"),3) *100;
	            yInput = sensitivityY * Mathf.Pow(-Input.GetAxis("JoyY"),3) *100;
			}
			else
			{
				// Read the mouse input axis
	            xInput = Input.GetAxis("Mouse X") * sensitivityX;
	            yInput = Input.GetAxis("Mouse Y") * sensitivityY;
			}
			
			//Average new inputs with old
            averageXInput = xInput + xInputOld;
            averageYInput = yInput + yInputOld;
            averageXInput *= 0.25f;
            averageYInput *= 0.25f;
            rotationX += averageXInput;
            rotationY += averageYInput;
            rotationX = ClampAngle (rotationX, minimumX, maximumX);
            rotationY = ClampAngle (rotationY, minimumY, maximumY);            

            Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
            _transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxes.JoyX)
        {
            // Pass inputs to inputOlds first...
            xInputOld=xInput;        
			
		
            if (Xon >= 0.02)
			// Read the Joy input axis
            xInput = Mathf.Pow(Input.GetAxis("JoyX"),3) * sensitivityX * 1000;            
			else 
			// Read the mouse input axis
            xInput = Input.GetAxis("Mouse X") * sensitivityX;    
			
            //Average new inputs with old
            averageXInput = xInput + xInputOld;
            averageXInput *= 0.25f;
            rotationX += averageXInput;
            rotationX = ClampAngle (rotationX, minimumX, maximumX); 

            Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
            _transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            // Pass inputs to inputOlds first...
            yInputOld=yInput; 
			
			if (Yon >= 0.02)
            // Read the Joy input axis
            yInput = sensitivityY * Mathf.Pow(-Input.GetAxis("JoyY"),3) *500;
			else			// Read the mouse input axis
            yInput = Input.GetAxis("Mouse Y") * sensitivityY;

            //Average new inputs with old
            averageYInput = yInput + yInputOld;
            averageYInput *= 0.25f;
            rotationY += averageYInput;
            rotationY += yInput;
            rotationY = ClampAngle (rotationY, minimumY, maximumY);

            Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
            _transform.localRotation = originalRotation * yQuaternion;
        }
    }    

    IEnumerator Start()
    {
        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
		yield return new WaitForSeconds(0.1f);
        originalRotation = _transform.localRotation;
		_isStartDone = true;
    }    

    public static float ClampAngle (float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;

        return Mathf.Clamp (angle, min, max);
    }
	
	public void SetMouseSensitity(float sensitity)
	{
		sensitivityX = sensitivityY = sensitity;
	}
}
