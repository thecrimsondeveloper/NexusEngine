using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using ToyBox;
using UnityEngine;

namespace Toolkit.Users
{
    public class User : MonoBehaviour
    {
        public string name;
        public int age;
        public UserSettings settings;
        public UserSession currentSession;
        public List<UserSession> pastSessions = new List<UserSession>();

        private void Start()
        {
            LoadUser();
        }
        public void LogAction(UserAction userAction)
        {
            if (currentSession == null)
            {
                LoadUser();
            }



            currentSession.actionsTaken.Add(userAction);
        }


        public void LoadUser()
        {
            currentSession = ScriptableObject.CreateInstance<UserSession>();
        }

        [Button]
        public void SaveUser()
        {
            pastSessions.Add(currentSession);
            currentSession = null;

            string json = JsonUtility.ToJson(this);

            Debug.Log(json);
        }
    }
}
