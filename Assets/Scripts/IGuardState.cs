using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGuardState
{
    public IGuardState StateTask(GuardController guard);

}
