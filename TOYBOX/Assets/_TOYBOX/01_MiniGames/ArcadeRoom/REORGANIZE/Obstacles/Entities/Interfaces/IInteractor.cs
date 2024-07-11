namespace ToyBox.Minigames.BeatEmUp
{
    public interface IInteractor : IEntity
    {
        public void OnInteract(IInteractable interactable);
    }
}
