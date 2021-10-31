using System;
using System.Threading.Tasks;
using Business.Services;
using Business.Tests.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using RestSharp;
using Xunit;

namespace Business.Tests.Services
{
    public class ImageReaderTests
    {
        private readonly IFormFile _file;
        private readonly IImageAnalyzer _imageAnalyzer;
        private readonly IImageAnalyzeResultReader _imageAnalyzeResultReader;
        private readonly ImageReader _imageReader;

        public ImageReaderTests()
        {
            _file = Substitute.For<IFormFile>();
            var successfulResponse = RestClientHelper.SuccessfulResponseWithContent("It worked!")
                .WithHeaders(("Operation-Location", "SomeLocation"));
            _imageAnalyzer = Substitute.For<IImageAnalyzer>();
            _imageAnalyzeResultReader = Substitute.For<IImageAnalyzeResultReader>();
            _imageAnalyzer.AnalyzeAsync(_file).Returns(successfulResponse);

            _imageReader = new ImageReader(_imageAnalyzer, _imageAnalyzeResultReader);
        }

        [Fact]
        public async Task ItAnalyzesTheImage()
        {
            await _imageReader.GetHsaTextInformationFromImage(_file);

            await _imageAnalyzer.Received(1).AnalyzeAsync(_file);
        }

        [Fact]
        public async Task GivenOperationLocationHeaderIsNotPresent_ItThrowsAnException()
        {
            var successfulResponseWithEmptyHeaders =
                RestClientHelper.SuccessfulResponseWithContent("It worked!").WithEmptyHeaders();
            _imageAnalyzer.AnalyzeAsync(_file).Returns(successfulResponseWithEmptyHeaders);

            var thrownException = await Assert.ThrowsAsync<Exception>(async () =>
                await _imageReader.GetHsaTextInformationFromImage(_file));
            thrownException.Message.Should().Be("Response does not include Operation-Location header");
        }

        [Fact]
        public async Task GivenOperationLocationHeaderIsPresent_ItAnalyzesTheImage()
        {
            var response = RestClientHelper.SuccessfulResponseWithContent("Image is uploaded")
                .WithHeaders(("Operation-Location", "https://example.com"));
            _imageAnalyzer.AnalyzeAsync(_file).Returns(response);
            
            await _imageReader.GetHsaTextInformationFromImage(_file);

            await _imageAnalyzeResultReader.Received(1).GetAnalyzeResultAsync("https://example.com");
        }
    }
}