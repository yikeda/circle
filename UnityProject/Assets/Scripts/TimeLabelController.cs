using UnityEngine;

public class TimeLabelController : MonoBehaviour
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
		newRotation.y = 0;
		newRotation.z = -newRotation.z;
		transform.rotation = Quaternion.Euler(newRotation);
	}	
}
