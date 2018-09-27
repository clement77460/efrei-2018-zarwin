using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;

namespace Zarwin.Shared.Contracts
{
    public interface IInstantSimulator
    {
        Result Run(Parameters parameters);
    }
}
