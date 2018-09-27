namespace Zarwin.Shared.Contracts.Core
{
    public interface ISoldier
    {
        int Id { get; }

        int HealthPoints { get; }
        int Level { get; }

        void Hurt(int damage);
    }
}
