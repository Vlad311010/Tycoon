
namespace Interfaces
{
    public interface IInteractable
    {
        public void Interact(AICore core);
    }

    public interface IClickable
    {
        public void OnClick();
    }

    public interface IContainPersistentData
    {
        public uint LoadOrder { get; }
        public void Save();
        public void Load();
    }

}
