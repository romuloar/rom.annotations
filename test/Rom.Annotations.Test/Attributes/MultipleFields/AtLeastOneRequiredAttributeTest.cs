using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rom.Annotations.Test.Attributes.MultipleFields
{
    public class AtLeastOneRequiredAttributeTests
    {
        private class Model
        {
            [AtLeastOneRequired("FieldB", "FieldC")]
            public string FieldA { get; set; }
            public string FieldB { get; set; }
            public string FieldC { get; set; }
        }

        [Fact]
        public void Valid_When_OneFieldIsFilled()
        {
            var model = new Model { FieldC = "Hello" };
            var ctx = new ValidationContext(model);
            var result = Validator.TryValidateObject(model, ctx, null, true);
            Assert.True(result);
        }

        [Fact]
        public void Invalid_When_NoFieldsAreFilled()
        {
            var model = new Model();
            var ctx = new ValidationContext(model);
            var results = new List<ValidationResult>();
            var result = Validator.TryValidateObject(model, ctx, results, true);
            Assert.False(result);
        }
    }
}
