using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour
{
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
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
	}

    void FixedUpdate()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            // Pass inputs to inputOlds first...
            xInputOld=xInput;
            yInputOld=yInput;        

            // Read the mouse input axis
            xInput = Input.GetAxis("Mouse X") * sensitivityX;
            yInput = Input.GetAxis("Mouse Y") * sensitivityY;
			
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
        else if (axes == RotationAxes.MouseX)
        {
            // Pass inputs to inputOlds first...
            xInputOld=xInput;        

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

            // Read the mouse input axis
            yInput = Input.GetAxis("Mouse Y") * sensitivityY;

            //Average new inputs with old
            averageYInput = yInput + yInputOld;
            averageYInput *= 0.25f;
            rotationY += averageYInput;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = ClampAngle (rotationY, minimumY, maximumY);

            Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
            _transform.localRotation = originalRotation * yQuaternion;
        }
    }    

    void Start()
    {
        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
        originalRotation = _transform.localRotation;
    }    

    public static float ClampAngle (float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;

        return Mathf.Clamp (angle, min, max);
    }
}
