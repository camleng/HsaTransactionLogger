using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using RestSharp;

namespace Business.Tests.TestHelpers
{
    public static class RestResponseExtensions
    {
        public static IRestResponse WithHeaders(this IRestResponse response,
            params (string Name, string Value)[] headers)
        {
            var headersToInclude = headers
                .Select(header => new Parameter(header.Name, header.Value, ParameterType.HttpHeader)).ToList();
            response.Headers.Returns(headersToInclude);
            return response;
        }

        public static IRestResponse WithEmptyHeaders(this IRestResponse response)
        {
            response.Headers.Returns(new List<Parameter>());
            return response;
        }
    }
}