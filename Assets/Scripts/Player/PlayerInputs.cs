using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInputs
{
    private static Key[] keys = new Key[]
    {
        new Key("Interact", KeyCode.E),
        new Key("Inventory", KeyCode.Q),
    };

    public static KeyCode GetKey(string keyName)
    {
        for (int x = 0; x < keys.Length; ++x)
        {
            if (keys[x].Name == keyName)
                return keys[x];
        }

        return KeyCode.None;
    }

    public static void ResetToDefault()
    {
        for (int x = 0; x < keys.Length; ++x)
            keys[x].Reset();
    }
}

public struct Key
{
    private readonly string name;
    private readonly KeyCode defaultKey;
    private KeyCode key;

    public string Name { get => name; }

    public Key(string keyName, KeyCode keyToAssign)
    {
        name = keyName;
        defaultKey = keyToAssign;
        key = keyToAssign;
    }

    public void Reset()
    {
        key = defaultKey;
    }

    public static implicit operator KeyCode(Key keyObj) => keyObj.key;
}