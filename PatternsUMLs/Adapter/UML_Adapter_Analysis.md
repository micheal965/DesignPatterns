# Adapter Pattern - Detailed Analysis

## Pattern Classification

**Category**: Structural Design Pattern  
**Purpose**: Convert the interface of a class into another interface clients expect  
**Also Known As**: Wrapper

---

## Problem Statement

### What Problem Does Adapter Solve?

You have two incompatible interfaces that need to work together:

1. **Existing Class (Adaptee)**: Has useful functionality but wrong interface
2. **Client Code**: Expects a different interface
3. **Cannot Modify**: Either the client code or the adaptee (or both)

### Real-World Scenario

```
┌─────────────────────────────────────────────────────────┐
│  PROBLEM: Interface Incompatibility                     │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  Your Application         Third-Party Library          │
│  ┌─────────────┐          ┌─────────────┐             │
│  │ Expects:    │    ✗     │ Provides:   │             │
│  │             │  ←─/─→   │             │             │
│  │ ProcessPay()│          │ Charge()    │             │
│  │ RefundPay() │          │ CreateRef() │             │
│  └─────────────┘          └─────────────┘             │
│                                                         │
│  ❌ Interfaces don't match!                            │
│  ❌ Can't modify third-party code!                     │
│  ❌ Can't rewrite your application!                    │
│                                                         │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│  SOLUTION: Adapter Pattern                              │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  Your App      Adapter           Third-Party           │
│  ┌─────────┐  ┌─────────┐       ┌─────────┐          │
│  │Expects: │  │ Adapter │       │Provides:│          │
│  │         │  │         │       │         │          │
│  │Process()│─>│Process()│──────>│Charge() │          │
│  │Refund() │─>│Refund() │──────>│CreateR()│          │
│  └─────────┘  └─────────┘       └─────────┘          │
│                                                         │
│  ✅ Interfaces match!                                  │
│  ✅ No modification to existing code!                  │
│  ✅ Clean separation of concerns!                      │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

## Structure Analysis

### Components Breakdown

#### 1. Target (Interface)
```csharp
public interface ITarget
{
    void Request();
}
```
- **What**: Interface that the client expects
- **Why**: Defines the domain-specific interface
- **When**: Client code uses this interface

#### 2. Adapter (Class)
```csharp
public class Adapter : ITarget
{
    private Adaptee _adaptee;
    
    public Adapter(Adaptee adaptee)
    {
        _adaptee = adaptee;
    }
    
    public void Request()
    {
        // Translate to adaptee's interface
        _adaptee.SpecificRequest();
    }
}
```
- **What**: Converts Target interface to Adaptee interface
- **Why**: Bridges the gap between incompatible interfaces
- **How**: Holds reference to Adaptee and translates calls

#### 3. Adaptee (Existing Class)
```csharp
public class Adaptee
{
    public void SpecificRequest()
    {
        // Existing functionality
    }
}
```
- **What**: Existing class with incompatible interface
- **Why**: Has useful functionality you want to reuse
- **Note**: Usually cannot be modified (third-party, legacy)

#### 4. Client (User Code)
```csharp
class Client
{
    static void Main()
    {
        ITarget target = new Adapter(new Adaptee());
        target.Request();  // Works with expected interface
    }
}
```
- **What**: Code that uses the Target interface
- **Why**: Works with Target, unaware of Adaptee
- **Benefit**: Doesn't need to change when using adapted classes

---

## Adapter Variants

### 1. Object Adapter (Composition) ✅ Recommended

```csharp
┌─────────────────────┐
│  Adapter            │
├─────────────────────┤
│  - adaptee: Adaptee │◄────┐ Composition
├─────────────────────┤     │ (has-a)
│  + Request()        │     │
└─────────────────────┘     │
                            │
                    ┌───────┴──────┐
                    │   Adaptee    │
                    └──────────────┘
```

**Advantages:**
- ✅ More flexible
- ✅ Can adapt multiple adaptees
- ✅ Can adapt all subclasses of Adaptee
- ✅ Works in single-inheritance languages (C#, Java)

**Code:**
```csharp
public class Adapter : ITarget
{
    private Adaptee _adaptee;  // Composition
    
    public void Request()
    {
        _adaptee.SpecificRequest();
    }
}
```

### 2. Class Adapter (Inheritance) ❌ Not Available in C#

```csharp
┌─────────────────────┐
│  Adapter            │
│  : ITarget, Adaptee │◄────┐ Multiple Inheritance
├─────────────────────┤     │ (is-a both)
│  + Request()        │     │
└─────────────────────┘     │
         △                  │
         │                  │
    ┌────┴────┐            │
    │         │            │
ITarget    Adaptee◄────────┘
```

**Not possible in C#** because:
- C# doesn't support multiple inheritance
- Can only inherit from one class
- Works in C++ with multiple inheritance

---

## Interaction Flow

### Detailed Sequence

```
1. Client creates Adapter with Adaptee instance
   Client ──> new Adapter(adaptee)

2. Client calls method on Target interface
   Client ──> target.Request()

3. Adapter receives call
   Adapter.Request() invoked

4. Adapter translates to Adaptee's interface
   Adapter ──> adaptee.SpecificRequest()

5. Adaptee performs actual work
   Adaptee.SpecificRequest() executes

6. Result flows back through Adapter to Client
   Adaptee ──> Adapter ──> Client
```

### Data Flow Diagram

```
┌────────┐                                         ┌─────────┐
│ Client │                                         │ Adaptee │
└───┬────┘                                         └────▲────┘
    │                                                   │
    │  1. Request()                                     │
    ├──────────────────────────────┐                   │
    │                              │                   │
    │                      ┌───────▼────────┐          │
    │                      │    Adapter     │          │
    │                      ├────────────────┤          │
    │                      │ + Request()    │          │
    │                      │   {            │          │
    │                      │     Transform  │  2. SpecificRequest()
    │                      │     Parameters │──────────┘
    │                      │     ↓          │
    │                      │     adaptee.   │
    │                      │     Specific() │
    │                      │   }            │
    │                      └────────────────┘
    │                              │
    │  3. Return Result            │
    │◄─────────────────────────────┘
    │
```

---

## Implementation Details

### Complete Payment Adapter Example

```csharp
// ============================================
// TARGET INTERFACE - What your app expects
// ============================================

public interface IPaymentProcessor
{
    PaymentResult ProcessPayment(decimal amount, CreditCard card);
    bool RefundPayment(string transactionId, decimal amount);
    PaymentStatus GetStatus(string transactionId);
}

public class PaymentResult
{
    public bool Success { get; set; }
    public string TransactionId { get; set; }
    public string Message { get; set; }
}

public class CreditCard
{
    public string Number { get; set; }
    public string CVV { get; set; }
    public string ExpiryDate { get; set; }
    public string HolderName { get; set; }
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}

// ============================================
// ADAPTEE - Stripe API (Third-party)
// ============================================

public class StripePaymentGateway
{
    // Stripe's actual API methods
    public StripeChargeResponse CreateCharge(
        int amountInCents,
        string currency,
        StripeTokenRequest token)
    {
        Console.WriteLine($"Stripe: Processing ${amountInCents / 100.0}");
        
        return new StripeChargeResponse
        {
            Id = $"ch_{Guid.NewGuid().ToString().Substring(0, 8)}",
            Status = "succeeded",
            Amount = amountInCents
        };
    }

    public StripeRefundResponse CreateRefund(string chargeId, int? amount = null)
    {
        Console.WriteLine($"Stripe: Creating refund for {chargeId}");
        
        return new StripeRefundResponse
        {
            Id = $"re_{Guid.NewGuid().ToString().Substring(0, 8)}",
            Status = "succeeded"
        };
    }

    public StripeChargeResponse RetrieveCharge(string chargeId)
    {
        Console.WriteLine($"Stripe: Retrieving charge {chargeId}");
        
        return new StripeChargeResponse
        {
            Id = chargeId,
            Status = "succeeded",
            Amount = 10000
        };
    }
}

// Stripe's data structures
public class StripeTokenRequest
{
    public string CardNumber { get; set; }
    public string Cvc { get; set; }
    public int ExpMonth { get; set; }
    public int ExpYear { get; set; }
}

public class StripeChargeResponse
{
    public string Id { get; set; }
    public string Status { get; set; }
    public int Amount { get; set; }
}

public class StripeRefundResponse
{
    public string Id { get; set; }
    public string Status { get; set; }
}

// ============================================
// ADAPTER - Stripe Adapter
// ============================================

public class StripePaymentAdapter : IPaymentProcessor
{
    private readonly StripePaymentGateway _stripe;

    public StripePaymentAdapter()
    {
        _stripe = new StripePaymentGateway();
    }

    public PaymentResult ProcessPayment(decimal amount, CreditCard card)
    {
        try
        {
            // Transform your data model to Stripe's model
            var token = new StripeTokenRequest
            {
                CardNumber = card.Number,
                Cvc = card.CVV,
                ExpMonth = int.Parse(card.ExpiryDate.Split('/')[0]),
                ExpYear = int.Parse(card.ExpiryDate.Split('/')[1])
            };

            // Transform amount: dollars to cents
            int amountInCents = (int)(amount * 100);

            // Call Stripe's API
            var response = _stripe.CreateCharge(amountInCents, "usd", token);

            // Transform Stripe's response to your model
            return new PaymentResult
            {
                Success = response.Status == "succeeded",
                TransactionId = response.Id,
                Message = response.Status == "succeeded" 
                    ? "Payment processed successfully" 
                    : "Payment failed"
            };
        }
        catch (Exception ex)
        {
            return new PaymentResult
            {
                Success = false,
                TransactionId = null,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public bool RefundPayment(string transactionId, decimal amount)
    {
        try
        {
            int amountInCents = (int)(amount * 100);
            var response = _stripe.CreateRefund(transactionId, amountInCents);
            return response.Status == "succeeded";
        }
        catch
        {
            return false;
        }
    }

    public PaymentStatus GetStatus(string transactionId)
    {
        try
        {
            var charge = _stripe.RetrieveCharge(transactionId);
            
            // Map Stripe status to your status enum
            return charge.Status switch
            {
                "succeeded" => PaymentStatus.Completed,
                "pending" => PaymentStatus.Pending,
                "failed" => PaymentStatus.Failed,
                "refunded" => PaymentStatus.Refunded,
                _ => PaymentStatus.Failed
            };
        }
        catch
        {
            return PaymentStatus.Failed;
        }
    }
}

// ============================================
// ADAPTEE - PayPal API (Another third-party)
// ============================================

public class PayPalSDK
{
    public PayPalPaymentResponse ExecutePayment(
        double amount,
        string accountEmail,
        PayPalCreditCard card)
    {
        Console.WriteLine($"PayPal: Processing ${amount} to {accountEmail}");
        
        return new PayPalPaymentResponse
        {
            PaymentId = $"PAY-{Guid.NewGuid().ToString().Substring(0, 8)}",
            State = "approved",
            TotalAmount = amount
        };
    }

    public bool RefundTransaction(string paymentId)
    {
        Console.WriteLine($"PayPal: Refunding {paymentId}");
        return true;
    }

    public PayPalPaymentInfo GetPaymentInfo(string paymentId)
    {
        return new PayPalPaymentInfo
        {
            PaymentId = paymentId,
            State = "approved"
        };
    }
}

public class PayPalCreditCard
{
    public string Number { get; set; }
    public string Type { get; set; }
    public int ExpireMonth { get; set; }
    public int ExpireYear { get; set; }
}

public class PayPalPaymentResponse
{
    public string PaymentId { get; set; }
    public string State { get; set; }
    public double TotalAmount { get; set; }
}

public class PayPalPaymentInfo
{
    public string PaymentId { get; set; }
    public string State { get; set; }
}

// ============================================
// ADAPTER - PayPal Adapter
// ============================================

public class PayPalPaymentAdapter : IPaymentProcessor
{
    private readonly PayPalSDK _paypal;

    public PayPalPaymentAdapter()
    {
        _paypal = new PayPalSDK();
    }

    public PaymentResult ProcessPayment(decimal amount, CreditCard card)
    {
        try
        {
            // Transform to PayPal's data model
            var paypalCard = new PayPalCreditCard
            {
                Number = card.Number,
                Type = DetermineCardType(card.Number),
                ExpireMonth = int.Parse(card.ExpiryDate.Split('/')[0]),
                ExpireYear = int.Parse(card.ExpiryDate.Split('/')[1])
            };

            string accountEmail = $"customer_{card.Number.Substring(card.Number.Length - 4)}@email.com";

            // Call PayPal's API
            var response = _paypal.ExecutePayment((double)amount, accountEmail, paypalCard);

            // Transform response
            return new PaymentResult
            {
                Success = response.State == "approved",
                TransactionId = response.PaymentId,
                Message = response.State == "approved" 
                    ? "Payment approved" 
                    : "Payment not approved"
            };
        }
        catch (Exception ex)
        {
            return new PaymentResult
            {
                Success = false,
                TransactionId = null,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public bool RefundPayment(string transactionId, decimal amount)
    {
        try
        {
            return _paypal.RefundTransaction(transactionId);
        }
        catch
        {
            return false;
        }
    }

    public PaymentStatus GetStatus(string transactionId)
    {
        try
        {
            var info = _paypal.GetPaymentInfo(transactionId);
            
            return info.State switch
            {
                "approved" => PaymentStatus.Completed,
                "pending" => PaymentStatus.Pending,
                "failed" => PaymentStatus.Failed,
                _ => PaymentStatus.Failed
            };
        }
        catch
        {
            return PaymentStatus.Failed;
        }
    }

    private string DetermineCardType(string cardNumber)
    {
        if (cardNumber.StartsWith("4")) return "visa";
        if (cardNumber.StartsWith("5")) return "mastercard";
        return "unknown";
    }
}

// ============================================
// CLIENT CODE
// ============================================

class PaymentSystem
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║         ADAPTER PATTERN: PAYMENT SYSTEM                ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        var card = new CreditCard
        {
            Number = "4532-1234-5678-9010",
            CVV = "123",
            ExpiryDate = "12/25",
            HolderName = "John Doe"
        };

        // Use Stripe
        Console.WriteLine("Processing with Stripe:");
        Console.WriteLine(new string('-', 56));
        ProcessPaymentWithProvider(new StripePaymentAdapter(), 99.99m, card);
        
        Console.WriteLine();
        
        // Use PayPal
        Console.WriteLine("Processing with PayPal:");
        Console.WriteLine(new string('-', 56));
        ProcessPaymentWithProvider(new PayPalPaymentAdapter(), 149.99m, card);

        Console.WriteLine("\n✅ Same interface, different implementations!");
        Console.WriteLine("   Easy to switch payment providers.");
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static void ProcessPaymentWithProvider(
        IPaymentProcessor processor,
        decimal amount,
        CreditCard card)
    {
        // Process payment
        var result = processor.ProcessPayment(amount, card);
        
        if (result.Success)
        {
            Console.WriteLine($"✅ Payment successful!");
            Console.WriteLine($"   Transaction ID: {result.TransactionId}");
            Console.WriteLine($"   Amount: ${amount}");
            
            // Check status
            var status = processor.GetStatus(result.TransactionId);
            Console.WriteLine($"   Status: {status}");
            
            // Refund
            Console.WriteLine($"\nProcessing refund...");
            bool refunded = processor.RefundPayment(result.TransactionId, amount);
            Console.WriteLine(refunded ? "✅ Refund successful" : "❌ Refund failed");
        }
        else
        {
            Console.WriteLine($"❌ Payment failed: {result.Message}");
        }
    }
}
```

---

## Transformation Types

Adapters can perform various transformations:

### 1. Method Name Translation
```csharp
// Target expects: ProcessPayment()
// Adaptee provides: Charge()

public void ProcessPayment(decimal amount)
{
    _adaptee.Charge(amount);  // Name translation
}
```

### 2. Parameter Transformation
```csharp
// Target expects: dollars (decimal)
// Adaptee expects: cents (int)

public void ProcessPayment(decimal dollars)
{
    int cents = (int)(dollars * 100);
    _adaptee.Charge(cents);  // Parameter transformation
}
```

### 3. Data Structure Conversion
```csharp
// Target expects: CreditCard object
// Adaptee expects: Token string

public void ProcessPayment(CreditCard card)
{
    string token = ConvertToToken(card);
    _adaptee.Charge(token);  // Data structure conversion
}
```

### 4. Return Value Mapping
```csharp
// Target returns: PaymentResult object
// Adaptee returns: StripeResponse object

public PaymentResult ProcessPayment(decimal amount)
{
    var stripeResponse = _adaptee.Charge(amount);
    
    return new PaymentResult  // Return value mapping
    {
        Success = stripeResponse.Status == "succeeded",
        TransactionId = stripeResponse.Id
    };
}
```

### 5. Exception Translation
```csharp
public PaymentResult ProcessPayment(decimal amount)
{
    try
    {
        _adaptee.Charge(amount);
        return new PaymentResult { Success = true };
    }
    catch (StripeException ex)  // Stripe-specific exception
    {
        // Translate to generic exception or result
        return new PaymentResult 
        { 
            Success = false, 
            Message = "Payment failed" 
        };
    }
}
```

---

## Common Pitfalls

### ❌ Pitfall 1: Adding Business Logic

```csharp
// ❌ WRONG - Adapter contains business logic
public class BadAdapter : IPaymentProcessor
{
    public PaymentResult ProcessPayment(decimal amount, CreditCard card)
    {
        // ❌ Business logic doesn't belong in adapter
        if (amount > 1000)
        {
            SendFraudAlert();
            RequireAdditionalVerification();
        }
        
        // ❌ Adapter should only adapt, not add features
        LogTransaction(amount, card);
        UpdateCustomerRewards(amount);
        
        return _adaptee.Charge(amount);
    }
}

// ✅ CORRECT - Adapter only adapts
public class GoodAdapter : IPaymentProcessor
{
    public PaymentResult ProcessPayment(decimal amount, CreditCard card)
    {
        // ✅ Only translate interface
        var token = ConvertToToken(card);
        var response = _adaptee.Charge((int)(amount * 100), token);
        return MapToPaymentResult(response);
    }
}
```

### ❌ Pitfall 2: Two-Way Coupling

```csharp
// ❌ WRONG - Adapter depends on concrete Adaptee
public class BadAdapter : IPaymentProcessor
{
    private StripeAPI _stripe;  // ❌ Tight coupling
    
    public void ProcessPayment(decimal amount)
    {
        _stripe.Charge(amount);
        
        // ❌ Using Stripe-specific methods
        var stripeFeatures = _stripe.GetAdvancedFeatures();
    }
}

// ✅ CORRECT - Adapter depends on abstraction
public class GoodAdapter : IPaymentProcessor
{
    private IPaymentGateway _gateway;  // ✅ Depends on abstraction
    
    public void ProcessPayment(decimal amount)
    {
        _gateway.Charge(amount);
    }
}
```

### ❌ Pitfall 3: Incomplete Adaptation

```csharp
// ❌ WRONG - Incomplete implementation
public class BadAdapter : IPaymentProcessor
{
    public PaymentResult ProcessPayment(decimal amount, CreditCard card)
    {
        _adaptee.Charge(amount);
        // ❌ Forgot to return result
    }
    
    public bool RefundPayment(string transactionId)
    {
        // ❌ Not implemented
        throw new NotImplementedException();
    }
}

// ✅ CORRECT - Complete implementation
public class GoodAdapter : IPaymentProcessor
{
    public PaymentResult ProcessPayment(decimal amount, CreditCard card)
    {
        var response = _adaptee.Charge(amount);
        return MapToResult(response);  // ✅ Complete adaptation
    }
    
    public bool RefundPayment(string transactionId)
    {
        var response = _adaptee.Refund(transactionId);
        return response.Success;  // ✅ Fully implemented
    }
}
```

---

## Testing Adapters

### Unit Test Example

```csharp
[TestClass]
public class StripePaymentAdapterTests
{
    [TestMethod]
    public void ProcessPayment_ValidCard_ReturnsSuccess()
    {
        // Arrange
        var adapter = new StripePaymentAdapter();
        var card = new CreditCard
        {
            Number = "4532123456789010",
            CVV = "123",
            ExpiryDate = "12/25"
        };

        // Act
        var result = adapter.ProcessPayment(100m, card);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.TransactionId);
    }

    [TestMethod]
    public void ProcessPayment_TransformsAmountCorrectly()
    {
        // Arrange
        var mockStripe = new Mock<StripePaymentGateway>();
        var adapter = new StripePaymentAdapter(mockStripe.Object);

        // Act
        adapter.ProcessPayment(99.99m, new CreditCard());

        // Assert - Verify dollars converted to cents
        mockStripe.Verify(s => s.CreateCharge(
            9999,  // 99.99 dollars = 9999 cents
            It.IsAny<string>(),
            It.IsAny<StripeTokenRequest>()
        ), Times.Once);
    }
}
```

---

## Performance Considerations

### Overhead Analysis

```csharp
// Direct call (no adapter)
stripeAPI.Charge(100);  // 1 method call

// With adapter
IPaymentProcessor processor = new StripeAdapter();
processor.ProcessPayment(100);  
// 1. processor.ProcessPayment() - adapter method
// 2. Transform parameters
// 3. stripeAPI.Charge() - actual call
// 4. Transform result
// = 1 extra layer + transformation overhead
```

**Overhead:**
- ✅ Minimal: One extra method call
- ✅ Negligible: Parameter transformation is fast
- ❌ Can add up if adapter is complex
- ❌ May matter in high-performance scenarios

**Optimization:**
```csharp
// Cache transformed data if used multiple times
public class OptimizedAdapter : IPaymentProcessor
{
    private Dictionary<string, string> _tokenCache = new();

    public PaymentResult ProcessPayment(CreditCard card)
    {
        // Cache token transformation
        if (!_tokenCache.ContainsKey(card.Number))
        {
            _tokenCache[card.Number] = TransformToToken(card);
        }
        
        return _adaptee.Charge(_tokenCache[card.Number]);
    }
}
```

---

## Summary

### Key Points

1. **Purpose**: Make incompatible interfaces work together
2. **Structure**: Adapter wraps Adaptee and implements Target interface
3. **Benefit**: Reuse existing code without modification
4. **Tradeoff**: Extra indirection layer

### When to Use

✅ **Use Adapter when:**
- Integrating third-party libraries
- Working with legacy code
- Need consistent interface across different implementations
- Cannot modify existing classes

❌ **Don't use Adapter when:**
- You control both interfaces (refactor instead)
- Simple wrapper is enough (use Facade)
- Adding functionality (use Decorator)

### Design Principles

- **Single Responsibility**: Adapter only translates interfaces
- **Open/Closed**: Add new adapters without changing existing code
- **Dependency Inversion**: Depend on abstractions (ITarget), not concretions

---

## Quick Reference

| Aspect | Description |
|--------|-------------|
| **Type** | Structural Pattern |
| **Purpose** | Interface Conversion |
| **Method** | Composition (Object Adapter) |
| **Key Benefit** | Reuse incompatible code |
| **Main Tradeoff** | Extra indirection |
| **C# Support** | Object Adapter only |
| **Complexity** | Low to Medium |
| **Usage Frequency** | Very Common |

**Remember:** 🔌 Adapter = Interface Translator (Like USB-C to USB-A converter)
