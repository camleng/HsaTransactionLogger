using NSubstitute;
using RestSharp;

namespace Business.Tests.TestHelpers
{
    public static class RestClientHelper
    {
        public static IRestResponse SuccessfulResponseWithContent(string content)
        {
            var response = Substitute.For<IRestResponse>();
            response.IsSuccessful.Returns(true);
            response.Content.Returns(content);
            return response;
        }
        
        public static IRestResponse<T> SuccessfulResponseWithContent<T>(string content)
        {
            var response = Substitute.For<IRestResponse<T>>();
            response.IsSuccessful.Returns(true);
            response.Content.Returns(content);
            return response;
        }

        public static IRestResponse FailedResponseWithContent(string content)
        {
            var response = Substitute.For<IRestResponse>();
            response.IsSuccessful.Returns(false);
            response.Content.Returns(content);
            return response;
        }
        
        public static IRestResponse<T> FailedResponseWithContent<T>(string content)
        {
            var response = Substitute.For<IRestResponse<T>>();
            response.IsSuccessful.Returns(false);
            response.Content.Returns(content);
            return response;
        }
    }
}