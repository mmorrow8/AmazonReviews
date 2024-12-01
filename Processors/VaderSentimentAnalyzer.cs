using System;
using System.Collections.Generic;
using System.IO;

public class VaderSentimentAnalyzer
{
    private readonly Dictionary<string, float> _lexicon;

    public VaderSentimentAnalyzer(string lexiconFilePath)
    {
        _lexicon = LoadLexicon(lexiconFilePath);
    }

    private Dictionary<string, float> LoadLexicon(string lexiconFilePath)
    {
        var lexicon = new Dictionary<string, float>();

        foreach (var line in File.ReadAllLines(lexiconFilePath))
        {
            if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
            {
                var parts = line.Split('\t');
                lexicon[parts[0]] = float.Parse(parts[1]);
            }
        }

        return lexicon;
    }

    public int AnalyzeSentiment(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return 0;
        }

        var words = text.Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        float compoundScore = 0.0f;
        float absoluteScore = 0.0f;

        foreach (var word in words)
        {
            var lowerWord = word.ToLower();

            if (_lexicon.ContainsKey(lowerWord))
            {
                compoundScore += _lexicon[lowerWord];
                absoluteScore += Math.Abs(_lexicon[lowerWord]);
            }
        }

        var scaledScore = (int)Math.Round((compoundScore / absoluteScore) * 5, MidpointRounding.ToZero);
        scaledScore = scaledScore <= 0 ? 1 : scaledScore;
        return scaledScore;
    }
}
