using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//public class ChatMessage : MessageBase
//{
//    public string text;
//}

public class Chat : NetworkBehaviour {
    //const short CHAT_MSG = 100;

    [SyncVar (hook = "UpdateChat")]
    public string newText = "";
    private static string wholeText = "";
    
    private TextMeshProUGUI chatPanel;
    private InputField inputComponent;

	// Use this for initialization
	void Start () {
        inputComponent = GameObject.FindGameObjectWithTag("inputField").GetComponent<InputField>();
        chatPanel = GameObject.FindGameObjectWithTag("textField").GetComponent<TextMeshProUGUI>();

        SelectInputField();

        if(!isServer)
        {
            CmdJoinAlert(netId);
        }
    }
    
    void UpdateChat(string txt)
    {
        AppendNewTextToWhole(txt);
    }
	
    [Command]
    void CmdEnterText(NetworkInstanceId id, string inputText)
    {
        newText = string.Concat(id.ToString() + ": ", inputText, '\n');
    }

    [Command]
    public void CmdJoinAlert(NetworkInstanceId id)
    {
        newText = string.Concat("<color=green>", id.ToString(), " enters chatroom.</color>\n");
    }
    

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;

		if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            CmdEnterText(netId,inputComponent.text);

            SelectInputField();
        }
	}

    private void SelectInputField()
    {
        if (!isLocalPlayer) return;
        inputComponent.text = "";
        inputComponent.Select();
        inputComponent.ActivateInputField();
    }

    private void AppendNewTextToWhole(string txt)
    {
        wholeText = string.Concat(wholeText, txt);
        chatPanel.text = wholeText;
    }
}
