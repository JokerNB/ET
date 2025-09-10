namespace ET.Server
{
    public interface IDBPatcher
    {
        ETTask Run(DBComponent dbComponent, int zone);
    }
}