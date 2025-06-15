using Failsafe.Player.Interaction;
using Failsafe.PlayerMovements;
using UnityEngine;

namespace Failsafe.Player.ItemUsage
{
    public class CarryingItemUsage : MonoBehaviour
    {
        private PhysicsInteraction _physicsInteraction;
        private PlayerController _playerController;

        private void Start()
        {
            _physicsInteraction = GetComponent<PhysicsInteraction>();
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (!_playerController.InputHandler.UseTriggered) return;
            if (!_physicsInteraction._carryingObject) return;

            if (_physicsInteraction._carryingObject.TryGetComponent(out Item item))
            {
                item.ActionsGroups.ForEach(group => group.Event.Invoke());
            }
        }
    }
}