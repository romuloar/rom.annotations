namespace Rom.Annotations.Test.Attributes.Conditional
{
    public class RequiredListAttributeTest
    {
        private readonly RequiredListAttribute _attribute = new();

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenValueIsNull()
        {
            Assert.False(_attribute.IsValid(null));
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCollectionIsEmptyList()
        {
            var list = new List<string>();
            Assert.False(_attribute.IsValid(list));
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenCollectionHasItems_List()
        {
            var list = new List<int> { 1 };
            Assert.True(_attribute.IsValid(list));
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCollectionIsEmptyArray()
        {
            var array = new string[0];
            Assert.False(_attribute.IsValid(array));
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenCollectionHasItems_Array()
        {
            var array = new[] { "item" };
            Assert.True(_attribute.IsValid(array));
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCollectionIsEmptyIEnumerable()
        {
            IEnumerable<int> emptyEnum = new List<int>();
            Assert.False(_attribute.IsValid(emptyEnum));
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenCollectionHasItems_IEnumerable()
        {
            IEnumerable<int> enumWithItems = new List<int> { 42 };
            Assert.True(_attribute.IsValid(enumWithItems));
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenValueIsNotCollection()
        {
            var notCollection = 123;
            Assert.False(_attribute.IsValid(notCollection));
        }
    }
}
