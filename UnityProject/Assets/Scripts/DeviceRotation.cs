using UnityEngine;
using System;
using System.Collections.Generic;

public partial class DeviceRotation : MonoBehaviour
{
	public Quaternion Current;
	public Quaternion Z;
	
	private GameObject mMouseMove;
	private Quaternion mDefaultSceneRotation = Quaternion.identity;
	private Quaternion mDefaultDeviceRotation = Quaternion.identity;
	
    void Awake ()
    {
#if UNITY_EDITOR		
	    mMouseMove = new GameObject("MouseObject");
#elif UNITY_IPHONE && !UNITY_EDITOR
		Input.gyro.updateInterval = 0.01f;
#elif UNITY_ANDROID && !UNITY_EDITOR
		SensorHelper.ActivateRotation();
#endif
	}
		
	void Update()
    {
		Current = mDefaultSceneRotation * localRotation();
		
		var newRotation = Current.eulerAngles;			
		newRotation.x = 0;
		newRotation.y = 0;
		newRotation.z = -newRotation.z;
		Z = Quaternion.Euler(newRotation);		
    }
	
	public void ResetDeviceRotation()
	{
		mDefaultDeviceRotation = deviceRotation();	
	}
	
	public void ResetSceneRotation()
	{
		mDefaultSceneRotation = Current;
	}

	public void ResetSceneRotation(Quaternion rotation)
	{
		mDefaultSceneRotation = rotation;
	}
	
	private Quaternion localRotation()
	{
		return Quaternion.Inverse(mDefaultDeviceRotation) * deviceRotation();
	}
	
	private Quaternion deviceRotation()
	{	
#if UNITY_EDITOR 
		float speed = 4.0f;
		float xDeg = Input.GetAxis("Mouse X") * speed;
		mMouseMove.transform.Rotate(0.0f,0.0f,xDeg);
		return mMouseMove.transform.rotation;
#elif UNITY_IPHONE && !UNITY_EDITOR		
		Quaternion localRotation = Input.gyro.attitude;
		localRotation.z *= -1.0f;
		return Quaternion.Inverse(localRotation);
#elif UNITY_ANDROID && !UNITY_EDITOR
		Quaternion localRotation = SensorHelper.rotation;
		localRotation.z *= -1.0f;
		return Quaternion.Inverse(localRotation);
#endif
	}
}