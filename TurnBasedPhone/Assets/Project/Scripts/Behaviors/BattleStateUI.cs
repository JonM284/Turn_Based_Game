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
        [SerializeField] private TextMeshProUGUI teamTimerText;

        [SerializeField] private RectTransform endPosition;
        [SerializeField] private RectTransform offscreenPos;

        [SerializeField] private Vector2 initialPosition;

        [SerializeField] private float initialDuration, waitDuration, outDuration;

        [SerializeField] private RectTransform turnCounter;

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
            animationText.text = "Coin Flip";
            StartCoroutine(DoAnimation());
        }

        public void TeamTurnAnimation(int teamID = 0)
        {
            isInAnimation = true;
            if (teamID == 0) teamID = 1;
            if (!animationText.gameObject.activeInHierarchy) animationText.gameObject.SetActive(true);
            string _displayText = string.Format("Team {0} Turn", teamID);
            animationText.text = _displayText;
            teamTimerText.text = _displayText;
            StartCoroutine(DoAnimation());

        }

        private void EndGameAnimation()
        {
            animationText.text = "Team ~ wins!";
            StartCoroutine(DoAnimation());
        }

        public void SetTurnCountdown(bool _visible)
        {
            turnCounter.gameObject.SetActive(_visible);
        }


        private IEnumerator DoAnimation()
        {
            animationText.rectTransform.DOMove(endPosition.position, initialDuration);
            yield return new WaitForSeconds(initialDuration);
            //wait
            yield return new WaitForSeconds(waitDuration);
            animationText.rectTransform.DOMove(offscreenPos.position, outDuration);
            float offset = 0.6f;
            yield return new WaitForSeconds(outDuration + offset);
            animationText.gameObject.SetActive(false);
            isInAnimation = false;
        }
    }
}

