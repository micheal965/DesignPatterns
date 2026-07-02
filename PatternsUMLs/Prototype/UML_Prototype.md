# UML Diagram: Prototype Pattern

## What is Prototype Pattern?

**Prototype** is a creational design pattern that lets you copy existing objects without making your code dependent on their classes. Instead of creating new objects from scratch, you **clone** existing objects.

### Key Concept:
Instead of calling `new Car()` every time, you clone an existing car prototype and modify it as needed.

### Real-World Analogy:
Think of a photocopy machine. You have an original document (prototype) and you can make copies (clones) of it. The copies are independent - you can write on one copy without affecting the original.

---

## Class Diagram

```
┌─────────────────────────────────────┐
│  <<interface>>                      │
│  IPrototype                         │
├─────────────────────────────────────┤
│  + Clone(): IPrototype              │
└─────────────────────────────────────┘
                △
                │ implements
                │
┌───────────────┴──────────────────────┐
│  ConcretePrototype1                  │
├──────────────────────────────────────┤
│  - field1: string                    │
│  - field2: int                       │
│  - referenceField: Object            │
├──────────────────────────────────────┤
│  + Clone(): IPrototype               │
│  + CloneDeep(): IPrototype           │
└──────────────────────────────────────┘
```

## Detailed Class Diagram (Your Car Example)

```
┌─────────────────────────────────────┐
│  <<interface>>                      │
│  ICarPrototype                      │
├─────────────────────────────────────┤
│  + Clone(): ICarPrototype           │
└─────────────────────────────────────┘
                △
                │ implements
                │
┌───────────────┴──────────────────────┐
│  Car                                 │
├──────────────────────────────────────┤
│  + Model: string                     │
│  + Color: string                     │
│  + Engine: string                    │
│  + Sunroof: bool                     │
│  + EngineType: Engine                │◄─────┐
├──────────────────────────────────────┤      │ has-a
│  + Clone(): ICarPrototype            │      │
│  + ToString(): string                │      │
└──────────────────────────────────────┘      │
                                               │
                                               │
                              ┌────────────────┴────────┐
                              │  Engine                 │
                              ├─────────────────────────┤
                              │  + HorsePower: int      │
                              ├─────────────────────────┤
                              │  + Clone(): Engine      │
                              └─────────────────────────┘
```

## Shallow Copy vs Deep Copy Diagram

```
┌──────────────────────────────────────────────────────────────────┐
│                    SHALLOW COPY                                  │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Original Object              Cloned Object                      │
│  ┌─────────────────┐          ┌─────────────────┐              │
│  │ Value Type ✓    │          │ Value Type ✓    │              │
│  │ (copied)        │          │ (copied)        │              │
│  │                 │          │                 │              │
│  │ Reference  ─────┼──┐       │ Reference  ─────┼──┐           │
│  │ Type ✗          │  │       │ Type ✗          │  │           │
│  └─────────────────┘  │       └─────────────────┘  │           │
│                       │                            │           │
│                       │   ┌──────────────────┐     │           │
│                       └──>│  SHARED OBJECT   │<────┘           │
│                           │  (Problem!)      │                 │
│                           └──────────────────┘                 │
│                                                                  │
│  Problem: Both objects point to the same reference              │
└──────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│                    DEEP COPY                                     │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Original Object              Cloned Object                      │
│  ┌─────────────────┐          ┌─────────────────┐              │
│  │ Value Type ✓    │          │ Value Type ✓    │              │
│  │ (copied)        │          │ (copied)        │              │
│  │                 │          │                 │              │
│  │ Reference  ─────┼──┐       │ Reference  ─────┼──┐           │
│  │ Type ✓          │  │       │ Type ✓          │  │           │
│  └─────────────────┘  │       └─────────────────┘  │           │
│                       │                            │           │
│                       ↓                            ↓           │
│                ┌──────────────┐           ┌──────────────┐     │
│                │ Original     │           │ Cloned       │     │
│                │ Object       │           │ Object       │     │
│                └──────────────┘           └──────────────┘     │
│                                                                  │
│  Solution: Each object has its own copy of reference types      │
└──────────────────────────────────────────────────────────────────┘
```

## Sequence Diagram: Cloning Process

```
Client              Prototype           Clone               ReferencedObject
  │                     │                 │                        │
  │  Clone()            │                 │                        │
  ├────────────────────>│                 │                        │
  │                     │                 │                        │
  │                     │  MemberwiseClone()                       │
  │                     ├────────────────>│                        │
  │                     │                 │                        │
  │                     │  (Shallow copy) │                        │
  │                     │                 │                        │
  │                     │  Deep copy reference types               │
  │                     │                 │                        │
  │                     │                 │  new Object()          │
  │                     │                 ├───────────────────────>│
  │                     │                 │                        │
  │                     │                 │  copy properties       │
  │                     │                 ├───────────────────────>│
  │                     │                 │                        │
  │                     │  return clone   │                        │
  │                     │<────────────────┤                        │
  │                     │                 │                        │
  │  return clone       │                 │                        │
  │<────────────────────┤                 │                        │
  │                     │                 │                        │
```

---

## Complete Code Example: Shallow vs Deep Copy

```csharp
// ============================================
// PROTOTYPE INTERFACE
// ============================================

public interface ICarPrototype
{
    ICarPrototype Clone();
    ICarPrototype CloneDeep();
}

// ============================================
// REFERENCE TYPE - ENGINE
// ============================================

public class Engine
{
    public int HorsePower { get; set; }
    public string Type { get; set; }
    
    public Engine Clone()
    {
        return new Engine
        {
            HorsePower = this.HorsePower,
            Type = this.Type
        };
    }
    
    public override string ToString()
    {
        return $"{Type} - {HorsePower}HP";
    }
}

// ============================================
// PROTOTYPE - CAR
// ============================================

public class Car : ICarPrototype
{
    public string Model { get; set; }
    public string Color { get; set; }
    public bool Sunroof { get; set; }
    public Engine EngineType { get; set; }
    
    // ❌ SHALLOW COPY - Problem with reference types
    public ICarPrototype Clone()
    {
        return (Car)this.MemberwiseClone();
    }
    
    // ✅ DEEP COPY - Properly handles reference types
    public ICarPrototype CloneDeep()
    {
        Car clone = (Car)this.MemberwiseClone();
        
        // Deep copy reference types
        if (this.EngineType != null)
        {
            clone.EngineType = this.EngineType.Clone();
        }
        
        return clone;
    }
    
    public override string ToString()
    {
        return $"Car: {Model} | Color: {Color} | Sunroof: {Sunroof} | Engine: {EngineType}";
    }
}

// ============================================
// DEMONSTRATION
// ============================================

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║       PROTOTYPE PATTERN: SHALLOW vs DEEP COPY          ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        Console.WriteLine();
        
        // ============================================
        // DEMONSTRATION 1: SHALLOW COPY PROBLEM
        // ============================================
        
        Console.WriteLine("DEMONSTRATION 1: Shallow Copy Problem");
        Console.WriteLine(new string('=', 56));
        Console.WriteLine();
        
        Car originalCar1 = new Car
        {
            Model = "Sedan",
            Color = "Blue",
            Sunroof = true,
            EngineType = new Engine { HorsePower = 300, Type = "V6" }
        };
        
        Console.WriteLine("Original Car:");
        Console.WriteLine(originalCar1);
        Console.WriteLine();
        
        // Shallow clone
        Car shallowClone = (Car)originalCar1.Clone();
        
        Console.WriteLine("After Shallow Clone:");
        Console.WriteLine($"Original: {originalCar1}");
        Console.WriteLine($"Clone:    {shallowClone}");
        Console.WriteLine();
        
        // Modify the cloned car's engine
        Console.WriteLine("Modifying clone's engine HorsePower to 500...");
        shallowClone.EngineType.HorsePower = 500;
        shallowClone.Color = "Red";
        
        Console.WriteLine("\nAfter Modification:");
        Console.WriteLine($"Original: {originalCar1}");  // ❌ ALSO CHANGED!
        Console.WriteLine($"Clone:    {shallowClone}");
        
        Console.WriteLine("\n⚠️  PROBLEM: Both cars share the same Engine object!");
        Console.WriteLine("    Original car's engine was also modified.\n");
        
        // ============================================
        // DEMONSTRATION 2: DEEP COPY SOLUTION
        // ============================================
        
        Console.WriteLine(new string('=', 56));
        Console.WriteLine("DEMONSTRATION 2: Deep Copy Solution");
        Console.WriteLine(new string('=', 56));
        Console.WriteLine();
        
        Car originalCar2 = new Car
        {
            Model = "SUV",
            Color = "Black",
            Sunroof = true,
            EngineType = new Engine { HorsePower = 400, Type = "V8" }
        };
        
        Console.WriteLine("Original Car:");
        Console.WriteLine(originalCar2);
        Console.WriteLine();
        
        // Deep clone
        Car deepClone = (Car)originalCar2.CloneDeep();
        
        Console.WriteLine("After Deep Clone:");
        Console.WriteLine($"Original: {originalCar2}");
        Console.WriteLine($"Clone:    {deepClone}");
        Console.WriteLine();
        
        // Modify the cloned car's engine
        Console.WriteLine("Modifying clone's engine HorsePower to 600...");
        deepClone.EngineType.HorsePower = 600;
        deepClone.Color = "White";
        
        Console.WriteLine("\nAfter Modification:");
        Console.WriteLine($"Original: {originalCar2}");  // ✅ UNCHANGED!
        Console.WriteLine($"Clone:    {deepClone}");
        
        Console.WriteLine("\n✅ SUCCESS: Original car remains unchanged!");
        Console.WriteLine("Each car has its own independent Engine object.\n");
        
        // ============================================
        // DEMONSTRATION 3: PROTOTYPE REGISTRY
        // ============================================
        
        Console.WriteLine(new string('=', 56));
        Console.WriteLine("DEMONSTRATION 3: Prototype Registry Pattern");
        Console.WriteLine(new string('=', 56));
        Console.WriteLine();
        
        CarRegistry registry = new CarRegistry();
        
        // Register prototypes
        registry.AddPrototype("SportsCar", new Car
        {
            Model = "Sports",
            Color = "Red",
            Sunroof = false,
            EngineType = new Engine { HorsePower = 500, Type = "V8 Twin-Turbo" }
        });
        
        registry.AddPrototype("FamilyCar", new Car
        {
            Model = "Minivan",
            Color = "Silver",
            Sunroof = true,
            EngineType = new Engine { HorsePower = 250, Type = "V6" }
        });
        
        registry.AddPrototype("EconomyCar", new Car
        {
            Model = "Compact",
            Color = "White",
            Sunroof = false,
            EngineType = new Engine { HorsePower = 150, Type = "I4" }
        });
        
        // Clone from registry
        Console.WriteLine("Creating cars from registry prototypes:\n");
        
        Car sports1 = registry.GetPrototype("SportsCar");
        sports1.Color = "Yellow";
        Console.WriteLine($"Sports Car 1: {sports1}");
        
        Car sports2 = registry.GetPrototype("SportsCar");
        sports2.Color = "Blue";
        Console.WriteLine($"Sports Car 2: {sports2}");
        
        Car family = registry.GetPrototype("FamilyCar");
        Console.WriteLine($"Family Car:   {family}");
        
        Car economy = registry.GetPrototype("EconomyCar");
        Console.WriteLine($"Economy Car:  {economy}");
        
        Console.WriteLine("\n✅ All cars created from prototypes!");
        Console.WriteLine("   Each clone is independent.\n");
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}

// ============================================
// PROTOTYPE REGISTRY
// ============================================

public class CarRegistry
{
    private Dictionary<string, Car> prototypes = new Dictionary<string, Car>();
    
    public void AddPrototype(string key, Car prototype)
    {
        prototypes[key] = prototype;
    }
    
    public Car GetPrototype(string key)
    {
        if (prototypes.ContainsKey(key))
        {
            return (Car)prototypes[key].CloneDeep();
        }
        throw new ArgumentException($"Prototype '{key}' not found");
    }
    
    public void ListPrototypes()
    {
        Console.WriteLine("Available Prototypes:");
        foreach (var key in prototypes.Keys)
        {
            Console.WriteLine($"  - {key}");
        }
    }
}
```

---

## Another Example: Document Cloning

```csharp
// ============================================
// DOCUMENT PROTOTYPE EXAMPLE
// ============================================

public class Document : ICloneable
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Author Author { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    
    // Shallow copy
    public object Clone()
    {
        return this.MemberwiseClone();
    }
    
    // Deep copy
    public Document DeepClone()
    {
        Document clone = (Document)this.MemberwiseClone();
        
        // Deep copy Author
        if (this.Author != null)
            clone.Author = this.Author.Clone();
        
        // Deep copy Tags list
        clone.Tags = new List<string>(this.Tags);
        
        return clone;
    }
    
    public override string ToString()
    {
        return $"Document: {Title} by {Author?.Name} | Tags: {string.Join(", ", Tags)}";
    }
}

public class Author
{
    public string Name { get; set; }
    public string Email { get; set; }
    
    public Author Clone()
    {
        return new Author
        {
            Name = this.Name,
            Email = this.Email
        };
    }
}

// Usage
Document original = new Document
{
    Title = "Design Patterns",
    Content = "...",
    Author = new Author { Name = "John Doe", Email = "john@example.com" },
    Tags = new List<string> { "programming", "patterns", "oop" }
};

Document deepCopy = original.DeepClone();
deepCopy.Tags.Add("advanced");
deepCopy.Author.Name = "Jane Doe";

Console.WriteLine(original);  // Original unchanged
Console.WriteLine(deepCopy);  // Modified clone
```

---

## When to Use Prototype Pattern

### ✅ Use When:

1. **Object creation is expensive**
   - Complex initialization
   - Database queries required
   - Network calls needed

2. **Objects are similar with minor differences**
   - Clone and customize
   - Avoid repetitive initialization

3. **Reduce subclasses**
   - Instead of many subclasses, use prototypes
   - More flexible

4. **Runtime configuration**
   - Create objects based on runtime data
   - Prototype registry

### ❌ Don't Use When:

1. **Simple objects**
   - Constructor is sufficient
   - No complex initialization

2. **Deep copy is complex**
   - Circular references
   - Too many nested objects

---

## Advantages & Disadvantages

### ✅ Advantages:

1. **Avoid costly creation**
   - Clone instead of creating from scratch

2. **Reduce subclasses**
   - Use prototypes instead of inheritance

3. **Add/remove at runtime**
   - Dynamic prototype registry

4. **Configure complex objects**
   - Clone and customize

### ❌ Disadvantages:

1. **Deep copy complexity**
   - Hard to implement for complex objects
   - Circular references problematic

2. **Cloning may be tricky**
   - Reference types need special handling
   - Collections need copying

3. **Clone method maintenance**
   - Must update when adding new fields

---

## Shallow vs Deep Copy Decision Tree

```
                      Need to clone?
                           │
                           ↓
                   Has reference types?
                     /            \
                   NO              YES
                   ↓               ↓
            Shallow Copy OK    Modify cloned refs?
                               /            \
                             NO              YES
                             ↓               ↓
                      Shallow Copy OK   DEEP COPY REQUIRED
```

---

## Real-World Examples

### 1. **Game Development**
```csharp
// Clone enemy prototypes
Enemy goblin = enemyRegistry.GetPrototype("Goblin");
goblin.Position = new Vector3(10, 0, 5);

Enemy orc = enemyRegistry.GetPrototype("Orc");
orc.Position = new Vector3(15, 0, 10);
```

### 2. **Document Templates**
```csharp
// Clone document templates
Document invoice = templateRegistry.GetPrototype("Invoice");
invoice.CustomerName = "John Doe";
invoice.Date = DateTime.Now;
```

### 3. **Database Records**
```csharp
// Clone database objects
User templateUser = userRepository.GetTemplate();
User newUser = templateUser.Clone();
newUser.Email = "newuser@example.com";
```

### 4. **Configuration Objects**
```csharp
// Clone configuration
AppConfig prodConfig = configRegistry.GetPrototype("Production");
AppConfig stagingConfig = prodConfig.Clone();
stagingConfig.DatabaseUrl = "staging-db-url";
```

---

## Best Practices

### ✅ DO:

1. **Implement both shallow and deep copy**
   ```csharp
   public object Clone() { }        // Shallow
   public Car DeepClone() { }       // Deep
   ```

2. **Use prototype registry**
   - Centralize prototype management
   - Easy to add/remove prototypes

3. **Document copy behavior**
   - Clearly state shallow vs deep
   - List which fields are copied

4. **Test clone independence**
   - Ensure clones are truly independent
   - Test with reference types

### ❌ DON'T:

1. **Assume MemberwiseClone is enough**
   - It only does shallow copy
   - Reference types need special handling

2. **Forget to clone collections**
   - Lists, Arrays, Dictionaries
   - Need explicit copying

3. **Ignore circular references**
   - Can cause stack overflow
   - Use visited set pattern

---

## Summary

**Prototype Pattern:**
- Creates objects by **cloning** existing instances
- Avoids expensive object creation
- Two types: **Shallow Copy** and **Deep Copy**

**Your Implementation:**
- ❌ Uses `MemberwiseClone()` = **SHALLOW COPY**
- ⚠️ Shares `EngineType` reference between copies
- ✅ Works by accident (you create new Engine)
- 🔧 **Recommended**: Implement deep copy

**Key Takeaway:**
- **Shallow Copy**: Copies value types, shares references
- **Deep Copy**: Copies everything, including referenced objects
- **Always test**: Modify cloned reference types to verify independence

**Quick Rule:**
If your object has reference types (objects, collections), use **Deep Copy**!