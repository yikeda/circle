using UnityEngine;

public class CameraController : MonoBehaviour
{
	private DeviceRotation deviceRotation;
				
	void Awake ()		
    {
		this.deviceRotation = (DeviceRotation)gameObject.AddComponent("DeviceRotation");
		this.deviceRotation.ResetDeviceRotation();
		Application.targetFrameRate = 60;
		Input.gyro.updateInterval = 0.01f;
    }

    void Update()		
    {
		Quaternion rotation = Quaternion.Inverse(this.deviceRotation.Z);	
		var newRotation = rotation.eulerAngles;			
		newRotation.x = 90;
		newRotation.y = -newRotation.z;
		newRotation.z = 0;
		transform.rotation = Quaternion.Euler(newRotation);
	}	
}
