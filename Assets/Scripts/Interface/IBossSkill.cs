using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossSkill
{
    string GetSkillScriptName(); // Return Object Name.
    void DoSkill();
}
