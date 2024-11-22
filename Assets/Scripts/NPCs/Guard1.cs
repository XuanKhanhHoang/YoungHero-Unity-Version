using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard1 : NPCController
{
    protected override void InitDialog()
    {

        dialogs.Add(new List<string>
        {
            "Hello",
            "I'll be an strong slodier",
            "Fight until die"
        });

    }
}
