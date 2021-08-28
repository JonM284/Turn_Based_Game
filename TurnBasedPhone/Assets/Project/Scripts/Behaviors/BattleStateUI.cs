using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Project.Scripts.Behaviors
{
    public class BattleStateUI : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI animationText;

        [SerializeField] private RectTransform endPosition;

        private Vector3 initialPosition;

        [SerializeField] private float initialDuration, waitDuration, outDuration;

        public bool isInAnimation = false;


        private void Awake()
        {
            initialPosition = animationText.rectTransform.position;
        }

        /// <summary>
        /// Set Text animation to run
        /// </summary>
        /// <param name="_stateID">0=Setup, 1=CoinFlip, 2=EndGame, </param>
        public void DoTextAnimation(int _stateID)
        {
            isInAnimation = true;
            animationText.gameObject.SetActive(true);
            switch (_stateID)
            {
                case 2:
                    EndGameAnimation();
                    break;
                case 1:
                    CoinFlipAnimation();
                    break;
                default:
                    SetupAnimation();
                    break;
            }
        }

        private void SetupAnimation()
        {
            animationText.text = "Setup Level";
            StartCoroutine(DoAnimation());
        }

        private void CoinFlipAnimation()
        {
            animationText.text = "CoinFlip";
            StartCoroutine(DoAnimation());
        }

        public void TeamTurnAnimation(int teamID = 0)
        {
            isInAnimation = true;
            if (teamID == 0) teamID = 1;
            if (!animationText.gameObject.activeInHierarchy) animationText.gameObject.SetActive(true);
            animationText.text = $"Team {teamID} Turn";
            StartCoroutine(DoAnimation());

        }

        private void EndGameAnimation()
        {
            animationText.text = "Team ~ wins!";
            StartCoroutine(DoAnimation());
        }


        private IEnumerator DoAnimation()
        {
            animationText.rectTransform.DOMove(endPosition.position, initialDuration);
            yield return new WaitForSeconds(initialDuration);
            //wait
            yield return new WaitForSeconds(waitDuration);
            animationText.rectTransform.DOMove(initialPosition, outDuration);
            float offset = 0.6f;
            yield return new WaitForSeconds(outDuration + offset);
            animationText.gameObject.SetActive(false);
            isInAnimation = false;
        }
    }
}

