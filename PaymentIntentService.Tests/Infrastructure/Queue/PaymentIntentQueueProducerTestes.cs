using System.Net;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using Moq;
using PaymentIntentService.Application.DTOs;
using PaymentIntentService.Domain.Entities;
using PaymentIntentService.Infrastructure.Configuration.Settings;
using PaymentIntentService.Infrastructure.Queue;

namespace PaymentIntentService.Tests.Infrastructure.Queue;

[TestFixture]
public class PaymentIntentQueueProducerTests
{
    [SetUp]
    public void SetUp()
    {
        _mockAmazonSqs = new Mock<IAmazonSQS>();
        _queueSettings = Options.Create(new QueueSettings
            { QueueUrl = "https://sqs.amazonaws.com/123456789012/queue-name" });
        _queueProducer = new PaymentIntentQueueProducer(_mockAmazonSqs.Object, _queueSettings);
    }

    private Mock<IAmazonSQS> _mockAmazonSqs;
    private IOptions<QueueSettings> _queueSettings;
    private PaymentIntentQueueProducer _queueProducer;

    [Test]
    public async Task SendMessageAsync_ShouldSendPaymentIntentMessage_WhenValidPaymentIntentIsProvided()
    {
        var paymentIntent = new PaymentIntent("123456789", 100.50m, "Test Description", "Credit Card");

        _mockAmazonSqs
            .Setup(sqs => sqs.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new SendMessageResponse { HttpStatusCode = HttpStatusCode.OK });

        string capturedMessageBody = null!;

        _mockAmazonSqs
            .Setup(sqs => sqs.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
            .Callback<string, string, CancellationToken>((_, messageBody, _) => capturedMessageBody = messageBody)
            .ReturnsAsync(new SendMessageResponse { HttpStatusCode = HttpStatusCode.OK });

        await _queueProducer.SendMessageAsync(paymentIntent);

        Assert.That(capturedMessageBody, Is.Not.Null);
        var deserializedDto = JsonSerializer.Deserialize<PaymentIntentQueueDto>(capturedMessageBody);
        Assert.That(deserializedDto, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(deserializedDto.Id, Is.EqualTo(paymentIntent.Id));
            Assert.That(deserializedDto.PayerDocument, Is.EqualTo(paymentIntent.PayerDocument));
            Assert.That(deserializedDto.Amount, Is.EqualTo(paymentIntent.Amount));
            Assert.That(deserializedDto.Description, Is.EqualTo(paymentIntent.Description));
            Assert.That(deserializedDto.PaymentMethod, Is.EqualTo(paymentIntent.PaymentMethod));
        });

        _mockAmazonSqs.Verify(sqs =>
                sqs.SendMessageAsync(
                    _queueSettings.Value.QueueUrl,
                    It.IsAny<string>(),
                    CancellationToken.None),
            Times.Once);
    }

    [Test]
    public void SendMessageAsync_ShouldThrowApplicationException_WhenSqsThrowsException()
    {
        var paymentIntent = new PaymentIntent("123456789", 100.50m, "Test Description", "Credit Card");

        _mockAmazonSqs
            .Setup(sqs => sqs.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
            .ThrowsAsync(new InvalidOperationException("SQS error"));

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await _queueProducer.SendMessageAsync(paymentIntent));

        Assert.Multiple(() =>
        {
            Assert.That(ex.Message, Is.EqualTo("Error while sending the PaymentIntent message to the queue."));
            Assert.That(ex.InnerException, Is.TypeOf<InvalidOperationException>());
        });
    }
}