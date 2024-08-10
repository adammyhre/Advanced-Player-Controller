using System;

namespace UnityUtils.StateMachine 
{
    public abstract class Transition {
        public IState To { get; protected set; }
        public abstract bool Evaluate();
    }

    public class Transition<T> : Transition {
        public readonly T condition;

        public Transition(IState to, T condition) {
            To = to;
            this.condition = condition;
        }

        public override bool Evaluate() {
            // Check if the condition variable is a Func<bool> and call the Invoke method if it is not null
            var result = (condition as Func<bool>)?.Invoke();
            if (result.HasValue) {
                return result.Value;
            }
            
            // Check if the condition variable is an ActionPredicate and call the Evaluate method if it is not null
            result = (condition as ActionPredicate)?.Evaluate();
            if (result.HasValue) {
                return result.Value;
            }

            // Check if the condition variable is an IPredicate and call the Evaluate method if it is not null
            result = (condition as IPredicate)?.Evaluate();
            if (result.HasValue) {
                return result.Value;
            }

            // If the condition variable is not a Func<bool>, an ActionPredicate, or an IPredicate, return false
            return false;
        }
    }

    /// <summary>
    /// Represents a predicate that uses a Func delegate to evaluate a condition.
    /// </summary>
    public class FuncPredicate : IPredicate {
        readonly Func<bool> func;

        public FuncPredicate(Func<bool> func) {
            this.func = func;
        }

        public bool Evaluate() => func.Invoke();
    }

    /// <summary>
    /// Represents a predicate that encapsulates an action and evaluates to true once the action has been invoked.
    /// </summary>
    public class ActionPredicate : IPredicate {
        public bool flag;

        public ActionPredicate(ref Action eventReaction) => eventReaction += () => { flag = true; };

        public bool Evaluate() {
            bool result = flag;
            flag = false;
            return result;
        }
    }
}