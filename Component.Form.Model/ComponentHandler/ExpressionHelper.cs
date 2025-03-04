using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Component.Form.Model.ComponentHandler;

public static class ExpressionHelper
{

    public static async Task<List<string>> Validate(dynamic data, List<ValidationRule> validationRules)
    {
        var errors = new List<string>();
        if (validationRules == null || validationRules.Count == 0)
        {
            return errors;
        }

        foreach (var rule in validationRules)
        {
            var result = await EvaluateCondition(rule.Expression, data);
            if (!result)
            {
                errors.Add(rule.ErrorMessage);
            }
        }

        return errors;
    }

    public static async Task<bool> EvaluateCondition(string expression, dynamic data)
    {
        var options = ScriptOptions.Default.AddReferences(typeof(object).Assembly)  // System
            .AddReferences("Microsoft.CSharp")
            .AddImports("System", "System.Collections.Generic", "Microsoft.CSharp.RuntimeBinder");

        var script = CSharpScript.Create<bool>(expression, options, typeof(Globals));
        var result = await script.RunAsync(new Globals { Data = data });
        return result.ReturnValue;
    }

    public class Globals
    {
        public dynamic Data { get; set; }
    }
}
