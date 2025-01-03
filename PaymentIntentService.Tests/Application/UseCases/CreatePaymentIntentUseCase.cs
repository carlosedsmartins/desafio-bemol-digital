using Moq;
using PaymentIntentService.Application.Interfaces.Queue;
using PaymentIntentService.Application.UseCases;
using PaymentIntentService.Domain.Entities;

namespace PaymentIntentService.Tests.Application.UseCases;

[TestFixture]
public class CreatePaymentIntentUseCaseTests
{
    [SetUp]
    public void SetUp()
    {
        _mockQueueProducer = new Mock<IPaymentIntentQueueProducer>();
        _useCase = new CreatePaymentIntentUseCase(_mockQueueProducer.Object);
    }

    private Mock<IPaymentIntentQueueProducer> _mockQueueProducer;
    private CreatePaymentIntentUseCase _useCase;

    [Test]
    public async Task ExecuteAsync_ShouldCreatePaymentIntentAndSendToQueue_WhenParametersAreValid()
    {
        const string payerDocument = "123456789";
        const decimal amount = 100.50m;
        const string description = "Payment for service";
        const string paymentMethod = "Credit Card";

        _mockQueueProducer
            .Setup(q => q.SendMessageAsync(It.IsAny<PaymentIntent>()))
            .Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(payerDocument, amount, description, paymentMethod);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.PayerDocument, Is.EqualTo(payerDocument));
            Assert.That(result.Amount, Is.EqualTo(amount));
            Assert.That(result.Description, Is.EqualTo(description));
            Assert.That(result.PaymentMethod, Is.EqualTo(paymentMethod));
        });
        _mockQueueProducer.Verify(q => q.SendMessageAsync(It.Is<PaymentIntent>(
            pi => pi.PayerDocument == payerDocument &&
                  pi.Amount == amount &&
                  pi.Description == description &&
                  pi.PaymentMethod == paymentMethod)), Times.Once);
    }

    [Test]
    public void ExecuteAsync_ShouldThrowApplicationException_WhenPaymentIntentCreationFails()
    {
        const string payerDocument = "123456789";
        const decimal amount = 100.50m;
        const string description = "Payment for service";
        const string paymentMethod = "Credit Card";

        _mockQueueProducer
            .Setup(q => q.SendMessageAsync(It.IsAny<PaymentIntent>()))
            .ThrowsAsync(new InvalidOperationException("Queue error"));

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await _useCase.ExecuteAsync(payerDocument, amount, description, paymentMethod));

        Assert.Multiple(() =>
        {
            Assert.That(ex.Message, Is.EqualTo("Error while creating the PaymentIntent."));
            Assert.That(ex.InnerException, Is.TypeOf<InvalidOperationException>());
        });
    }
}