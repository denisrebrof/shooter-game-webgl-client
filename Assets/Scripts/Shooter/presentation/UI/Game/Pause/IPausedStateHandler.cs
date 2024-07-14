using System;

namespace Shooter.presentation.UI.Game.Pause
{
    public interface IPausedStateHandler
    {
        IObservable<bool> GetPausedStateFlow();
    }
}