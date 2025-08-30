namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.Cast))]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    [FriendOfAttribute(typeof(ET.Client.Buff))]
    public static class ActionsHelper
    {
        public static Actions CreateActions(this ActionsTempComponent self, int configId, int idx)
        {
            return self.AddChild<Actions, int, int>(configId, idx);
        }

        public static Actions CreateActions(this Cast self, int configId, int idx, Unit_Client Owner, Unit_Client skillUnit,
        ActionsRunType actionsRunType, bool autoRun = true, bool autoDispose = true)
        {
            Actions actions = self.GetComponent<ActionsTempComponent>().CreateActions(configId, idx);
            actions.Caster = self.OwnerUnit;
            if (Owner != null)
                actions.Owner = Owner;
            actions.SkillUnit = skillUnit;

            RunActions(actions, actionsRunType, autoRun, autoDispose);

            if (actions.IsDisposed)
            {
                return null;
            }

            return actions;
        }

        public static Actions CreateActions(this Buff self, int configId, int idx, ActionsRunType actionsRunType, bool autoRun = true,
        bool autoDispose = true)
        {
            Actions actions = self.GetComponent<ActionsTempComponent>().CreateActions(configId, idx);
            actions.Owner = self.Owner;
            RunActions(actions, actionsRunType, autoRun, autoDispose);

            if (actions.IsDisposed)
                return null;
            return actions;
        }

        public static void RunActions(Actions actions, ActionsRunType actionsRunType, bool autoRun = true, bool autoDispose = true)
        {
            if (autoRun)
            {
                if (autoDispose)
                {
                    using (actions)
                    {
                        RunActions(actions, actionsRunType);
                    }
                }
                else
                {
                    RunActions(actions, actionsRunType);
                }
            }
        }

        public static void RunActions(Actions actions, ActionsRunType actionsRunType)
        {
            IActions actionsHandler = ActionsDispatcherComponent.Instance.Get(actions.Config.ActionType);
            if (actionsHandler == null)
            {
                Unit_Client owner = actions.Owner;
                Log.Error($"Error： Actions type not found: {owner?.Id} , ActionsConfigId : {actions.ConfigId}");
                actions.Dispose();
                return;
            }

            actionsHandler.Run(actions, actionsRunType);
        }
    }
}