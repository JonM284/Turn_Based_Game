using Project.Scripts.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Behaviors
{


    public class BattleSystem : MonoBehaviour
    {

        public enum BattleState
        {
            GENERATION,
            PLAYER_SETUP,
            COIN_FLIP,
            TEAM_ONE_TURN,
            TEAM_TWO_TURN,
            END
        }
        private BattleState currentState = BattleState.GENERATION;

        [SerializeField] private GridGeneration gridGen;
        [SerializeField] private TeamBattleState[] teams;
        [SerializeField] private BattleStateUI stateUI;
        [SerializeField] private Pathfinding pathFinder;
        [SerializeField] private A_Grid pathfindingGridGen;

        [SerializeField]private List<Color> teamDistinguishers;

        [Header("Turn Countdown")]
        [Tooltip("Max amount of time for both players in seconds")]
        [SerializeField] private float maxTimer;
        [SerializeField] private Slider roundSlider;
        private float currentTimer;
        private bool m_end_Turn = false;

        private int testInt = 0;

        public bool RunThroughGame;



        private void Awake()
        {
           if(RunThroughGame) StartCoroutine(Initialize());
           else gridGen.GenerateGrid();
        }


        private void Update()
        {
            if (currentState == BattleState.TEAM_ONE_TURN || currentState == BattleState.TEAM_TWO_TURN)
            {
                if (currentTimer < maxTimer && !m_end_Turn)
                {
                    currentTimer += Time.deltaTime;
                }

                if (currentTimer >= maxTimer && !m_end_Turn)
                {
                    m_end_Turn = true;
                }

                roundSlider.value = currentTimer / maxTimer;
            }
        }

        private void ResetTimer()
        {
            if (m_end_Turn) m_end_Turn = false;
            currentTimer = 0;
        }


        private void References()
        {
                //Saving prefabs during runtime.
                //If the path has a value (aka the prefab exists) this will replace the current prefab with the current gameobject
                //IF the path does not have a value this will create a new prefab with the path name you put (aka first like created a prefab with the name
                //TestNewPlayer)
                //new player = IN SCENE GAMEOBJECT
                //PrefabUtility.SaveAsPrefabAsset(newPlayer, "Assets/Project/Prefabs/TestNewPlayer.prefab");
                //PrefabUtility.SaveAsPrefabAsset(newTeamMember, "Assets/Project/Prefabs/TeamMember1.prefab");
        }
        //TODO:COLORS MIGHT JUST BE FOR TESTING
        private IEnumerator Initialize()
        {
            //setup grid
            Debug.Log("Begin Generating Grid");

            gridGen.GenerateGrid();
            yield return new WaitUntil(() => gridGen.genState == GridGeneration.GenerationState.FINISHED);
            Debug.Log("Finished generating grid");
            yield return new WaitForSeconds(1);
            //setup team
            foreach (var team in teams)
            {
                int randomTeamColor = Random.Range(0, teamDistinguishers.Count);
                team.InitilizeTeam(teamDistinguishers[randomTeamColor]);
                team.pathfinder = this.pathFinder;
                teamDistinguishers.RemoveAt(randomTeamColor);
            }
            StartCoroutine(PlayerLevelSetup());
        }

        private IEnumerator PlayerLevelSetup()
        {
            stateUI.SetTurnCountdown(false);
            currentState = BattleState.PLAYER_SETUP;
            stateUI.DoTextAnimation(0);
            Debug.Log("<color=#00ff00>start test level setup</color>");
            yield return new WaitUntil(() => !stateUI.isInAnimation);
            Debug.Log("<color=#00ff00>Finished test level setup</color>");
            yield return new WaitForSeconds(1);
            StartCoroutine(CoinFlip());
        }

        private IEnumerator CoinFlip()
        {
            currentState = BattleState.COIN_FLIP;
            Debug.Log("<color=yellow>start coinflip</color>");
            stateUI.DoTextAnimation(1);
            int coinFlip = Random.Range(0,2);
            yield return new WaitUntil(() => !stateUI.isInAnimation);
            Debug.Log("<color=#00ff00>finished coin flip</color>");
            //TODO:Wait for coin flip animation to end
            yield return new WaitForSeconds(1);
            if(coinFlip == 0) StartCoroutine(TeamOneTurn());
            else StartCoroutine(TeamTwoTurn());
            stateUI.SetTurnCountdown(true);
        }

        private IEnumerator TeamOneTurn()
        {
            currentState = BattleState.TEAM_ONE_TURN;
            ResetTimer();
            Debug.Log("<color=cyan>start team 1 turn</color>");
            stateUI.TeamTurnAnimation(1);
            int currentTeamID = 0;
            teams[currentTeamID].SetTeamTurnState(TeamBattleState.State.BEGIN_TURN);
            //wait until team is done with turn
            //yield return new WaitUntil(() => gridGen.genState == GridGeneration.GenerationState.FINISHED);
            //test
            yield return new WaitUntil(() => !stateUI.isInAnimation);
            //If team 1 is dead team 2 wins, otherwise it's team 2's turn
            testInt++;
           
            yield return new WaitUntil(() => teams[0].currentState == TeamBattleState.State.COMPLETED_ACTIONS || m_end_Turn);
            Debug.Log("<color=cyan>finish team 1 turn</color>");
            //test, TODO: if team is dead
            teams[currentTeamID].SetTeamTurnState(TeamBattleState.State.WAIT_FOR_TURN);
            if (testInt < 3) StartCoroutine(TeamTwoTurn());
            else StartCoroutine(EndGame());
        }

        private IEnumerator TeamTwoTurn()
        {
            currentState = BattleState.TEAM_TWO_TURN;
            ResetTimer();
            Debug.Log("<color=orange>start team 2 turn</color>");
            stateUI.TeamTurnAnimation(2);
            int currentTeamID = 1;
            teams[currentTeamID].SetTeamTurnState(TeamBattleState.State.BEGIN_TURN);
            //wait until team is done with turn
            //yield return new WaitUntil(() => gridGen.genState == GridGeneration.GenerationState.FINISHED);
            //test
            yield return new WaitUntil(() => !stateUI.isInAnimation);
            //time between animation and offset
            yield return new WaitUntil(() => teams[1].currentState == TeamBattleState.State.COMPLETED_ACTIONS || m_end_Turn);
            Debug.Log("<color=orange>finish team 2 turn</color>");
            //If team 2 is dead team 1 wins, otherwise it's team 1's turn
            teams[currentTeamID].SetTeamTurnState(TeamBattleState.State.WAIT_FOR_TURN);
            if (!teams[currentTeamID].teamIsDead) StartCoroutine(TeamOneTurn());
            else StartCoroutine(EndGame());
        }

        private IEnumerator EndGame()
        {
            currentState = BattleState.END;
            stateUI.DoTextAnimation(2);
            yield return new WaitUntil(() => !stateUI.isInAnimation);
            Debug.Log("<color=red>Game Over</color>");
        }

        private void GeneratePathfidingGrid()
        {

        }
    }
}

