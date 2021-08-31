using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Player
{
    public class PlayerBattleCanvas : MonoBehaviour
    {

        [SerializeField] private Button m_attackActionButton;
        [SerializeField] private Button m_moveActionButton;
        [SerializeField] private Button m_abilityActionButton;

        private PlayerCharacterBehavior currentPlayer;


        // Start is called before the first frame update
        void Start()
        {
            InitializeButtons();
        }


        public void SetCurrentPlayer(PlayerCharacterBehavior _newPlayer)
        {
            currentPlayer = _newPlayer;
        }

        private void InitializeButtons()
        {
            m_moveActionButton.onClick.AddListener(MovePlayer);
            m_attackActionButton.onClick.AddListener(Attack);
            m_abilityActionButton.onClick.AddListener(UseAbility);
        }

        public void Attack()
        {
            currentPlayer.DoAttackAction();
        }

        public void MovePlayer()
        {
            currentPlayer.MovePath();
        }

        public void UseAbility()
        {
            currentPlayer.DoAbilityAction();
        }

    }
}

