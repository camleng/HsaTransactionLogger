using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Business.Services;
using Business.Tests.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using NSubstitute;
using RestSharp;
using Xunit;

namespace Business.Tests.Services
{
    public class ImageAnalyzerTests
    {
        private readonly ImageAnalyzer _imageAnalyzer;
        private readonly IRestClient _client;
        private readonly byte[] _bytes;
        private readonly IFormFile _file;

        public ImageAnalyzerTests()
        {
            _bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            _file = new FormFile(new MemoryStream(_bytes), 0, _bytes.Length, "Data", "dummy.txt");

            _client = Substitute.For<IRestClient>();
            var successfulResponse = RestClientHelper.SuccessfulResponseWithContent("It worked!")
                .WithHeaders(("Operation-Location", "SomeLocation"));
            _client.Post(Arg.Any<IRestRequest>()).Returns(successfulResponse);
            var configuration = ConfigurationHelper.CreateConfiguration();
            _imageAnalyzer = new ImageAnalyzer(_client, configuration);
        }
          
        [Fact]
        public async Task ItPostsTheRestRequest()
        {
            await _imageAnalyzer.AnalyzeAsync(_file);

            var fileParameter = new FileParameter
                { ContentLength = _bytes.Length, ContentType = "image/png", Name = "file" };
            _client.Received(1).Post(Arg.Is<RestRequest>(r =>
                    r.Files.Count == 1
                    && r.Files[0].Name == fileParameter.Name
                    && r.Files[0].ContentLength == fileParameter.ContentLength
                    && r.Files[0].ContentType == fileParameter.ContentType
                    && r.Parameters.Exists(p =>
                        p.Value != null && p.Name == "ApiKeyName" && p.Value.ToString() == "ApiKeyValue")
                    && r.Resource == "AnalyzeAddressValue"
                )
            );
        }

        [Fact]
        public async Task GivenAnalyzeRequestFails_ItThrowsAnException()
        {
            var failedResponse = RestClientHelper.FailedResponseWithContent("Something happened");
            _client.Post(Arg.Any<IRestRequest>()).Returns(failedResponse);

            var thrownException = await Assert.ThrowsAsync<Exception>(async () =>
                await _imageAnalyzer.AnalyzeAsync(_file));
            thrownException.Message.Should().Be("Something happened");
        }
    }
}