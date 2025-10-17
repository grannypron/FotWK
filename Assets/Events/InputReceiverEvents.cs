using System;
using UnityEngine;
using UnityEngine.UI;

public class InputReceiverEvents : MonoBehaviour
{
    public delegate void InputCallback(string key);

    private InputCallback mInputCallback = null;

    private string CURSOR_OBJECT_NAME = "inputCursor";

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (GameObject.Find(CURSOR_OBJECT_NAME) == null)
        {
            throw new Exception("'" + CURSOR_OBJECT_NAME + "' game object not found in events class that inherits from InputReceiverSceneEvents.  Ensure that a cursor named '" + CURSOR_OBJECT_NAME + "' exists in your scene");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (mInputCallback != null && Input.anyKeyDown)
        {
            mInputCallback(Input.inputString);
            mInputCallback = null;
        }

    }

    public void ActivateInputKeypress(InputCallback inputCallback)
    {
        this.mInputCallback = inputCallback;
        InputField inputCursor = GameObject.Find(CURSOR_OBJECT_NAME).GetComponent<InputField>();
        inputCursor.Select();
        inputCursor.ActivateInputField();
    }
    public static InputReceiverEvents GetInputReceiverEvents()
    {
        return GameObject.Find("EventSystem").GetComponent<InputReceiverEvents>();
    }
}
