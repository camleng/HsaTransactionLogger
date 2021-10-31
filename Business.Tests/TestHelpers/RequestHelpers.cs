using System.Linq;
using RestSharp;

namespace Business.Tests.TestHelpers
{
    public static class RequestHelpers
    {
        public static bool MatchingRequest(IRestRequest actual, IRestRequest expected)
        {
            var actualHeaders = actual.Parameters.Where(p => p.Type == ParameterType.HttpHeader);
            var expectedHeaders = expected.Parameters.Where(p => p.Type == ParameterType.HttpHeader);
            var headers = actualHeaders.Zip(expectedHeaders);

            return headers.All(h => h.First.Name == h.Second.Name
                                    && h.First.Value == h.Second.Value);
        }
    }
}