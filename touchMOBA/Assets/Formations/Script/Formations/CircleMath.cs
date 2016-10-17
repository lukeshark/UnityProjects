using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if PLAYMAKER
#endif
#if PLAYMAKER_1_8_OR_NEWER

#endif

public class CircleMath : MonoBehaviour
{
	#region Public Variables

	[Header ("Gizmo Color")]
	public Color pointColor = Color.red;
	public Color lineColor = Color.cyan;
    [Header("Circle Properties")]
    public bool isClosed = false;

	[Range (2, 100)]
	public int points = 10;
	[Range (0f, 360f)]
	public float maxAngle = 180;

    [Range(0.1f, 5f)]
    public float radius = 1f;

	[Header("Mover Properties")]
	[Range (0f, 360f)]
    public float moverAngle = 35f;
 
	[Range (0f, 360f)]
	public float initAngle = 180;
	#endregion

	#region Private Variables

	#endregion

	#region Main Methods

	void OnDrawGizmos ()
	{
		//initAngle = transform.localEulerAngles.y * -1;
		//moverAngle = maxAngle / 2;
 
		
		// draw axis lines 
		
		Vector3 yPos = transform.localPosition + (transform.forward * radius);
		
		//Vector3 yNegPos = transform.localPosition - (transform.forward * radius);
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine ( transform.localPosition,yPos);

		var leaderIndex = Mathf.RoundToInt(points / 2);
        
		// find the angles in radians and find the positions
		for (int i = 0; i <= points; i++) {

			Vector3 curPos = GetcirclePosition (i);

			 

			// draw the lines
			int nextPointID = i + 1;
			Vector3 nextPointPos ;
			if (i == points) {

                if (isClosed)
                {
                    nextPointPos = GetcirclePosition(0);
                    Gizmos.color = lineColor;
                    Gizmos.DrawLine(curPos, nextPointPos);
                }

            } else {
				nextPointPos = GetcirclePosition (nextPointID);
                Gizmos.color = lineColor;
                Gizmos.DrawLine(curPos, nextPointPos);
            }
	        
			// if i= leaderIndex cambiar color
			
			if (i== leaderIndex){
				Gizmos.color = Color.green;
				Gizmos.DrawSphere (curPos, 0.15f);
				
			}else{
				
				Gizmos.color = pointColor;
				Gizmos.DrawSphere (curPos, 0.1f);
			}

			

	   
		} // end for


		// draw triangle
		Gizmos.color = Color.cyan;
		
		Gizmos.DrawSphere(transform.localPosition, 0.1f);
		
		float moverRadians = ((transform.localEulerAngles.y * -1) + moverAngle) * Mathf.Deg2Rad;
		
		Vector3 moverPos = new Vector3 (Mathf.Cos(moverRadians), 1f, Mathf.Sin(moverRadians)) * radius + transform.localPosition ;
		
		Gizmos.color = Color.white;
		
		Gizmos.DrawSphere(moverPos, 0.3f);
		
		//Gizmos.DrawLine(transform.localPosition, moverPos );
		
		//Gizmos.DrawLine(transform.localPosition,  new Vector3( moverPos.x , transform.localPosition.y,  transform.localPosition.z)) ;
		//Gizmos.DrawLine(new Vector3( moverPos.x , transform.localPosition.y,  transform.localPosition.z),  moverPos) ;
		
		
	}

	#endregion

	#region  Utility Methodos

	Vector3 GetcirclePosition (int pointID )
	{

		float curAngle = ((float)pointID / (float)points) * (maxAngle) ;
		
		curAngle = curAngle + (transform.localEulerAngles.y * -1);
		float curRadians = (curAngle + moverAngle)   * Mathf.Deg2Rad;

		Vector3 curPos = new Vector3 (Mathf.Cos (curRadians), 1f, Mathf.Sin (curRadians)) * radius;
			
		return curPos + transform.localPosition;
	}

	#endregion


}
