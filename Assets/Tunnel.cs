using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
    public class Tunnel : MonoBehaviour
    {
        public bool reverse;

        public GameObject listPoints;

        void Start()
        {

        }

        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("VehicleTeam0") || other.gameObject.CompareTag("VehicleTeam1"))
            {
                SimpleController scriptController = other.gameObject.GetComponent<SimpleController>();

                if (scriptController.inTunnel == 0)
                {
                    scriptController.inTunnel = 1;
                    GameObject.Find("MainCamera").GetComponent<MotionBlur>().enabled = true;
                    scriptController.targetWayPoint = null;
                    scriptController.reverseTunnel = reverse;
                    scriptController.wayPointList = listPoints;
                }

            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("VehicleTeam0") || other.gameObject.CompareTag("VehicleTeam1"))
            {
                SimpleController scriptController = other.gameObject.GetComponent<SimpleController>();
                if (scriptController.currentWayPoint <= 0 ||scriptController.currentWayPoint>14)
                {
                    GameObject.Find("MainCamera").GetComponent<MotionBlur>().enabled = false;
                }
                /*print (other.gameObject.name + " uscito");

                if (other.gameObject.CompareTag ("VehicleTeam0") || other.gameObject.CompareTag ("VehicleTeam1")) {
                    Tunnel tunnelScript = endTunnel.GetComponent<Tunnel>();
                    if(tunnelScript.                     || scriptController.currentWayPoint < scriptController.wayPointList.transform.childCount
                    SimpleController scriptController = other.gameObject.GetComponent<SimpleController> ();
                    scriptController.inTunnel = 0;*/

            }
        }
    }
}
