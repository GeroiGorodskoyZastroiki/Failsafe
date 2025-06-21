using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Failsafe.PlayerMovements.Controllers;
using Failsafe.PlayerMovements.States;
using Failsafe.Scripts.Health;
using FMODUnity;
using TMPro;
using VContainer;
using Failsafe.Player.View;
using VContainer.Unity;
using Failsafe.Player.Model;


namespace Failsafe.PlayerMovements
{
    /// <summary>
    /// Движение персонажа
    /// </summary>
    public class PlayerController : IInitializable, ITickable, IFixedTickable
    {
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly PlayerNoiseParameters _noiseParametrs;
        private readonly SignalManager _signalManager;
        public readonly InputHandler InputHandler;
        private readonly PlayerView _playerView;
        private readonly IHealth _health;
        private readonly IStamina _stamina;
        private readonly PlayerStaminaController _playerStaminaController;
        private readonly PlayerMovementSpeedModifier _playerMovementSpeedModifier;
        private PlayerMovementController _movementController;
        private PlayerRotationController _playerRotationController;
        private PlayerBodyController _playerBodyController;
        private BehaviorStateMachine _behaviorStateMachine;
        private PlayerLedgeController _ledgeController;
        private PlayerGravityController _playerGravity;
        private PlayerNoiseController _noiseController;
        private StepController _stepController;

        public BehaviorStateMachine StateMachine => _behaviorStateMachine;
        public PlayerMovementController PlayerMovementController => _movementController;
        public PlayerGravityController PlayerGravityController => _playerGravity;

        public PlayerController(
            PlayerMovementParameters movementParametrs,
            PlayerNoiseParameters noiseParametrs,
            SignalManager signalManager,
            InputHandler inputHandler,
            PlayerView playerView,
            IHealth health,
            IStamina stamina,
            PlayerStaminaController playerStaminaController,
            PlayerMovementSpeedModifier playerMovementSpeedModifier)
        {
            _movementParametrs = movementParametrs;
            _noiseParametrs = noiseParametrs;
            _signalManager = signalManager;
            InputHandler = inputHandler;
            _playerView = playerView;
            _health = health;
            _stamina = stamina;
            _playerStaminaController = playerStaminaController;
            _playerMovementSpeedModifier = playerMovementSpeedModifier;
        }

        public void Initialize()
        {
            _movementController = new PlayerMovementController(_playerView.CharacterController);
            _playerRotationController = new PlayerRotationController(_playerView.PlayerTransform, _playerView.PlayerRigHead, InputHandler);
            _playerBodyController = new PlayerBodyController(_playerView.CharacterController);
            _ledgeController = new PlayerLedgeController(_playerView.PlayerTransform, _playerView.PlayerCamera, _playerView.PlayerGrabPoint, _movementParametrs);
            _playerGravity = new PlayerGravityController(_movementController, _playerView.CharacterController, _movementParametrs);
            _noiseController = new PlayerNoiseController(_playerView.PlayerTransform, _noiseParametrs, _signalManager);
            _stepController = new StepController(_playerView.CharacterController, _movementParametrs, _playerView.FootstepEvent);

            InitializeStateMachine();
        }



        private void InitializeStateMachine()
        {
            var deathState = new DeathState();
            var forcedStates = new List<BehaviorForcedState>
            {
                 deathState
            };
            _behaviorStateMachine = new BehaviorStateMachine(forcedStates);

            var standingState = new StandingState(InputHandler, _movementController);
            var walkState = new WalkState(InputHandler, _movementController, _movementParametrs, _noiseController, _stepController);
            var runState = new SprintState(InputHandler, _movementController, _movementParametrs, _noiseController, _stepController, _playerStaminaController);
            var slideState = new SlideState(InputHandler, _movementController, _movementParametrs, _playerBodyController, _playerRotationController);
            var crouchState = new CrouchState(InputHandler, _movementController, _movementParametrs, _playerBodyController, _noiseController, _stepController);
            var jumpState = new JumpState(InputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _playerStaminaController);
            var fallState = new FallState(InputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _noiseController, _playerMovementSpeedModifier);
            var grabLedgeState = new GrabLedgeState(InputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _playerGravity, _playerRotationController, _ledgeController);
            var climbingUpState = new ClimbingUpState(InputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _playerGravity, _ledgeController);
            var climbingOnState = new ClimbingOnState(InputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _playerGravity, _ledgeController);
            var climbingOverState = new ClimbingOverState(InputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _playerGravity, _ledgeController);
            var ledgeJumpState = new LedgeJumpState(InputHandler, _playerView.CharacterController, _movementParametrs, _playerView.PlayerCamera);
            var crouchIdleState = new CrouchIdle(_playerBodyController, _movementController, _movementParametrs, _noiseController, _stepController);


            Func<bool> runStatePrecondition = () => InputHandler.MoveForward && InputHandler.SprintTriggered && !_stamina.IsEmpty;
            Func<bool> jumpStatePrecondition = () => InputHandler.JumpTriggered && !_stamina.IsEmpty && _playerGravity.IsGroundedFor(0.1f);

            standingState.AddTransition(walkState, () => !InputHandler.MovementInput.Equals(Vector2.zero));
            standingState.AddTransition(crouchIdleState, () => InputHandler.CrouchTrigger.IsTriggered, InputHandler.CrouchTrigger.ReleaseTrigger);
            standingState.AddTransition(climbingOverState, () => InputHandler.JumpTriggered && _ledgeController.CanClimbOverLedge());
            standingState.AddTransition(climbingOnState, () => InputHandler.JumpTriggered && _ledgeController.CanClimbOnLedge());
            standingState.AddTransition(jumpState, () => jumpStatePrecondition());

            walkState.AddTransition(runState, () => runStatePrecondition());
            walkState.AddTransition(climbingOverState, () => InputHandler.JumpTriggered && _ledgeController.CanClimbOverLedge());
            walkState.AddTransition(climbingOnState, () => InputHandler.JumpTriggered && _ledgeController.CanClimbOnLedge());
            walkState.AddTransition(jumpState, () => jumpStatePrecondition());
            walkState.AddTransition(crouchState, () => InputHandler.CrouchTrigger.IsTriggered, InputHandler.CrouchTrigger.ReleaseTrigger);
            walkState.AddTransition(fallState, () => _playerGravity.IsFalling);
            walkState.AddTransition(standingState, () => InputHandler.MovementInput.Equals(Vector2.zero));

            runState.AddTransition(walkState, () => !runStatePrecondition());
            runState.AddTransition(climbingOverState, () => InputHandler.JumpTriggered && _ledgeController.CanClimbOverLedge());
            runState.AddTransition(climbingOnState, () => InputHandler.JumpTriggered && _ledgeController.CanClimbOnLedge());
            runState.AddTransition(jumpState, () => jumpStatePrecondition());
            runState.AddTransition(slideState, () => InputHandler.CrouchTrigger.IsTriggered && runState.CanSlide(), InputHandler.CrouchTrigger.ReleaseTrigger);
            runState.AddTransition(fallState, () => _playerGravity.IsFalling);

            slideState.AddTransition(crouchState, () => slideState.SlideFinished());
            slideState.AddTransition(walkState, () => InputHandler.CrouchTrigger.IsTriggered && slideState.CanStand() && _playerBodyController.CanStand(), InputHandler.CrouchTrigger.ReleaseTrigger);
            slideState.AddTransition(fallState, () => _playerGravity.IsFalling);

            crouchState.AddTransition(runState, () => runStatePrecondition() && _playerBodyController.CanStand());
            crouchState.AddTransition(walkState, () => InputHandler.CrouchTrigger.IsTriggered && _playerBodyController.CanStand(), InputHandler.CrouchTrigger.ReleaseTrigger);
            crouchState.AddTransition(fallState, () => _playerGravity.IsFalling);
            crouchState.AddTransition(crouchIdleState, () => InputHandler.MovementInput.Equals(Vector2.zero));
            crouchState.AddTransition(jumpState, () => jumpStatePrecondition());

            crouchIdleState.AddTransition(crouchState, () => !InputHandler.MovementInput.Equals(Vector2.zero));
            crouchIdleState.AddTransition(standingState, () => InputHandler.CrouchTrigger.IsTriggered && _playerBodyController.CanStand(), InputHandler.CrouchTrigger.ReleaseTrigger);
            crouchIdleState.AddTransition(jumpState, () => jumpStatePrecondition());

            jumpState.AddTransition(runState, () => runStatePrecondition() && jumpState.CanGround() && _playerGravity.IsGrounded);
            jumpState.AddTransition(walkState, () => jumpState.CanGround() && _playerGravity.IsGrounded);
            jumpState.AddTransition(fallState, jumpState.InHightPoint);
            jumpState.AddTransition(grabLedgeState, () => InputHandler.GrabLedgeTrigger.IsTriggered && _ledgeController.CanGrabToLedgeGrabPointInView());

            fallState.AddTransition(walkState, () => _playerGravity.IsGrounded);
            fallState.AddTransition(grabLedgeState, () => InputHandler.GrabLedgeTrigger.IsTriggered && _ledgeController.CanGrabToLedgeGrabPointInView());

            grabLedgeState.AddTransition(fallState, () => InputHandler.MoveBack && grabLedgeState.CanFinish());
            grabLedgeState.AddTransition(climbingUpState, () => InputHandler.MoveForward && grabLedgeState.CanFinish() && climbingUpState.CanClimb());
            grabLedgeState.AddTransition(ledgeJumpState, () => InputHandler.JumpTriggered && grabLedgeState.CanFinish());

            ledgeJumpState.AddTransition(grabLedgeState, () => _ledgeController.CanGrabToLedgeGrabPointInView());
            ledgeJumpState.AddTransition(fallState, ledgeJumpState.InHightPoint);

            climbingUpState.AddTransition(walkState, () => climbingUpState.ClimbFinish());
            climbingOnState.AddTransition(walkState, () => climbingOnState.ClimbFinish());
            climbingOverState.AddTransition(fallState, () => climbingOverState.ClimbFinish());

            _behaviorStateMachine.SetInitState(walkState);

        }

        public void Tick()
        {
            _ledgeController.HandleFindingLedge();
            _playerRotationController.HandlePlayerRotation();
            _playerGravity.HandleGravity();
            _behaviorStateMachine.Update();
            _stepController.Update();
            _playerMovementSpeedModifier.Update();
            if (_health.IsDead)
            {
                _behaviorStateMachine.ForseChangeState<DeathState>();
            }
        }

        public void FixedTick()
        {
            _movementController.HandleMovement();
            _playerGravity.CheckGrounded();

        }
    }
}
