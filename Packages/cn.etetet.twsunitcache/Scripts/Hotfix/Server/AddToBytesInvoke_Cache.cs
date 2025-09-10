namespace ET.Server
{
    [Invoke(SceneType.UnitCache)]
    public class AddToBytesInvoke_Cache : AInvokeHandler<AddToBytes>
    {
        public override void Handle(AddToBytes args)
        {
            Unit unit = args.unit;
            unit?.GetComponent<UnitDBSaveComponent>().AddToBytes(args.type, args.bytes);
        }
    }
}