using Moq;
using PaymentProcessorService.Application.Interfaces.Repositories;
using PaymentProcessorService.Application.UseCases;
using PaymentProcessorService.Domain.Entities;

namespace PaymentProcessorService.Tests.Application.UseCases;

[TestFixture]
public class ProcessPaymentUseCaseTests
{
    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<IProcessedPaymentRepository>();
        _useCase = new ProcessPaymentUseCase(_mockRepository.Object);
    }

    private Mock<IProcessedPaymentRepository> _mockRepository;
    private ProcessPaymentUseCase _useCase;

    [Test]
    public async Task ExecuteAsync_ShouldAddProcessedPaymentToRepository_WhenValidParametersAreProvided()
    {
        var id = Guid.NewGuid();
        const string payerDocument = "123456789";
        const decimal amount = 150.75m;
        const string description = "Test Payment";
        const string paymentMethod = "Credit Card";
        var createdAt = DateTime.UtcNow;

        await _useCase.ExecuteAsync(id, payerDocument, amount, description, paymentMethod, createdAt);

        _mockRepository.Verify(repo => repo.AddAsync(It.Is<ProcessedPayment>(payment =>
            payment.Id == id &&
            payment.PayerDocument == payerDocument &&
            payment.Amount == amount &&
            payment.Description == description &&
            payment.PaymentMethod == paymentMethod &&
            payment.CreatedAt == createdAt &&
            payment.ProcessedAt <= DateTime.UtcNow &&
            payment.ProcessedAt >= DateTime.UtcNow.AddSeconds(-1)
        )), Times.Once);
    }

    [Test]
    public void ExecuteAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
    {
        var id = Guid.NewGuid();
        const string payerDocument = "123456789";
        const decimal amount = 150.75m;
        const string description = "Test Payment";
        const string paymentMethod = "Credit Card";
        var createdAt = DateTime.UtcNow;

        _mockRepository
            .Setup(repo => repo.AddAsync(It.IsAny<ProcessedPayment>()))
            .ThrowsAsync(new Exception("Repository error"));

        var ex = Assert.ThrowsAsync<ApplicationException>(async () =>
            await _useCase.ExecuteAsync(id, payerDocument, amount, description, paymentMethod, createdAt));
        Assert.Multiple(() =>
        {
            Assert.That(ex.Message, Is.EqualTo("Error while processing the PaymentIntent."));
            Assert.That(ex.InnerException, Is.TypeOf<Exception>());
        });
        Assert.That(ex.InnerException.Message, Is.EqualTo("Repository error"));
    }
}