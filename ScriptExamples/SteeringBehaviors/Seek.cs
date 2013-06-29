using UnityEngine;
using System.Collections;

/********************************************************************
 * 
 * Script - Seek
 * 
 * Simple example of a seek behavior.
 * 
 * Use - attach to a GameObject, give it a target.
 * 
 * target - the target the agent will try to seek
 * maxForce - max steering force applied to the agent
 * maxSpeed - max speed the agent will move
 * 
 * Author - Jason Roth
 * 
 *******************************************************************/

public class Seek : MonoBehaviour {
	
	public Transform	target;
	public float 		maxForce;
	public float 		maxSpeed;
	
	private Transform 	_transform;
	private Vector3 	_velocity;
	private Vector3		_desiredVelocity;
	private Vector3 	_acceleration;
	
	void Awake() {
		_transform = transform;
		_velocity = Vector3.zero;
		_desiredVelocity = Vector3.zero;
		_acceleration = Vector3.zero;
	}
	
	void Update() {
		
		// find desired velocity to calulate steering force
		_desiredVelocity = Vector3.Normalize(target.position - _transform.position) * maxSpeed;
		
		// calculate steering
		Vector3 steeringForce = Vector3.ClampMagnitude(_desiredVelocity - _velocity, maxForce);
		ApplyForce(steeringForce);
		
		// calculate final velocity.
		_velocity = Vector3.ClampMagnitude(_velocity + steeringForce, maxSpeed);
		
		// apply velocity
		_transform.position += _velocity * Time.deltaTime;
		
		// apply rotation to look at target
		_transform.rotation = Quaternion.Slerp(_transform.rotation,
			Quaternion.LookRotation(_desiredVelocity), 5.0f * Time.deltaTime);
		
		Debug.DrawRay(_transform.position, _transform.forward, Color.magenta);
		Debug.Log(_velocity);
	}
	
	private void ApplyForce(Vector3 force) {
		_acceleration += force;
	}
}
