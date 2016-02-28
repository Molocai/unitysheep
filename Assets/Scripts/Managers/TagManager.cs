using UnityEngine;
using System.Collections;

public class TagManager {

    public string Value;
	private TagManager(string value)
    {
        Value = value;
    }

    public override string ToString() { return Value; }

    public static TagManager Player { get { return new TagManager("Player"); } }
}
