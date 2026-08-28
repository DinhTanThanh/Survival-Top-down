using UnityEngine;

public class MinionCommand
{
    public MinionCommandType commandType;
    public Transform target;
    public Vector3 formationOffset;
    public float duration;
    public MinionCommand(MinionCommandType commandType, Transform target, Vector3 formationOffset, float duration)
    {
        this.commandType = commandType;
        this.target = target;
        this.formationOffset = formationOffset;
        this.duration = duration;
    }
}
