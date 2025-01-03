using PaymentProcessorService.Domain.Entities;

namespace PaymentProcessorService.Tests.Domain.Entities;

[TestFixture]
public class ProcessedPaymentTests
{
    [Test]
    public void Constructor_ShouldInitializeProperties_WhenValidParametersProvided()
    {
        var id = Guid.NewGuid();
        const string payerDocument = "123456789";
        const decimal amount = 150.75m;
        const string description = "Test Payment";
        const string paymentMethod = "Credit Card";
        var createdAt = DateTime.UtcNow;

        var processedPayment = new ProcessedPayment(
            id,
            payerDocument,
            amount,
            description,
            paymentMethod,
            createdAt
        );

        Assert.Multiple(() =>
        {
            Assert.That(processedPayment.Id, Is.EqualTo(id));
            Assert.That(processedPayment.PayerDocument, Is.EqualTo(payerDocument));
            Assert.That(processedPayment.Amount, Is.EqualTo(amount));
            Assert.That(processedPayment.Description, Is.EqualTo(description));
            Assert.That(processedPayment.PaymentMethod, Is.EqualTo(paymentMethod));
            Assert.That(processedPayment.CreatedAt, Is.EqualTo(createdAt));
            Assert.That(processedPayment.ProcessedAt, Is.EqualTo(DateTime.UtcNow).Within(1).Seconds);
        });
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenPayerDocumentIsNullOrEmpty()
    {
        var id = Guid.NewGuid();
        const decimal amount = 150.75m;
        const string description = "Test Payment";
        const string paymentMethod = "Credit Card";
        var createdAt = DateTime.UtcNow;

        Assert.Throws<ArgumentNullException>(() =>
            _ = new ProcessedPayment(id, null!, amount, description, paymentMethod, createdAt));
        Assert.Throws<ArgumentNullException>(() =>
            _ = new ProcessedPayment(id, string.Empty, amount, description, paymentMethod, createdAt));
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenPaymentMethodIsNullOrEmpty()
    {
        var id = Guid.NewGuid();
        const string payerDocument = "123456789";
        const decimal amount = 150.75m;
        const string description = "Test Payment";
        var createdAt = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _ = new ProcessedPayment(id, payerDocument, amount, description, null!, createdAt));
        Assert.Throws<ArgumentNullException>(() =>
            _ = new ProcessedPayment(id, payerDocument, amount, description, string.Empty, createdAt));
    }

    [Test]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenAmountIsNegativeOrZero()
    {
        var id = Guid.NewGuid();
        const string payerDocument = "123456789";
        const string description = "Test Payment";
        const string paymentMethod = "Credit Card";
        var createdAt = DateTime.UtcNow;

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _ = new ProcessedPayment(id, payerDocument, 0, description, paymentMethod, createdAt));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _ = new ProcessedPayment(id, payerDocument, -1, description, paymentMethod, createdAt));
    }
}