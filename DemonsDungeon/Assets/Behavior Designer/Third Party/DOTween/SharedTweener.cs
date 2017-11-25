using UnityEngine;
using Tweener = DG.Tweening.Tweener;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [System.Serializable]
    public class SharedTweener : SharedVariable
    {
        public Tweener Value { get { return mValue; } set { mValue = value; } }
        [SerializeField]
        private Tweener mValue;

        public override object GetValue() { return mValue; }
        public override void SetValue(object value) { mValue = (Tweener)value; }

        public override string ToString() { return mValue == null ? "null" : mValue.ToString(); }
        public static implicit operator SharedTweener(Tweener value) { var sharedVariable = new SharedTweener(); sharedVariable.SetValue(value); return sharedVariable; }
    }
}