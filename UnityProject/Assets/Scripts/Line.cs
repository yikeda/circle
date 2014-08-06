using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour {

	public class LinePosition {
		float rotation;
		float distance;

		public LinePosition(float rotation, float distance)
		{
			this.Rotation = rotation;
			this.Distance = distance;			
		}
		
		public float Rotation
		{
			get { return rotation; }
			set { rotation = value; }
		}

		public float Distance
		{
			get { return distance; }
			set { distance = value; }
		}
	}

	[SerializeField] int size = 100;
	[SerializeField] float distanceOffset = 0.1f;
	[SerializeField] int rotationRange = 10;
	
	
	private List<LinePosition> positions;
	private LineRenderer lineRenderer;
	
	void Start () {
		lineRenderer = this.GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(size);
		
		positions = new List<LinePosition>();

		for(int i = 0; i < size; i++)
		{
			float rotation = rotationRange * Mathf.Deg2Rad * i;
			float distance = i * distanceOffset;
			positions.Add(new LinePosition(rotation,distance));			
		}

	}
	void Update () {
		int index = 0;
		foreach(LinePosition linePosiiton in positions)
		{			
			float x = Mathf.Cos(linePosiiton.Rotation) * linePosiiton.Distance;
			float z = Mathf.Sin(linePosiiton.Rotation) * linePosiiton.Distance;
						
			Vector3 position = new Vector3(x,0,z);
			lineRenderer.SetPosition(index, position);
			index++;

			if (distanceOffset * size < linePosiiton.Distance)				
			{
				linePosiiton.Distance = 0;
			}
			else
			{
				linePosiiton.Distance += 0.1f;				
			}
		}
	}			
}
