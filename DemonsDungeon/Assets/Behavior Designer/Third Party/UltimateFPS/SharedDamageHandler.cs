using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedDamageHandler : SharedVariable
    {
        public vp_DamageHandler Value { get { return mValue; } set { mValue = value; } }
        [SerializeField]
        private vp_DamageHandler mValue;

        public override object GetValue() { return mValue; }
        public override void SetValue(object value) { mValue = (vp_DamageHandler)value; }

        public override string ToString() { return (mValue == null ? "null" : mValue.name); }
    }
}