namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.Cast))]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    [FriendOfAttribute(typeof(ET.Client.Buff))]
    [FriendOfAttribute(typeof(ET.Client.BulletComponent))]
    public static class ActionsHelper
    {
        public static Actions CreateActions(this ActionsTempComponent self, int configId)
        {
            return self.AddChild<Actions, int>(configId);
        }

        public static Actions CreateActions(this Cast self, int configId, Unit_Client Owner, ActionsRunType actionsRunType, bool autoRun = true,
        bool autoDispose = true)
        {
            Actions actions = self.GetComponent<ActionsTempComponent>().CreateActions(configId);
            actions.Caster = self.Caster;
            actions.Owner = Owner;

            RunActions(actions, actionsRunType, autoRun, autoDispose);

            if (actions.IsDisposed)
            {
                return null;
            }

            return actions;
        }

        public static Actions CreateActions(this Buff self, int configId, ActionsRunType actionsRunType, bool autoRun = true, bool autoDispose = true)
        {
            Actions actions = self.GetComponent<ActionsTempComponent>().CreateActions(configId);
            actions.Owner = self.Owner;
            RunActions(actions, actionsRunType, autoRun, autoDispose);

            if (actions.IsDisposed)
                return null;
            return actions;
        }

        public static Actions CreateActions(this BulletComponent self, int configId, Unit_Client owner, Unit_Client caster, ActionsRunType actionsRunType,
        bool autoRun = true, bool autoDispose = true)
        {
            Actions actions = self.GetComponent<ActionsTempComponent>().CreateActions(configId);
            actions.Caster = caster;
            actions.Owner = owner;
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