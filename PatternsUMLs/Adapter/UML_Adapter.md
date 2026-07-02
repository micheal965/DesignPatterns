# UML Diagram: Adapter Pattern

## What is Adapter Pattern?

**Adapter** is a structural design pattern that allows objects with incompatible interfaces to collaborate. It acts as a bridge between two incompatible interfaces by wrapping one interface to make it compatible with another.

### Key Concept:
Just like a power adapter converts one type of electrical plug to another, the Adapter pattern converts one interface to another that the client expects.

### Real-World Analogy:
Think of a **travel power adapter**. You have a US plug (220V) but need to use it in Europe (110V). The adapter doesn't change the device or the power outlet - it just translates between them.

Another example: **Memory card reader** - your laptop has a USB port, but your camera uses an SD card. The card reader adapts the SD card interface to work with USB.

---

## Class Diagram

```
┌─────────────────────────────────────┐
│  Client                             │
│                                     │
│  Uses Target interface              │
└──────────────┬──────────────────────┘
               │ uses
               │
               ↓
┌─────────────────────────────────────┐
│  <<interface>>                      │
│  ITarget                            │
├─────────────────────────────────────┤
│  + Request(): void                  │
└─────────────────────────────────────┘
               △
               │ implements
               │
┌──────────────┴──────────────────────┐
│  Adapter                            │
├─────────────────────────────────────┤
│  - adaptee: Adaptee                 │◄─────┐
├─────────────────────────────────────┤      │ has-a
│  + Request(): void                  │      │ (composition)
└─────────────────────────────────────┘      │
                                              │
                              ┌───────────────┴──────────┐
                              │  Adaptee                 │
                              ├──────────────────────────┤
                              │  + SpecificRequest():    │
                              │    void                  │
                              └──────────────────────────┘
```

## Object Adapter vs Class Adapter

### Object Adapter (Composition) - Recommended in C#

```
┌─────────────────┐         ┌──────────────────┐
│  ITarget        │         │  Adaptee         │
├─────────────────┤         ├──────────────────┤
│ + Request()     │         │ + SpecificReq()  │
└─────────────────┘         └──────────────────┘
        △                            △
        │                            │
        │ implements                 │ has-a
        │                            │
┌───────┴────────────────────────────┴───┐
│  Adapter                               │
├────────────────────────────────────────┤
│  - adaptee: Adaptee                    │
├────────────────────────────────────────┤
│  + Request():                          │
│    {                                   │
│      adaptee.SpecificRequest()         │
│    }                                   │
└────────────────────────────────────────┘
```

### Class Adapter (Multiple Inheritance) - Not available in C#

```
┌─────────────────┐         ┌──────────────────┐
│  ITarget        │         │  Adaptee         │
├─────────────────┤         ├──────────────────┤
│ + Request()     │         │ + SpecificReq()  │
└─────────────────┘         └──────────────────┘
        △                            △
        │                            │
        └────────────┬───────────────┘
                     │ multiple inheritance
                     │ (not possible in C#)
              ┌──────┴───────┐
              │  Adapter     │
              ├──────────────┤
              │ + Request()  │
              └──────────────┘
```

**Note**: C# doesn't support multiple inheritance, so we use **Object Adapter** (composition).

---

## Real-World Example: Payment Gateway Adapter

```
┌─────────────────────────────────────────────────────────────┐
│                    E-Commerce Application                   │
└──────────────────────┬──────────────────────────────────────┘
                       │ uses
                       ↓
┌─────────────────────────────────────────┐
│  <<interface>>                          │
│  IPaymentProcessor                      │
├─────────────────────────────────────────┤
│  + ProcessPayment(amount, card): bool   │
│  + RefundPayment(transId): bool         │
└─────────────────────────────────────────┘
               △                 △
               │                 │
               │                 │
┌──────────────┴───┐     ┌──────┴─────────────┐
│  StripeAdapter   │     │  PayPalAdapter     │
├──────────────────┤     ├────────────────────┤
│  - stripe:       │     │  - paypal:         │
│    StripeAPI     │◄───┐│    PayPalSDK       │◄───┐
├──────────────────┤    ││├────────────────────┤    │
│  + ProcessPay()  │    ││  + ProcessPay()    │    │
│  + RefundPay()   │    ││  + RefundPay()     │    │
└──────────────────┘    │└────────────────────┘    │
                        │                           │
         ┌──────────────┘                ┌──────────┘
         │                               │
┌────────┴──────────┐          ┌────────┴──────────┐
│  StripeAPI        │          │  PayPalSDK        │
│  (3rd Party)      │          │  (3rd Party)      │
├───────────────────┤          ├───────────────────┤
│  + Charge()       │          │  + SendPayment()  │
│  + CreateRefund() │          │  + RefundOrder()  │
└───────────────────┘          └───────────────────┘
```

---

## Sequence Diagram: Payment Processing

```
Client          IPayment        StripeAdapter       StripeAPI
  │             Processor             │                 │
  │                 │                 │                 │
  │  ProcessPayment(100, card)        │                 │
  ├────────────────>│                 │                 │
  │                 │                 │                 │
  │                 │  ProcessPayment()│                │
  │                 ├────────────────>│                 │
  │                 │                 │                 │
  │                 │                 │  Charge(100,    │
  │                 │                 │  card_token)    │
  │                 │                 ├────────────────>│
  │                 │                 │                 │
  │                 │                 │  Success        │
  │                 │                 │<────────────────┤
  │                 │                 │                 │
  │                 │  return true    │                 │
  │                 │<────────────────┤                 │
  │                 │                 │                 │
  │  return true    │                 │                 │
  │<────────────────┤                 │                 │
  │                 │                 │                 │
```

---

## Complete Code Example: Media Player Adapter

```csharp
// ============================================
// TARGET INTERFACE - What client expects
// ============================================

public interface IMediaPlayer
{
    void Play(string audioType, string fileName);
}

// ============================================
// ADAPTEE - Incompatible existing classes
// ============================================

public interface IAdvancedMediaPlayer
{
    void PlayVlc(string fileName);
    void PlayMp4(string fileName);
}

public class VlcPlayer : IAdvancedMediaPlayer
{
    public void PlayVlc(string fileName)
    {
        Console.WriteLine($"Playing VLC file: {fileName}");
    }

    public void PlayMp4(string fileName)
    {
        // VlcPlayer doesn't support MP4
    }
}

public class Mp4Player : IAdvancedMediaPlayer
{
    public void PlayVlc(string fileName)
    {
        // Mp4Player doesn't support VLC
    }

    public void PlayMp4(string fileName)
    {
        Console.WriteLine($"Playing MP4 file: {fileName}");
    }
}

// ============================================
// ADAPTER - Makes Adaptee compatible with Target
// ============================================

public class MediaAdapter : IMediaPlayer
{
    private IAdvancedMediaPlayer advancedPlayer;

    public MediaAdapter(string audioType)
    {
        if (audioType.Equals("vlc", StringComparison.OrdinalIgnoreCase))
        {
            advancedPlayer = new VlcPlayer();
        }
        else if (audioType.Equals("mp4", StringComparison.OrdinalIgnoreCase))
        {
            advancedPlayer = new Mp4Player();
        }
    }

    public void Play(string audioType, string fileName)
    {
        if (audioType.Equals("vlc", StringComparison.OrdinalIgnoreCase))
        {
            advancedPlayer.PlayVlc(fileName);
        }
        else if (audioType.Equals("mp4", StringComparison.OrdinalIgnoreCase))
        {
            advancedPlayer.PlayMp4(fileName);
        }
    }
}

// ============================================
// CONCRETE TARGET - Audio Player
// ============================================

public class AudioPlayer : IMediaPlayer
{
    private MediaAdapter mediaAdapter;

    public void Play(string audioType, string fileName)
    {
        // Built-in support for MP3
        if (audioType.Equals("mp3", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Playing MP3 file: {fileName}");
        }
        // Use adapter for other formats
        else if (audioType.Equals("vlc", StringComparison.OrdinalIgnoreCase) ||
                 audioType.Equals("mp4", StringComparison.OrdinalIgnoreCase))
        {
            mediaAdapter = new MediaAdapter(audioType);
            mediaAdapter.Play(audioType, fileName);
        }
        else
        {
            Console.WriteLine($"Invalid media format: {audioType}");
        }
    }
}

// ============================================
// CLIENT CODE
// ============================================

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║            ADAPTER PATTERN: MEDIA PLAYER               ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        AudioPlayer audioPlayer = new AudioPlayer();

        Console.WriteLine("Testing different media formats:\n");
        
        audioPlayer.Play("mp3", "song.mp3");
        audioPlayer.Play("mp4", "video.mp4");
        audioPlayer.Play("vlc", "movie.vlc");
        audioPlayer.Play("avi", "video.avi");

        Console.WriteLine("\n✅ Adapter allows playing incompatible formats!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
```

**Output:**
```
╔════════════════════════════════════════════════════════╗
║            ADAPTER PATTERN: MEDIA PLAYER               ║
╚════════════════════════════════════════════════════════╝

Testing different media formats:

Playing MP3 file: song.mp3
Playing MP4 file: video.mp4
Playing VLC file: movie.vlc
Invalid media format: avi

✅ Adapter allows playing incompatible formats!
```

---

## Example: Payment Gateway Adapter

```csharp
// ============================================
// TARGET INTERFACE
// ============================================

public interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount, string cardNumber);
    bool RefundPayment(string transactionId);
}

// ============================================
// ADAPTEE - Third-party Stripe API
// ============================================

public class StripeAPI
{
    public string Charge(int amountInCents, string token)
    {
        Console.WriteLine($"Stripe: Charging ${amountInCents / 100.0} using token {token}");
        return $"stripe_tx_{Guid.NewGuid().ToString().Substring(0, 8)}";
    }

    public bool CreateRefund(string chargeId)
    {
        Console.WriteLine($"Stripe: Refunding charge {chargeId}");
        return true;
    }
}

// ============================================
// ADAPTEE - Third-party PayPal SDK
// ============================================

public class PayPalSDK
{
    public string SendPayment(double amount, string email)
    {
        Console.WriteLine($"PayPal: Sending ${amount} to {email}");
        return $"paypal_tx_{Guid.NewGuid().ToString().Substring(0, 8)}";
    }

    public bool RefundOrder(string orderId)
    {
        Console.WriteLine($"PayPal: Refunding order {orderId}");
        return true;
    }
}

// ============================================
// ADAPTER - Stripe Adapter
// ============================================

public class StripeAdapter : IPaymentProcessor
{
    private readonly StripeAPI _stripeAPI;
    private string _lastTransactionId;

    public StripeAdapter()
    {
        _stripeAPI = new StripeAPI();
    }

    public bool ProcessPayment(decimal amount, string cardNumber)
    {
        // Convert dollars to cents
        int amountInCents = (int)(amount * 100);
        
        // Stripe uses tokens instead of card numbers
        string token = $"tok_{cardNumber.Substring(cardNumber.Length - 4)}";
        
        _lastTransactionId = _stripeAPI.Charge(amountInCents, token);
        
        return !string.IsNullOrEmpty(_lastTransactionId);
    }

    public bool RefundPayment(string transactionId)
    {
        return _stripeAPI.CreateRefund(transactionId);
    }
}

// ============================================
// ADAPTER - PayPal Adapter
// ============================================

public class PayPalAdapter : IPaymentProcessor
{
    private readonly PayPalSDK _paypalSDK;
    private string _lastTransactionId;

    public PayPalAdapter()
    {
        _paypalSDK = new PayPalSDK();
    }

    public bool ProcessPayment(decimal amount, string cardNumber)
    {
        // PayPal needs email, simulate extracting from card
        string email = $"customer_{cardNumber.Substring(cardNumber.Length - 4)}@email.com";
        
        _lastTransactionId = _paypalSDK.SendPayment((double)amount, email);
        
        return !string.IsNullOrEmpty(_lastTransactionId);
    }

    public bool RefundPayment(string transactionId)
    {
        return _paypalSDK.RefundOrder(transactionId);
    }
}

// ============================================
// CLIENT CODE
// ============================================

class PaymentDemo
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║         ADAPTER PATTERN: PAYMENT PROCESSING            ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        // Client code works with IPaymentProcessor interface
        ProcessOrder(new StripeAdapter(), 99.99m, "4532-1234-5678-9010");
        Console.WriteLine();
        
        ProcessOrder(new PayPalAdapter(), 149.99m, "5105-1051-0510-5100");
        Console.WriteLine();

        Console.WriteLine("✅ Different payment gateways adapted to same interface!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static void ProcessOrder(IPaymentProcessor paymentProcessor, decimal amount, string card)
    {
        Console.WriteLine($"Processing payment of ${amount}...");
        
        bool success = paymentProcessor.ProcessPayment(amount, card);
        
        if (success)
        {
            Console.WriteLine("✅ Payment successful!");
        }
        else
        {
            Console.WriteLine("❌ Payment failed!");
        }
    }
}
```

**Output:**
```
╔════════════════════════════════════════════════════════╗
║         ADAPTER PATTERN: PAYMENT PROCESSING            ║
╚════════════════════════════════════════════════════════╝

Processing payment of $99.99...
Stripe: Charging $99.99 using token tok_9010
✅ Payment successful!

Processing payment of $149.99...
PayPal: Sending $149.99 to customer_5100@email.com
✅ Payment successful!

✅ Different payment gateways adapted to same interface!
```

---

## Example: Data Source Adapter

```csharp
// ============================================
// TARGET INTERFACE
// ============================================

public interface IDataProvider
{
    List<string> GetData();
}

// ============================================
// ADAPTEE - Legacy Database System
// ============================================

public class LegacyDatabase
{
    public string[] FetchRecords()
    {
        Console.WriteLine("Fetching from Legacy Database...");
        return new string[] { "Record1", "Record2", "Record3" };
    }
}

// ============================================
// ADAPTEE - Modern REST API
// ============================================

public class RestAPI
{
    public Dictionary<int, string> GetJsonData()
    {
        Console.WriteLine("Fetching from REST API...");
        return new Dictionary<int, string>
        {
            { 1, "Data1" },
            { 2, "Data2" },
            { 3, "Data3" }
        };
    }
}

// ============================================
// ADAPTER - Database Adapter
// ============================================

public class DatabaseAdapter : IDataProvider
{
    private readonly LegacyDatabase _database;

    public DatabaseAdapter(LegacyDatabase database)
    {
        _database = database;
    }

    public List<string> GetData()
    {
        string[] records = _database.FetchRecords();
        return new List<string>(records);
    }
}

// ============================================
// ADAPTER - REST API Adapter
// ============================================

public class RestAPIAdapter : IDataProvider
{
    private readonly RestAPI _api;

    public RestAPIAdapter(RestAPI api)
    {
        _api = api;
    }

    public List<string> GetData()
    {
        Dictionary<int, string> jsonData = _api.GetJsonData();
        return jsonData.Values.ToList();
    }
}

// ============================================
// CLIENT CODE
// ============================================

class DataDemo
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║        ADAPTER PATTERN: DATA SOURCE ADAPTER            ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        // Client works with unified interface
        IDataProvider dbProvider = new DatabaseAdapter(new LegacyDatabase());
        DisplayData(dbProvider);
        
        Console.WriteLine();
        
        IDataProvider apiProvider = new RestAPIAdapter(new RestAPI());
        DisplayData(apiProvider);

        Console.WriteLine("\n✅ Different data sources adapted to same interface!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static void DisplayData(IDataProvider provider)
    {
        List<string> data = provider.GetData();
        
        Console.WriteLine("Data retrieved:");
        foreach (string item in data)
        {
            Console.WriteLine($"  - {item}");
        }
    }
}
```

---

## When to Use Adapter Pattern

### ✅ Use When:

1. **Integrate legacy code**
   - Old system with incompatible interface
   - Don't want to modify legacy code

2. **Third-party libraries**
   - External API with different interface
   - Want consistent interface across different vendors

3. **Interface mismatch**
   - Have classes that should work together
   - But interfaces are incompatible

4. **Reusability**
   - Want to reuse existing classes
   - But interface doesn't match requirements

### ❌ Don't Use When:

1. **Can modify source**
   - You control both classes
   - Better to refactor directly

2. **Simple wrapper needed**
   - Just need to add functionality
   - Use Decorator pattern instead

3. **Too many adapters**
   - Sign of poor design
   - Consider refactoring

---

## Adapter vs Similar Patterns

### Adapter vs Facade

```
ADAPTER:                          FACADE:
- Converts one interface          - Simplifies complex interface
- One-to-one relationship         - One-to-many relationship
- Makes incompatible compatible   - Makes complex simple

┌─────────┐                       ┌─────────┐
│ Client  │                       │ Client  │
└────┬────┘                       └────┬────┘
     │                                 │
     ↓                                 ↓
┌─────────┐     ┌─────────┐     ┌──────────┐
│ Adapter │────>│ Adaptee │     │  Facade  │
└─────────┘     └─────────┘     └────┬─────┘
                                      │
                              ┌───────┼───────┐
                              ↓       ↓       ↓
                         ┌───┐  ┌───┐  ┌───┐
                         │ A │  │ B │  │ C │
                         └───┘  └───┘  └───┘
```

### Adapter vs Decorator

```
ADAPTER:                          DECORATOR:
- Changes interface               - Keeps same interface
- For compatibility               - Adds functionality
- Wraps incompatible object       - Wraps compatible object

┌─────────┐                       ┌───────────┐
│ ITarget │                       │ IComponent│
└────△────┘                       └─────△─────┘
     │                                  │
     │                            ┌─────┴─────┐
┌────┴─────┐                      │           │
│ Adapter  │                 ┌────┴────┐ ┌────┴────┐
│          │────>Adaptee     │Concrete │ │Decorator│
└──────────┘                 └─────────┘ └─────────┘
```

### Adapter vs Proxy

```
ADAPTER:                          PROXY:
- Different interface             - Same interface
- Adapt existing object           - Control access
- Interface conversion            - Add control layer

┌─────────┐                       ┌─────────┐
│ Adapter │────>Adaptee           │  Proxy  │────>RealSubject
│(Different interface)            │(Same interface)
└─────────┘                       └─────────┘
```

---

## Advantages & Disadvantages

### ✅ Advantages:

1. **Single Responsibility Principle**
   - Separates interface conversion logic
   - Each adapter handles one conversion

2. **Open/Closed Principle**
   - Add new adapters without changing existing code
   - Extend system with new adaptees

3. **Flexibility**
   - Can switch implementations easily
   - Add new adapters at runtime

4. **Reusability**
   - Reuse existing classes
   - Don't need to modify legacy code

### ❌ Disadvantages:

1. **Complexity increase**
   - More classes to maintain
   - Indirection layer

2. **Performance overhead**
   - Extra method calls
   - Object creation overhead

3. **Can be overused**
   - Too many adapters = design smell
   - May indicate need for refactoring

---

## Two-Way Adapter

Sometimes you need bidirectional adaptation:

```csharp
public class TwoWayAdapter : ITarget, IAdaptee
{
    private readonly ITarget _target;
    private readonly IAdaptee _adaptee;

    public TwoWayAdapter(ITarget target)
    {
        _target = target;
    }

    public TwoWayAdapter(IAdaptee adaptee)
    {
        _adaptee = adaptee;
    }

    // ITarget implementation
    public void Request()
    {
        if (_adaptee != null)
            _adaptee.SpecificRequest();
        else
            _target.Request();
    }

    // IAdaptee implementation
    public void SpecificRequest()
    {
        if (_target != null)
            _target.Request();
        else
            _adaptee.SpecificRequest();
    }
}
```

---

## Pluggable Adapters

Use Strategy pattern with Adapter for flexible adaptation:

```csharp
public interface IAdaptationStrategy
{
    void Adapt();
}

public class Adapter : ITarget
{
    private readonly IAdaptationStrategy _strategy;

    public Adapter(IAdaptationStrategy strategy)
    {
        _strategy = strategy;
    }

    public void Request()
    {
        _strategy.Adapt();
    }
}

// Different adaptation strategies
public class StripeAdaptationStrategy : IAdaptationStrategy
{
    private readonly StripeAPI _stripe;

    public StripeAdaptationStrategy(StripeAPI stripe)
    {
        _stripe = stripe;
    }

    public void Adapt()
    {
        _stripe.Charge(100, "token");
    }
}

public class PayPalAdaptationStrategy : IAdaptationStrategy
{
    private readonly PayPalSDK _paypal;

    public PayPalAdaptationStrategy(PayPalSDK paypal)
    {
        _paypal = paypal;
    }

    public void Adapt()
    {
        _paypal.SendPayment(100, "email");
    }
}
```

---

## Best Practices

### ✅ DO:

1. **Prefer composition over inheritance**
   - Use Object Adapter (composition)
   - More flexible than Class Adapter

2. **Keep adapters simple**
   - Single responsibility
   - Only convert interface

3. **Use dependency injection**
   ```csharp
   public class PaymentService
   {
       private readonly IPaymentProcessor _processor;
       
       public PaymentService(IPaymentProcessor processor)
       {
           _processor = processor;
       }
   }
   ```

4. **Document adaptation logic**
   - Explain what's being adapted
   - Document any data transformations

### ❌ DON'T:

1. **Add business logic in adapters**
   - Adapters should only convert
   - Business logic belongs elsewhere

2. **Create adapters for simple wrappers**
   - If no interface mismatch, don't use adapter
   - Use simple wrapper or facade

3. **Modify adaptee**
   - Adapters should work with existing code
   - Don't change adaptee's interface

---

## Real-World Use Cases

### 1. **Database Drivers**
```csharp
// ADO.NET adapts different database providers
IDbConnection connection;

// SQL Server
connection = new SqlConnection(connectionString);

// MySQL
connection = new MySqlConnection(connectionString);

// PostgreSQL
connection = new NpgsqlConnection(connectionString);

// All use same IDbConnection interface
```

### 2. **Logging Adapters**
```csharp
public interface ILogger
{
    void Log(string message);
}

// Adapter for Serilog
public class SerilogAdapter : ILogger
{
    private readonly Serilog.ILogger _logger;
    
    public void Log(string message)
    {
        _logger.Information(message);
    }
}

// Adapter for NLog
public class NLogAdapter : ILogger
{
    private readonly NLog.Logger _logger;
    
    public void Log(string message)
    {
        _logger.Info(message);
    }
}
```

### 3. **Cloud Storage Adapters**
```csharp
public interface ICloudStorage
{
    void Upload(string path, byte[] data);
    byte[] Download(string path);
}

public class AzureBlobAdapter : ICloudStorage { }
public class AWSS3Adapter : ICloudStorage { }
public class GoogleCloudAdapter : ICloudStorage { }
```

### 4. **Authentication Providers**
```csharp
public interface IAuthProvider
{
    bool Authenticate(string username, string password);
}

public class ActiveDirectoryAdapter : IAuthProvider { }
public class OAuth2Adapter : IAuthProvider { }
public class LDAPAdapter : IAuthProvider { }
```

---

## Summary

**Adapter Pattern:**
- Converts one interface to another
- Makes incompatible interfaces work together
- Uses composition (has-a relationship)
- Doesn't change behavior, only interface

**Key Components:**
1. **Target**: Interface client expects
2. **Adapter**: Converts Adaptee to Target
3. **Adaptee**: Existing incompatible class
4. **Client**: Uses Target interface

**When to Use:**
- Integrate third-party libraries
- Work with legacy code
- Create reusable code with different interfaces
- Switch between different implementations

**Quick Rule:**
If you need to use a class but its interface doesn't match what you need, use **Adapter**!

**Remember:**
🔌 **Adapter = Interface Translator**
- Like a power adapter or USB converter
- Doesn't change functionality
- Just makes it compatible
