namespace ET.Client
{
    public class ActionsAttribute : BaseAttribute
    {
        public int ActionsType { get; }

        public ActionsAttribute(int actionsType)
        {
            this.ActionsType = actionsType;
        }
    }
}