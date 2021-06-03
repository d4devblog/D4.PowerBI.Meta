using D4.PowerBI.Meta.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace D4.PowerBI.Meta.Tests.ModelExtensions
{
    public class ReportPageTests
    {
        [Fact]
        public void WHEN_no_config_settings_exist_THEN_get_formatting_returns_null()
        {
            var sut = new ReportPage
            {
                Name = "Test-Page",
                DisplayName = "Page Name",
                Configuration = new List<ConfigurableProperty>()
            };

            var result = sut.TryGetPageFormatting(out var resultValue);

            result.Should().BeFalse();
            resultValue.Should().BeNull();
        }

        [Fact]
        public void WHEN_incorrect_config_settings_exist_THEN_get_formatting_returns_null()
        {
            var sut = new ReportPage
            {
                Name = "Test-Page",
                DisplayName = "Page Name",
                Configuration = new List<ConfigurableProperty>
                {
                    new ConfigurableProperty{ Name = "not-formatting", Value = 123 }
                }
            };

            var result = sut.TryGetPageFormatting(out var resultValue);

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

            var anotherRootNode = new ConfigurableProperty
            {
                Name = "another-root"
            };

            var sut = new ReportPage
            {
                Name = "Test-Page",
                DisplayName = "Page Name",
                Configuration = new List<ConfigurableProperty>
                {
                    objectsNode,
                    anotherRootNode
                }
            };

            var result = sut.TryGetPageFormatting(out var resultValue);

            result.Should().BeTrue();
            resultValue.Should().BeEquivalentTo(styleProperties);
        }
    }
}
