using System;

namespace Shooter.presentation.UI.Pause
{
    public interface IPausedStateHandler
    {
        IObservable<bool> GetPausedStateFlow();
    }
}