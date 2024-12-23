using console_store.Objects;
using System.Text.RegularExpressions;
using console_store.Enums;

namespace console_store.Servicers;

public class RpnService
{
    private string _mathRegex = "(?<=[-+*/()\\^])|(?=[-+*/()\\^])";
    private string _statement;
    private Stack<string> _stack = new Stack<string>();
    private List<string> _finalMembers = new List<string>();
    private List<string> _firstMembers = new List<string>();

    public RpnService(string statement)
    {
        _statement = statement.Replace(" ", string.Empty);
        Console.WriteLine(_statement);
    }

    public string Calculate() {
        string[] members = Regex.Split(_statement, _mathRegex);

        foreach (string member in members) {
            _firstMembers.Add(member);
        }

        for (int i = 0; i < _firstMembers.Count; i++){
            string x = _firstMembers[i];
            MathMemberType memberType = MathMember.GetMathMemberType(x);
            HandleMathMemberType(memberType, x);
        }

        while (_stack.Count > 0) {
            _finalMembers.Add(_stack.Pop());
        }
        
        ProcessFinalStatement();

        return _finalMembers[0];
    }

    private void ProcessFinalStatement()
    {
        for (int i = 0; i < _finalMembers.Count; i++)
        {
            if (i + 1 >= _finalMembers.Count) return;
            
            string mathMember = _finalMembers[i + 1];

            if (mathMember.Length == 1 &&
                MathMember.GetMathMemberType(mathMember) == MathMemberType.Operator) {

                double result = Operate(
                    Convert.ToDouble(_finalMembers[i-1]),
                    Convert.ToDouble(_finalMembers[i]),
                    new Operator(mathMember)
                );

                for (int j = 0; j < 3; j++) _finalMembers.RemoveAt(i-1);

                _finalMembers.Insert(i-1, result.ToString());

                if (_finalMembers.Count > 1) {
                    ProcessFinalStatement();
                }

                return;
            }
        }
    }

    private readonly Dictionary<char, int> _priorities = new Dictionary<char, int>
    {
        {'=', 1},
        {'+', 2},
        {'-', 2},
        {'*', 3},
        {'/', 3},
        {'^', 4}
    };

    private int GetOperatorPriority(Operator operatorItem)
    {
        if (_priorities.ContainsKey(operatorItem.GetValue()[0])) return _priorities[operatorItem.GetValue()[0]];

        throw new ArgumentException($"Unknown operand: {operatorItem.GetValue()}");
    }

    private void HandleMathMemberType(MathMemberType mathMemberType, string x)
    {
        switch (mathMemberType) {
            case MathMemberType.OpeningBracket:
                _stack.Push(x);
                break;
            case MathMemberType.ClosingBracket:
                while (!IsStackEmpty() && !IsLastOpBracket()) {
                    _finalMembers.Add(_stack.Pop());
                }

                _stack.Pop();
                break;
            case MathMemberType.Operand:
                _finalMembers.Add(x);
                break;
            case MathMemberType.Operator:

                while (!IsStackEmpty() && !IsLastOpBracket() && CurrentHasLowerPriority(x[0])) {
                    _finalMembers.Add(_stack.Pop());
                }

                _stack.Push(x);
                break;
        }
    }

    private bool IsStackEmpty() => _stack.Count == 0;
    private bool IsLastOpBracket() => !IsStackEmpty() && _stack.Peek() == "(";
    private bool CurrentHasLowerPriority(char x) => GetOperatorPriority(new Operator(x.ToString())) <= GetOperatorPriority(new Operator(_stack.Peek().ToString()));

    private double Operate(double a, double b, Operator op)
    {
        switch (op.GetValue()[0]) {
            case '+': return a + b;
            case '-': return a - b;
            case '*': return a * b;
            case '/': return a / b;
            case '^': return Math.Pow(a, b);
        }

        throw new Exception("Wrong operator");
    }
}