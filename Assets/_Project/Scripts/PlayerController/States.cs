using UnityEngine;
using UnityUtils.StateMachine;

namespace AdvancedController {
    public class GroundedState : IState {
        readonly PlayerController controller;

        public GroundedState(PlayerController controller) {
            this.controller = controller;
        }

        public void OnEnter() {
            controller.OnGroundContactRegained();
        }
    }

    public class FallingState : IState {
        readonly PlayerController controller;

        public FallingState(PlayerController controller) {
            this.controller = controller;
        }

        public void OnEnter() {
            controller.OnFallStart();
        }
    }

    public class SlidingState : IState {
        readonly PlayerController controller;

        public SlidingState(PlayerController controller) {
            this.controller = controller;
        }

        public void OnEnter() {
            controller.OnGroundContactLost();
        }
    }

    public class RisingState : IState {
        readonly PlayerController controller;

        public RisingState(PlayerController controller) {
            this.controller = controller;
        }

        public void OnEnter() {
            controller.OnGroundContactLost();
        }
    }

    public class JumpingState : IState {
        readonly PlayerController controller;

        public JumpingState(PlayerController controller) {
            this.controller = controller;
        }

        public void OnEnter() {
            controller.OnGroundContactLost();
            controller.OnJumpStart();
        }
    }
}