using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Rewired;


namespace Project.Scripts.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        private Vector3 m_desiredCameraPos;
        private Vector3 offset;
        private float rotationAmount;
        private float inputDir;

        private Rewired.Player m_player;

        [SerializeField] private float rotationSpeed;
        [SerializeField] private Transform target;
        [Range(1, 2)]
        [SerializeField] private float zoom;
        [SerializeField] private float zoomSpeed;
        //space is given in pixels on screen in unity
        [Space(25)]
        [SerializeField] private float duration;
        [Range(1,20)]
        [SerializeField] private int intensity;
        private bool m_isShakingCamera = false;

        // Start is called before the first frame update
        void Start()
        {
            m_player = ReInput.players.GetPlayer(0);
            SetPosition(target);
        }

        // Update is called once per frame
        void Update()
        {
            PlayerInput();
            transform.RotateAround(target.position, Vector3.up, rotationAmount);
        }

        public void SetPosition(Transform _target)
        {
            target = _target;
            transform.DOMove(_target.position, 0.6f);
        }


        void PlayerInput()
        {
            float rotation_set = m_player.GetAxisRaw("Horizontal");

            rotationAmount = Mathf.Lerp(rotationAmount, rotation_set, rotationSpeed * Time.deltaTime);

            float zoom_set = m_player.GetAxisRaw("Vertical");

            if (m_player.GetButtonDown("TestAction")) ShakeCamera();

            if (zoom_set == 1) zoom += Time.deltaTime * zoomSpeed;
            else if(zoom_set == -1) zoom -= Time.deltaTime * zoomSpeed;
            zoom = Mathf.Clamp(zoom, 1, 3);
            transform.localScale = new Vector3(zoom, zoom, zoom);
        }

        /// <summary>
        /// Cause the camera to shake (action)
        /// </summary>
        /// <param name="_duration">How long the camera will shake for.</param>
        /// <param name="_intensity">How much the camera will bounce around KEEP BEWEEN 0,15 </param>
        public void ShakeCamera(float _duration = 0.5f , int _intensity = 10)
        {
            if (m_isShakingCamera)
            {
                return;
            }
            else
            {
                m_isShakingCamera = true;
                StartCoroutine(ShakeCameraWait());
            }
        }

        private IEnumerator ShakeCameraWait(float _duration = 0.5f, int _intensity = 10)
        {
            transform.DOShakePosition(_duration, 1, _intensity);
            yield return new WaitForSeconds(duration + 0.5f);
            m_isShakingCamera = false;
        }
    }
}

