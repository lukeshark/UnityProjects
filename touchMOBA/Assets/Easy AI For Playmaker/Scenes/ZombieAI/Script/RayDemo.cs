using UnityEngine;
using System.Collections;

public class RayDemo : MonoBehaviour
{

	public void OnDrawGizmos ()
	{

		Gizmos.DrawRay (transform.position, transform.forward * 100);

	}
}
