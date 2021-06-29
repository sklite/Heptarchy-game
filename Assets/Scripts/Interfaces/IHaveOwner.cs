using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IHaveOwner
    {
        void SetColor(Color col);
        void SetOwner(BasePlayer newOwner);
        BasePlayer GetOwner();
    }
}