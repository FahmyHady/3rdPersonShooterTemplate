using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
//using GameAnalyticsSDK;
//using FunGames.Sdk.Analytics;

public enum GameAnalyticsEvents { LevelCompleted, NewLevelStarted, DeviceBuggedCanNotVibrate }
public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent<bool>> eventDictionaryBool;
    private Dictionary<string, UnityEvent<int>> eventDictionaryInt;
    private Dictionary<string, UnityEvent<float>> eventDictionaryFloat;
    private Dictionary<string, UnityEvent<Vector3>> eventDictionaryVector3;
    private Dictionary<string, UnityEvent<Transform>> eventDictionaryTransform;
    private Dictionary<string, UnityEvent> eventDictionaryNull;


    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionaryBool == null)
        {
            eventDictionaryBool = new Dictionary<string, UnityEvent<bool>>();
        }     
        if (eventDictionaryTransform == null)
        {
            eventDictionaryTransform = new Dictionary<string, UnityEvent<Transform>>();
        }
        if (eventDictionaryInt == null)
        {
            eventDictionaryInt = new Dictionary<string, UnityEvent<int>>();
        }
        if (eventDictionaryFloat == null)
        {
            eventDictionaryFloat = new Dictionary<string, UnityEvent<float>>();
        }   
        if (eventDictionaryVector3 == null)
        {
            eventDictionaryVector3 = new Dictionary<string, UnityEvent<Vector3>>();
        }
        if (eventDictionaryNull == null)
        {
            eventDictionaryNull = new Dictionary<string, UnityEvent>();
        }
    }
    //#region GameAnalyticsEvent
    //public static void FireGameAnalyticsEvent(GameAnalyticsEvents whichEventToFire, int level = 0)
    //{
    //    switch (whichEventToFire)
    //    {
    //        case GameAnalyticsEvents.LevelCompleted:
    //            GameAnalytics.NewDesignEvent("IceCubes: Level Completed", level);
    //            FunGamesAnalytics.NewDesignEvent("IceCubes: Level Completed", level.ToString());
    //            break;
    //        case GameAnalyticsEvents.NewLevelStarted:
    //            GameAnalytics.NewDesignEvent("IceCubes: New Level Started", level);
    //            FunGamesAnalytics.NewDesignEvent("IceCubes: New Level Started", level.ToString());
    //            break;
    //        case GameAnalyticsEvents.DeviceBuggedCanNotVibrate:
    //            GameAnalytics.NewDesignEvent("IceCubes: This Device Cannot Vibrate", level);
    //            break;
    //    }
    //}

    //#endregion

    #region EventBool
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool>
    {

    }

    public static void StartListening(string eventName, UnityAction<bool> listener)
    {
        UnityEvent<bool> thisEvent = null;
        if (instance.eventDictionaryBool.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new BoolEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionaryBool.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<bool> listener)
    {
        if (eventManager == null) return;
        UnityEvent<bool> thisEvent = null;
        if (instance.eventDictionaryBool.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, bool toggle)
    {
        UnityEvent<bool> thisEvent = null;
        if (instance.eventDictionaryBool.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(toggle);
        }
    }
    #endregion  
    #region EventInt
    [System.Serializable]
    public class IntEvent : UnityEvent<int>
    {

    }

    public static void StartListening(string eventName, UnityAction<int> listener)
    {
        UnityEvent<int> thisEvent = null;
        if (instance.eventDictionaryInt.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new IntEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionaryInt.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<int> listener)
    {
        if (eventManager == null) return;
        UnityEvent<int> thisEvent = null;
        if (instance.eventDictionaryInt.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, int toggle)
    {
        UnityEvent<int> thisEvent = null;
        if (instance.eventDictionaryInt.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(toggle);
        }
    }
    #endregion 
    #region EventFloat
    [System.Serializable]
    public class FloatEvent : UnityEvent<float>
    {

    }

    public static void StartListening(string eventName, UnityAction<float> listener)
    {
        UnityEvent<float> thisEvent = null;
        if (instance.eventDictionaryFloat.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new FloatEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionaryFloat.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<float> listener)
    {
        if (eventManager == null) return;
        UnityEvent<float> thisEvent = null;
        if (instance.eventDictionaryFloat.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, float toggle)
    {
        UnityEvent<float> thisEvent = null;
        if (instance.eventDictionaryFloat.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(toggle);
        }
    }
    #endregion
    
    #region EventVector3
    [System.Serializable]
    public class Vector3Event : UnityEvent<Vector3>
    {

    }

    public static void StartListening(string eventName, UnityAction<Vector3> listener)
    {
        UnityEvent<Vector3> thisEvent = null;
        if (instance.eventDictionaryVector3.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new Vector3Event();
            thisEvent.AddListener(listener);
            instance.eventDictionaryVector3.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<Vector3> listener)
    {
        if (eventManager == null) return;
        UnityEvent<Vector3> thisEvent = null;
        if (instance.eventDictionaryVector3.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, Vector3 toggle)
    {
        UnityEvent<Vector3> thisEvent = null;
        if (instance.eventDictionaryVector3.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(toggle);
        }
    }
    #endregion

    #region EventNone
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionaryNull.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionaryNull.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionaryNull.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionaryNull.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
    #endregion

    #region EventTransform
    [System.Serializable]
    public class TransformEvent : UnityEvent<Transform>
    {

    }

    public static void StartListening(string eventName, UnityAction<Transform> listener)
    {
        UnityEvent<Transform> thisEvent = null;
        if (instance.eventDictionaryTransform.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new TransformEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionaryTransform.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<Transform> listener)
    {
        if (eventManager == null) return;
        UnityEvent<Transform> thisEvent = null;
        if (instance.eventDictionaryTransform.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, Transform toggle)
    {
        UnityEvent<Transform> thisEvent = null;
        if (instance.eventDictionaryTransform.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(toggle);
        }
    }
    #endregion  
}