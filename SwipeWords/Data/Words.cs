namespace SwipeWords.Data;

public struct Words
{
    public List<string> CorrectWords { get; set; }
    public List<string> IncorrectWords { get; set; }

    public Words(List<string> correctWords, List<string> incorrectWords)
    {
        CorrectWords = correctWords;
        IncorrectWords = incorrectWords;
    }
}