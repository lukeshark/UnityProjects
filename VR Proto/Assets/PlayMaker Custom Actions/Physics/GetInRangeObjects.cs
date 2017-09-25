// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Physics)]
    [Tooltip("Robs new function")]
    public class GetInRangeObjects : FsmStateAction
    {
        [RequiredField]
        public FsmFloat time;
        public FsmEvent finishEvent;
        public bool realTime;
        public string tag = "Grabbable";

        public float timeFieldScale;
        private float timer;

        public GameObject timeSphere;
        private Collider[] inRangeObjs;
        private List<GameObject> pushObjs;

        public override void OnEnter()
        {

//            if (time.Value <= 0)
//            {
//                Fsm.Event(finishEvent);
//                Finish();
//                return;
//            }

//            startTime = FsmTime.RealtimeSinceStartup;
//            timer = 0f;
        }

        public override void OnUpdate()
        {
            // update time
            
            inRangeObjs = Physics.OverlapSphere(timeSphere.transform.position, timeFieldScale);
            pushObjs = new List<GameObject>();

            foreach (Collider i in inRangeObjs)
            {
                if (i.gameObject.tag == tag)
                {
                    Debug.Log("pull object added");
                    Debug.Log(i.gameObject);
                    pushObjs.Add(i.gameObject);
                }
            }
            
            foreach (GameObject i in pushObjs)
            {
                Vector3 direction = i.transform.position - timeSphere.transform.position;
                i.GetComponent<Rigidbody>().AddForce(512 * direction.normalized);
            }           
        }
    }
}
