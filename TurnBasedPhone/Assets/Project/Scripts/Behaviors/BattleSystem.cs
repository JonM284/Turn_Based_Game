using Project.Scripts.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Rewired;

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
        public BattleState currentState = BattleState.GENERATION;

        public GridGeneration gridGen;
        public TeamBattleState[] teams;
        public BattleStateUI stateUI;

        private int testInt = 0;

        public bool RunThroughGame;

        private void Awake()
        {
           if(RunThroughGame) StartCoroutine(Initialize());
           else gridGen.GenerateGrid();
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

        private IEnumerator Initialize()
        {
            //setup grid
            //setup team
            Debug.Log("Begin Generating Grid");
            gridGen.GenerateGrid();
            yield return new WaitUntil(() => gridGen.genState == GridGeneration.GenerationState.FINISHED);
            Debug.Log("Finished generating grid");
            yield return new WaitForSeconds(1);
            StartCoroutine(PlayerLevelSetup());
        }

        private IEnumerator PlayerLevelSetup()
        {
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
            yield return new WaitUntil(() => !stateUI.isInAnimation);
            Debug.Log("<color=#00ff00>finished coin flip</color>");
            yield return new WaitForSeconds(1);
            StartCoroutine(TeamTwoTurn());
        }

        private IEnumerator TeamOneTurn()
        {
            currentState = BattleState.TEAM_ONE_TURN;
            Debug.Log("<color=cyan>start team 1 turn</color>");
            stateUI.TeamTurnAnimation(1);
            //wait until team is done with turn
            //yield return new WaitUntil(() => gridGen.genState == GridGeneration.GenerationState.FINISHED);
            //test
            yield return new WaitUntil(() => !stateUI.isInAnimation);
            //If team 1 is dead team 2 wins, otherwise it's team 2's turn
            testInt++;
            Debug.Log("<color=cyan>finish team 1 turn</color>");
            yield return new WaitForSeconds(1);
            //test, TODO: if team is dead
            if (testInt < 3) StartCoroutine(TeamTwoTurn());
            else StartCoroutine(EndGame());
        }

        private IEnumerator TeamTwoTurn()
        {
            currentState = BattleState.TEAM_TWO_TURN;
            Debug.Log("<color=orange>start team 2 turn</color>");
            stateUI.TeamTurnAnimation(2);
            //wait until team is done with turn
            //yield return new WaitUntil(() => gridGen.genState == GridGeneration.GenerationState.FINISHED);
            //test
            yield return new WaitUntil(() => !stateUI.isInAnimation);
            Debug.Log("<color=orange>finish team 2 turn</color>");
            yield return new WaitForSeconds(1);
            //If team 2 is dead team 1 wins, otherwise it's team 1's turn
            if (!teams[1].teamIsDead) StartCoroutine(TeamOneTurn());
            else StartCoroutine(EndGame());
        }

        private IEnumerator EndGame()
        {
            currentState = BattleState.END;
            stateUI.DoTextAnimation(2);
            yield return new WaitUntil(() => !stateUI.isInAnimation);
            Debug.Log("<color=red>Game Over</color>");
        }
    }
}
