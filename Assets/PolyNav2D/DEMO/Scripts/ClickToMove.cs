using UnityEngine;
using System.Collections.Generic;

//example

namespace CreativeSpore.SuperTilemapEditor
{
    [RequireComponent(typeof(PolyNavAgent))]

    public class ClickToMove : MonoBehaviour {

        private PolyNavAgent _agent;
        public PolyNavAgent agent {
            get
            {
                if (!_agent)
                    _agent = GetComponent<PolyNavAgent>();
                return _agent;
            }
        }

        void Update() {
           // if (Input.GetMouseButton(0))
            //    agent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));

           
            
        }

        

    }
}