static class PriceReport
{
    public static void Run(string[] args)
    {
        if (args.Length != 2)
            throw new FormatException("Usage: price study.tsv");
        var scenarios = Rows(args[1], 8).Select(r =>
        {
            var price = Amount(r[1]);
            var demand = Integer(r[2]);
            var batch = Integer(r[3]);
            var batches = Integer(r[4]);
            Require(batch > 0, "Batch size must be positive.");
            var made = checked(batch * batches);
            var cost = Amount(r[5]);
            var setup = Amount(r[6]);
            var salvage = Amount(r[7]);
            var sale = Math.Min(demand, made);
            var left = made - sale;
            return (Name: r[0], Made: made, Sold: sale, Left: left, Unmet: demand - sale, Margin: price - cost, Profit: sale * price + left * salvage - made * cost - setup);
        }).ToArray();
        foreach (var s in scenarios)
            Console.WriteLine($"{s.Name}: produced {s.Made}, sold {s.Sold}, leftovers {s.Left}, unfilled {s.Unmet}, unit margin {s.Margin:0.00}, profit {s.Profit:0.00}");
        var best = scenarios.Max(s => s.Profit);
        Console.WriteLine($"Best profit: {string.Join(", ", scenarios.Where(s => s.Profit == best).Select(s => s.Name))} ({best:0.00})");
    }

    static List<string[]> Rows(string file, int width = 0, bool allowEmpty = false)
    {
        using var reader = file == "-" ? Console.In : File.OpenText(file);
        var rows = new List<string[]>();
        int line = 0;
        while (reader.ReadLine()is string s)
        {
            line++;
            if (string.IsNullOrWhiteSpace(s) || s.TrimStart().StartsWith("#"))
                continue;
            var cells = s.Split('\t', StringSplitOptions.TrimEntries);
            if ((width > 0 && cells.Length != width) || (!allowEmpty && cells.Any(string.IsNullOrEmpty)))
                throw new FormatException($"Invalid columns on line {line}.");
            rows.Add(cells);
        }

        if (rows.Count == 0)
            throw new FormatException("Empty input.");
        return rows;
    }

    static void Require(bool condition, string message)
    {
        if (!condition)
            throw new FormatException(message);
    }

    static decimal Amount(string text)
    {
        var value = decimal.Parse(text, System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint);
        Require(value >= 0, "Quantity must be nonnegative.");
        return value;
    }

    static long Integer(string text)
    {
        var value = long.Parse(text);
        Require(value >= 0, "Count must be nonnegative.");
        return value;
    }
}
