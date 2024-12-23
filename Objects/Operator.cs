namespace console_store.Objects;

public class Operator : MathMember
{
    public static readonly List<char> Operators = ['+','-','/','*','^'];
    
    public Operator(string value) : base(value) {}
    
    protected override void Validate(string value)
    {
        if (!Operators.Contains(value[0]))
        {
            throw new ArgumentException("Value must be one of the following: +, -, /, *, ^");
        }
    }
}