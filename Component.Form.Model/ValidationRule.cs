using System;

namespace Component.Form.Model;

public class ValidationRule
{
    public string Id { get; set; } = string.Empty;
    public string Expression { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}
