using Project.Scripts.Behaviors;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Project.Scripts.Player
{
    public class PlayerCharacterBehavior : MonoBehaviour
    {

        public enum CharacterTurnState
        {
            IDLE,
            DO_TURN,
            SELECTING_MOVE,
            MOVING,
            ATTACKING,
            USING_ABILITY,
            TURN_FINISH
        }

        public enum CharacterHealthState
        {
            FULL_HEALTH,
            INJURED,
            DEAD
        }

        [Header("Player states")]
        public CharacterTurnState currentTurnState = CharacterTurnState.IDLE;

        public CharacterHealthState healthState = CharacterHealthState.FULL_HEALTH;


        [SerializeField] private bool m_canMove, m_canAttack, m_canUseAbility;
        private bool m_isSelected = false;

        [Header("Player variables")]
        //In current context, SPEED is tied to tile movement spaces.
        [SerializeField] private int m_currentSpeed;
        [SerializeField] private int slowedSpeed, maxSpeed, normalSpeed;

        [SerializeField] private Rigidbody rb;

        [Header("Battle variables")]
        [SerializeField] private Vector3[] path;

        public TeamBattleState teamBattleState;
        public int teamMemberID;

        [Header("Character variables")]
        [SerializeField] private SkinnedMeshRenderer playerModel;


        [Header("Weapon Variables")]
        [SerializeField] private float attackRange;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (currentTurnState == CharacterTurnState.ATTACKING)
            {
              
            }
        }

        private void OnMouseDown()
        {
            if (currentTurnState == CharacterTurnState.DO_TURN) {
                teamBattleState.ClearTeamSelections();
                SetSelection(true, this.transform, this);
                
            }
        }

        public void SetTeamColor(Color _teamColor)
        {
            playerModel.materials[0].SetColor("_OutlineColor", _teamColor);
        }

        private void SetSelection(bool _selected, Transform _player, PlayerCharacterBehavior _playerBehavior)
        {
            m_isSelected = _selected;
            teamBattleState.SetSelectedObject(_player, _playerBehavior);
        }

        public void SetSelectionState(bool _selected)
        {
            m_isSelected = _selected;
        }

        public void FinishTurn()
        {
            Debug.Log("Finished turn" +transform.name);
            currentTurnState = CharacterTurnState.TURN_FINISH;
            teamBattleState.SetPlayerTurnOver(teamMemberID);
        }

        public void SetPath(Vector3[] _path)
        {
            if (path.Length > 0)
            {
                for (int i = 0; i < path.Length; i++)
                {
                    path[i] = Vector3.zero;
                }
                path = new Vector3[0];
            }

            if (_path != null && _path.Length > 0)
            {
                path = new Vector3[_path.Length];
                for (int i = 0; i < _path.Length; i++)
                {
                    path[i] = _path[i];
                }
            }
        }

        public void InitializeMove()
        {
            teamBattleState.SetMovingCharacter();
        }

        public void MovePath()
        {
            Debug.Log("<color=green>Moving Player</color>");
            if (path.Length > 0 && path != null)
            {
                float _estimatedDuration = path.Length / m_currentSpeed;
                rb.DOPath(path, _estimatedDuration, PathType.Linear);
                currentTurnState = CharacterTurnState.MOVING;
                StartCoroutine(DoMoveAction(_estimatedDuration, 1f));
            }
            else
            {
                m_canMove = false;
                CheckEndTurn();
            }
            
        }

        private IEnumerator DoMoveAction(float _duration, float offset)
        {
            yield return new WaitForSeconds(_duration + offset);
            m_canMove = false;
            CheckEndTurn();
        }

        public void InitializeAttackAction()
        {
            SetTurnState(CharacterTurnState.ATTACKING);
        }

        public void CancelAttackAction()
        {

        }

        public void DoAttackAction()
        {
            Debug.Log("<color=green>Attacking Player</color>");
            m_canAttack = false;
            CheckEndTurn();
        }

        public void DoAbilityAction()
        {
            Debug.Log("<color=green>Using ability Player</color>");
            m_canUseAbility = false;
            CheckEndTurn();
        }

        public void CheckEndTurn()
        {
            if (!m_canMove && !m_canUseAbility && !m_canAttack)
                FinishTurn();
            else
                currentTurnState = CharacterTurnState.DO_TURN;
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

