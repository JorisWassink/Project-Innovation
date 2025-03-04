using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.Basic
{
    [AddComponentMenu("")]
    public class BasicNetManager : NetworkManager
    {
        List<TiltScript> playersList = TiltScript.playersList;
        
        /// <summary>
        /// Called on the server when a client adds a new player with NetworkClient.AddPlayer.
        /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            TiltScript.ResetPlayerNumbers();
            UpdateCameras();
        }

        /// <summary>
        /// Called on the server when a client disconnects.
        /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            TiltScript.ResetPlayerNumbers();
            UpdateCameras();
        }

        private void UpdateCameras()
        {
            int playerCount = playersList.Count;
            

            for (int i = 0; i < playerCount; i++)
            {
                switch (playerCount)
                {
                    case 1:
                        break;
                    case 2:
                        if (i == 0)
                        {
                            playersList[i].cameraTransform.GetComponent<Camera>().rect = new Rect(0, 0, .5f, 1);
                        }
                        else
                        {
                            playersList[i].cameraTransform.GetComponent<Camera>().rect = new Rect(.5f, 0, .5f, 1);
                        }
                        break;
                    case 3:
                        if (i == 0)
                        {
                            playersList[i].cameraTransform.GetComponent<Camera>().rect = new Rect(0, 0, .5f, .5f);
                        }
                        else if (i == 1)
                        {
                            playersList[i].cameraTransform.GetComponent<Camera>().rect = new Rect(.5f, 0, .5f, .5f);

                        }
                        else
                        {
                            playersList[i].cameraTransform.GetComponent<Camera>().rect = new Rect(0, .5f, 1, .5f);
                        }
                        break;
                    case 4:
                        if (i == 0)
                        {
                            playersList[i].cameraTransform.GetComponent<Camera>().rect = new Rect(0, 0, .5f, .5f);
                        }
                        else if (i == 1)
                        {
                            playersList[i].cameraTransform.GetComponent<Camera>().rect = new Rect(.5f, 0, .5f, .5f);

                        }
                        else if (i == 2)
                        {
                            playersList[i].cameraTransform.GetComponent<Camera>().rect = new Rect(0, .5f, .5f, .5f);
                        }
                        else
                        {
                            playersList[i].cameraTransform.GetComponent<Camera>().rect = new Rect(.5f, .5f, .5f, .5f);

                        }
                        break;
                    default:
                        Debug.LogWarning("More than 4 players are not supported");
                        break;
                }
            }
        }

    }
}