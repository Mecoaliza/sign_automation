public static class GetTransactionData
{
    public static string Run(List<string> queue, int index)
    {
        return index < queue.Count ? queue[index] : null;
    }
}