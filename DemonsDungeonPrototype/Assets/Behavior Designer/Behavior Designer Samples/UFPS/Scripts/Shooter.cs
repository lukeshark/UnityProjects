using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;

namespace BehaviorDesigner.Samples
{
    public class Shooter : MonoBehaviour
    {
        public GameObject target;
        public float shootInterval;
        public Vector3 shootForce;
        public Behavior behaviorTree;

        private SharedDamageHandler damageHander;
        private WaitForSeconds waitTime;

        public void Start()
        {
            waitTime = new WaitForSeconds(shootInterval);
            damageHander = behaviorTree.GetVariable("Target") as SharedDamageHandler;

            StartCoroutine(Shoot());
        }

        public IEnumerator Shoot()
        {
            yield return new WaitForSeconds(2);

            while (true) {
                var pidgen = GameObject.Instantiate(target, transform.position, transform.rotation) as GameObject;
                pidgen.GetComponent<Rigidbody>().AddForce(shootForce, ForceMode.VelocityChange);

                damageHander.Value = pidgen.GetComponent<vp_DamageHandler>();

                yield return waitTime;
            }
        }
    }
}