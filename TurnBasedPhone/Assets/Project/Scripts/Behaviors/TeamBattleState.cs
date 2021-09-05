using Project.Scripts.Camera;
using Project.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Project.Scripts.Behaviors
{
    public class TeamBattleState : MonoBehaviour
    {

        
        public enum State {
            SETUP_LEVEL,
            WAIT_FOR_TURN,
            BEGIN_TURN,
            DOING_ACTIONS,
            COMPLETED_ACTIONS
        }

        [Header("Team state variables")]
        public State currentState = State.WAIT_FOR_TURN;

        public List<PlayerCharacterBehavior> teamMembers;

        public bool[] playerFinishedTurn;
        public bool[] playerIsDead;
        public bool teamIsDead = false;

        [Header("Team Camera")]
        [SerializeField] private CameraMovement cam;
        [Space]
        [Header("Individual combat")]
        [SerializeField] private PlayerCharacterBehavior currentPlayer;
        [SerializeField] private PlayerCharacterBehavior receivingPlayer;
        [SerializeField] private Transform movePosition;
        [SerializeField] private Transform abilityUseLocation;
        [Header("Individual combat UI")]
        [SerializeField] private Button m_attackActionButton;
        [SerializeField] private Button m_moveActionButton;
        [SerializeField] private Button m_abilityActionButton;
        [SerializeField] private GameObject teamCanvas;

        public void InitilizeTeam(Color _teamColor)
        {
            //TODO: Make this script call the desired team color.
            //TODO: maybe start coin flip earlier?
            //get character prefabs

            //set colors
            InitializeTeamColors(_teamColor);
            InitializeButtons();
            //add characters to teamMembers

            //give teamMembers this battleState script
            InitializeTeamMembers();
            //set health and turn state

            //set locations
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void AddTeamMembers()
        {

        }

        private void InitializeButtons()
        {
            currentPlayer = teamMembers[0];
            m_moveActionButton.onClick.AddListener(CallCurrentPlayerMove);
            m_attackActionButton.onClick.AddListener(CallCurrentPlayerAttack);
            m_abilityActionButton.onClick.AddListener(CallCurrentPlayerAbility);

            m_abilityActionButton.onClick.Invoke();
        }

        #region Button functions
        private void CallCurrentPlayerMove()
        {
            Debug.Log(currentPlayer.name);
            currentPlayer.MovePath();
        }

        private void CallCurrentPlayerAttack()
        {
            currentPlayer.DoAttackAction();
        }

        private void CallCurrentPlayerAbility()
        {
            currentPlayer.DoAbilityAction();
        }

        #endregion


        #region Methods
        public void InitializeTeamMembers()
        {
            playerFinishedTurn = new bool[teamMembers.Count];
            playerIsDead = new bool[teamMembers.Count];
            for (int i = 0; i < teamMembers.Count; i++)
            {
                teamMembers[i].teamBattleState = this;
                teamMembers[i].teamMemberID = i;
            }
        }

        private void InitializeTeamColors(Color _teamColor)
        {
            foreach (var member in teamMembers)
            {
                member.SetTeamColor(_teamColor);
            }
        }

        private void ResetPlayerTurns()
        {
            for (int i = 0; i < playerFinishedTurn.Length; i++)
            {
                playerFinishedTurn[i] = false;
            }
            SetTeamTurnState(State.DOING_ACTIONS);
        }

        
        public void SetTeamTurnState(State _state)
        {
            currentState = _state;

            if (currentState == State.DOING_ACTIONS)
                teamCanvas.SetActive(true);
            else
                teamCanvas.SetActive(false);

            if (currentState == State.BEGIN_TURN)
            {
                ResetPlayerTurns();
                foreach (var member in teamMembers)
                {
                    member.SetTurnState(member.currentTurnState = PlayerCharacterBehavior.CharacterTurnState.DO_TURN);
                    member.ResetActions();
                }
            }else if (currentState == State.COMPLETED_ACTIONS)
            {
                foreach (var member in teamMembers)
                {
                    member.SetTurnState(member.currentTurnState = PlayerCharacterBehavior.CharacterTurnState.IDLE);
                }
            }
        }

        public void SetPlayerTurnOver(int _teamMemberID)
        {
            playerFinishedTurn[_teamMemberID] = true;

            int playerFinishedCount = 0;
            for (int i = 0; i < playerFinishedTurn.Length; i++)
            {
                if (playerFinishedTurn[i]) playerFinishedCount++;
            }
            if (playerFinishedCount >= teamMembers.Count)
            {
                SetTeamTurnState(State.COMPLETED_ACTIONS);
            }
        }

        public void SetSelectedObject(Transform _target, PlayerCharacterBehavior _player)
        {
            cam.SetPosition(_target);
            currentPlayer = _player;
        }


        public void ClearTeamSelections()
        {
            foreach (var character in teamMembers)
            {
                character.SetSelectionState(false);
            }
        }

        #endregion
    }
}

