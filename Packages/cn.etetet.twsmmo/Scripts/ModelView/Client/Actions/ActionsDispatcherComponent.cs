using System;
using System.Collections.Generic;

namespace ET.Client
{
    [CodeProcess]
    public class ActionsDispatcherComponent : Singleton<ActionsDispatcherComponent> , ISingletonAwake
    {
        private Dictionary<int, IActions> dic;

        public void Awake()
        {
            dic = new();
            var types = CodeTypes.Instance.GetTypes(typeof(ActionsAttribute));
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(typeof(ActionsAttribute), false);
                if (attributes.Length == 0)
                {
                    continue;
                }
                
                ActionsAttribute actionsAttribute = attributes[0] as ActionsAttribute;
                object obj = Activator.CreateInstance(type);
                IActions iactions = obj as IActions;
                if (iactions == null)
                {
                    throw new Exception($"class : {type.FullName} not implement IActions interface");
                }
                this.dic[actionsAttribute.ActionsType] = iactions;
            }
        }

        public IActions Get(int type)
        {
            return this.dic.GetValueOrDefault(type);
        }
    }
}