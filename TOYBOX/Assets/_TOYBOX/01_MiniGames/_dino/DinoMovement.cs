using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class DinoMovement : MonoBehaviour
    {



        public Rigidbody rb;
        

        // Update is called once per frame
        void Update()
        {
            // Start is called before the first frame update
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Flap(); 
            }
            if (Input.GetKeyDown(KeyCode.UpArrow ))
            {
                Flap();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow ))
            {
                MoveRight ();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();

            }


            MoveFoward();
        }

        void Flap()
        {
            rb.AddForce(Vector3.up *200);
        }

        void MoveFoward()
        {

            rb.AddForce(new Vector3(0, 0, 50) * Time.deltaTime);
        }






        void MoveLeft()
        {

            rb.AddForce(new Vector3(-50, 0, 0));

        }
        void MoveRight()
        {

            rb.AddForce(new Vector3(50, 0, 0));

        }
    }
}
