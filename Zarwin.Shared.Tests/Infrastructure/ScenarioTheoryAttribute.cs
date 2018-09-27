using Xunit;
using Xunit.Sdk;

namespace Zarwin.Shared.Tests.Infrastructure
{
    [XunitTestCaseDiscoverer("Zarwin.Shared.Tests.Infrastructure.ScenarioTheoryDiscoverer", "Zarwin.Shared.Tests")]
    public class ScenarioTheoryAttribute : TheoryAttribute
    {
    }
}
