using System;

namespace ET.Server
{
    [Event(SceneType.All)]
    public class UnitAddComponentByGetNull_SaveToDB : AEvent<Scene, UnitAddComponentByGetNull>
    {
        protected override async ETTask Run(Scene scene, UnitAddComponentByGetNull a)
        {
            Unit unit = a.unit;
            Type type = a.type;
            
            unit.GetComponent<UnitDBSaveComponent>()?.AddChanges(type);
            await ETTask.CompletedTask;
        }
    }
}