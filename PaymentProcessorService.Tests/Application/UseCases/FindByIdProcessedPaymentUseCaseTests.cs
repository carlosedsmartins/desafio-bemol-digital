using Moq;
using PaymentProcessorService.Application.Interfaces.Repositories;
using PaymentProcessorService.Application.UseCases;
using PaymentProcessorService.Domain.Entities;

namespace PaymentProcessorService.Tests.Application.UseCases;

[TestFixture]
public class FindByIdProcessedPaymentUseCaseTests
{
    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<IProcessedPaymentRepository>();
        _useCase = new FindByIdProcessedPaymentUseCase(_mockRepository.Object);
    }

    private Mock<IProcessedPaymentRepository> _mockRepository;
    private FindByIdProcessedPaymentUseCase _useCase;

    [Test]
    public async Task ExecuteAsync_ShouldReturnProcessedPayment_WhenPaymentExists()
    {
        var id = Guid.NewGuid();
        var processedPayment = new ProcessedPayment(
            id,
            "123456789",
            100.50m,
            "Test Payment",
            "Credit Card",
            DateTime.UtcNow);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(processedPayment);

        var result = await _useCase.ExecuteAsync(id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(processedPayment.Id));
            Assert.That(result.PayerDocument, Is.EqualTo(processedPayment.PayerDocument));
            Assert.That(result.Amount, Is.EqualTo(processedPayment.Amount));
            Assert.That(result.Description, Is.EqualTo(processedPayment.Description));
            Assert.That(result.PaymentMethod, Is.EqualTo(processedPayment.PaymentMethod));
        });
        _mockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public void ExecuteAsync_ShouldThrowInvalidOperationException_WhenPaymentDoesNotExist()
    {
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((ProcessedPayment)null!);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await _useCase.ExecuteAsync(id));
        Assert.That(ex.InnerException, Is.TypeOf<InvalidOperationException>());
        Assert.That(ex.InnerException.Message,
            Is.EqualTo("Payment not found. Please check the provided details and try again."));
        _mockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public void ExecuteAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
    {
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Repository error"));

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await _useCase.ExecuteAsync(id));
        Assert.Multiple(() =>
        {
            Assert.That(ex.Message, Is.EqualTo("Error while processing the PaymentIntent."));
            Assert.That(ex.InnerException, Is.TypeOf<Exception>());
        });
        Assert.That(ex.InnerException.Message, Is.EqualTo("Repository error"));
        _mockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }
}