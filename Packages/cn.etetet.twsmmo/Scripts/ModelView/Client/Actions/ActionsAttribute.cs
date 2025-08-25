namespace ET.Client
{
    public class ActionsAttribute : BaseAttribute
    {
        public ActionType ActionsType { get; }

        public ActionsAttribute(ActionType actionsType)
        {
            this.ActionsType = actionsType;
        }
    }
}