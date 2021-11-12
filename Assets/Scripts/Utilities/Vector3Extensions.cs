using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
	public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
	{
		return new Vector3(Mathf.Max(min.x, Mathf.Min(max.x, vector.x)), 
						   Mathf.Max(min.y, Mathf.Min(max.y, vector.y)),
						   Mathf.Max(min.z, Mathf.Min(max.z, vector.z)));
	}
	public static Vector3 Clamp(this Vector3 vector, Vector2 min, Vector2 max)
	{
		return new Vector3(Mathf.Max(min.x, Mathf.Min(max.x, vector.x)),
						   Mathf.Max(min.y, Mathf.Min(max.y, vector.y)));
	}
}

