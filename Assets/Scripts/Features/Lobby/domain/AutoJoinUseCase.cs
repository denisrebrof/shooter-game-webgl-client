public class AutoJoinUseCase
{
    private static AutoJoinUseCase instance;

    public static AutoJoinUseCase Instance
    {
        get
        {
            if (instance != null)
                return instance;

            instance = new AutoJoinUseCase();
            return instance;
        }
    }

    private AutoJoinAction lastAction = AutoJoinAction.None;

    public bool AutoJointSetting = true;

    public bool AutoJoinAvailable => lastAction == AutoJoinAction.Joined && AutoJointSetting;

    public void SetAction(AutoJoinAction action) => lastAction = action;
}

public enum AutoJoinAction
{
    None,
    Left,
    Joined
}