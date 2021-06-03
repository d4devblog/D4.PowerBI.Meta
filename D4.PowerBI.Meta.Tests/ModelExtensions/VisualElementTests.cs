using D4.PowerBI.Meta.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace D4.PowerBI.Meta.Tests.ModelExtensions
{
    public class VisualElementTests
    {
        [Fact]
        public void WHEN_no_config_settings_exist_THEN_get_formatting_returns_null()
        {
            var sut = new VisualElement
            {
                Name = "Test-Element",
                VisualType = "TestVisual",
                Configuration = new List<ConfigurableProperty>()
            };

            var result = sut.TryGetVisualFormatting(out var resultValue);

            result.Should().BeFalse();
            resultValue.Should().BeNull();
        }

        [Fact]
        public void WHEN_incorrect_config_settings_exist_THEN_get_formatting_returns_null()
        {
            var sut = new VisualElement
            {
                Name = "Test-Element",
                VisualType = "TestVisual",
                Configuration = new List<ConfigurableProperty>
                {
                    new ConfigurableProperty{ Name = "not-formatting", Value = 123 }
                }
            };

            var result = sut.TryGetVisualFormatting(out var resultValue);

            result.Should().BeFalse();
            resultValue.Should().BeNull();
        }

        [Fact]
        public void WHEN_correct_config_settings_exist_THEN_get_formatting_returns_null()
        {
            var styleProperties = new List<ConfigurableProperty>
            {
                new ConfigurableProperty{ Name = "font-colour", Value = "000" },
                new ConfigurableProperty{ Name = "font-family", Value = "Arial" }
            };

            var objectsNode = new ConfigurableProperty
            {
                Name = "objects",
                ChildProperties = styleProperties
            };

            var visualNode = new ConfigurableProperty
            {
                Name = "singleVisual",
                ChildProperties = new List<ConfigurableProperty>
                {
                    objectsNode
                }
            };

            var anotherRootNode = new ConfigurableProperty
            {
                Name = "another-root"
            };

            var sut = new VisualElement
            {
                Name = "Test-Element",
                VisualType = "TestVisual",
                Configuration = new List<ConfigurableProperty>
                {
                    visualNode,
                    anotherRootNode
                }
            };

            var result = sut.TryGetVisualFormatting(out var resultValue);

            result.Should().BeTrue();
            resultValue.Should().BeEquivalentTo(styleProperties);
        }
    }
}
