var random = new Random();
FilePathBuilder pathToRandom = new FilePathBuilder("text.txt");
FilePathBuilder pathToSort = new FilePathBuilder("text2.txt");

RandomListWriter randomWriter = new RandomListWriter(random);
randomWriter.Write(pathToRandom);

SortedTextWriter sortedTextWriter = new SortedTextWriter(random);
sortedTextWriter.Write(pathToRandom, pathToSort);




Console.ReadKey();

class TextWriter
{
    protected int _numberOfListsToWrite = 4;
    protected Random _random;
    public TextWriter(Random random)
    {
        _random = random;
    }
}

class RandomListWriter : TextWriter
{
    public RandomListWriter(Random random) : base(random)
    {
    }

    public void Write(FilePathBuilder pathBuilder)
    {
        RandomListGenerator randomListGenerator = new RandomListGenerator(_random);

        File.WriteAllText(pathBuilder.Path, randomListGenerator.GenerateTextToWrite(_numberOfListsToWrite));
    }
}

class SortedTextWriter : TextWriter
{
    public SortedTextWriter(Random random) : base(random)
    {
    }

    public void Write(FilePathBuilder pathToRandom, FilePathBuilder pathToSorted)
    {
        var listOfStringLists = File.ReadLines(pathToRandom.Path).Select(item => item.Split(",")).ToList();
        List<List<int>> listsOfIntList = new List<List<int>>();

        foreach (var stringList in listOfStringLists)
        {
            List<int> newIntList = new List<int>();
            foreach (var item in stringList)
            {
                int result;
                if (int.TryParse(item, out result))
                {
                    newIntList.Add(result);
                }
            }
            listsOfIntList.Add(newIntList.OrderBy(x => x).ToList());
        }
        string stringToWrite = string.Join(Environment.NewLine, listsOfIntList.Select(list => string.Join(",", list)));
        File.WriteAllText(pathToSorted.Path, stringToWrite);

    }
}

class FilePathBuilder
{


    public FilePathBuilder(string path)
    {
        Path = path;
    }
    public string Path { get; }

}

class RandomListGenerator
{
    private Random _random;
    private const int listLength = 6;

    public RandomListGenerator(Random random)
    {
        _random = random;
    }


    public string GenerateTextToWrite(int times)
    {

        string textGenerated = "";

        textGenerated = string.Join(Environment.NewLine,
            Enumerable.Range(1, times).
            Select(_ => string.Join(",",
                Enumerable.Range(1, listLength).
                Select(__ => _random.Next(1, 21)).
                ToList())));

        return textGenerated;
    }
}



