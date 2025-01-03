using PaymentIntentService.Domain.Entities;

namespace PaymentIntentService.Tests.Domain.Entities;

[TestFixture]
public class PaymentIntentTests
{
    [Test]
    public void Constructor_ShouldInitializeProperties_WhenValidParametersProvided()
    {
        const string payerDocument = "123456789";
        const decimal amount = 100.50m;
        const string description = "Payment for service";
        const string paymentMethod = "Credit Card";

        var paymentIntent = new PaymentIntent(payerDocument, amount, description, paymentMethod);

        Assert.Multiple(() =>
        {
            Assert.That(paymentIntent.PayerDocument, Is.EqualTo(payerDocument));
            Assert.That(paymentIntent.Amount, Is.EqualTo(amount));
            Assert.That(paymentIntent.Description, Is.EqualTo(description));
            Assert.That(paymentIntent.PaymentMethod, Is.EqualTo(paymentMethod));
            Assert.That(paymentIntent.Id, Is.Not.EqualTo(Guid.Empty));

            Assert.That(paymentIntent.CreatedAt, Is.EqualTo(DateTime.UtcNow).Within(1).Seconds);
        });
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenPayerDocumentIsNullOrEmpty()
    {
        const decimal amount = 100.50m;
        const string description = "Payment for service";
        const string paymentMethod = "Credit Card";

        Assert.Throws<ArgumentNullException>(() => _ = new PaymentIntent(null!, amount, description, paymentMethod));
        Assert.Throws<ArgumentNullException>(() =>
            _ = new PaymentIntent(string.Empty, amount, description, paymentMethod));
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenPaymentMethodIsNullOrEmpty()
    {
        const string payerDocument = "123456789";
        const decimal amount = 100.50m;
        const string description = "Payment for service";

        Assert.Throws<ArgumentNullException>(() => _ = new PaymentIntent(payerDocument, amount, description, null!));
        Assert.Throws<ArgumentNullException>(() =>
            _ = new PaymentIntent(payerDocument, amount, description, string.Empty));
    }

    [Test]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenAmountIsNegativeOrZero()
    {
        const string payerDocument = "123456789";
        const string description = "Payment for service";
        const string paymentMethod = "Credit Card";

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _ = new PaymentIntent(payerDocument, 0, description, paymentMethod));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _ = new PaymentIntent(payerDocument, -1, description, paymentMethod));
    }
}