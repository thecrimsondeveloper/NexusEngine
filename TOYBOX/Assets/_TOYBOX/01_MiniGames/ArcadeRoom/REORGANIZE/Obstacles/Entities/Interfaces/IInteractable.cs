namespace ToyBox.Minigames.BeatEmUp
{
    public interface IInteractable : IEntity
    {
        public void Interact(IInteractor interactor);
    }
}
