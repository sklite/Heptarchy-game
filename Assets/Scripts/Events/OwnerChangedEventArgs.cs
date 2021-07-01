using System;
using Assets.Scripts.Players;

namespace Assets.Scripts.Events
{
    public class OwnerChangedEventArgs : EventArgs
    {
        public OwnerChangedEventArgs(BasePlayer oldOwner, BasePlayer newOwner)
        {
            NewOwner = newOwner;
            OldOwner = oldOwner;
        }

        public BasePlayer OldOwner { get; }
        public BasePlayer NewOwner { get; }
    }
}