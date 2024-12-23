using console_store.Servicers;

namespace console_store;

class Program
{
    static void Main(string[] args)
    {
        string example = "15^2 + ( ( 1 + 2 ) *  4 ) -3,25";
        Console.WriteLine($"Enter math expression. Example: {example}");
        
        string? statement = Console.ReadLine();

        if (statement != null) {
            try {
                RpnService service = new RpnService(statement);
                Console.WriteLine(service.Calculate());
            } catch {
                Console.WriteLine("You entered an invalid expression.");
            }
        }
    }
}
