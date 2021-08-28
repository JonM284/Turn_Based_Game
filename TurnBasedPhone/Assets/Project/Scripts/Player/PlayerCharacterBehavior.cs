using Project.Scripts.Behaviors;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Player
{
    public class PlayerCharacterBehavior : MonoBehaviour
    {

        public enum CharacterTurnState
        {
            IDLE,
            DO_TURN,
            TURN_FINISH
        }

        public enum CharacterHealthState
        {
            FULL_HEALTH,
            INJURED,
            DEAD
        }

        public CharacterTurnState currentTurnState = CharacterTurnState.IDLE;

        public CharacterHealthState healthState = CharacterHealthState.FULL_HEALTH;


        [SerializeField] private bool m_canMove, m_canAttack, m_canUseAbility;
        private bool m_isSelected = false;

        //In current context, SPEED is tied to tile movement spaces.
        [SerializeField] private int m_currentSpeed;
        [SerializeField] private int slowedSpeed, maxSpeed, normalSpeed;

        [SerializeField] private Rigidbody rb;

        private List<Vector3> path;

        public TeamBattleState teamBattleState;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnMouseDown()
        {
            if (currentTurnState == CharacterTurnState.DO_TURN) {
                teamBattleState.ClearTeamSelections();
                SetSelection(true, this.transform);
            }
        }

        private void SetSelection(bool _selected, Transform _player)
        {
            m_isSelected = _selected;
            teamBattleState.SetSelectedObject(_player);
        }

        public void SetSelectionState(bool _selected)
        {
            m_isSelected = _selected;
        }

        public void SetWaypoint(Vector3 newPos)
        {

        }

        /// <summary>
        /// Set the character turn order.
        /// </summary>
        /// <param name="turnState">0=Idle, 1=Team turn</param>
        public void SetTurnState(CharacterTurnState turnState)
        {
            currentTurnState = turnState;
        }

        public void ResetActions()
        {
            m_canMove = true;
            m_canAttack = true;
            m_canUseAbility = true;
        }

        /// <summary>
        /// Set health state of character
        /// </summary>
        /// <param name="healthStateID">0=Healthy, 1=Injured, 2=Dead</param>
        public void SetHealthState(int healthStateID)
        {
            switch (healthStateID)
            {
                case 2:
                    healthState = CharacterHealthState.DEAD;
                    break;
                case 1:
                    healthState = CharacterHealthState.INJURED;
                    break;
                default:
                    healthState = CharacterHealthState.DEAD;
                    break;
            }
        }
    }
}

