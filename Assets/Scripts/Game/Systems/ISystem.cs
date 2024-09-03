namespace Game.Systems
{   
    public interface ISystem
    {
        ESystemAccessType AccessType { get; }
        void Destroy();
    }
}
