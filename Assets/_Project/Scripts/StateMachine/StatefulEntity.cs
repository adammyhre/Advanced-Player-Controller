using UnityEngine;

namespace UnityUtils.StateMachine {
    public abstract class StatefulEntity : MonoBehaviour {
        protected StateMachine stateMachine;

        /// <summary>
        /// Awake or Start can be used to declare all states and transitions.
        /// </summary>
        /// <example>
        /// <code>
        /// protected override void Awake() {
        ///     base.Awake();
        /// 
        ///     var state = new State1(this);
        ///     var anotherState = new State2(this);
        ///
        ///     At(state, anotherState, () => true);
        ///     At(state, anotherState, myFunc);
        ///     At(state, anotherState, myPredicate);
        /// 
        ///     Any(anotherState, () => true);
        ///
        ///     stateMachine.SetState(state);
        /// </code> 
        /// </example>
        protected virtual void Awake() {
            stateMachine = new StateMachine();
        }

        protected virtual void Update() => stateMachine.Update();
        protected virtual void FixedUpdate() => stateMachine.FixedUpdate();

        protected void At<T>(IState from, IState to, T condition) => stateMachine.AddTransition(from, to, condition);

        protected void Any<T>(IState to, T condition) => stateMachine.AddAnyTransition(to, condition);
    }
}