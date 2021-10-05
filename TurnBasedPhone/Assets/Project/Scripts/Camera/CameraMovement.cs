using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Rewired;
using UnityEngine.UI;


namespace Project.Scripts.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        private Vector3 m_desiredCameraPos;
        private Vector3 offset;

        private float inputDir;

        private Rewired.Player m_player;


        [Header("Camera Variables")]
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
        [Tooltip("Amount of positions the camera can rotate to")]
        [SerializeField] private int rotationAngles = 2;
        private bool m_isShakingCamera = false;

        [SerializeField]
        private Button rightRotateButton = null;
        [SerializeField]
        private Button leftRotateButton = null;

        //Touch controls
        [Header("Touch Controls")]
        [SerializeField] private float touchDistance = 30f;
        private Vector2 initialTouchPos;
        private Vector2 currentTouchPos;
        private bool fingerDown = false;
        [SerializeField] private float desiredRotationAmount;
        [SerializeField] private Vector3 rotationAmount;


        private float rotationMax = 360;
        [SerializeField]private float rotationAddAmount;

        // Start is called before the first frame update
        void Start()
        {
            m_player = ReInput.players.GetPlayer(0);
            SetPosition(target);
            rotationAddAmount = rotationMax / rotationAngles;
            rightRotateButton.onClick.AddListener(RotateRight);
            leftRotateButton.onClick.AddListener(RotateLeft);
        }

        // Update is called once per frame
        void Update()
        {
            PlayerInput();
        }

        public void SetPosition(Transform _target)
        {
            target = _target;
            transform.DOMove(_target.position, 0.6f);
        }


        void PlayerInput()
        {

            Vector3 desiredRotation = new Vector3(0, desiredRotationAmount, 0);
            rotationAmount = Vector3.Slerp(rotationAmount, desiredRotation, rotationSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(rotationAmount);

            float zoom_set = m_player.GetAxisRaw("Vertical");

            if (m_player.GetButtonDown("TestAction")) ShakeCamera();

            if (zoom_set == 1) zoom += Time.deltaTime * zoomSpeed;
            else if(zoom_set == -1) zoom -= Time.deltaTime * zoomSpeed;
            zoom = Mathf.Clamp(zoom, 1, 3);
            transform.localScale = new Vector3(zoom, zoom, zoom);
        }

        private void RotateRight()
        {
            RotateCamera(-rotationAddAmount);
        }

        private void RotateLeft()
        {
            RotateCamera(rotationAddAmount);
        }

        private void RotateCamera(float _amount)
        {
            desiredRotationAmount += _amount;

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

