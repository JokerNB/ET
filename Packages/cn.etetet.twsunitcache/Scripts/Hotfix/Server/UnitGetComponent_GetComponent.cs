using System;

namespace ET.Server
{
    [Event(SceneType.All)]
    public class UnitGetComponent_GetComponent : AEvent<Scene, UnitGetComponent>
    {
        protected override async ETTask Run(Scene scene, UnitGetComponent a)
        {
            Unit unit = a.unit;
            Type type = a.Type;

            unit.GetComponent<UnitDBSaveComponent>()?.AddChanges(type);

            //判定Unit身上是否存在需要获取的值
            if (unit.Components.ContainsKey(type.TypeHandle.Value.ToInt64()))
                return;

            UnitDBSaveComponent unitDBSaveComponent = unit.GetComponent<UnitDBSaveComponent>();
            if(unitDBSaveComponent == null)
                return;
            
            //Unit身上不存在需要挂在的组件，这个时候就从字节数组容器中，并进行反序列化挂在Unit身上
            if(!unit.GetComponent<UnitDBSaveComponent>().Bytes.TryGetValue(type,out byte[] bytes))
                return;
            
            //这里的意图就是延迟组件的反序列化时间，玩家有用到对应组件再对需要的组件进行反序列化操作，抹平CPU消耗尖峰
            Entity t= MongoHelper.Deserialize(type,bytes) as Entity;
            unit.AddComponent(t);

            await ETTask.CompletedTask;
        }
    }
}