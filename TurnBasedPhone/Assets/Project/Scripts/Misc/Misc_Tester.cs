namespace Project.Scripts.Misc
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Misc_Tester : MonoBehaviour
    {

        public GameObject[] projectiles;
        public Transform[] positions;
        [SerializeField]
        private bool shoot;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Shooting());
        }

        IEnumerator Shooting()
        {
            while (!shoot)
            {
                for (int i = 0; i < projectiles.Length; i++)
                {
                    Instantiate(projectiles[i], positions[i].position, Quaternion.Euler(transform.forward));
                }
                yield return new WaitForSeconds(1.5f);
            }
        }
        

    }
}
