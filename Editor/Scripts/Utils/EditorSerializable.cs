using System;

using UnityEditor;

///////////////////////////////////////////////////////////////////////////
/// <summary>
/// Utility class used to bring basic serialization and deserialization
/// methods for data meant to be stored in the EditorPrefs
/// </summary>
/// <typeparam name="T">The type of the class that inherits from this class
/// and needs to be managed</typeparam>
public class EditorSerializable<T> where T : new()
{

    ///////////////////////////////////////////////////////////////////////////
    /// Converts the copied data to a string representation
    public new string ToString()
    {
        try
        {
            return EditorJsonUtility.ToJson(this);
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    ///////////////////////////////////////////////////////////////////////////
    /// Parses a string to copied data
    public static bool TryParse(string str, out T data)
    {
        try
        {
            data = new T();
            EditorJsonUtility.FromJsonOverwrite(str, data);
            return true;
        }
        catch (Exception)
        {
            data = default;
            return false;
        }
    }
}
