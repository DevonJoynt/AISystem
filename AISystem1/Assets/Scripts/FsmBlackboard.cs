using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Blackboard for <see cref="FsmState"/>s.
/// </summary>
public class FsmBlackboard : MonoBehaviour
{
    /// <summary>
    ///     Global blackboard shared by all <see cref="FsmState"/>.
    /// </summary>
    public static FsmBlackboard GlobalBlackboard
    {
        get
        {
            if (globalBlackboard == null)
            {
                globalBlackboard = CreateGlobalFsmBlackboard();
            }
            return globalBlackboard;
        }
    }
    private static FsmBlackboard globalBlackboard;

    /// <summary>
    ///     Information store inside the blackboard.
    /// </summary>
    public readonly Dictionary<string, object> Variables = new();


    /// <summary>
    ///     Clear contents of blackboard.
    /// </summary>
    public void Clear()
    {
        Variables.Clear();
    }

    /// <summary>
    ///     Remove value from blackboard.
    /// </summary>
    /// <param name="valueName">The name of the vriable stored within the blackboard to remove.</param>
    /// <returns>
    ///     True if removal is successful, false otherwise.
    /// </returns>
    public bool Remove(string valueName)
    {
        bool whiteboardDoesContainValue = Variables.ContainsKey(valueName);
        if (whiteboardDoesContainValue)
        {
            Variables.Remove(valueName);
            return true;
        }
        else return false;
    }

    /// <summary>
    ///     Adds or updates value in blackboard.
    /// </summary>
    /// <typeparam name="T">The type of variable to add.</typeparam>
    /// <param name="valueName">The name of the variable within the blackboard to update.</param>
    /// <param name="value">The value to update.</param>
    public void Set<T>(string valueName, T value)
    {
        bool whiteboardDoesContainValue = Variables.ContainsKey(valueName);
        if (whiteboardDoesContainValue)
        {
            // SET
            Variables[valueName] = value;
        }
        else
        {
            // ADD
            Variables.Add(valueName, value);
        }
    }

    /// <summary>
    ///     Get a value stored in the blackboard.
    /// </summary>
    /// <typeparam name="T">The type of variable to add.</typeparam>
    /// <param name="valueName">The name of the variable within the blackboard to update.</param>
    /// <returns>
    ///     The value requested.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if blackboard does not contain <paramref name="valueName"/>.
    /// </exception>
    /// <exception cref="InvalidCastException">
    ///     Thrown if requested type <typeparamref name="T"/> does not match the value's type.
    /// </exception>
    public T Get<T>(string valueName)
    {
        // Reject request if not contained in blackboard
        bool containsValue = Variables.ContainsKey(valueName);
        if (!containsValue)
        {
            string msg = $"{nameof(FsmBlackboard)} of {name} does not contain value called \"{valueName}\".";
            throw new ArgumentException(msg);
        }

        // Ensure type is correct
        object value = Variables[valueName];
        Type valueType = value.GetType();
        Type requestType = typeof(T);
        if (valueType != requestType)
        {
            string msg =
                $"{name}'s {nameof(FsmBlackboard)} value \"{valueName}\" is type " +
                $"\"{valueType.Name}\" but the requested type is \"{requestType}\".";
            throw new InvalidCastException(msg);
        }

        return (T)value;
    }

    public bool GetBool(string valueName) => Get<bool>(valueName);
    public int GetInt(string valueName) => Get<int>(valueName);
    public float GetFloat(string valueName) => Get<float>(valueName);
    public Vector3 GetVector3(string valueName) => Get<Vector3>(valueName);
    public Quaternion GetQuaternion(string valueName) => Get<Quaternion>(valueName);
    public Transform GetTransform(string valueName) => Get<Transform>(valueName);
    public GameObject GetGameObject(string valueName) => Get<GameObject>(valueName);
    public string GetString(string valueName) => Get<string>(valueName);

    private static FsmBlackboard CreateGlobalFsmBlackboard()
    {
        var gobj = new GameObject("FsmGlobalBlackboard");
        var fsmBlackboard = gobj.AddComponent<FsmBlackboard>();
        DontDestroyOnLoad(gobj);
        return fsmBlackboard;
    }

}
