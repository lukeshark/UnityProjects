using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = System.Object;

namespace AxlPlay
{
    public class StateMachineRunner : MonoBehaviour
    {
        private List<IStateMachine> stateMachineList = new List<IStateMachine>();

        /// <summary>
        /// Creates a stateMachine token object which is used to managed to the state of a monobehaviour. 
        /// </summary>
        /// <typeparam name="T">An Enum listing different state transitions</typeparam>
        /// <param name="component">The component whose state will be managed</param>
        /// <returns></returns>
        public StateMachine<T> Initialize<T>(MonoBehaviour component) where T : struct, IConvertible, IComparable
        {
            var fsm = new StateMachine<T>(this, component);

            stateMachineList.Add(fsm);

            return fsm;
        }

        /// <summary>
        /// Creates a stateMachine token object which is used to managed to the state of a monobehaviour. Will automatically transition the startState
        /// </summary>
        /// <typeparam name="T">An Enum listing different state transitions</typeparam>
        /// <param name="component">The component whose state will be managed</param>
        /// <param name="startState">The default start state</param>
        /// <returns></returns>
        public StateMachine<T> Initialize<T>(MonoBehaviour component, T startState) where T : struct, IConvertible, IComparable
        {
            var fsm = Initialize<T>(component);

            fsm.ChangeState(startState);

            return fsm;
        }

        void FixedUpdate()
        {

            for (int i = 0; i < stateMachineList.Count; i++)
            {
                var fsm = stateMachineList[i];
                if (!fsm.IsInTransition && fsm.Component.enabled)
                {
                    fsm.CurrentStateMap.FixedUpdate();
                }
            }
        }

        void Update()
        {
            for (int i = 0; i < stateMachineList.Count; i++)
            {

                var fsm = stateMachineList[i];
                if (!fsm.IsInTransition && fsm.Component.enabled)
                {
                    fsm.CurrentStateMap.Update();
                }
            }
        }

        void LateUpdate()
        {
            for (int i = 0; i < stateMachineList.Count; i++)
            {
                var fsm = stateMachineList[i];

                if (!fsm.IsInTransition && fsm.Component.enabled)
                {
                    fsm.CurrentStateMap.LateUpdate();
                }
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            //if(currentState != null && !IsInTransition)
            //{
            //	currentState.OnCollisionEnter(collision);
            //}
            for (int i = 0; i < stateMachineList.Count; i++)
            {
                var fsm = stateMachineList[i];
                if (!fsm.IsInTransition && fsm.Component.enabled)
                {
                    fsm.CurrentStateMap.OnCollisionEnter(collision);
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {

            for (int i = 0; i < stateMachineList.Count; i++)
            {
                var fsm = stateMachineList[i];
                if (!fsm.IsInTransition && fsm.Component.enabled)
                {
                    fsm.CurrentStateMap.OnTriggerEnter(other);
                }
            }

        }
        void OnTriggerStay(Collider other)
        {

            for (int i = 0; i < stateMachineList.Count; i++)
            {
                var fsm = stateMachineList[i];
                if (!fsm.IsInTransition && fsm.Component.enabled)
                {
                    fsm.CurrentStateMap.OnTriggerStay(other);
                }
            }

        }
        void OnTriggerExit(Collider other)
        {

            for (int i = 0; i < stateMachineList.Count; i++)
            {
                var fsm = stateMachineList[i];
                if (!fsm.IsInTransition && fsm.Component.enabled)
                {
                    fsm.CurrentStateMap.OnTriggerExit(other);
                }
            }

        }
        public static void DoNothing()
        {
        }

        public static void DoNothingCollider(Collider other)
        {
        }

        public static void DoNothingCollision(Collision other)
        {
        }

        public static void DoNothingTrigger(Collider other)
        {
        }

        public static IEnumerator DoNothingCoroutine()
        {
            yield break;
        }
    }


    public class StateMapping
    {
        public object state;

        public bool hasEnterRoutine;
        public Action EnterCall = StateMachineRunner.DoNothing;
        public Func<IEnumerator> EnterRoutine = StateMachineRunner.DoNothingCoroutine;

        public bool hasExitRoutine;
        public Action ExitCall = StateMachineRunner.DoNothing;
        public Func<IEnumerator> ExitRoutine = StateMachineRunner.DoNothingCoroutine;

        public Action Finally = StateMachineRunner.DoNothing;
        public Action Update = StateMachineRunner.DoNothing;
        public Action LateUpdate = StateMachineRunner.DoNothing;
        public Action FixedUpdate = StateMachineRunner.DoNothing;
        public Action<Collision> OnCollisionEnter = StateMachineRunner.DoNothingCollision;
        public Action<Collider> OnTriggerEnter = StateMachineRunner.DoNothingTrigger;
        public Action<Collider> OnTriggerStay = StateMachineRunner.DoNothingTrigger;
        public Action<Collider> OnTriggerExit = StateMachineRunner.DoNothingTrigger;
        public StateMapping(object state)
        {
            this.state = state;
        }

    }
}
