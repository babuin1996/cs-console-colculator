namespace console_store.Objects;

public class Operand : MathMember
{
    public Operand(string value) : base(value) {}
    
    protected override void Validate(string value)
    {
        if (!IsValidOperand(value)) {
            throw new ArgumentException("Value must be numeric " + value);
        }
    }

    public static bool IsValidOperand(string operand)
    {
        return double.TryParse(operand, out _);
    }
}