using System;

namespace EindToernooi_Poule
{
    public static class PopupManager
    {
        public static event MessageEventHandler MessageEvent;
        public delegate void MessageEventHandler(string arg);

        public static void ShowMessage(string message)
        {
            MessageEvent?.Invoke(message);
        }
    }
}
