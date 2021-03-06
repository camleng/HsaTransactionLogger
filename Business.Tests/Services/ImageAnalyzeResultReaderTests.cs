using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;
using Business.Services;
using Business.Tests.TestHelpers;
using FluentAssertions;
using NSubstitute;
using RestSharp;
using Xunit;

namespace Business.Tests.Services
{
    public class ImageAnalyzeResultReaderTests
    {
        private readonly IRestClient _client;
        private readonly ITaskProxy _task;
        
        private ImageAnalyzeResultReader _imageAnalyzeResultReader;
        private readonly HsaResult _hsaResult;

        public ImageAnalyzeResultReaderTests()
        {
            _hsaResult = new HsaResult { Status = "succeeded" };
            _client = Substitute.For<IRestClient>();
            var successfulResponse = RestClientHelper.SuccessfulResponseWithContent<HsaResult>("Success");
            successfulResponse.Data = _hsaResult;
            _client.Get<HsaResult>(Arg.Any<IRestRequest>()).Returns(successfulResponse);
            var configuration = ConfigurationHelper.CreateConfiguration();
            _task = Substitute.For<ITaskProxy>();
            _imageAnalyzeResultReader = new ImageAnalyzeResultReader(_client, configuration, _task);
        }

        [Fact]
        public async Task ItGetsTheAnalyzeResult()
        {
            await _imageAnalyzeResultReader.GetAnalyzeResultAsync("OperationLocationAddress");

            var request = new RestRequest();
            request.AddHeader("ApiKeyName", "ApiKeyValue");
            _client.Received(1)
                .Get<HsaResult>(Arg.Is<IRestRequest>(r =>
                    RequestHelpers.MatchingRequest(r, request)));
        }

        [Fact]
        public async Task GivenAnalyzeResultResponseFails_ItThrowsAnException()
        {
            var failedResponse = RestClientHelper.FailedResponseWithContent<HsaResult>("Something failed");
            _client.Get<HsaResult>(Arg.Any<IRestRequest>()).Returns(failedResponse);

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await _imageAnalyzeResultReader.GetAnalyzeResultAsync("OperationLocationAddress"));
            exception.Message.Should().Be(failedResponse.Content);
        }

        [Fact]
        public async Task GivenAnalyzeResultResponseSucceeds_AndOperationSucceeded_ItReturnsTheaData()
        {
            var result = await _imageAnalyzeResultReader.GetAnalyzeResultAsync("OperationLocationAddress");

            result.Should().Be(_hsaResult);
        }

        [Fact]
        public async Task GivenAnalyzeResultResponseSucceeds_ButOperationHasNotYetSucceeded_ItDelaysExecution()
        {
            var runningResponse = RestClientHelper.SuccessfulResponseWithContent<HsaResult>("Success");
            runningResponse.Data = new HsaResult { Status = "running" };
            var successfulResponse = RestClientHelper.SuccessfulResponseWithContent<HsaResult>("Success");
            successfulResponse.Data = new HsaResult { Status = "succeeded" };
            _client.Get<HsaResult>(Arg.Any<IRestRequest>()).Returns(runningResponse, successfulResponse);

            await _imageAnalyzeResultReader.GetAnalyzeResultAsync("OperationLocationAddress");

            await _task.Received(1).Delay(1000);
        }

        [Fact]
        public async Task
            GivenAnalyzeResultResponseSucceeds_ButOperationHasNotYetSucceeded_ItChecksTheOperationStatusAgain()
        {
            var runningResponse = RestClientHelper.SuccessfulResponseWithContent<HsaResult>("Success");
            runningResponse.Data = new HsaResult { Status = "running" };
            var successfulResponse = RestClientHelper.SuccessfulResponseWithContent<HsaResult>("Success");
            successfulResponse.Data = new HsaResult { Status = "succeeded" };
            _client.Get<HsaResult>(Arg.Any<IRestRequest>())
                .Returns(runningResponse, runningResponse, runningResponse, successfulResponse);

            await _imageAnalyzeResultReader.GetAnalyzeResultAsync("OperationLocationAddress");

            _client.Received(4).Get<HsaResult>(Arg.Any<IRestRequest>());
        }

        [Fact]
        public async Task
            GivenAnalyzeResultResponseSucceeds_ButOperationHasNotYetSucceeded_ItDoublesTheDelayedExecutionAmountEachIteration()
        {
            var runningResponse = RestClientHelper.SuccessfulResponseWithContent<HsaResult>("Success");
            runningResponse.Data = new HsaResult { Status = "running" };
            var successfulResponse = RestClientHelper.SuccessfulResponseWithContent<HsaResult>("Success");
            successfulResponse.Data = new HsaResult { Status = "succeeded" };
            _client.Get<HsaResult>(Arg.Any<IRestRequest>())
                .Returns(runningResponse, runningResponse, runningResponse, runningResponse, successfulResponse);

            await _imageAnalyzeResultReader.GetAnalyzeResultAsync("OperationLocationAddress");

            await _task.Received(1).Delay(1000);
            await _task.Received(1).Delay(2000);
            await _task.Received(1).Delay(4000);
            await _task.Received(1).Delay(8000);
        }

        [Fact]
        public async Task
            GivenAnalyzeResultResponseSucceeds_AndMaxNumberOfRetriesHasBeenExceededForOperation_ItThrowsAnException()
        {
            var configuration = ConfigurationHelper.CreateConfiguration(
                new List<KeyValuePair<string, string>>
                    { new("CognitiveServicesConfig:MaxNumberOfRetries", "2") });

            var runningResponse = RestClientHelper.SuccessfulResponseWithContent<HsaResult>("Success");
            runningResponse.Data = new HsaResult { Status = "running" };
            var successfulResponse = RestClientHelper.SuccessfulResponseWithContent<HsaResult>("Success");
            successfulResponse.Data = new HsaResult { Status = "succeeded" };

            _client.Get<HsaResult>(Arg.Any<IRestRequest>())
                .Returns(runningResponse, runningResponse, runningResponse, successfulResponse);

            _imageAnalyzeResultReader = new ImageAnalyzeResultReader(_client, configuration, _task);

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await _imageAnalyzeResultReader.GetAnalyzeResultAsync("OperationLocationAddress"));
            exception.Message.Should().Be("Max number of retries exceeded");
        }
    }
}