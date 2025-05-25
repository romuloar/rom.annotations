namespace Rom.Annotations.Test.Attributes.Date
{
    public class DateIsUtcAttributeTest
    {
        private readonly DateIsUtcAttribute _attribute = new();

        [Fact]
        public void NullValue_IsValid()
        {
            Assert.True(_attribute.IsValid(null));
        }

        [Fact]
        public void DateTimeUtc_IsValid()
        {
            var utcDate = new DateTime(2024, 5, 24, 15, 0, 0, DateTimeKind.Utc);
            Assert.True(_attribute.IsValid(utcDate));
        }

        [Fact]
        public void DateTimeLocal_IsInvalid()
        {
            var localDate = new DateTime(2024, 5, 24, 15, 0, 0, DateTimeKind.Local);
            Assert.False(_attribute.IsValid(localDate));
        }

        [Fact]
        public void DateTimeUnspecified_IsInvalid()
        {
            var unspecifiedDate = new DateTime(2024, 5, 24, 15, 0, 0, DateTimeKind.Unspecified);
            Assert.False(_attribute.IsValid(unspecifiedDate));
        }

        [Fact]
        public void InvalidType_IsInvalid()
        {
            Assert.False(_attribute.IsValid("not a date"));
        }
    }
}
