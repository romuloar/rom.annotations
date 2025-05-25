# Rom.Annotations

Welcome to **Rom.Annotations** — 
a growing library of custom attributes to enhance validation and domain modeling using a clean, 
declarative approach with .NET DataAnnotations style.
A collection of reusable .NET validation attributes for models and DTOs.
  
Supports .NET Standard 2.0 and .NET 9.

## 🔧 Why Rom.Annotation?

.NET’s built-in `[Required]`, `[Range]`, `[StringLength]` and others are great — but real-world validations are often conditional, field-dependent, or more complex.

Rom.Annotation brings the power to express those rules **with clarity and maintainability**.

## ✨ Features

- Attribute-based validation for common and advanced scenarios
- Easy integration with ASP.NET Core, Blazor, and other .NET projects
- Customizable error messages

---

## Installation

```bash
dotnet install Rom.Annotation
Nuget-Install Rom.Annotation
```

## 📚 Table of Contents

| Group name      | Attributes | Group name      | Attributes |
|:---------------:|:----------|:---------------:|:-----------|
| **📦 Collection** | 🔸 [ListCountMax](#listcountmax)<br/>🔸 [ListCountMin](#listcountmin)<br/>🔸 [ListCountRange](#listcountrange)<br/>🔸 [ListItemsUnique](#listitemsunique)<br/>🔸 [ListItemsCondition](#listitemscondition) | **🔀 Comparison** | 🔸 [CompareFields](#comparefields)<br/>🔸 [GreaterThan](#greaterthan)<br/>🔸 [LessThan](#lessthan)<br/>🔸 [NotEqualTo](#notequalto) |
| **⚡ Conditional** | 🔸 [ConditionalPattern](#conditionalpattern)<br/>🔸 [ConditionalValidation](#conditionalvalidation)<br/>🔸 [RangeIf](#rangeif)<br/>🔸 [RequiredGuid](#requiredguid)<br/>🔸 [RequiredIf](#requiredif)<br/>🔸 [RequiredEnum](#RequiredEnum)<br/>🔸 [RequiredIfFalse](#requirediffalse)<br/>🔸 [RequiredIfInSet](#requiredifinset)<br/>🔸 [RequiredIfNullOrWhiteSpace](#requiredifnullorwhitespace)<br/>🔸 [RequiredIfTrue](#requirediftrue)<br/>🔸 [RequiredList](#requiredlist)<br/>🔸 [RequiredString](#requiredstring) | **🛠️ Custom** | 🔸 [PredicateValidation](#predicatevalidation) |
| **📅 Date** | 🔸 [DateEarlierThan](#dateearlierthan)<br/>🔸 [DateIsUtc](#dateisutc)<br/>🔸 [DateLaterThan](#datelaterthan)<br/>🔸 [DateRange](#daterange) | **🔖 Generic** | 🔸 [AllowedValues](#allowedvalues)<br/>🔸 [DisallowedValues](#disallowedvalues) | **🪁 Multiple Fields** | 🔸 [AtLeastOneRequired](#atleastonerequired)<br/>🔸 [MutuallyExclusive](#mutuallyexclusive)<br/>🔸 [OnlyOneRequired](#onlyonerequired) |
| **🔢 Numeric** | 🔸 [DecimalPrecision](#decimalprecision) | **🔤 String** | 🔸 [StringContains](#stringcontains)<br/>🔸 [StringLengthEquals](#stringlengthequals)<br/>🔸 [StringNotContains](#stringnotcontains) |

### 📦 Collection

## 📌 ListCountMax

Validates that the list has at most a maximum number of items.
	
### 🚀 Quick Example
```csharp
	// Example 1: List of strings with maximum 3 items
	public class Model
	{
		[ListCountMax(3, ErrorMessage = "The list must contain at most 3 items.")]
		public List<string> Items { get; set; }
	}

	// Example 2: List with 4 items (validation fails)
	public class LargeModel
	{
		[ListCountMax(3)]
		public List<string> Values { get; set; }
	}
```

## 📌 ListCountMin  
Validates that the list has at least a minimum number of items.

### 🚀 Quick Example
```csharp
// Example 1: List of integers with minimum 2 items
public class Model
{
	[ListCountMin(2, ErrorMessage = "The list must contain at least 2 items.")]
	public List<int> Numbers { get; set; }
}

// Example 2: Empty list (validation fails)
public class EmptyModel
{
	[ListCountMin(1)]
	public List<string> Items { get; set; }
}
```

## 📌 ListCountRange

Validates that the list has between a minimum and maximum number of items (inclusive).
### 🚀 Quick Example
```csharp
// Example 1: Array of strings with between 2 and 4 items
public class Model
{
    [ListCountRange(2, 4, ErrorMessage = "The list must contain between 2 and 4 items.")]
    public string[] Values { get; set; }
}

// Example 2: List with 1 item (validation fails)
public class SmallModel
{
    [ListCountRange(2, 4)]
    public List<string> Items { get; set; }
}

// Example 3: List with 5 items (validation fails)
public class LargeModel
{
    [ListCountRange(2, 4)]
    public List<string> Items { get; set; }
}
 ```

## 📌 ListItemsUnique

Ensures all items in the list are unique (no duplicates).

### 🚀 Quick Example
```csharp
// Example 1: List of integers without duplicates
public class Model
{
    [ListItemsUnique(ErrorMessage = "The list must contain unique items.")]
    public List<int> Ids { get; set; }
}

// Example 2: List with duplicates (validation fails)
public class DuplicateModel
{
    [ListItemsUnique]
    public List<int> Numbers { get; set; }
}
```

## 📌 ListItemsCondition

Validates each item in the list against a condition or attribute.
### 🚀 Quick Example
```csharp
// Example 1: List of strings where each string must be non-empty
public class Model
{
	[ListItemsCondition(typeof(RequiredStringAttribute), ErrorMessage = "All items must be non-empty strings.")]
	public List<string> Tags { get; set; }
}

// Example 2: List with empty string (validation fails)
public class InvalidModel
{
	[ListItemsCondition(typeof(RequiredStringAttribute))]
	public List<string> Names { get; set; }
}

// Example: Sample RequiredStringAttribute for validation
public class RequiredStringAttribute : ValidationAttribute
{
	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		if (value is string s && !string.IsNullOrWhiteSpace(s))
			return ValidationResult.Success;
		return new ValidationResult("String must be non-empty.");
	}
}
```

### 🔀 Comparison

## 📌 CompareFields	
Validates if two fields in the same object are equal or different.

### 🚀 Quick Example
```csharp

// Example 1: Validate that two string fields are equal (default behavior).
public class UserModel
{
	[CompareFields("ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
	public string Password { get; set; }

	public string ConfirmPassword { get; set; }
}

// Example 2: Validate that two string fields are different.
public class SettingsModel
{
	[CompareFields(nameof(BackupEmail), mustBeEqual: false, ErrorMessage = "Primary and Backup emails must be different.")]
	public string PrimaryEmail { get; set; }

	public string BackupEmail { get; set; }
}

// Example 3: Validate that two numeric fields are equal.
public class OrderModel
{
	[CompareFields("InvoiceNumber", ErrorMessage = "Order Number must match Invoice Number.")]
	public int OrderNumber { get; set; }

	public int InvoiceNumber { get; set; }
}
```

## 📌 GreaterThan	
Validates that the current field is greater than the value of another specified field.

### 🚀 Quick Example
```csharp
// Example 1: Numeric comparison
public class RangeModel
{
	public int MinValue { get; set; }

	[GreaterThan("MinValue", ErrorMessage = "MaxValue must be greater than MinValue")]
	public int MaxValue { get; set; }
}

// Example 2: DateTime comparison
public class EventModel
{
	public DateTime StartDate { get; set; }

	[GreaterThan("StartDate")]
	public DateTime EndDate { get; set; }
}

// Example 3: Nullable values
public class NullableModel
{
	public int? Threshold { get; set; }

	[GreaterThan("Threshold")]
	public int? Value { get; set; }
}

```

## 📌 LessThan	
Validates that the value of the property is less than the value of another property.
Supports numeric types and DateTime.

### 🚀 Quick Example
```csharp
### 🚀 Quick Example
```csharp
// Example 1: Numeric comparison
public class RangeModel
{
	public int MaxValue { get; set; }

	[LessThan("MaxValue", ErrorMessage = "MinValue must be less than MaxValue")]
	public int MinValue { get; set; }
}

// Example 2: DateTime comparison
public class EventModel
{
	public DateTime EndDate { get; set; }

	[LessThan("EndDate")]
	public DateTime StartDate { get; set; }
}

// Example 3: Nullable values
public class NullableModel
{
	public int? Value { get; set; }

	[LessThan("Value")]
	public int? Threshold { get; set; }
}
```

## 📌 NotEqualTo	
Validates that the value of the current field is NOT equal to the value of the specified other field.

### 🚀 Quick Example
```csharp
	
// Example 1: Different string values
public class SampleModel
{
	public string Password { get; set; }

	[NotEqualTo("Password", ErrorMessage = "ConfirmPassword must not be the same as Password")]
	public string ConfirmPassword { get; set; }
}

// Example 2: Different integers
public class NumbersModel
{
	public int StartValue { get; set; }

	[NotEqualTo("StartValue")]
	public int EndValue { get; set; }
}


// Example 3: Null handling

public class NullableFields
{
	public string FieldOne { get; set; }

	[NotEqualTo("FieldOne")]
	public string FieldTwo { get; set; }
}

```

### ⚡ Conditional

## 📌 ConditionalPattern  
Validates a string property with a regex pattern if another property has a specific value.

### 🚀 Quick Example
```csharp
// Example 1: Validate SSN only if 'ConditionField' equals "Yes"
public class UserModel
{
	public string ConditionField { get; set; }

	[ConditionalPattern("ConditionField", "Yes", @"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "Invalid SSN format")]
	public string SSN { get; set; }
}

// Example 2: Validate phone number only if status is "Active"
public class ContactModel
{
	public string Status { get; set; }

	[ConditionalPattern("Status", "Active", @"^\(\d{3}\) \d{3}-\d{4}$", ErrorMessage = "Phone must be in format (xxx) xxx-xxxx")]
	public string PhoneNumber { get; set; }
}

// Example 3: Validation skipped when condition does not match
public class SampleModel
{
	public string Mode { get; set; }

	[ConditionalPattern("Mode", "Enabled", @"^\d+$")]
	public string NumericField { get; set; }
}


// Example 4: with Enum values
public enum PaymentType
{
	Cash,
	CreditCard,
	DebitCard
}

public class PaymentModel
{
	public PaymentType PaymentMethod { get; set; }

	[ConditionalPattern("PaymentMethod", PaymentType.CreditCard, @"^\d{16}$", ErrorMessage = "Credit Card number must have 16 digits")]
	public string CreditCardNumber { get; set; }

	[ConditionalPattern("PaymentMethod", PaymentType.DebitCard, @"^\d{16}$", ErrorMessage = "Debit Card number must have 16 digits")]
	public string DebitCardNumber { get; set; }
}

```

## 📌 ConditionalValidation
Validates a field using a specified validation attribute if another property has a specific value.

### 🚀 Quick Example
```csharp
// Example 1: Apply [Required] only if Status == "Active"
public class UserModel
{
	public string Status { get; set; }

	[ConditionalValidation("Status", "Active", typeof(RequiredAttribute), ErrorMessage = "Username is required when status is Active")]
	public string Username { get; set; }
}

// Example 2: Apply [Range] only if AgeGroup == "Adult"
public class AgeModel
{
	public string AgeGroup { get; set; }

	[ConditionalValidation("AgeGroup", "Adult", typeof(RangeAttribute), 18, 65, ErrorMessage = "Age must be between 18 and 65 for adults")]
	public int Age { get; set; }
}

// Example 3: Apply [StringLength] only if Type == "Custom"
public class ProductModel
{
	public string Type { get; set; }

	[ConditionalValidation("Type", "Custom", typeof(StringLengthAttribute), 5, ErrorMessage = "CustomCode must be at most 5 characters long")]
	public string CustomCode { get; set; }
}

// Example 4: No validation when the condition is not met
public class PaymentModel
{
	public string PaymentType { get; set; }

	[ConditionalValidation("PaymentType", "Card", typeof(RequiredAttribute))]
	public string CardNumber { get; set; }
}
```

## 📌 RangeIf
Validates if a numeric field is within a given range only if another field has a specific value.

### 🚀 Quick Example
```csharp

// Example 1: Range applies only if "Status" is "Active"
public class Order
{
	public string Status { get; set; }

	[RangeIf("Status", "Active", 1, 100, ErrorMessage = "Quantity must be between 1 and 100 when status is Active.")]
	public int Quantity { get; set; }
}

// Example 2: Range applies only if "Type" is 2 (int)
public class Product
{
	public int Type { get; set; }

	[RangeIf("Type", 2, 0.5, 10.5, ErrorMessage = "Weight must be between 0.5 and 10.5 when Type is 2.")]
	public double Weight { get; set; }
}

// Example 3: Range applies only if "IsSpecial" is true (bool)
public class Customer
{
	public bool IsSpecial { get; set; }

	[RangeIf("IsSpecial", true, 1000, 5000, ErrorMessage = "CreditLimit must be between 1000 and 5000 for special customers.")]
	public decimal CreditLimit { get; set; }
}

// Example 4: Validates numeric range conditionally based on another enum field’s value.
public enum OrderStatus
{
	Pending,
	Active,
	Completed
}

public class Order
{
	public OrderStatus Status { get; set; }

	[RangeIf("Status", OrderStatus.Active, 1, 100, ErrorMessage = "Quantity must be between 1 and 100 when status is Active.")]
	public int Quantity { get; set; }
}
```

## 📌 RequiredEnum
Ensures that an enum value is selected and is not the default (usually zero).
### 🚀 Quick Example
```csharp
public enum OrderStatus
{
	None = 0,
	Pending = 1,
	Shipped = 2,
	Delivered = 3
}

// Example 1: Status cannot be None (default)
public class OrderModel
{
	[RequiredEnum(ErrorMessage = "OrderStatus must be selected.")]
	public OrderStatus Status { get; set; }
}
```

## 📌 RequiredGuid	
Validates that a Guid property is not Guid.Empty.

### 🚀 Quick Example
```csharp
// Example 1: Basic usage - Guid property must not be Guid.Empty
public class User
{
	[RequiredGuid(ErrorMessage = "User ID must be a valid non-empty Guid.")]
	public Guid UserId { get; set; }
}

// Example 2: Nullable Guid - still validates if value is provided
public class Document
{
	[RequiredGuid(ErrorMessage = "Document ID must be a valid Guid.")]
	public Guid? DocumentId { get; set; }
}

// Example 3: Using with other attributes - combined validation
public class Order
{
	[RequiredGuid]
	public Guid OrderId { get; set; }

	[Required]
	[StringLength(50)]
	public string CustomerName { get; set; }
}
```

## 📌 RequiredIf
Makes the field required if another field has a specific value.

### 🚀 Quick Example
```csharp
// Example 1: String — required if Type is "Manual"
// Instructions will only be required if Type is "Manual".
public class TaskModel
{
	public string Type { get; set; }

	[RequiredIf(nameof(Type), "Manual", ErrorMessage = "Instructions are required for manual tasks.")]
	public string Instructions { get; set; }
}

//Example 2: Boolean — required if IsActive is true
//Email will be required only when IsActive == true.
public class UserModel
{
	public bool IsActive { get; set; }

	[RequiredIf(nameof(IsActive), true, ErrorMessage = "Email is required for active users.")]
	public string Email { get; set; }
}


//Example 3: Enum — required if Status is OrderStatus.Pending
Notes will be required only when Status == OrderStatus.Pending.
public enum OrderStatus
{
	None,
	Pending,
	Approved,
	Cancelled
}

public class OrderModel
{
	public OrderStatus Status { get; set; }

	[RequiredIf(nameof(Status), OrderStatus.Pending, ErrorMessage = "Notes are required if the order is pending.")]
	public string Notes { get; set; }
}
```

## 📌 RequiredIfFalse
Makes the field required if another boolean field is false.

### 🚀 Quick Example
```csharp
//Example 1: Field Required When Boolean is False
public class FeatureToggle
{
	public bool IsFeatureActive { get; set; }

	[RequiredIfFalse(nameof(IsFeatureActive), ErrorMessage = "Description is required when the feature is inactive.")]
	public string Description { get; set; }
}

//Example 2: Comments Required If Flag is False
public class Survey
{
	public bool HasResponded { get; set; }

	[RequiredIfFalse(nameof(HasResponded), ErrorMessage = "Reason must be provided if the user has not responded.")]
	public string Reason { get; set; }
}

//Example 3: Signature Required if Approved is False
public class ApprovalForm
{
	public bool IsApproved { get; set; }

	[RequiredIfFalse(nameof(IsApproved))]
	public string Signature { get; set; }
}
```
## 📌 RequiredIfInSet	
Makes the field required if the value of another field is in the specified set of values.

### 🚀 Quick Example
```csharp
public class Example1
{
	public string Status { get; set; } = "Pending";

	[RequiredIfInSet("Status", "Pending", "InProgress", ErrorMessage = "Description is required if Status is Pending or InProgress")]
	public string? Description { get; set; }
}

// Example where Description is required because Status == "Pending"
var example = new Example1 { Status = "Pending", Description = null };
// Validation will fail

// Example where Description is not required because Status == "Completed"
var example2 = new Example1 { Status = "Completed", Description = null };
// Validation will succeed
```

## 📌 RequiredIfNullOrWhiteSpace		
Makes the field required if another property is not null.
Useful when a field must be filled only when another is present. 

### 🚀 Quick Example
```csharp
// Example 1 – Conditional Required with string
public class Customer
{
	public string ReferralCode { get; set; }

	[RequiredIfNullOrWhiteSpace("ReferralCode", ErrorMessage = "ReferrerName is required when ReferralCode is provided")]
	public string ReferrerName { get; set; }
}

// Example 2 – Conditional Required with object	
public class Product
{
	public object Promotion { get; set; }

	[RequiredIfNullOrWhiteSpace("Promotion", ErrorMessage = "PromotionDescription is required when Promotion exists")]
	public string PromotionDescription { get; set; }
}

// Example 3 – Conditional Required with Enum
public enum OrderType
{
	None,
	Custom,
	Standard
}

public class Order
{
	public OrderType? Type { get; set; }

	[RequiredIfNotNull("Type", ErrorMessage = "Description is required when Type is selected")]
	public string Description { get; set; }
}
```

## 📌 RequiredIfTrue	
Makes the field required if another boolean property is true.

### 🚀 Quick Example
```csharp
//Example 1: Field Required When Boolean Is True
public class UserSettings
{
	public bool EnableTwoFactor { get; set; }

	[RequiredIfTrue(nameof(EnableTwoFactor), ErrorMessage = "PhoneNumber is required when two-factor is enabled.")]
	public string PhoneNumber { get; set; }
}
//Example 2: Conditional Comments Field
public class Report
{
	public bool HasComments { get; set; }

	[RequiredIfTrue(nameof(HasComments), ErrorMessage = "Comments must be filled if 'HasComments' is true.")]
	public string Comments { get; set; }
}
//Example 3: Boolean Flag Requiring Signature
public class Document
{
	public bool RequiresSignature { get; set; }

	[RequiredIfTrue(nameof(RequiresSignature))]
	public string Signature { get; set; }
}
```

## 📌 RequiredList	
Validates that a collection property is not null and contains at least one item.
Supports IEnumerable, arrays, List, Collection, etc.

### 🚀 Quick Example
```csharp
// Example 1: List<T>
public class Order
{
	[RequiredList(ErrorMessage = "Order items cannot be empty.")]
	public List<string> Items { get; set; }
}

// Example 2: Array
public class Survey
{
	[RequiredList(ErrorMessage = "At least one question is required.")]
	public string[] Questions { get; set; }
}

// Example 3: IEnumerable<T>
public class Report
{
	[RequiredList(ErrorMessage = "Report data must contain entries.")]
	public IEnumerable<int> DataPoints { get; set; }
}

```

## 📌 RequiredString	
Validates that a string property is not null, empty or whitespace.

### 🚀 Quick Example
```csharp
public class Example1
{
	[RequiredString(ErrorMessage = "Name is required")]
	public string? Name { get; set; }
}

// Example with invalid values:
// Name = null, "", or "   "
// Validation will fail.

var example = new Example1 { Name = null };

// Example with valid value:
// Name = "John Doe"
// Validation will succeed.

var example2 = new Example1 { Name = "John Doe" };
```

### 🛠️ Custom

## 📌 PredicateValidation
Allows custom validation logic by specifying a predicate method name on the model class.
### 🚀 Quick Example
```csharp
public class TestModel
{
	[PredicateValidation("IsValidAge", ErrorMessage = "Age must be 18 or older.")]
	public int Age { get; set; }

	public static bool IsValidAge(object value)
	{
		if (value is int age)
			return age >= 18;
		return false;
	}
}
```

### 📅 Date

## 📌 DateEarlierThan	
Validates that the value of the property is earlier than the value of another property.
Supports DateTime and nullable DateTime.

### 🚀 Quick Example
```csharp
// Example 1: Simple date validation
public class EventModel
{
	public DateTime EndDate { get; set; }

	[DateEarlierThan("EndDate", ErrorMessage = "StartDate must be earlier than EndDate")]
	public DateTime StartDate { get; set; }
}

// Example 2: Null values allowed
public class NullableEventModel
{
	public DateTime? EndDate { get; set; }

	[DateEarlierThan("EndDate")]
	public DateTime? StartDate { get; set; }
}

// Example 3: Custom error message
public class CustomMessageModel
{
	public DateTime Deadline { get; set; }

	[DateEarlierThan("Deadline", ErrorMessage = "The date must be before the deadline")]
	public DateTime AppointmentDate { get; set; }
}

```

## 📌 DateIsUtc
Validates that the value of the property is a UTC DateTime.

### 🚀 Quick Example
```csharp
// Example 1: Valid UTC DateTime
public class EventModel
{
	[DateIsUtc(ErrorMessage = "StartDate must be in UTC")]
	public DateTime StartDate { get; set; }
}

// Example 2: Nullable DateTime - valid if null or UTC
public class NullableEventModel
{
	[DateIsUtc]
	public DateTime? EndDate { get; set; }
}

// Example 3: Invalid non-UTC DateTime will fail validation
public class WrongEventModel
{
	[DateIsUtc]
	public DateTime CreatedAt { get; set; }
}
```

## 📌 DateLaterThan  
Validates that the value of the date property is later than the value of another date property.

### 🚀 Quick Example
```csharp
// Example 1: Valid dates where EndDate is later than StartDate
public class EventModel
{
	public DateTime StartDate { get; set; }

	[DateLaterThan("StartDate", ErrorMessage = "EndDate must be later than StartDate")]
	public DateTime EndDate { get; set; }
}

// Example 2: Nullable DateTime properties (valid if either is null)
public class NullableEventModel
{
	public DateTime? StartDate { get; set; }

	[DateLaterThan("StartDate")]
	public DateTime? EndDate { get; set; }
}

// Example 3: Validation error if EndDate is earlier than StartDate
public class InvalidEventModel
{
	public DateTime StartDate { get; set; }

	[DateLaterThan("StartDate")]
	public DateTime EndDate { get; set; }
}	
```

## 📌 DateRange  
Validates that the date is within the specified inclusive range.

### 🚀 Quick Example
```csharp
// Example 1: Date inside the range
public class EventModel
{
	[DateRange("2024-01-01", "2024-12-31", ErrorMessage = "Date must be within the year 2024.")]
	public DateTime EventDate { get; set; }
}

// Example 2: Date before the range (validation fails)
public class PastEventModel
{
	[DateRange("2024-01-01", "2024-12-31")]
	public DateTime EventDate { get; set; }
}

// Example 3: Date after the range (validation fails)
public class FutureEventModel
{
	[DateRange("2024-01-01", "2024-12-31")]
	public DateTime EventDate { get; set; }
}	
```

### 🔖 Generic

## 📌 AllowedValues

Validates that the value belongs to a specified set of allowed values.
### 🚀 Quick Example
```csharp
// Example 1: Only "Admin", "User", or "Guest" are allowed roles
public class UserModel
{
	[AllowedValues("Admin", "User", "Guest", ErrorMessage = "Role must be Admin, User, or Guest.")]
	public string Role { get; set; }
}
```

## 📌 DisallowedValues

Validates that the value does NOT belong to a specified set of disallowed values.
### 🚀 Quick Example
```csharp
// Example 1: Role cannot be "Banned" or "Suspended"
public class UserModel
{
	[DisallowedValues("Banned", "Suspended", ErrorMessage = "Role cannot be Banned or Suspended.")]
	public string Role { get; set; }
}
```

### 🔢 Numeric

## 📌 DecimalPrecision

Controls the number of decimal places allowed in a numeric value.
### 🚀 Quick Example
```csharp
// Example 1: Allows up to 2 decimal places
public class ProductModel
{
	[DecimalPrecision(2, ErrorMessage = "Price can have up to 2 decimal places.")]
	public decimal Price { get; set; }
}
```

### 🔤 String

## 📌 StringContains
Validates that a string property contains a specified substring.
### 🚀 Quick Example
```csharp
// Example 1: Email must contain "@domain.com"
public class UserModel
{
	[StringContains("@domain.com", ErrorMessage = "Email must be a corporate domain.")]
	public string Email { get; set; }
}
```

## 📌 StringLengthEquals
Validates that the string length is exactly equal to the specified value.
Examples:
```csharp
public class TestModel
{
	[StringLengthEquals(5, ErrorMessage = "Code must be exactly 5 characters.")]
	public string Code { get; set; }
}

// Valid: model.Code = "12345"
// Invalid: model.Code = "1234" or "123456"
```

## 📌 StringNotContains
Validates that a string property does NOT contain a specified substring.
### 🚀 Quick Example
```csharp
// Example 1: Username must not contain spaces
public class UserModel
{
	[StringNotContains(" ", ErrorMessage = "Username cannot contain spaces.")]
	public string Username { get; set; }
}
```

## Contribution

Contributions are welcome! Please feel free to submit a Pull Request.

## License

MIT

## Author

Romulo Ribeiro
