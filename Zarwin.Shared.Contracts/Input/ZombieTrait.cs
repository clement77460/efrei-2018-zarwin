namespace Zarwin.Shared.Contracts.Input
{
    public enum ZombieTrait
    {
        /// <summary>
        /// Dies after only receiving 2 damage in a single turn
        /// </summary>
        Tough = 1,

        /// <summary>
        /// Each soldier damaged pass cannot attack that turn
        /// </summary>
        Incapacitating = 2,

        Normal = 3,
    }
}
