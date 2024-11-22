using System.Collections.Generic;

public class NPC_VillageFemale1 : NPCController
{
    protected override void InitDialog()
    {
        dialogs.Add(new List<string>
        {
            "I'm an happy villager!",
            "My fruits is so delicious!",
            "Hoping that the wheather is always good!"
        });
    }
}