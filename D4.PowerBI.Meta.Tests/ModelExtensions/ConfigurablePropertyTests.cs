using D4.PowerBI.Meta.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace D4.PowerBI.Meta.Tests.ModelExtensions
{
    public class ConfigurablePropertyTests
    {
        [Fact]
        public void WHEN_single_property_gets_value_with_matched_name_THEN_value_returned()
        {
            var sut = new ConfigurableProperty
            {
                Name = "propertyName",
                Value = 123
            };

            var tryResult = sut.TryGetValue(new string[1] { "propertyName" }, 
                out var valueResult);

            tryResult.Should().BeTrue();
            valueResult.Should().Be(123);
        }

        [Fact]
        public void WHEN_single_property_gets_property_with_matched_name_THEN_value_returned()
        {
            var sut = new ConfigurableProperty
            {
                Name = "propertyName",
                Value = 123
            };

            var tryResult = sut.TryGetProperty(new string[1] { "propertyName" },
                out var valueResult);

            tryResult.Should().BeTrue();
            valueResult.Should().NotBeNull();
            valueResult?.Name.Should().Be("propertyName");
            valueResult?.Value.Should().Be(123);
        }

        [Fact]
        public void WHEN_single_property_gets_value_with_incorrect_name_THEN_null_returned()
        {
            var sut = new ConfigurableProperty
            {
                Name = "propertyName",
                Value = 123
            };

            var tryResult = sut.TryGetValue(new string[1] { "notPropertyName" },
                out var valueResult);

            tryResult.Should().BeFalse();
            valueResult.Should().BeNull();
        }

        [Fact]
        public void WHEN_single_property_gets_property_with_incorrect_name_THEN_null_returned()
        {
            var sut = new ConfigurableProperty
            {
                Name = "propertyName",
                Value = 123
            };

            var tryResult = sut.TryGetProperty(new string[1] { "notPropertyName" },
                out var valueResult);

            tryResult.Should().BeFalse();
            valueResult.Should().BeNull();
        }

        [Fact]
        public void WHEN_multiple_properties_gets_value_with_matched_name_THEN_value_returned()
        {
            var sut = new List<ConfigurableProperty>
            {
                new ConfigurableProperty { Name = "prop1", Value = "val1" },
                new ConfigurableProperty { Name = "prop2", Value = "val2" }
            };

            var tryResult = sut.TryGetValue(new string[1] { "prop2" },
                out var valueResult);

            tryResult.Should().BeTrue();
            valueResult.Should().Be("val2");
        }

        [Fact]
        public void WHEN_multiple_properties_gets_property_with_matched_name_THEN_value_returned()
        {
            var sut = new List<ConfigurableProperty>
            {
                new ConfigurableProperty { Name = "prop1", Value = "val1" },
                new ConfigurableProperty { Name = "prop2", Value = "val2" }
            };

            var tryResult = sut.TryGetProperty(new string[1] { "prop2" },
                out var valueResult);

            tryResult.Should().BeTrue();
            valueResult.Should().NotBeNull();
            valueResult?.Name.Should().Be("prop2");
            valueResult?.Value.Should().Be("val2");
        }

        [Fact]
        public void WHEN_multiple_properties_gets_value_with_incorrect_name_THEN_null_returned()
        {
            var sut = new List<ConfigurableProperty>
            {
                new ConfigurableProperty { Name = "prop1", Value = "val1" },
                new ConfigurableProperty { Name = "prop2", Value = "val2" }
            };

            var tryResult = sut.TryGetValue(new string[1] { "prop3" },
                out var valueResult);

            tryResult.Should().BeFalse();
            valueResult.Should().BeNull();
        }

        [Fact]
        public void WHEN_multiple_properties_gets_property_with_incorrect_name_THEN_null_returned()
        {
            var sut = new List<ConfigurableProperty>
            {
                new ConfigurableProperty { Name = "prop1", Value = "val1" },
                new ConfigurableProperty { Name = "prop2", Value = "val2" }
            };

            var tryResult = sut.TryGetProperty(new string[1] { "prop3" },
                out var valueResult);

            tryResult.Should().BeFalse();
            valueResult.Should().BeNull();
        }

        [Fact]
        public void WHEN_nested_properties_gets_value_with_matched_node_names_THEN_value_returned()
        {
            var leafNode = new ConfigurableProperty { 
                Name = "leafNode", 
                Value = "leaf-value" 
            };

            var parentNode = new ConfigurableProperty { 
                Name = "parentNode",
                ChildProperties = new List<ConfigurableProperty> { leafNode }
            };

            var sut = new ConfigurableProperty
            {
                Name = "rootNode",
                ChildProperties = new List<ConfigurableProperty> { parentNode }
            };
            
            var tryResult = sut.TryGetValue(new string[3] { "rootNode", "parentNode", "leafNode" },
                out var valueResult);

            tryResult.Should().BeTrue();
            valueResult.Should().Be("leaf-value");
        }

        [Fact]
        public void WHEN_nested_properties_gets_property_with_matched_node_names_THEN_value_returned()
        {
            var leafNode = new ConfigurableProperty
            {
                Name = "leafNode",
                Value = "leaf-value"
            };

            var parentNode = new ConfigurableProperty
            {
                Name = "parentNode",
                ChildProperties = new List<ConfigurableProperty> { leafNode }
            };

            var sut = new ConfigurableProperty
            {
                Name = "rootNode",
                ChildProperties = new List<ConfigurableProperty> { parentNode }
            };

            var tryResult = sut.TryGetProperty(new string[3] { "rootNode", "parentNode", "leafNode" },
                out var valueResult);

            tryResult.Should().BeTrue();
            valueResult.Should().NotBeNull();
            valueResult?.Name.Should().Be("leafNode");
            valueResult?.Value.Should().Be("leaf-value");
        }

        [Fact]
        public void WHEN_nested_properties_gets_value_with_incorrect_node_names_THEN_value_returned()
        {
            var leafNode = new ConfigurableProperty
            {
                Name = "leafNode",
                Value = "leaf-value"
            };

            var parentNode = new ConfigurableProperty
            {
                Name = "parentNode",
                ChildProperties = new List<ConfigurableProperty> { leafNode }
            };

            var sut = new ConfigurableProperty
            {
                Name = "rootNode",
                ChildProperties = new List<ConfigurableProperty> { parentNode }
            };

            var tryResult = sut.TryGetValue(new string[3] { "rootNode", "not-the-parentNode", "leafNode" },
                out var valueResult);

            tryResult.Should().BeFalse();
            valueResult.Should().BeNull();
        }

        [Fact]
        public void WHEN_nested_properties_gets_property_with_incorrect_node_names_THEN_value_returned()
        {
            var leafNode = new ConfigurableProperty
            {
                Name = "leafNode",
                Value = "leaf-value"
            };

            var parentNode = new ConfigurableProperty
            {
                Name = "parentNode",
                ChildProperties = new List<ConfigurableProperty> { leafNode }
            };

            var sut = new ConfigurableProperty
            {
                Name = "rootNode",
                ChildProperties = new List<ConfigurableProperty> { parentNode }
            };

            var tryResult = sut.TryGetProperty(new string[3] { "rootNode", "not-the-parentNode", "leafNode" },
                out var valueResult);

            tryResult.Should().BeFalse();
            valueResult.Should().BeNull();
        }
    }
}
