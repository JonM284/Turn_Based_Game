using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        /// <summary>
        /// DESCRIPTION: this script is the top of the ability polymorphism chain. All abilities will derive from this class and change their 
        /// form in their own respective types.
        /// </summary>
        //[CreateAssetMenu(menuName = "Ability/Melee_Ability")] this will allow you to make new ability assets
        #region Variables
        [Header("Ability Info")]
        [Tooltip("Name to display for ability")]
        [SerializeField]
        private string m_abilityName;

        [Tooltip("Description of ability")]
        [TextArea(3, 5)]
        [SerializeField]
        private string m_ability_Description;

        [Tooltip("Legnth of time until player can use ability again.")]
        [SerializeField]
        private float m_cooldown;

        [Tooltip("Image to be placed in ability slot.")]
        [SerializeField]
        private Sprite m_abilityImage;

        

        #endregion

        public void Initialize(GameObject _reciever, int _abilityId)
        {
            
            try
            {
                
            }
            catch
            {
                Debug.Log($"<color=red>Could not assign {m_abilityName}, to {_reciever.name}");
            }
        }

        public abstract void DoAction(GameObject _reciever);

    }
}

