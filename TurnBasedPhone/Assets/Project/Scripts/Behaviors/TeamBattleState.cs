using Project.Scripts.Camera;
using Project.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Behaviors
{
    public class TeamBattleState : MonoBehaviour
    {

        public enum State{
            SETUP_LEVEL,
            WAIT_FOR_TURN,
            BEGIN_TURN,
            DOING_ACTIONS,
            COMPLETED_ACTIONS
        }

        public State currentState = State.WAIT_FOR_TURN;

        public List<PlayerCharacterBehavior> teamMembers;

        public bool[] playerIsDead;
        public bool teamIsDead = false;

        [SerializeField] private CameraMovement cam;

        public void InitilizeTeam()
        {
            //get character prefabs
            //add characters to teamMembers
            //set health and turn state
            //set locations
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        
        public void SetTeamTurnState()
        {
            currentState = State.BEGIN_TURN;
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

