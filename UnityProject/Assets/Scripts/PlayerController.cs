using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private DeviceRotation deviceRotation;
				
	void Awake ()		
    {
		this.deviceRotation = (DeviceRotation)gameObject.AddComponent("DeviceRotation");
		//this.deviceRotation.ResetDeviceRotation();
    }

    void Update()		
    {
		Quaternion rotation = Quaternion.Inverse(this.deviceRotation.Z);	
		var newRotation = rotation.eulerAngles;			
		newRotation.x = 0;
		newRotation.y = -newRotation.z;
		newRotation.z = 0;
		transform.rotation = Quaternion.Euler(newRotation);
	}	
}
