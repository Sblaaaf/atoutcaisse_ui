using DGsys.UI.Components.Grid;

namespace DGsys.UI.Tests;

public class ColumnFilterTests
{
    [Theory]
    [InlineData(FilterOperator.Contains, "oll", "OLLIVIER", true)]
    [InlineData(FilterOperator.NotContains, "oll", "OLLIVIER", false)]
    [InlineData(FilterOperator.StartsWith, "oll", "OLLIVIER", true)]
    [InlineData(FilterOperator.EndsWith, "ier", "OLLIVIER", true)]
    [InlineData(FilterOperator.Equals, "test", "TEST", true)]
    [InlineData(FilterOperator.NotEquals, "test", "TEST", false)]
    public void Filtres_texte_insensibles_a_la_casse(FilterOperator op, string value, string cell, bool expected) =>
        Assert.Equal(expected, new ColumnFilter(op, value).Matches(cell, cell));

    [Theory]
    [InlineData(FilterOperator.LessOrEqual, "10", 9.99, true)]
    [InlineData(FilterOperator.LessOrEqual, "10", 10.01, false)]
    [InlineData(FilterOperator.GreaterOrEqual, "9,5", 9.99, true)] // virgule française acceptée
    public void Filtres_numeriques(FilterOperator op, string value, double raw, bool expected) =>
        Assert.Equal(expected, new ColumnFilter(op, value).Matches((decimal)raw, raw.ToString()));
}
