
namespace Interfaces
{
    public interface IInteractable
    {
        public void Interact(AICore core);
    }

    public interface IClickable
    {
        public void OnLeftClick();
        public void OnRightClick();
        // public void OnMouseHover();
    }

    public interface IContainPersistentData
    {
        public uint LoadOrder { get; }
        public void Save();
        public void Load();
    }

}
