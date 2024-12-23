using console_store.Enums;

namespace console_store.Objects;

abstract public class MathMember
{
    protected string _value;

    public MathMember(string value)
    {
        Validate(value);
        _value = value;
    }
    
    public string GetValue()
    {
        return _value;
    }

    public static MathMemberType GetMathMemberType(string member)
    {
        if (member.Length > 1) {
            if (Operand.IsValidOperand(member)) return MathMemberType.Operand;
        } else {
            char m = member[0];
            if (Operand.IsValidOperand(member)) return MathMemberType.Operand;
            if (Operator.Operators.Contains(m)) return MathMemberType.Operator;
            if (m == '(') return MathMemberType.OpeningBracket;
            if (m == ')') return MathMemberType.ClosingBracket;
        }

        throw new Exception($"Invalid math member type: {member}");
    }

    protected abstract void Validate(string value);
}