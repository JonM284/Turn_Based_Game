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
        [SerializeField] private Button attackAction = null;
        [SerializeField] private Button moveAction = null;
        [SerializeField] private Button abilityAction = null;

        public void InitilizeTeam()
        {
            //get character prefabs

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

            if(currentState == State.BEGIN_TURN)
            {
                ResetPlayerTurns();
                foreach (var member in teamMembers)
                {
                    member.SetTurnState(member.currentTurnState = PlayerCharacterBehavior.CharacterTurnState.DO_TURN);
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

        public void SetSelectedObject(Transform _target)
        {
            cam.SetPosition(_target);
        }


        public void ClearTeamSelections()
        {
            foreach (var character in teamMembers)
            {
                character.SetSelectionState(false);
            }
        }
    }
}

