using Rom.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class RequiredIfAttributeTest
    {
        private class TestModel
        {
            public string Status { get; set; }

            [RequiredIf("Status", "Active", ErrorMessage = "Name is required if status is Active")]
            public string Name { get; set; }
        }

        private IList<ValidationResult> ValidateModel(TestModel model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void Should_Fail_Validation_When_Condition_Met_And_Field_Is_Empty()
        {
            var model = new TestModel { Status = "Active", Name = null };
            var results = ValidateModel(model);

            Assert.Single(results);
            Assert.Equal("Name is required if status is Active", results[0].ErrorMessage);
        }

        [Fact]
        public void Should_Pass_Validation_When_Condition_Met_And_Field_Has_Value()
        {
            var model = new TestModel { Status = "Active", Name = "John" };
            var results = ValidateModel(model);

            Assert.Empty(results);
        }

        [Fact]
        public void Should_Pass_Validation_When_Condition_Not_Met()
        {
            var model = new TestModel { Status = "Inactive", Name = null };
            var results = ValidateModel(model);

            Assert.Empty(results);
        }

        [Fact]
        public void Should_Fail_When_Dependent_Property_Does_Not_Exist()
        {
            var attr = new RequiredIfAttribute("UnknownProperty", "Test");
            var model = new { Foo = "bar" };
            var context = new ValidationContext(model) { MemberName = "Foo" };
            var result = attr.GetValidationResult(null, context);

            Assert.Equal("Unknown property: UnknownProperty", result.ErrorMessage);
        }

        public enum OrderStatus
        {
            None,
            Pending,
            Approved,
            Cancelled
        }

        private class OrderModel
        {
            public OrderStatus Status { get; set; }

            [RequiredIf(nameof(Status), OrderStatus.Pending, ErrorMessage = "Notes are required if the order is pending.")]
            public string Notes { get; set; }
        }

        private IList<ValidationResult> ValidateModel(OrderModel model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void Should_Fail_When_Status_Is_Pending_And_Notes_Is_Null()
        {
            var model = new OrderModel
            {
                Status = OrderStatus.Pending,
                Notes = null
            };

            var results = ValidateModel(model);

            Assert.Single(results);
            Assert.Equal("Notes are required if the order is pending.", results[0].ErrorMessage);
        }

        [Fact]
        public void Should_Pass_When_Status_Is_Pending_And_Notes_Is_Filled()
        {
            var model = new OrderModel
            {
                Status = OrderStatus.Pending,
                Notes = "Some details"
            };

            var results = ValidateModel(model);

            Assert.Empty(results);
        }

        [Fact]
        public void Should_Pass_When_Status_Is_Not_Pending()
        {
            var model = new OrderModel
            {
                Status = OrderStatus.Approved,
                Notes = null
            };

            var results = ValidateModel(model);

            Assert.Empty(results);
        }
    }
}
